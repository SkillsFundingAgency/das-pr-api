using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.PR.Domain.Interfaces;
public interface IOperationsEntity
{
    public List<Operation> Operations { get; set; }
}
