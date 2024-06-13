using System.Runtime.Serialization;

namespace SFA.DAS.PR.Domain.Entities;

public enum PermissionAction : short
{
    [EnumMember(Value = "approvals relationship")]
    ApprovalsRelationship,

    [EnumMember(Value = "recruit relationship")]
    RecruitRelationship,

    [EnumMember(Value = "permission created")]
    PermissionCreated,

    [EnumMember(Value = "permission updated")]
    PermissionUpdated,

    [EnumMember(Value = "permission deleted")]
    PermissionDeleted
}