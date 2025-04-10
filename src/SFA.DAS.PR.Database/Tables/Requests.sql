﻿CREATE TABLE [dbo].[Requests]
(
    [Id] UNIQUEIDENTIFIER NOT NULL DEFAULT NEWID(),
    [RequestType] VARCHAR(20) NOT NULL,
    [Ukprn] BIGINT NOT NULL,
    [RequestedBy] VARCHAR(255) NOT NULL,
    [RequestedDate] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [AccountLegalEntityId] BIGINT NULL,
    [EmployerOrganisationName] NVARCHAR(250) NULL,
    [EmployerContactFirstName] NVARCHAR(200) NULL,
    [EmployerContactLastName] NVARCHAR(200) NULL,
    [EmployerContactEmail] NVARCHAR(255) NULL,
    [EmployerPAYE] NVARCHAR(16) NULL,
    [EmployerAORN] VARCHAR(25) NULL,
    [Status] VARCHAR(10) NOT NULL DEFAULT 'New',
    [ActionedBy] varchar(255) null,
    [UpdatedDate] DATETIME2 NULL,
    CONSTRAINT [PK_Requests] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Requests_Ukprn] FOREIGN KEY (Ukprn) REFERENCES [Providers] ([Ukprn]),
    CONSTRAINT [FK_Requests_AccountLegalEntityId] FOREIGN KEY (AccountLegalEntityId) REFERENCES [AccountLegalEntities] ([Id])

);
GO

CREATE INDEX [IX_Requests_Ukprn] ON [dbo].[Requests]([Ukprn], [Status], [AccountLegalEntityId])
INCLUDE ([Id], [RequestedDate]);
GO

CREATE INDEX [IX_Requests_Ukprn_Email] ON [dbo].[Requests]([Ukprn], [Status], [EmployerContactEmail]);
GO

CREATE INDEX [IX_Requests_Ukprn_Paye] ON [dbo].[Requests]([Ukprn], [Status], [EmployerPAYE]);

GO