CREATE TABLE [dbo].[Notifications]
(
    [Id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    [TemplateName] NVARCHAR(200) NOT NULL,
    [NotificationType] VARCHAR(10) NOT NULL,
    [Ukprn] BIGINT,
    [EmailAddress] NVARCHAR(255) NULL,
    [Contact] NVARCHAR(400) NULL,
    [RequestId] UNIQUEIDENTIFIER NULL,
    [AccountLegalEntityId] BIGINT NULL,
    [PermitApprovals] SMALLINT NULL,
    [PermitRecruit] SMALLINT NULL,
    [CreatedBy] VARCHAR(255) NOT NULL,
    [CreatedDate] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [SentTime] DATETIME2 NULL,
    CONSTRAINT [PK_Notifications] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Notifications_Ukprn] FOREIGN KEY (Ukprn) REFERENCES [Providers] ([Ukprn]),
    CONSTRAINT [FK_Notifications_AccountLegalEntityId] FOREIGN KEY (AccountLegalEntityId) REFERENCES [AccountLegalEntities] ([Id])

);
GO

CREATE INDEX [IX_Notifications] ON [dbo].[Notifications]([TemplateName], [CreatedDate], [SentTime]);
GO