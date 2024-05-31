namespace SFA.DAS.PR.Application.Permissions.Queries.GetEmployerRelationships;
public class GetEmployerRelationshipsQueryResult
{
    public GetEmployerRelationshipsQueryResult()
    {

    }

    public GetEmployerRelationshipsQueryResult(List<AccountLegalEntityPermissionsModel> accountLegalEntities)
    {
        this.AccountLegalEntities = accountLegalEntities;
    }

    public List<AccountLegalEntityPermissionsModel> AccountLegalEntities { get; set; } = [];
}