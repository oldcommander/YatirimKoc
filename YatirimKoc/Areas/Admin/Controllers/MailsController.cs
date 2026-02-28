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

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateMailCommand command)
    {
        if (ModelState.IsValid)
        {
            await _mediator.Send(command);
            TempData["SuccessMessage"] = "Mail şablonu başarıyla oluşturuldu.";
            return RedirectToAction(nameof(Index));
        }
        return View(command);
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var mail = await _mediator.Send(new GetMailByIdQuery(id));
        if (mail == null) return NotFound();

        var command = new UpdateMailCommand
        {
            Id = mail.Id,
            Code = mail.Code,
            Subject = mail.Subject,
            BodyHtml = mail.BodyHtml,
            IsActive = mail.IsActive
        };

        return View(command);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UpdateMailCommand command)
    {
        if (ModelState.IsValid)
        {
            var result = await _mediator.Send(command);
            if (result)
            {
                TempData["SuccessMessage"] = "Mail şablonu başarıyla güncellendi.";
                return RedirectToAction(nameof(Index));
            }
            ModelState.AddModelError("", "Güncelleme sırasında bir hata oluştu.");
        }
        return View(command);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteMailCommand(id));
        return Json(new { success = result, message = result ? "Şablon başarıyla silindi." : "Silme işlemi sırasında bir hata oluştu." });
    }
}