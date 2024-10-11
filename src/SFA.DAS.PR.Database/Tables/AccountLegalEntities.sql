CREATE TABLE [dbo].[AccountLegalEntities]
(
    [Id] BIGINT NOT NULL,
    [PublicHashedId] CHAR(6) NOT NULL, 
    [AccountId] BIGINT NOT NULL, 
    [Name] NVARCHAR(100) NOT NULL, 
    [Created] DATETIME2 NOT NULL,
    [Updated] DATETIME2 NULL,
    [Deleted] DATETIME2 NULL,
    CONSTRAINT [PK_AccountLegalEntities] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AccountLegalEntities_Accounts_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [Accounts] ([Id]),
    INDEX [IX_AccountLegalEntities_AccountId] NONCLUSTERED ([AccountId] ASC)
)
GO

CREATE NONCLUSTERED INDEX [Idx_AccountLegalEntities_Hashedid] ON [dbo].[Accounts] ([HashedId])
GO

CREATE INDEX [IX_AccountLegalEntities] ON [dbo].[AccountLegalEntities] ([Id],[Deleted]) 
  INCLUDE ([AccountId], [PublicHashedId], [Name], [Created], [Updated]);
GO
