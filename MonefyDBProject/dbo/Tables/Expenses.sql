CREATE TABLE [dbo].[Expenses] (
    [Id]          INT             IDENTITY (1, 1) NOT NULL,
    [Amount]      DECIMAL (18, 2) NOT NULL,
    [Comment]     NVARCHAR (MAX)  NULL,
    [Date]        DATETIME2 (7)   NOT NULL,
    [Description] NVARCHAR (MAX)  NULL,
    [IsDeleted]   BIT             NOT NULL,
    [UserId]      INT             NOT NULL,
    CONSTRAINT [PK_Expenses] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Expenses_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_Expenses_UserId]
    ON [dbo].[Expenses]([UserId] ASC);

