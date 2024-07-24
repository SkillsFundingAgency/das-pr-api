CREATE VIEW [dbo].[ProviderRelationships]
AS 

SELECT 
  apv.ProviderUkprn Ukprn, 
  ale.Id AccountLegalEntityId, 
  ale.PublicHashedId AgreementId, 
  aple.Id AccountProviderLegalEntityId, 
  ale.[Name] EmployerName, 
  req.Id RequestId, 
  CAST(ISNULL(per.Operation0, 0) AS BIT) HasCreateCohortPermission, 
  CAST(ISNULL(per.Operation1, 0) AS BIT) HasRecruitmentPermission, 
  CAST(ISNULL(per.Operation2, 0) AS BIT) HasRecruitmentWithReviewPermission
FROM [dbo].[AccountProviderLegalEntities] aple
  JOIN [dbo].[AccountLegalEntities] ale on ale.[Id] = aple.[AccountLegalEntityId]
  JOIN [dbo].[AccountProviders] apv on apv.[Id] = aple.[AccountProviderId]
  LEFT JOIN [dbo].[Requests] req 
      on ale.Id = req.AccountLegalEntityId 
      AND req.Ukprn = apv.ProviderUkprn 
      AND req.[Status] IN ('New','Sent')
  LEFT JOIN (
      SELECT [AccountProviderLegalEntityId],
        MAX(CASE WHEN [Operation] = 0 THEN 1 ELSE 0 END) Operation0,
        MAX(CASE WHEN [Operation] = 1 THEN 1 ELSE 0 END) Operation1,
        MAX(CASE WHEN [Operation] = 2 THEN 1 ELSE 0 END) Operation2
      FROM [dbo].[Permissions] 
      GROUP BY [AccountProviderLegalEntityId]
      ) per on aple.Id = per.AccountProviderLegalEntityId

UNION ALL

SELECT 
  req.Ukprn Ukprn, 
  req.[AccountLegalEntityId] AccountLegalEntityId, 
  null AgreementId, 
  null AccountProviderLegalEntityId, 
  ISNULL(req.EmployerOrganisationName, ale.Name) EmployerName, 
  req.Id RequestId,  
  CAST(0 AS BIT) HasCreateCohortPermission, 
  CAST(0 AS BIT) HasRecruitmentPermission, 
  CAST(0 AS BIT) HasRecruitmentWithReviewPermission
FROM [dbo].[Requests] req
  LEFT JOIN [dbo].[AccountProviders] apv on req.Ukprn = apv.ProviderUkprn
  LEFT JOIN [dbo].[AccountLegalEntities] ale on ale.[Id] = req.[AccountLegalEntityId]
WHERE req.RequestType IN ('AddAccount','CreateAccount') 
  AND req.[Status] IN ('New','Sent')

GO
