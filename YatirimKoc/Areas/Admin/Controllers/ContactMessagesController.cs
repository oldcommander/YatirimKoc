using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YatirimKoc.Application.Features.ContactMessages.Commands;
using YatirimKoc.Application.Features.ContactMessages.Queries;
using YatirimKoc.Domain.Enums;

namespace YatirimKoc.Areas.Admin.Controllers;

[Area("Admin")]
// [Authorize(Roles = "SuperAdmin,Admin")]
public class ContactMessagesController : Controller
{
    private readonly IMediator _mediator;

    public ContactMessagesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IActionResult> Index(string? searchTerm, ContactMessageStatus? statusId, int pageNumber = 1)
    {
        ViewData["CurrentSearch"] = searchTerm;
        ViewData["CurrentStatus"] = statusId;

        var query = new GetAllContactMessagesQuery
        {
            SearchTerm = searchTerm,
            Status = statusId,
            PageNumber = pageNumber
        };

        var messages = await _mediator.Send(query);
        return View(messages);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var message = await _mediator.Send(new GetContactMessageDetailQuery(id));
        if (message == null) return NotFound();

        if (message.Status == ContactMessageStatus.Pending)
        {
            await _mediator.Send(new UpdateContactMessageStatusCommand { Id = id, NewStatus = ContactMessageStatus.Read });
            message.Status = ContactMessageStatus.Read;
        }

        return View(message);
    }

    [HttpPost]
    public async Task<IActionResult> MarkAsReplied(Guid id)
    {
        var result = await _mediator.Send(new UpdateContactMessageStatusCommand { Id = id, NewStatus = ContactMessageStatus.Replied });
        return Json(new { success = result, message = result ? "Mesaj başarıyla cevaplandı olarak işaretlendi." : "İşlem sırasında bir hata oluştu." });
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteContactMessageCommand(id));
        return Json(new { success = result, message = result ? "Mesaj başarıyla silindi." : "Mesaj silinirken bir hata oluştu." });
    }
}