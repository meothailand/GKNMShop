CREATE TABLE [dbo].[Expense]
(
	[Id] INT IDENTITY NOT NULL PRIMARY KEY,
	[DateRecord] DATE NOT NULL,
	[Amount] DECIMAL NOT NULL,
	[ExpenseNote] nvarchar(200) NOT NULL
)
