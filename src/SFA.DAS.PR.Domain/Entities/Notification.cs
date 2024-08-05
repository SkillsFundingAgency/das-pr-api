using SFA.DAS.PR.Domain.Models;

namespace SFA.DAS.PR.Domain.Entities;

public class Notification
{
    public Guid Id { get; set; }
    public required string TemplateName { get; set; }
    public required string NotificationType { get; set; }
    public long Ukprn { get; set; }
    public string? EmailAddress { get; set; }
    public string? Contact { get; set; }
    public Guid? RequestId { get; set; }
    public long? AccountLegalEntityId { get; set; }
    public int? PermitApprovals { get; set; }
    public int? PermitRecruit { get; set; }
    public required string CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? SentTime { get; set; }

    public static implicit operator Notification(NotificationModel source) => new()
    {
        TemplateName = source.TemplateName,
        NotificationType = source.NotificationType,
        Ukprn = source.Ukprn!.Value,
        EmailAddress = source.EmailAddress,
        Contact = source.Contact,
        RequestId = source.RequestId,
        AccountLegalEntityId = source.AccountLegalEntityId,
        PermitApprovals = source.PermitApprovals,
        PermitRecruit = source.PermitRecruit,
        CreatedBy = source.CreatedBy,
        CreatedDate = DateTime.UtcNow
    };
}
