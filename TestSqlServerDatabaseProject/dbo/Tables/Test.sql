CREATE TABLE [dbo].[Test] (
    [Id]   INT           IDENTITY (1, 1) NOT NULL,
    [Name] NVARCHAR (50) NULL,
    CONSTRAINT [PK_Test] PRIMARY KEY CLUSTERED ([Id] ASC)
);

