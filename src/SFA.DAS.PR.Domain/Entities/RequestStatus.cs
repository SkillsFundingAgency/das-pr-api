using System.ComponentModel;
using System.Runtime.Serialization;

namespace SFA.DAS.PR.Domain.Entities;

public enum RequestStatus : short
{
    [EnumMember(Value = "new")]
    New,

    [EnumMember(Value = "sent")]
    Sent,

    [EnumMember(Value = "accepted")]
    Accepted,

    [EnumMember(Value = "declined")]
    Declined,

    [EnumMember(Value = "expired")]
    Expired,

    [EnumMember(Value = "deleted")]
    Deleted
}
