CREATE TABLE [dbo].[Goods] (
    [Id]       INT           IDENTITY (1, 1) NOT NULL,
    [Name]     NVARCHAR (50) NULL,
    [Producer] NCHAR (10)    NULL,
    [Cost] MONEY NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

