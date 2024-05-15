namespace SFA.DAS.PR.Application.Permissions.Queries.GetPermissions;
public class GetPermissionsQueryResult
{
    public GetPermissionsQueryResult()
    {

    }

    public GetPermissionsQueryResult(List<AccountLegalEntityPermissionsModel> legalEntities)
    {
        this.LegalEntities = legalEntities;
    }

    public List<AccountLegalEntityPermissionsModel> LegalEntities { get; set; } = [];
}