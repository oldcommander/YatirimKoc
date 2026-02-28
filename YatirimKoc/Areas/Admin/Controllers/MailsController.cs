using MediatR;
using Microsoft.AspNetCore.Mvc;
using YatirimKoc.Application.Features.Mails.Commands;
using YatirimKoc.Application.Features.Mails.Queries;

namespace YatirimKoc.Areas.Admin.Controllers;

[Area("Admin")]
public class MailsController : Controller
{
    private readonly IMediator _mediator;

    public MailsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IActionResult> Index(string? searchTerm, int pageNumber = 1)
    {
        ViewData["CurrentSearch"] = searchTerm;
        var query = new GetAllMailsQuery { SearchTerm = searchTerm, PageNumber = pageNumber };
        var mails = await _mediator.Send(query);

        return View(mails);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            TempData["ErrorMessage"] = "Geçerli bir e-posta adresi giriniz.";
            return RedirectToAction(nameof(Index));
        }

        var result = await _mediator.Send(new CreateMailCommand { Email = email });

        if (result)
            TempData["SuccessMessage"] = "Yeni abone e-posta havuzuna eklendi.";
        else
            TempData["ErrorMessage"] = "Bu e-posta adresi havuzda zaten mevcut!";

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteMailCommand(id));
        return Json(new { success = result, message = result ? "Abone başarıyla silindi." : "Silme işlemi sırasında hata oluştu." });
    }
}