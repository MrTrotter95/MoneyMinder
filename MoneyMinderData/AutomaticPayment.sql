CREATE TABLE [dbo].[AutomaticPayment]
(
	[ID] INT IDENTITY (1, 1) NOT NULL,
	[FK_AccountID] INT NULL,
	[FK_FrequencyID] INT NULL,
	[FK_CategoryID] INT NULL,
	[Name] NVARCHAR (50) NOT NULL,
	[Amount] DECIMAL(8,2) NOT NULL,
	[StartDate] DATETIME NOT NULL,
	[EndDate] DATETIME NULL,
	PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT [FK_Automatic_Payment_AccountID] FOREIGN KEY ([FK_AccountID]) 
        REFERENCES [dbo].[Account] ([ID]) ON DELETE CASCADE,
	CONSTRAINT [FK_Automatic_Payment_FrequencyID] FOREIGN KEY ([FK_FrequencyID]) 
        REFERENCES [dbo].[PaymentFrequency] ([ID]) ON DELETE CASCADE,
	CONSTRAINT [FK_Automatic_Payment_CategoryID] FOREIGN KEY ([FK_CategoryID]) 
        REFERENCES [dbo].[TransactionCategory] ([ID]) ON DELETE CASCADE,
)
