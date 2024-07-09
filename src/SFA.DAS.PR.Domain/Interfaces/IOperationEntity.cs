using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.PR.Domain.Interfaces;

public interface IOperationEntity
{
    public Operation? Operation { get; set; }
}
