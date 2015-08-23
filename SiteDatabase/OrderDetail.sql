CREATE TABLE [dbo].[OrderDetail]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[OrderId] INT NOT NULL CONSTRAINT FK_Order_OrderDetail Foreign Key ([OrderId]) References [Order]([Id]) ON DELETE CASCADE,
	[ItemId] INT NOT NULL CONSTRAINT FK_Item_OrderDetail Foreign Key ([ItemId]) References [Item]([Id]),
	[ToppingId] INT CONSTRAINT FK_Item Foreign Key ([ToppingId]) References [Orderdetail]([Id]), 
	[ItemName] NVARCHAR(200) NOT NULL,
	[Unit] NVARCHAR(20),
	[Quantity] INT NOT NULL DEFAULT 1,
	[Price] DECIMAL NOT NULL
)
