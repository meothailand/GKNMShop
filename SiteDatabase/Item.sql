CREATE TABLE [dbo].[Item]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[ItemName] nvarchar(200) NOT NULL,
	[UrlName] NVARCHAR(250),
	[ItemDescription] NVARCHAR(300),
	[CategoryId] INT NOT NULL Constraint FK_Category_Item Foreign Key ([CategoryId]) References [Category]([Id]) ON DELETE CASCADE,
	[Price] Decimal NOT NULL,
	[Unit] NVARCHAR(20) NOT NULL,
	[IsFeatured] BIT NOT NULL DEFAULT 0,
	[Image] nvarchar(100), 
	[IsTopping] BIT NOT NULL DEFAULT 0
)
