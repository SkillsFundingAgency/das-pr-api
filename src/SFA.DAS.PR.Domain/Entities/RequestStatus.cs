namespace SFA.DAS.PR.Domain.Entities;

public enum RequestStatus : short
{
    New,
    Sent,
    Accepted,
    Declined,
    Expired,
    Deleted
}
