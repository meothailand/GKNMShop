CREATE TABLE [dbo].[ItemPhoto]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[PhotoDescription] NVARCHAR(100),
	[PhotoFileName] NVARCHAR(200),
	[ItemId] INT CONSTRAINT FK_ItemPhoto_Item FOREIGN KEY REFERENCES [dbo].[Item]([Id])
)
