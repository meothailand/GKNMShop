CREATE TABLE [dbo].[Customer]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[CustomerName] NVARCHAR(50) NOT NULL,
	[CustomerEmail] NVARCHAR(50) NOT NULL UNIQUE,
	[CustomerPhone] VARCHAR(20) NOT NULL,
	[CustomerPassword] NVARCHAR(10) CONSTRAINT CK_PasswordLength CHECK (LEN([CustomerPassword]) >= 6),
	[ShipAddress] NVARCHAR(50),
	[ShipDistrict] NVARCHAR(20),
	[RecieveInfo] BIT NOT NULL DEFAULT 1,
	[CustomerPoint] INT NOT NULL DEFAULT 0
)
