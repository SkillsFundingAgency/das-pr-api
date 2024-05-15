CREATE TABLE [dbo].[Permissions]
(
    [Id] BIGINT NOT NULL IDENTITY,
    [AccountProviderLegalEntityId] BIGINT NOT NULL,
    [Operation] SMALLINT NOT NULL,
    CONSTRAINT [PK_Permissions] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Permissions_AccountProviderLegalEntities_AccountProviderLegalEntityId] 
    FOREIGN KEY ([AccountProviderLegalEntityId]) 
    REFERENCES [AccountProviderLegalEntities] ([Id]) ON DELETE CASCADE
);
GO 

CREATE UNIQUE INDEX [IX_Permissions_AccountProviderLegalEntityId_Operation] 
ON [dbo].[Permissions] ([AccountProviderLegalEntityId] ASC,[Operation] ASC);
GO


