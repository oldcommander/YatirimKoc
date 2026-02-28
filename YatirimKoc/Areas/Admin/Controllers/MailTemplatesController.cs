using MediatR;
using Microsoft.AspNetCore.Mvc;
using YatirimKoc.Application.Features.MailTemplates.Commands;
using YatirimKoc.Application.Features.MailTemplates.Queries;

namespace YatirimKoc.Areas.Admin.Controllers;

[Area("Admin")]
public class MailTemplatesController : Controller
{
    private readonly IMediator _mediator;

    public MailTemplatesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IActionResult> Index(string? searchTerm, int pageNumber = 1)
    {
        ViewData["CurrentSearch"] = searchTerm;
        var query = new GetAllMailTemplatesQuery { SearchTerm = searchTerm, PageNumber = pageNumber };
        var templates = await _mediator.Send(query);
        return View(templates);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateMailTemplateCommand command)
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
        var template = await _mediator.Send(new GetMailTemplateByIdQuery(id));
        if (template == null) return NotFound();

        var command = new UpdateMailTemplateCommand
        {
            Id = template.Id,
            Code = template.Code,
            Subject = template.Subject,
            BodyHtml = template.BodyHtml,
            IsActive = template.IsActive
        };

        return View(command);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(UpdateMailTemplateCommand command)
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
        var result = await _mediator.Send(new DeleteMailTemplateCommand(id));
        return Json(new { success = result, message = result ? "Şablon başarıyla silindi." : "Silme işlemi sırasında bir hata oluştu." });
    }
}