using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YatirimKoc.Application.Features.ContactMessages.Commands;
using YatirimKoc.Application.Features.ContactMessages.Queries;
using YatirimKoc.Domain.Enums;

namespace YatirimKoc.Areas.Admin.Controllers;

[Area("Admin")]
// [Authorize(Roles = "SuperAdmin,Admin")] // Projenizdeki auth yapısına göre aktif edebilirsiniz.
public class ContactMessagesController : Controller
{
    private readonly IMediator _mediator;

    public ContactMessagesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IActionResult> Index()
    {
        var messages = await _mediator.Send(new GetAllContactMessagesQuery());
        return View(messages);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var message = await _mediator.Send(new GetContactMessageDetailQuery(id));
        if (message == null) return NotFound();

        // Mesaj detayına tıklandığında statüsü Pending ise otomatik Read (Okundu) yapalım.
        if (message.Status == ContactMessageStatus.Pending)
        {
            await _mediator.Send(new UpdateContactMessageStatusCommand { Id = id, NewStatus = ContactMessageStatus.Read });
            message.Status = ContactMessageStatus.Read; // View için modeli güncelle
        }

        return View(message);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkAsReplied(Guid id)
    {
        await _mediator.Send(new UpdateContactMessageStatusCommand { Id = id, NewStatus = ContactMessageStatus.Replied });
        TempData["SuccessMessage"] = "Mesaj durumu başarıyla 'Cevaplandı' olarak güncellendi.";
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteContactMessageCommand(id));
        if (result)
            TempData["SuccessMessage"] = "Mesaj başarıyla silindi.";
        else
            TempData["ErrorMessage"] = "Mesaj silinirken bir hata oluştu.";

        return RedirectToAction(nameof(Index));
    }
}