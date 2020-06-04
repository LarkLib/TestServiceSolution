USE [TestDapper]
GO
/****** Object:  Table [dbo].[Customers]    Script Date: 06/04/2020 18:36:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customers](
	[CustomerID] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](max) NULL,
	[LastName] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Customers] PRIMARY KEY CLUSTERED 
(
	[CustomerID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Posts]    Script Date: 06/04/2020 18:36:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Posts](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](50) NULL,
	[Content] [nvarchar](255) NULL,
	[OwnerId] [int] NOT NULL
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 06/04/2020 18:36:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NULL
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[Customers] ON 

GO
INSERT [dbo].[Customers] ([CustomerID], [FirstName], [LastName], [Email]) VALUES (1, N'fa', N'la', N'a@test.com')
GO
INSERT [dbo].[Customers] ([CustomerID], [FirstName], [LastName], [Email]) VALUES (2, N'fb', N'lb', N'b@test.com')
GO
INSERT [dbo].[Customers] ([CustomerID], [FirstName], [LastName], [Email]) VALUES (3, N'fc', N'lc', N'c@test.com')
GO
INSERT [dbo].[Customers] ([CustomerID], [FirstName], [LastName], [Email]) VALUES (4, N'1', N'1', NULL)
GO
INSERT [dbo].[Customers] ([CustomerID], [FirstName], [LastName], [Email]) VALUES (5, N'2', N'2', NULL)
GO
INSERT [dbo].[Customers] ([CustomerID], [FirstName], [LastName], [Email]) VALUES (6, N'3', N'3', NULL)
GO

SET IDENTITY_INSERT [dbo].[Customers] OFF
GO
SET IDENTITY_INSERT [dbo].[Posts] ON 

GO
INSERT [dbo].[Posts] ([ID], [Title], [Content], [OwnerId]) VALUES (1, N't1', N'c1', 1)
GO
INSERT [dbo].[Posts] ([ID], [Title], [Content], [OwnerId]) VALUES (2, N't2', N'c2', 1)
GO
INSERT [dbo].[Posts] ([ID], [Title], [Content], [OwnerId]) VALUES (3, N't3', N'c3', 1)
GO
INSERT [dbo].[Posts] ([ID], [Title], [Content], [OwnerId]) VALUES (4, N't4', N'c4', 2)
GO
SET IDENTITY_INSERT [dbo].[Posts] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON 

GO
INSERT [dbo].[Users] ([ID], [Name]) VALUES (1, N'aaa')
GO
INSERT [dbo].[Users] ([ID], [Name]) VALUES (2, N'bbb')
GO
INSERT [dbo].[Users] ([ID], [Name]) VALUES (3, N'ccc')
GO
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
/****** Object:  StoredProcedure [dbo].[spGetUser]    Script Date: 06/04/2020 18:36:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[spGetUser](@id int)
as
select * from Users where ID=@id
GO
/****** Object:  StoredProcedure [dbo].[spMagicProc]    Script Date: 06/04/2020 18:36:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create proc [dbo].[spMagicProc](@a int,@b nvarchar(50) output)
as
set @b='Test output'
select * from Users where ID=@a
return 100
GO
