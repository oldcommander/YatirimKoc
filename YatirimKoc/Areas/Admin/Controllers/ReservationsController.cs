using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using YatirimKoc.Application.Features.Reservations.Commands;
using YatirimKoc.Application.Features.Reservations.Queries;
using YatirimKoc.Domain.Enums;

namespace YatirimKoc.Areas.Admin.Controllers;

[Area("Admin")]
public class ReservationsController : Controller
{
    private readonly IMediator _mediator;

    public ReservationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IActionResult> Index(string? searchTerm, ReservationStatus? statusId, int pageNumber = 1)
    {
        ViewData["CurrentSearch"] = searchTerm;
        ViewData["CurrentStatus"] = statusId;

        var query = new GetAllReservationsQuery
        {
            SearchTerm = searchTerm,
            Status = statusId,
            PageNumber = pageNumber
        };

        var reservations = await _mediator.Send(query);
        return View(reservations);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var reservation = await _mediator.Send(new GetReservationDetailQuery(id));
        if (reservation == null) return NotFound();

        return View(reservation);
    }

    [HttpPost]
    public async Task<IActionResult> UpdateStatus(Guid id, ReservationStatus status)
    {
        var result = await _mediator.Send(new UpdateReservationStatusCommand { Id = id, NewStatus = status });

        string statusText = status switch
        {
            ReservationStatus.Approved => "Onaylandı",
            ReservationStatus.Rejected => "Reddedildi",
            ReservationStatus.Cancelled => "İptal Edildi",
            _ => "Beklemede"
        };

        return Json(new { success = result, message = result ? $"Randevu başarıyla '{statusText}' olarak işaretlendi." : "Durum güncellenirken bir hata oluştu." });
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteReservationCommand(id));
        return Json(new { success = result, message = result ? "Randevu talebi başarıyla silindi." : "Silme işlemi sırasında bir hata oluştu." });
    }
}