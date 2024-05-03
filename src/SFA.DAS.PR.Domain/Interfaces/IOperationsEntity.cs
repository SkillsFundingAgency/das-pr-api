using SFA.DAS.PR.Domain.Entities;

namespace SFA.DAS.PR.Domain.Interfaces;
public interface IOperationsEntity
{
    public List<Operation> Operations { get; set; }
}
