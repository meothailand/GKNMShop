CREATE TABLE [dbo].[Banner]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[BannerLink] VARCHAR(300),
	[Caption1] NVARCHAR(50),
	[Caption2] NVARCHAR(50),
	[Caption3] NVARCHAR(50),
	[BannerImage] VARCHAR(100) NOT NULL,
	[Type] NVARCHAR(20) NOT NULL DEFAULT 'Big banner',
	[IsActive] bit NOT NULL DEFAULT 1
)
