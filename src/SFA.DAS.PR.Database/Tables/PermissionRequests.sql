CREATE TABLE [dbo].[PermissionRequests]
(
    [Id] UNIQUEIDENTIFIER DEFAULT NEWID(),
    [RequestId] UNIQUEIDENTIFIER NOT NULL,
    [Operation] SMALLINT NOT NULL,
    CONSTRAINT [PK_PermissionRequests] PRIMARY KEY ( [Id] ),
    CONSTRAINT [FK_PermissionRequests_RequestId] FOREIGN KEY (RequestId) REFERENCES [Requests] ([Id])
);
GO

CREATE INDEX [IX_PermissionRequests_RequestsId] ON [dbo].[PermissionRequests] (RequestId)
INCLUDE (Operation);
GO
