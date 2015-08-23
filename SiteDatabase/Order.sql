CREATE TABLE [dbo].[Order]
(
	[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	[OrderNumber] VARCHAR(6) NULL UNIQUE,
	[CustomerId] INT CONSTRAINT FK_Order_Customer FOREIGN KEY ([CustomerId]) REFERENCES [Customer](Id),
	[CustomerName] NVARCHAR(50) NOT NULL,
	[ContactNumber] VARCHAR(20) NOT NULL,
	[EmailAddress] NVARCHAR(50),
	[OrderedBy] INT Constraint FK_User_Order Foreign Key ([OrderedBy]) References [User]([Id]),
	[OrderedDate] DATETIME NOT NULL DEFAULT GETDATE(),
	[Delivery] BIT NOT NULL DEFAULT 1,
	[ShipmentAddress] NVARCHAR(50),
	[ShipmentDistrict] NVARCHAR(20),
	[ShipmentDate] DATETIME NOT NULL DEFAULT GETDATE(),
	[ShipmentFee] Decimal NOT NULL Default 0,
	[Total] Decimal NOT NULL Default 0,
	[Note] NVARCHAR(100),
	[Status] bit DEFAULT 0
)
GO
CREATE TRIGGER [dbo].[Insert_Order_Trigger] ON [dbo].[Order]
AFTER INSERT
AS BEGIN
DECLARE @OrderNo AS varchar(6); 
DECLARE @Limit AS Int;
DECLARE @Id AS Int;
SET @Limit = 6; 
SET @OrderNo = (SELECT CONVERT(varchar(6), Id) FROM [INSERTED]);
SET @Id = (SELECT [Id] FROM [INSERTED]);
WHILE(@Limit > LEN(@OrderNo))
	BEGIN
		SET @OrderNo = '0' + @OrderNo
	END
UPDATE [dbo].[Order] SET [OrderNumber] = @OrderNo WHERE [dbo].[Order].[Id] =  @Id;
END
