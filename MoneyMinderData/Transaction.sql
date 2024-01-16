CREATE TABLE [dbo].[Transaction]
(
	[ID] INT IDENTITY (1, 1) NOT NULL,
	[FK_AccountID] INT NULL,
	[FK_TransactionTypeID] INT NULL,
	[TransactionDate] DATETIME NOT NULL,
	[Description] NVARCHAR(50) NULL,
	[Amount] DECIMAL(8,2) NOT NULL,
	[Balance] DECIMAL(8,2) NOT NULL,
	PRIMARY KEY CLUSTERED ([ID] ASC), 
    CONSTRAINT [FK_Transaction_Account] FOREIGN KEY ([FK_AccountID]) 
		REFERENCES [dbo].[Account] ([ID]) ON DELETE CASCADE,
	CONSTRAINT [FK_Transaction_TransactionType] FOREIGN KEY ([FK_TransactionTypeID]) 
		REFERENCES [dbo].[TransactionType] ([ID]) ON DELETE CASCADE,

)
