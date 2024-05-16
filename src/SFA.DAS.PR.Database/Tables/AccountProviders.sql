CREATE TABLE [dbo].[AccountProviders]
(
    [Id] BIGINT NOT NULL IDENTITY,
    [AccountId] BIGINT NOT NULL,
    [ProviderUkprn] BIGINT NOT NULL,
    [Created] DATETIME2 NOT NULL,
    CONSTRAINT [PK_AccountProviders] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AccountProviders_Accounts_AccountId] FOREIGN KEY ([AccountId]) REFERENCES [Accounts] ([Id]),
    CONSTRAINT [FK_AccountProviders_Providers_ProviderUkprn] FOREIGN KEY ([ProviderUkprn]) REFERENCES [Providers] ([Ukprn])
);
GO 

CREATE UNIQUE INDEX [IXU_AccountProviders_ProviderUkprn_AccountId] 
ON [dbo].[AccountProviders] ([ProviderUkprn],[AccountId])
INCLUDE([Id]); 
GO

