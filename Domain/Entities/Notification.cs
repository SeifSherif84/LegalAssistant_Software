using Domain.Entities;
using Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public enum NotificationChannel
{
    InApp,
    Email,
    Sms,
    Push
}

public enum NotificationPriority
{
    Low,
    Normal,
    High,
    Critical
}

public class Notification
{
    [Key]
    public int Id { get; set; }

    // FK to Identity User (GUID) — the account who should receive the notification
    [Required]
    public Guid UserId { get; set; }
    public UserApp? User { get; set; }

    // Human-readable message (short)
    [Required, MaxLength(1024)]
    public string Message { get; set; } = string.Empty;

    // Optional structured payload (e.g. { "caseId": 12, "sessionId": 5 })
    // Stored as JSON string; you can deserialize when needed.
    public string? PayloadJson { get; set; }

    // Optional links to domain entities for quick navigation
    public int? CaseId { get; set; }
    public Case? Case { get; set; }

    public int? CourtSessionId { get; set; }
    public CourtSession? CourtSession { get; set; }

    public int? DocumentId { get; set; }
    public Document? Document { get; set; }

    // Where to deliver (InApp is primary). Use as hint for delivery service.
    [Required]
    public NotificationChannel Channel { get; set; } = NotificationChannel.InApp;

    // Priority for UX / escalation
    public NotificationPriority Priority { get; set; } = NotificationPriority.Normal;

    // Read / unread tracking. Use ReadAt for exact time of reading.
    public bool IsRead { get; set; } = false;
    public DateTime? ReadAt { get; set; }

    // Created at (store in UTC)
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Optional expiration (after which it can be auto-archived/deleted)
    public DateTime? ExpiresAt { get; set; }

    // Delivery attempts / status (for email/sms/push reliability)
    public int DeliveryAttempts { get; set; } = 0;
    public string? DeliveryStatus { get; set; } // e.g., "Pending", "Sent", "Failed"

    // Soft-delete / archive flag — don't physically delete immediately
    public bool IsArchived { get; set; } = false;

    // Optional admin/internal notes (not shown to user)
    public string? InternalNotes { get; set; }
}
