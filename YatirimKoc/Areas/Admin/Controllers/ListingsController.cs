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
        if (!ModelState.IsValid)
        {
            ViewBag.TransactionTypes = await _transactionTypeRepository.GetAllAsync();
            ViewBag.PropertyTypes = await _propertyTypeRepository.GetAllAsync();
            return View(model);
        }
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

            // KOORDİNATLARI EŞLEŞTİRİYORUZ (EKLENEN KISIM)
            Latitude = model.Latitude,
            Longitude = model.Longitude,

            FeatureValues = model.FeatureValues
        };

        if (model.Files != null && model.Files.Any())
        {
            var uploaded = await _fileUploadService.UploadAsync(model.Files, "listings");
            command.ImageUrls = uploaded;
        }

        await _mediator.Send(command);

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Index()
    {
        var listings = await _mediator.Send(new GetAllListingsQuery());
        return View(listings);
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
}