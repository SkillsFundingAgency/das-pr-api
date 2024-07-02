CREATE TABLE [dbo].[Requests]
(
    [Id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    [RequestType] VARCHAR(10) NOT NULL,
    [Ukprn] BIGINT NOT NULL,
    [ProviderUserFullName] NVARCHAR(255) NOT NULL,
    [RequestedBy] VARCHAR(255) NOT NULL,
    [RequestedDate] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [AccountLegalEntityId] BIGINT NULL,
    [EmployerUserRef] UNIQUEIDENTIFIER NULL,
    [EmployerOrganisationName] NVARCHAR(250) NULL,
    [EmployerContactFirstName] NVARCHAR(200) NULL,
    [EmployerContactLastName] NVARCHAR(200) NULL,
    [EmployerContactEmail] NVARCHAR(255) NULL,
    [EmployerPAYE] NVARCHAR(16) NULL,
    [EmployerAORN] VARCHAR(25) NULL,
    [Status] VARCHAR(10) DEFAULT 'new',
    [UpdatedDate] DATETIME2 NULL,
    CONSTRAINT [PK_Requests] PRIMARY KEY ([Id])
);
GO

ALTER TABLE [dbo].[Requests]  WITH CHECK ADD  CONSTRAINT [FK_Requests_AccountLegalEntities] FOREIGN KEY([AccountLegalEntityId])
REFERENCES [dbo].[AccountLegalEntities] ([Id])
GO

ALTER TABLE [dbo].[Requests] CHECK CONSTRAINT [FK_Requests_AccountLegalEntities]
GO

ALTER TABLE [dbo].[Requests]  WITH CHECK ADD  CONSTRAINT [FK_Requests_Providers] FOREIGN KEY([Ukprn])
REFERENCES [dbo].[Providers] ([Ukprn])
GO

ALTER TABLE [dbo].[Requests] CHECK CONSTRAINT [FK_Requests_Providers]
GO

CREATE INDEX [IX_Requests_Ukprn] ON [dbo].[Requests]([Ukprn], [Status], [AccountLegalEntityId])
INCLUDE ([Id], [RequestedDate]);
GO