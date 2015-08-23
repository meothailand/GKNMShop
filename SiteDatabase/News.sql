CREATE TABLE [dbo].[News]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[Title] NVARCHAR(200) NOT NULL,
	[UrlTitle] NVARCHAR(250),
	[Content] NTEXT,
	[NewsAvatar] binary,
	[IsHotNew] BIT NOT NULL DEFAULT 0,
	[IsPublic] BIT NOT NULL DEFAULT 0,
	[DatePosted] DATETIME NOT NULL DEFAULT GETDATE(),
	[PostedBy] INT NOT NULL CONSTRAINT FK_User_News Foreign Key ([PostedBy]) References [User]([Id])
)
