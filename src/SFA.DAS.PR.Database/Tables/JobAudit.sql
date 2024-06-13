CREATE TABLE [dbo].[JobAudit]
(
    [Id] BIGINT NOT NULL IDENTITY(1,1),
    [JobName] NVARCHAR(100) NOT NULL, 
    [JobInfo] NVARCHAR(MAX) NULL,
    [ExecutedOn] DATETIME2 NOT NULL DEFAULT GETUTCDATE(), 
    CONSTRAINT JobAudit_PK PRIMARY KEY ([Id]),
    INDEX JobAudit_IX ([JobName], [ExecutedOn])
)
