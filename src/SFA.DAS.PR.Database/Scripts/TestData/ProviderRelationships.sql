-- Active Request with one permission 
DECLARE 
    @ukprn int = 11001001,
    @requestId UNIQUEIDENTIFIER,
	@accountId int = 101010,
	@accountProviderId bigint,
	@accountProviderLegalEntityId bigint;

INSERT INTO [Providers] VALUES(@ukprn,'Gurukul Academy','2018-11-15 20:30:31.3466667',NULL);

INSERT INTO [Accounts] VALUES(@accountId,'AABBCC','DDEEFF','The blue marble','2024-03-20 13:16:49.0003259',NULL);

INSERT INTO [AccountProviders] ([AccountId],[ProviderUkprn],[Created])  VALUES (@accountId, @ukprn,'2024-03-20 11:56:48.6318631')

select @accountProviderId = id from AccountProviders where ProviderUkprn = @ukprn and AccountId = @accountId

INSERT INTO [AccountLegalEntities] VALUES(4001,'YHGRFT',@accountId,'Relationship with cohort','2024-03-20 13:16:49.0007772',NULL,NULL);
INSERT INTO [AccountLegalEntities] VALUES(4002,'FHIEPA',@accountId,'Relationship with Advert','2024-03-20 11:02:30.6318631',NULL,NULL);
INSERT INTO [AccountLegalEntities] VALUES(4003,'KFURDI',@accountId,'Relationship with Review Advert','2024-03-20 11:02:30.6318631',NULL,NULL);
INSERT INTO [AccountLegalEntities] VALUES(4004,'ASLKYU',@accountId,'Relationship with Cohort and Advert','2024-03-20 11:02:30.6318631',NULL,NULL);
INSERT INTO [AccountLegalEntities] VALUES(4005,'KGBNTR',@accountId,'Relationship with no permission','2024-03-20 11:02:30.6318631',NULL,NULL);
INSERT INTO [AccountLegalEntities] VALUES(4006,'WMUNPL',@accountId,'No Relationship','2024-03-20 11:02:30.6318631',NULL,NULL);

INSERT INTO [AccountProviderLegalEntities] ([AccountProviderId],[AccountLegalEntityId],[Created],[Updated])
VALUES 
	(@accountProviderId,4001,'2024-03-20 12:06:07.6318631',NULL),
	(@accountProviderId,4002,'2024-03-20 12:06:07.6318631',NULL),
	(@accountProviderId,4003,'2024-03-20 12:06:07.6318631',NULL),
	(@accountProviderId,4004,'2024-03-20 12:06:07.6318631',NULL),
	(@accountProviderId,4005,'2024-03-20 12:06:07.6318631',NULL)

select @accountProviderLegalEntityId = Id from AccountProviderLegalEntities where AccountLegalEntityId = 4001
INSERT INTO [Permissions] ([AccountProviderLegalEntityId],[Operation]) VALUES(@accountProviderLegalEntityId, 0);

select @accountProviderLegalEntityId = Id from AccountProviderLegalEntities where AccountLegalEntityId = 4002
INSERT INTO [Permissions] ([AccountProviderLegalEntityId],[Operation]) VALUES(@accountProviderLegalEntityId, 1);

select @accountProviderLegalEntityId = Id from AccountProviderLegalEntities where AccountLegalEntityId = 4003
INSERT INTO [Permissions] ([AccountProviderLegalEntityId],[Operation]) VALUES(@accountProviderLegalEntityId, 2);

select @accountProviderLegalEntityId = Id from AccountProviderLegalEntities where AccountLegalEntityId = 4004
INSERT INTO [Permissions] ([AccountProviderLegalEntityId],[Operation]) VALUES(@accountProviderLegalEntityId, 0);
INSERT INTO [Permissions] ([AccountProviderLegalEntityId],[Operation]) VALUES(@accountProviderLegalEntityId, 1);


DELETE PermissionRequests
DELETE Requests

-- New account request with one permission
SET @requestId = NEWID();

INSERT INTO [dbo].[Requests]
	([Id], [RequestType], [Ukprn] ,[RequestedBy] ,[EmployerOrganisationName] 
	,[EmployerContactFirstName] ,[EmployerContactLastName] ,
    [EmployerContactEmail] ,[EmployerPAYE] ,[EmployerAORN])
	VALUES (@requestId, 'CreateAccount', @ukprn, 'me', 'First org ltd'
	,'FirstName','LastName'
	,'FirstName.LastName@First.com'
	,'A094734889001'
	,'307/NL88001')
INSERT INTO [dbo].[PermissionRequests]
		([Id], [RequestId], [Operation])
		VALUES  (NEWID(), @requestId, 0);  

-- New account request with two permission
SET @requestId = NEWID();

INSERT INTO [dbo].[Requests]
	([Id], [RequestType], [Ukprn] ,[RequestedBy] ,[EmployerOrganisationName] 
	,[EmployerContactFirstName] ,[EmployerContactLastName] ,
    [EmployerContactEmail] ,[EmployerPAYE] ,[EmployerAORN])
	VALUES (@requestId, 'CreateAccount', @ukprn, 'me', 'Second org ltd'
	,'FirstName','LastName'
	,'FirstName.LastName@Second.com'
	,'A094734889002'
	,'307/NL88002')
INSERT INTO [dbo].[PermissionRequests]
		([Id], [RequestId], [Operation])
		VALUES  (NEWID(), @requestId, 0);  
INSERT INTO [dbo].[PermissionRequests]
		([Id], [RequestId], [Operation])
		VALUES  (NEWID(), @requestId, 1);  

-- change permissions on existing relationship
SET @requestId = NEWID();

INSERT INTO [dbo].[Requests]
	([Id], [RequestType], [Ukprn] ,[RequestedBy], AccountLegalEntityId)
	VALUES (@requestId, 'Permission', @ukprn, 'me', 4004)
INSERT INTO [dbo].[PermissionRequests]
		([Id], [RequestId], [Operation])
		VALUES  (NEWID(), @requestId, 2); 

-- set permissions on existing relationship
SET @requestId = NEWID();

INSERT INTO [dbo].[Requests]
	([Id], [RequestType], [Ukprn] ,[RequestedBy], AccountLegalEntityId)
	VALUES (@requestId, 'AddAccount', @ukprn, 'me', 4006)
INSERT INTO [dbo].[PermissionRequests]
		([Id], [RequestId], [Operation])
		VALUES  (NEWID(), @requestId, 0); 
        

-- expired request on existing relationship
SET @requestId = NEWID();

INSERT INTO [dbo].[Requests]
	([Id], [RequestType], [Ukprn] ,[RequestedBy], AccountLegalEntityId, [Status])
	VALUES (@requestId, 'CreateAccount', @ukprn, 'me', 327511, 'Expired')    
INSERT INTO [dbo].[PermissionRequests]
		([Id], [RequestId], [Operation])
		VALUES  (NEWID(), @requestId, 2); 

-- Declined request 
SET @requestId = NEWID();

INSERT INTO [dbo].[Requests]
	([Id], [RequestType], [Ukprn] ,[RequestedBy] ,[EmployerOrganisationName] 
	,[EmployerContactFirstName] ,[EmployerContactLastName] ,
	[EmployerContactEmail] ,[EmployerPAYE] ,[EmployerAORN], [Status], ActionedBy)
VALUES (@requestId, 'CreateAccount', @ukprn, 'me', 'Third org ltd'
	,'FirstName','LastName'
	,'FirstName.LastName@Third.com'
	,'A094734889003'
	,'307/NL88003', 'Declined', 'EmployerUser@ThirdOrg.com')
INSERT INTO [dbo].[PermissionRequests]
		([Id], [RequestId], [Operation])
		VALUES  (NEWID(), @requestId, 0);
