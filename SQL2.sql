CREATE TABLE [dbo].[Users] (
    [UserID]   INT           IDENTITY (1, 1) NOT NULL,
    [FullName] NVARCHAR (50) NOT NULL,
    [Email]    NVARCHAR (50) NOT NULL,
    [Password] NVARCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([UserID] ASC),
    UNIQUE NONCLUSTERED ([FullName] ASC)
);



GO

CREATE TABLE [dbo].[Posts] (
    [PostID]      INT           IDENTITY (1, 1) NOT NULL,
    [UserID]      INT           NOT NULL,
    [Title]       NVARCHAR (100) NOT NULL,
    [Description] TEXT  NOT NULL,
    PRIMARY KEY CLUSTERED ([PostID] ASC),
    FOREIGN KEY ([UserID]) REFERENCES [dbo].[Users] ([UserID])
);



GO

CREATE TABLE [dbo].[Comments] (
    [CommentID]   INT  IDENTITY (1, 1) NOT NULL,
    [PostID]      INT  NOT NULL,
    [UserID]      INT  NOT NULL,
    [Description] TEXT NOT NULL,
    PRIMARY KEY CLUSTERED ([CommentID] ASC),
    FOREIGN KEY ([UserID]) REFERENCES [dbo].[Users] ([UserID]) ON DELETE CASCADE,
    FOREIGN KEY ([PostID]) REFERENCES [dbo].[Posts] ([PostID]) ON DELETE CASCADE,
);

GO