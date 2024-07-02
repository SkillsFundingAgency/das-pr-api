CREATE TABLE [dbo].[PermissionRequests]
(
    [Id] uniqueidentifier default NEWID(),
    [RequestId] uniqueidentifier NOT NULL,
    [Operation] SMALLINT NOT NULL,
    CONSTRAINT [PK_PermissionRequests] PRIMARY KEY ( [Id] )
);
GO

CREATE INDEX [IX_PermissionRequests_RequestsId] ON [dbo].[PermissionRequests] (RequestId)
INCLUDE (Operation);
GO