using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YatirimKoc.Application.Features.Users.Commands;
using YatirimKoc.Application.Features.Users.Queries;
using YatirimKoc.Domain.Entities.Identity;

namespace YatirimKoc.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = RoleConstants.SuperAdmin)] // Kilit nokta: Sadece SuperAdmin girebilir!
public class UsersController : Controller
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // 1. LİSTELEME EKRANI
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var users = await _mediator.Send(new GetAllUsersQuery());
        return View(users);
    }

    // 2. YENİ KULLANICI EKLEME EKRANI (GET)
    [HttpGet]
    public IActionResult Create()
    {
        return View(new CreateUserCommand());
    }

    // 3. YENİ KULLANICI EKLEME İŞLEMİ (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateUserCommand command)
    {
        if (!ModelState.IsValid)
            return View(command);

        try
        {
            await _mediator.Send(command);
            TempData["SuccessMessage"] = "Kullanıcı başarıyla oluşturuldu.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(command);
        }
    }

    // 4. SİLME İŞLEMİ (POST) - Soft Delete
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteUserCommand { Id = id });

        if (result)
            TempData["SuccessMessage"] = "Kullanıcı başarıyla pasife alındı (Silindi).";
        else
            TempData["ErrorMessage"] = "Kullanıcı silinirken bir hata oluştu.";

        return RedirectToAction(nameof(Index));
    }

    // 5. GÜNCELLEME EKRANI (GET)
    [HttpGet]
    public async Task<IActionResult> Edit(Guid id)
    {

        // Önce kullanıcının mevcut verilerini çekiyoruz
        var userDetail = await _mediator.Send(new GetUserDetailQuery { UserId = id });
        ViewBag.ProfileImageUrl = userDetail.ProfileImageUrl;

        // Ekranda formun (UpdateUserCommand) dolması için verileri aktarıyoruz
        var command = new UpdateUserCommand
        {
            Id = userDetail.Id,
            FirstName = userDetail.FirstName,
            LastName = userDetail.LastName,
            Email = userDetail.Email,
            Role = userDetail.Role,
            IsActive = userDetail.IsActive,
            Title = userDetail.Title ?? "",
            Phone = userDetail.Phone,
            Biography = userDetail.Biography,
            LicenseNumber = userDetail.LicenseNumber,
            ExperienceYear = userDetail.ExperienceYear,
            Specialties = userDetail.Specialties,
            SpokenLanguages = userDetail.SpokenLanguages,
            InstagramUrl = userDetail.InstagramUrl,
            LinkedInUrl = userDetail.LinkedInUrl,
            FacebookUrl = userDetail.FacebookUrl
        };

        return View(command);
    }

    // 6. GÜNCELLEME İŞLEMİ (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UpdateUserCommand command)
    {
        if (!ModelState.IsValid)
            return View(command);

        try
        {
            await _mediator.Send(command);
            TempData["SuccessMessage"] = "Danışman bilgileri başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View(command);
        }
    }
}