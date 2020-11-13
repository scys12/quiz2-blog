CREATE TABLE [dbo].[Users]
(
	[UserID] INT NOT NULL PRIMARY KEY,
	[FullName] nvarchar (50) NOT NULL,
	[Email] nvarchar (50) NOT NULL,
	[Password] nvarchar (50) NOT NULL,

);

GO

CREATE TABLE [dbo].[Posts]
(
	[PostID] INT NOT NULL PRIMARY KEY,
	[UserID] INT NOT NULL FOREIGN KEY REFERENCES Users(UserID),
	[Title] nvarchar (50) NOT NULL,
	[Description] nvarchar (50) NOT NULL,
)

GO

CREATE TABLE [dbo].[Comments]
(
	[CommentID] INT NOT NULL PRIMARY KEY,
	[PostID] INT NOT NULL FOREIGN KEY REFERENCES Posts(PostID),
	[UserID] INT NOT NULL FOREIGN KEY REFERENCES Users(UserID),
	[Title] nvarchar (50) NOT NULL,
	[Description] nvarchar (50) NOT NULL,
)

GO