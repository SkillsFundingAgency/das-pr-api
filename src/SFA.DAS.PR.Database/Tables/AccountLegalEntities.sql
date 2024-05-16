CREATE TABLE [dbo].[AccountLegalEntities]
(
    [Id] BIGINT NOT NULL,
    [PublicHashedId] NVARCHAR(6) NOT NULL, 
    [AccountId] BIGINT NOT NULL, 
    [Name] NVARCHAR(100) NOT NULL, 
    [Created] DATETIME2 NOT NULL,
    [Updated] DATETIME2 NULL,
    [Deleted] DATETIME2 NULL,
    CONSTRAINT [PK_AccountLegalEntities] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AccountLegalEntities_Accounts_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [dbo].[Accounts] ([Id])
)
GO

CREATE UNIQUE INDEX [IX_AccountLegalEntities_PublicHashedId]
ON [dbo].[AccountLegalEntities] ([PublicHashedId]) 
INCLUDE ([Id], [AccountId], [Name], [Deleted]);
GO




