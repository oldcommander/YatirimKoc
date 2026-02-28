using YatirimKoc.Domain.Enums;

namespace YatirimKoc.Application.Features.Reservations.Dtos;

public class ReservationDto
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }
    public DateTime ReservationDate { get; set; }
    public string TimeSlot { get; set; } = null!;
    public ReservationStatus Status { get; set; }
    public string? Note { get; set; }
    public Guid AdminProfileId { get; set; }
    public DateTime CreatedAt { get; set; }
}