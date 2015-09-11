CREATE TABLE [dbo].[User]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[UserName] NVARCHAR(50) COLLATE SQL_Latin1_General_CP1_CS_AS NOT NULL,
	[Email] NVARCHAR(50) COLLATE SQL_Latin1_General_CP1_CS_AS NOT NULL UNIQUE,
	[Password] NVARCHAR(20) COLLATE SQL_Latin1_General_CP1_CS_AS NOT NULL Constraint CK_Length CHECK (LEN([Password]) >= 8),
	[Role] NVARCHAR(10) NOT NULL DEFAULT 'Member' Constraint CK_Role CHECK ([Role] In ('Admin', 'Editor', 'Member')),
	[IsActive] bit NOT NULL DEFAULT 0
)
