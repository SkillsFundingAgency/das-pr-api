CREATE TABLE [dbo].[PermissionsAudit]
(
    [Id] UNIQUEIDENTIFIER DEFAULT NEWID() CONSTRAINT [PK_PermissionsAudit] PRIMARY KEY,
    [Eventtime] DATETIME2 NOT NULL,
    [Action] VARCHAR(30) NOT NULL,
    [Ukprn] BIGINT NOT NULL, 
    [AccountLegalEntityId] BIGINT NOT NULL, 
    [EmployerUserRef] UNIQUEIDENTIFIER NULL,
    [Operations] VARCHAR(MAX) NOT NULL,
    CONSTRAINT [FK_PermissionsAudit_Ukprn] FOREIGN KEY (Ukprn) REFERENCES [Providers] ([Ukprn]),
    CONSTRAINT [FK_PermissionsAudit_AccountLegalEntityId] FOREIGN KEY (AccountLegalEntityId) REFERENCES [AccountLegalEntities] ([Id])
);
GO

CREATE INDEX [IX_PermissionsAudit_Ukprn_Eventtime] 
    ON [dbo].[PermissionsAudit] ([Ukprn], [Eventtime]) 
    INCLUDE ([Action], [AccountLegalEntityId], [Operations]);
GO