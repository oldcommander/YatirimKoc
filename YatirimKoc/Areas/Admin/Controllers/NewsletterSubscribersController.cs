using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
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

        public async Task<IActionResult> Index(string searchTerm, bool? isActive, string sortOrder, int pageNumber = 1)
        {
            // Filtre seçimlerinin View'da korunması için ViewBag'e atıyoruz
            ViewBag.SearchTerm = searchTerm;
            ViewBag.IsActive = isActive;
            ViewBag.SortOrder = sortOrder ?? "date_desc";

            var query = new GetAllSubscribersQuery
            {
                SearchTerm = searchTerm,
                IsActive = isActive,
                SortOrder = ViewBag.SortOrder,
                PageNumber = pageNumber,
                PageSize = 10
            };

            var result = await _mediator.Send(query);
            return View(result);
        }

        // AJAX istekleri için güncellendi
        [HttpPost]
        public async Task<IActionResult> ToggleStatus([FromBody] Guid id)
        {
            var result = await _mediator.Send(new ToggleSubscriberStatusCommand { Id = id });
            if (result)
                return Json(new { success = true, message = "Abonelik durumu güncellendi." });

            return Json(new { success = false, message = "Kayıt bulunamadı veya bir hata oluştu." });
        }

        // AJAX istekleri için güncellendi
        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] Guid id)
        {
            var result = await _mediator.Send(new DeleteSubscriberCommand { Id = id });
            if (result)
                return Json(new { success = true, message = "Abone başarıyla silindi." });

            return Json(new { success = false, message = "Kayıt bulunamadı veya bir hata oluştu." });
        }
    }
}