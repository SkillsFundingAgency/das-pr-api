namespace SFA.DAS.PR.Domain.Entities;

public class PermissionRequest
{
    public Guid Id { get; set; }

    public Guid RequestId { get; set; }

    public short Operation { get; set; }

    public virtual Request Request { get; set; } = null!;
}