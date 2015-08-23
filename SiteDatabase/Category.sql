CREATE TABLE [dbo].[Category]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[CategoryName] NVARCHAR(100) NOT NULL UNIQUE,
	[UrlName] NVARCHAR(150),
	[ParentCategory] INT Constraint FK_Category Foreign Key ([ParentCategory]) References [Category]([Id]),
	[CategoryDesciption] NVARCHAR(500)
)
