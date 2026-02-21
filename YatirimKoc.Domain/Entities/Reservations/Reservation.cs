using YatirimKoc.Domain.Entities.Admin;
using YatirimKoc.Domain.Enums;

namespace YatirimKoc.Domain.Entities.Reservations;

public class Reservation
{
    public Guid Id { get; set; }

    // Kullanıcı bilgileri (Identity değil)
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? Phone { get; set; }

    // Randevu bilgileri
    public DateTime ReservationDate { get; set; }
    public string TimeSlot { get; set; } = null!; // 14:00 - 15:00

    public ReservationStatus Status { get; set; } = ReservationStatus.Pending;

    public string? Note { get; set; }

    // Danışman
    public Guid AdminProfileId { get; set; }
    public AdminProfile AdminProfile { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
