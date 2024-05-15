namespace SFA.DAS.PR.Application.Permissions.Queries.GetAllPermissionsForAccount;
public class GetAllPermissionsForAccountQueryResult
{
    public GetAllPermissionsForAccountQueryResult()
    {

    }

    public GetAllPermissionsForAccountQueryResult(List<AccountLegalEntityPermissionsModel> legalEntities)
    {
        this.LegalEntities = legalEntities;
    }

    public List<AccountLegalEntityPermissionsModel> LegalEntities { get; set; } = [];
}