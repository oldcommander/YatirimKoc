using MediatR;
using Microsoft.AspNetCore.Mvc;
using YatirimKoc.Application.Features.Listings.Commands;
using YatirimKoc.Application.Features.Listings.Queries;
using YatirimKoc.Application.Interfaces;
using YatirimKoc.Web.Areas.Admin.Models;

namespace YatirimKoc.Web.Areas.Admin.Controllers;

[Area("Admin")]
public class ListingsController : Controller
{
    private readonly IMediator _mediator;
    private readonly IFileUploadService _fileUploadService;
    private readonly ITransactionTypeRepository _transactionTypeRepository;
    private readonly IPropertyTypeRepository _propertyTypeRepository;

    public ListingsController(
        IMediator mediator,
        IFileUploadService fileUploadService,
        ITransactionTypeRepository transactionTypeRepository,
        IPropertyTypeRepository propertyTypeRepository)
    {
        _mediator = mediator;
        _fileUploadService = fileUploadService;
        _transactionTypeRepository = transactionTypeRepository;
        _propertyTypeRepository = propertyTypeRepository;
    }

    [HttpGet]
    public async Task<IActionResult> CreateWizard()
    {
        var model = new CreateListingWizardViewModel();

        ViewBag.TransactionTypes = await _transactionTypeRepository.GetAllAsync();
        ViewBag.PropertyTypes = await _propertyTypeRepository.GetAllAsync();

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> CreateWizard(CreateListingWizardViewModel model)
    {
        // 1. Doğrulama Kontrolü
        if (!ModelState.IsValid)
        {
            // Eğer model hatalıysa sayfa yenilenir.
            ViewBag.TransactionTypes = await _transactionTypeRepository.GetAllAsync();
            ViewBag.PropertyTypes = await _propertyTypeRepository.GetAllAsync();
            return View(model);
        }

        // 2. Giriş Yapan Kullanıcının ID'sini Al
        var userIdString = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        Guid.TryParse(userIdString, out Guid userId);

        // 3. Command (Komut) Nesnesini Doldur
        var command = new CreateListingCommand
        {
            Title = model.Title,
            Description = model.Description ?? "",
            Price = model.Price,
            City = model.City,
            District = model.District,
            TransactionTypeId = model.TransactionTypeId.Value,
            PropertyTypeId = model.PropertyTypeId.Value,
            IsPublished = model.IsPublished,
            Latitude = model.Latitude,
            Longitude = model.Longitude,
            FeatureValues = model.FeatureValues,
            UserId = userId // YENİ: Kullanıcı ID'si eklendi
        };

        // 4. Dosyaları Sunucuya Yükle
        if (model.Files != null && model.Files.Any())
        {
            // İPUCU: Dosya servisi fiziksel klasör yoksa oluşturmalıdır.
            var uploadedUrls = await _fileUploadService.UploadAsync(model.Files, "listings");
            command.ImageUrls = uploadedUrls;
        }

        // 5. Veritabanına Kaydet
        await _mediator.Send(command);

        // 6. Başarılıysa Listeleme Sayfasına Yönlendir
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> Index(string searchTerm, Guid? transactionTypeId, Guid? propertyTypeId, int pageNumber = 1)
    {
        var query = new GetAllListingsQuery
        {
            SearchTerm = searchTerm,
            TransactionTypeId = transactionTypeId,
            PropertyTypeId = propertyTypeId,
            PageNumber = pageNumber,
            PageSize = 10
        };

        var result = await _mediator.Send(query);

        ViewBag.TransactionTypes = await _transactionTypeRepository.GetAllAsync();
        ViewBag.PropertyTypes = await _propertyTypeRepository.GetAllAsync();

        // Filtreleme değerlerini View'da tutmak için
        ViewData["CurrentSearch"] = searchTerm;
        ViewData["CurrentTrans"] = transactionTypeId;
        ViewData["CurrentProp"] = propertyTypeId;

        return View(result);
    }

    // DİNAMİK ÖZELLİK ÇEKME (AJAX ENDPOINT)
    [HttpGet]
    public async Task<IActionResult> GetFeatures(Guid propertyTypeId)
    {
        if (propertyTypeId == Guid.Empty) return Json(new List<FeatureDto>());

        var query = new GetFeaturesByPropertyTypeQuery { PropertyTypeId = propertyTypeId };
        var features = await _mediator.Send(query);

        return Json(features);
    }

    public async Task<IActionResult> Preview(Guid id)
    {
        var query = new GetListingDetailQuery { Id = id };
        var listing = await _mediator.Send(query);

        if (listing == null) return NotFound();

        return View(listing);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            // 1. MediatR Command nesnesini oluştur
            var command = new DeleteListingCommand { Id = id };

            // 2. Handler'a gönder ve sonucu al
            var result = await _mediator.Send(command);

            // 3. Sonuca göre JSON dön (AJAX tarafındaki JavaScript bu yanıta göre progress bar'ı dolduracak)
            if (result)
            {
                return Json(new { success = true, message = "İlan başarıyla silindi." });
            }
            else
            {
                return Json(new { success = false, message = "Silinmek istenen ilan bulunamadı veya zaten silinmiş." });
            }
        }
        catch (Exception ex)
        {
            // Beklenmedik bir hata (Örn: Veritabanı bağlantı sorunu) olursa AJAX'a bildir
            return Json(new { success = false, message = "Silme işlemi sırasında bir hata oluştu: " + ex.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {
        // İlanı tüm ilişkileriyle (özellikler, resimler) çekiyoruz
        var listing = await _mediator.Send(new GetListingDetailQuery { Id = id });
        if (listing == null) return NotFound();

        // View'a gidecek modeli dolduruyoruz
        var model = new UpdateListingViewModel
        {
            Id = listing.Id,
            Title = listing.Title,
            Description = listing.Description,
            Price = listing.Price,
            City = listing.City,
            District = listing.District,
            TransactionTypeId = listing.TransactionTypeId,
            PropertyTypeId = listing.PropertyTypeId,
            IsPublished = listing.IsPublished,
            Latitude = listing.Latitude,
            Longitude = listing.Longitude,
            ExistingImages = listing.Images.ToList(),

            // Veritabanındaki özellikleri Dictionary'e çeviriyoruz ki JS ile kolayca eşleştirelim
            FeatureValues = listing.FeatureValues.ToDictionary(x => x.FeatureId, x => x.Value)
        };

        ViewBag.TransactionTypes = await _transactionTypeRepository.GetAllAsync();
        ViewBag.PropertyTypes = await _propertyTypeRepository.GetAllAsync();

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(UpdateListingViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.TransactionTypes = await _transactionTypeRepository.GetAllAsync();
            ViewBag.PropertyTypes = await _propertyTypeRepository.GetAllAsync();
            return View(model);
        }

        var command = new UpdateListingCommand
        {
            Id = model.Id,
            Title = model.Title,
            Description = model.Description ?? "",
            Price = model.Price,
            City = model.City,
            District = model.District,
            TransactionTypeId = model.TransactionTypeId,
            PropertyTypeId = model.PropertyTypeId,
            IsPublished = model.IsPublished,
            Latitude = model.Latitude,
            Longitude = model.Longitude,
            FeatureValues = model.FeatureValues,
            DeletedImageIds = model.DeletedImageIds ?? new List<Guid>()
        };

        // Eğer yeni dosya eklendiyse sunucuya yükle ve URL'lerini Command'a ver
        if (model.NewFiles != null && model.NewFiles.Any())
        {
            command.NewImageUrls = await _fileUploadService.UploadAsync(model.NewFiles, "listings");
        }

        await _mediator.Send(command);

        // Başarılı olursa listeye dön
        return RedirectToAction("Index");
    }
}