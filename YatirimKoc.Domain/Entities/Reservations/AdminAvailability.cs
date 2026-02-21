namespace YatirimKoc.Domain.Entities.Reservations;

public class AdminAvailability
{
    public Guid Id { get; set; }

    public Guid AdminProfileId { get; set; }

    public DayOfWeek DayOfWeek { get; set; }

    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }

    public int SlotDurationMinutes { get; set; } = 60;

    public bool IsActive { get; set; } = true;
}
