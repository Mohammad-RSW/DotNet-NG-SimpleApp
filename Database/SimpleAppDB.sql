
CREATE DATABASE SimpleAppDB
GO


USE SimpleAppDB
GO

/*
USE master
GO
DROP DATABASE SimpleAppDB
GO
*/

CREATE TABLE dbo.[User] (
	ID INT PRIMARY KEY IDENTITY,
	UserName VARCHAR(32) NOT NULL UNIQUE,
	Email VARCHAR(254) NOT NULL UNIQUE,
	[Password] BINARY(16) NOT NULL,
	FullName NVARCHAR(100) DEFAULT NULL,
	Avatar VARCHAR(256),
	CreatedAt DateTime DEFAULT GETDATE(),
	UpdatedAt DateTime DEFAULT NULL,
	LastLogin DateTime DEFAULT NULL,
	IsActive BIT DEFAULT 1,
	IsVerified BIT DEFAULT 0,
	IsAdmin BIT DEFAULT 0,
)

INSERT INTO dbo.[User] ([UserName], [Email], [Password], [FullName], [Avatar], [IsAdmin]) 
VALUES ('Admin', 'Admin@gmail.com', CONVERT(VARBINARY(16), '0xC4CA4238A0B923820DCC509A6F75849B99', 1), 'Mohammad', 'default_avatar.png', 1)
