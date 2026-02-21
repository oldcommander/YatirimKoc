using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YatirimKoc.Application.Features.Settings.Commands.Update;
using YatirimKoc.Application.Features.SiteSettings.Queries;

namespace YatirimKoc.Web.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class SiteSettingsController : Controller
{
    private readonly IMediator _mediator;

    public SiteSettingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var settings = await _mediator.Send(new GetSiteSettingsQuery());
        return View(settings);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(UpdateSiteSettingsCommand command)
    {
        if (!ModelState.IsValid)
            return View("Index", await _mediator.Send(new GetSiteSettingsQuery()));

        await _mediator.Send(command);
        return RedirectToAction(nameof(Index));
    }
}
