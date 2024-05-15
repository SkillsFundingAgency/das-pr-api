CREATE TABLE [dbo].[Accounts]
(
    [Id] BIGINT NOT NULL,
    [HashedId] NVARCHAR(100) NOT NULL,
    [PublicHashedId] NVARCHAR(100) NOT NULL,
    [Name] NVARCHAR(100) NOT NULL,
    [Created] DATETIME2 NOT NULL,
    [Updated] DATETIME2 NULL,
    CONSTRAINT [PK_Accounts] PRIMARY KEY CLUSTERED ([Id] ASC)
);
GO

CREATE NONCLUSTERED INDEX [IX_Accounts_Hashedid]
ON [dbo].[Accounts] ([HashedId])
INCLUDE([Id],[Name]);
GO