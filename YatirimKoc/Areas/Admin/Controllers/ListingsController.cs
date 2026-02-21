using MediatR;
using Microsoft.AspNetCore.Mvc;
using YatirimKoc.Application.Features.Listings.Commands;
using YatirimKoc.Application.Interfaces;

namespace YatirimKoc.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ListingsController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IFileUploadService _fileUploadService;
        private readonly IListingCategoryRepository _categoryRepository;
        private readonly IListingTypeRepository _typeRepository;

        public ListingsController(
            IMediator mediator,
            IFileUploadService fileUploadService,
            IListingCategoryRepository categoryRepository,
            IListingTypeRepository typeRepository)
        {
            _mediator = mediator;
            _fileUploadService = fileUploadService;
            _categoryRepository = categoryRepository;
            _typeRepository = typeRepository;
        }

        // ================================
        // WIZARD GET
        // ================================
        [HttpGet]
        public async Task<IActionResult> CreateWizard()
        {
            var model = new CreateListingWizardViewModel();

            ViewBag.Categories = await _categoryRepository.GetAllAsync();
            ViewBag.Types = await _typeRepository.GetAllAsync();

            return View(model);
        }

        // ================================
        // WIZARD POST
        // ================================
        [HttpPost]
        public async Task<IActionResult> CreateWizard(CreateListingWizardViewModel model)
        {
            ViewBag.Categories = await _categoryRepository.GetAllAsync();
            ViewBag.Types = await _typeRepository.GetAllAsync();

            if (!ModelState.IsValid)
            {
                // Validasyon hatalı, wizard view tekrar göster
                return View(model);
            }

            // Modeli CreateListingCommand'e çevir
            var command = new CreateListingCommand
            {
                Title = model.Title,
                Price = (decimal)model.Price,
                ListingCategoryId = model.ListingCategoryId.Value,
                ListingTypeId = model.ListingTypeId.Value,
                IsPublished = model.IsPublished,
                // Dosya upload ve diğer alanlar eklenebilir
            };

            if (model.Files != null && model.Files.Any())
            {
                var uploaded = await _fileUploadService.UploadAsync(model.Files, "listings");
                command.ImageUrls = uploaded;
            }

            await _mediator.Send(command);

            return RedirectToAction("Index");
        }

        // ================================
        // LIST
        // ================================
        public async Task<IActionResult> Index()
        {
            var listings = await _mediator.Send(new GetAllListingsQuery());
            return View(listings);
        }
    }
}
