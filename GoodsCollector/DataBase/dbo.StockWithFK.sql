CREATE TABLE [dbo].[Stock] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [GoodsID]   INT           NOT NULL,
    [Client]    NVARCHAR (50) NULL,
    [ManagerID] INT           NOT NULL,
    [Date]      DATE          NOT NULL,
    [Cost] MONEY NULL, 
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [GoodsId] FOREIGN KEY ([GoodsID]) REFERENCES [dbo].[Goods] ([Id]),
    CONSTRAINT [ManagerId] FOREIGN KEY ([ManagerID]) REFERENCES [dbo].[Managers] ([Id])
);

