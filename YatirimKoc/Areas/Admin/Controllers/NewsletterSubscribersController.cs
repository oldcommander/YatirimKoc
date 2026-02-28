using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YatirimKoc.Application.Features.Listings.Queries;
using YatirimKoc.Application.Features.NewsletterSubscribers.Commands;
using YatirimKoc.Application.Features.NewsletterSubscribers.Queries;

namespace YatirimKoc.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NewsletterSubscribersController : Controller
    {
        private readonly IMediator _mediator;

        public NewsletterSubscribersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // --- TOPLU MAİL GÖNDERME EKRANI (GET) ---
        [HttpGet]
        public async Task<IActionResult> SendBulk()
        {
            // 1. Şablonları Çek
            var templates = await _mediator.Send(new YatirimKoc.Application.Features.Mails.Queries.GetAllMailsQuery());
            ViewBag.Templates = templates.Items.Where(x => x.IsActive).ToList();

            // 2. Sadece "Yayında (IsPublished)" olan İlanları Çek
            var listings = await _mediator.Send(new GetAllListingsQuery());
            ViewBag.Listings = listings.Items.Where(x => x.IsPublished).ToList();

            return View();
        }

        // --- TOPLU MAİL GÖNDERME İŞLEMİ (POST) ---
        [HttpPost]
        public async Task<IActionResult> SendBulk(string templateCode, Dictionary<string, string> placeholders, Guid? selectedListingId)
        {
            if (string.IsNullOrWhiteSpace(templateCode))
            {
                TempData["Error"] = "Lütfen bir şablon seçin.";
                return RedirectToAction(nameof(SendBulk));
            }

            if (placeholders == null) placeholders = new Dictionary<string, string>();
            placeholders.TryAdd("Name", "Değerli Abonemiz");

            var allowedTemplates = new[] { "Marketing_Hot_Listing", "Newsletter_Special_Occasion" };
            if (!allowedTemplates.Contains(templateCode))
            {
                TempData["Error"] = "Güvenlik İhlali: Bu şablon sistem tarafından otomatik gönderilmektedir, toplu kullanılamaz.";
                return RedirectToAction(nameof(SendBulk));
            }

            if (templateCode == "Marketing_Hot_Listing")
            {
                if (!selectedListingId.HasValue)
                {
                    TempData["Error"] = "İlan tanıtımı için lütfen bir ilan seçiniz.";
                    return RedirectToAction(nameof(SendBulk));
                }

                var listing = await _mediator.Send(new GetListingDetailQuery { Id = selectedListingId.Value });
                if (listing == null)
                {
                    TempData["Error"] = "Seçilen ilan bulunamadı.";
                    return RedirectToAction(nameof(SendBulk));
                }

                var baseUrl = $"{Request.Scheme}://{Request.Host}";
                var imagePath = listing.Images?.FirstOrDefault()?.ImageUrl ?? "/images/default-listing.jpg";
                var fullImageUrl = imagePath.StartsWith("http") ? imagePath : baseUrl + imagePath;

                placeholders.TryAdd("ListingImageUrl", fullImageUrl);
                placeholders.TryAdd("Location", $"{listing.District}, {listing.City}");
                placeholders.TryAdd("Price", listing.Price.ToString("C0") + " " + listing.Currency);

                var featureList = listing.FeatureValues?.Select(fv => fv.Value).Take(3).ToList();
                var featuresText = (featureList != null && featureList.Any()) ? string.Join(", ", featureList) : "Lüks ve Fırsat İlan";

                placeholders.TryAdd("KeyFeatures", featuresText);
                placeholders.TryAdd("ListingUrl", baseUrl + $"/Listings/Detail/{listing.Id}");
            }

            var result = await _mediator.Send(new SendBulkNewsletterCommand
            {
                TemplateCode = templateCode,
                Placeholders = placeholders
            });

            if (result)
            {
                TempData["Success"] = "Toplu mail, tum aktif abonelere basariyla gonderildi.";
                return RedirectToAction(nameof(SendBulk)); // Başarı popup'ını göstermek için aynı sayfaya dönüyoruz
            }

            TempData["Error"] = "Mail gönderilirken bir hata oluştu veya sistemde aktif abone bulunamadı.";
            return RedirectToAction(nameof(SendBulk));
        }

        public async Task<IActionResult> Index(string searchTerm, bool? isActive, string sortOrder, int pageNumber = 1)
        {
            ViewBag.SearchTerm = searchTerm;
            ViewBag.IsActive = isActive;
            ViewBag.SortOrder = sortOrder ?? "date_desc";

            var result = await _mediator.Send(new GetAllSubscribersQuery
            {
                SearchTerm = searchTerm,
                IsActive = isActive,
                SortOrder = ViewBag.SortOrder,
                PageNumber = pageNumber,
                PageSize = 10
            });
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleStatus([FromBody] Guid id)
        {
            var result = await _mediator.Send(new ToggleSubscriberStatusCommand { Id = id });
            return Json(new { success = result, message = result ? "Abonelik durumu güncellendi." : "Bir hata oluştu." });
        }

        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] Guid id)
        {
            var result = await _mediator.Send(new DeleteSubscriberCommand { Id = id });
            return Json(new { success = result, message = result ? "Abone başarıyla silindi." : "Bir hata oluştu." });
        }

        [HttpGet]
        public async Task<IActionResult> MailLogs(string searchTerm, bool? isSuccess, int pageNumber = 1)
        {
            ViewBag.SearchTerm = searchTerm;
            ViewBag.IsSuccess = isSuccess;

            var result = await _mediator.Send(new GetAllMailLogsQuery
            {
                SearchTerm = searchTerm,
                IsSuccess = isSuccess,
                PageNumber = pageNumber,
                PageSize = 15
            });
            return View(result);
        }

        // ÇÖZÜM: _context hatası yerine Mediator üzerinden detay query'sini çağırıyoruz
        [HttpGet]
        public async Task<IActionResult> GetMailLogDetail(Guid id)
        {
            // Bu Query'i Application katmanında oluşturmuştuk
            var log = await _mediator.Send(new GetMailLogDetailQuery { Id = id });

            if (log == null) return NotFound();

            return Json(new
            {
                success = log.IsSuccess,
                error = log.ErrorMessage,
                date = log.CreatedAt.ToString("dd.MM.yyyy HH:mm:ss")
            });
        }
    }
}