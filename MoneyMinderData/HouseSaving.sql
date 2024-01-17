﻿CREATE TABLE [dbo].[HouseSaving]
(
	[ID] INT IDENTITY (1, 1) NOT NULL,
	[FK_AccountID] INT NULL,
	[Name] NVARCHAR (50) NOT NULL,
	[StartDate] DATETIME NOT NULL,
	[GoalDate] DATETIME NULL,
	[Amount] DECIMAL(8,2) NOT NULL,
	[GoalAmount] DECIMAL(8,2) NULL,
	PRIMARY KEY CLUSTERED ([ID] ASC), 
    CONSTRAINT [FK_HouseSaving_AccountID] FOREIGN KEY ([FK_AccountID]) 
		REFERENCES [dbo].[Account]([ID]) ON DELETE CASCADE,
)