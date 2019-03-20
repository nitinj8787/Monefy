CREATE TABLE [dbo].[Users] (
    [Id]        INT            IDENTITY (1, 1) NOT NULL,
    [FirstName] NVARCHAR (MAX) NULL,
    [IsDeleted] BIT            NOT NULL,
    [LastName]  NVARCHAR (MAX) NULL,
    [Password]  NVARCHAR (MAX) NULL,
    [Username]  NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC)
);

