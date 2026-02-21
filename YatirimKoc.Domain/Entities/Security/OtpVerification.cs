using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YatirimKoc.Domain.Entities.Security;

public class OtpVerification
{
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;
    public string Code { get; set; } = null!;

    public string Purpose { get; set; } = null!;
    // ContactForm, Reservation, Newsletter, PriceDrop

    public DateTime ExpiresAt { get; set; }
    public bool IsUsed { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

