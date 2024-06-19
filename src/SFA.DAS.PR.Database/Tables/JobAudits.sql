CREATE TABLE [dbo].[JobAudits]
(
    [Id] BIGINT NOT NULL IDENTITY(1,1),
    [JobName] NVARCHAR(100) NOT NULL, 
    [JobInfo] NVARCHAR(MAX) NULL,
    [ExecutedOn] DATETIME2 NOT NULL DEFAULT GETUTCDATE(), 
    CONSTRAINT PK_JobAudits PRIMARY KEY ([Id]),
    INDEX IX_JobAudits_JobName_ExecutedOn ([JobName], [ExecutedOn])
)
