namespace SFA.DAS.PR.Application.Permissions.Queries.GetEmployerRelationships;
public class GetEmployerRelationshipsQueryResult
{
    public GetEmployerRelationshipsQueryResult()
    {

    }

    public GetEmployerRelationshipsQueryResult(List<AccountLegalEntityPermissionsModel> legalEntities)
    {
        this.LegalEntities = legalEntities;
    }

    public List<AccountLegalEntityPermissionsModel> LegalEntities { get; set; } = [];
}