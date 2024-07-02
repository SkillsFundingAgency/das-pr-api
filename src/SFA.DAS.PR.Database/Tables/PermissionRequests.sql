CREATE TABLE [dbo].[PermissionRequests]
(
    [Id] uniqueidentifier default NEWID(),
    [RequestId] uniqueidentifier NOT NULL,
    [Operation] SMALLINT NOT NULL,
    CONSTRAINT [PK_PermissionRequests] PRIMARY KEY ( [Id] ),
);
GO

ALTER TABLE [dbo].[PermissionRequests]  WITH CHECK ADD  CONSTRAINT [FK_PermissionRequests_Requests] FOREIGN KEY([RequestId])
REFERENCES [dbo].[Requests] ([Id])
GO

ALTER TABLE [dbo].[PermissionRequests] CHECK CONSTRAINT [FK_PermissionRequests_Requests]
GO

CREATE INDEX [IX_PermissionRequests_RequestsId] ON [dbo].[PermissionRequests] (RequestId)
INCLUDE (Operation);
GO
