CREATE TABLE [dbo].[AccountProviderLegalEntities]
(
    [Id] BIGINT NOT NULL IDENTITY,
    [AccountProviderId] BIGINT NOT NULL,
    [AccountLegalEntityId] BIGINT NOT NULL,
    [Created] DATETIME2 NOT NULL,
    [Updated] DATETIME2 NULL,
    CONSTRAINT [PK_AccountProviderLegalEntities] PRIMARY KEY CLUSTERED ([Id]),
    CONSTRAINT [FK_AccountProviderLegalEntities_AccountProviders_AccountProviderId] 
        FOREIGN KEY ([AccountProviderId]) 
        REFERENCES [AccountProviders] ([Id]),
    CONSTRAINT [FK_AccountProviderLegalEntities_AccountLegalEntities_AccountLegalEntityId] 
        FOREIGN KEY ([AccountLegalEntityId]) 
        REFERENCES [AccountLegalEntities] ([Id]) 
        ON DELETE CASCADE
);
GO

CREATE UNIQUE INDEX [IX_AccountProviderLegalEntities_AccountProviderId_AccountLegalEntityId]
ON [dbo].[AccountProviderLegalEntities] ([AccountProviderId], [AccountLegalEntityId]) 
INCLUDE ([Id]);
GO

