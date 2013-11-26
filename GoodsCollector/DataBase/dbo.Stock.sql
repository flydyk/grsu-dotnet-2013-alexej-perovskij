CREATE TABLE [dbo].[Stock]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [GoodsID] INT NULL, 
    [Client] NVARCHAR(50) NULL, 
    [ManagerID] INT NULL,
	[Date] DATE NULL,
	[Cost] MONEY NULL	
)
