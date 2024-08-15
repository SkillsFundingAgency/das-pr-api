using MediatR;
using SFA.DAS.PR.Application.Mediatr.Responses;
using SFA.DAS.PR.Domain.Interfaces;
using SFA.DAS.ProviderRelationships.Types.Models;

namespace SFA.DAS.PR.Application.Requests.Commands.CreateNewAccountRequest;

public class CreateNewAccountRequestCommand : IRequest<ValidatedResponse<CreateNewAccountRequestCommandResult>>, IUkprnEntity, IOperationsEntity
{
    public long? Ukprn { get; set; }
    public required string RequestedBy { get; set; }
    public required string EmployerOrganisationName { get; set; }
    public required string EmployerContactFirstName { get; set; }
    public required string EmployerContactLastName { get; set; }
    public required string EmployerContactEmail { get; set; }
    public string? EmployerPAYE { get; set; }
    public string? EmployerAORN { get; set; }
    public List<Operation> Operations { get; set; } = [];
}
