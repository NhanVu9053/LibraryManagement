USE [LibraryManagementDB]
GO
SET IDENTITY_INSERT [dbo].[Wiki] ON 

INSERT [dbo].[Wiki] ([Id], [TableId], [TableName], [StatusId], [StatusName], [IsDeleted]) VALUES (1, 1, N'Book', 1, N'Cho mượn', 0)
INSERT [dbo].[Wiki] ([Id], [TableId], [TableName], [StatusId], [StatusName], [IsDeleted]) VALUES (2, 1, N'Book', 2, N'Tạm hết', 0)
INSERT [dbo].[Wiki] ([Id], [TableId], [TableName], [StatusId], [StatusName], [IsDeleted]) VALUES (3, 1, N'Book', 3, N'Cập nhật', 0)
INSERT [dbo].[Wiki] ([Id], [TableId], [TableName], [StatusId], [StatusName], [IsDeleted]) VALUES (4, 1, N'Book', 4, N'Xóa', 0)
INSERT [dbo].[Wiki] ([Id], [TableId], [TableName], [StatusId], [StatusName], [IsDeleted]) VALUES (5, 2, N'Student', 1, N'Bình thường', 0)
INSERT [dbo].[Wiki] ([Id], [TableId], [TableName], [StatusId], [StatusName], [IsDeleted]) VALUES (6, 2, N'Student', 2, N'Đang mượn ', 0)
INSERT [dbo].[Wiki] ([Id], [TableId], [TableName], [StatusId], [StatusName], [IsDeleted]) VALUES (7, 2, N'Student', 3, N'Tạm khóa', 0)
INSERT [dbo].[Wiki] ([Id], [TableId], [TableName], [StatusId], [StatusName], [IsDeleted]) VALUES (8, 2, N'Student', 4, N'Xóa', 0)
INSERT [dbo].[Wiki] ([Id], [TableId], [TableName], [StatusId], [StatusName], [IsDeleted]) VALUES (9, 3, N'LoanCard', 1, N'Đang mượn', 0)
INSERT [dbo].[Wiki] ([Id], [TableId], [TableName], [StatusId], [StatusName], [IsDeleted]) VALUES (10, 3, N'LoanCard', 2, N'Gia hạn', 0)
INSERT [dbo].[Wiki] ([Id], [TableId], [TableName], [StatusId], [StatusName], [IsDeleted]) VALUES (11, 3, N'LoanCard', 3, N'Quá hạn', 0)
INSERT [dbo].[Wiki] ([Id], [TableId], [TableName], [StatusId], [StatusName], [IsDeleted]) VALUES (12, 3, N'LoanCard', 4, N'Hoàn thành', 0)
INSERT [dbo].[Wiki] ([Id], [TableId], [TableName], [StatusId], [StatusName], [IsDeleted]) VALUES (13, 3, N'LoanCard', 5, N'Xóa', 0)
INSERT [dbo].[Wiki] ([Id], [TableId], [TableName], [StatusId], [StatusName], [IsDeleted]) VALUES (14, 4, N'LoanCardBook', 1, N'Bình thường', 0)
INSERT [dbo].[Wiki] ([Id], [TableId], [TableName], [StatusId], [StatusName], [IsDeleted]) VALUES (15, 4, N'LoanCardBook', 4, N'Xóa', 0)
INSERT [dbo].[Wiki] ([Id], [TableId], [TableName], [StatusId], [StatusName], [IsDeleted]) VALUES (20, 6, N'Category', 1, N'Bình thường', 0)
INSERT [dbo].[Wiki] ([Id], [TableId], [TableName], [StatusId], [StatusName], [IsDeleted]) VALUES (21, 6, N'Category', 4, N'Xóa', 0)
INSERT [dbo].[Wiki] ([Id], [TableId], [TableName], [StatusId], [StatusName], [IsDeleted]) VALUES (22, 7, N'BookArchive', 1, N'Bình thường', 0)
INSERT [dbo].[Wiki] ([Id], [TableId], [TableName], [StatusId], [StatusName], [IsDeleted]) VALUES (23, 7, N'BookArchive', 4, N'Xóa', 0)
INSERT [dbo].[Wiki] ([Id], [TableId], [TableName], [StatusId], [StatusName], [IsDeleted]) VALUES (24, 8, N'User', 1, N'Bình thường', 0)
INSERT [dbo].[Wiki] ([Id], [TableId], [TableName], [StatusId], [StatusName], [IsDeleted]) VALUES (25, 8, N'User', 2, N'Khóa', 0)
INSERT [dbo].[Wiki] ([Id], [TableId], [TableName], [StatusId], [StatusName], [IsDeleted]) VALUES (26, 8, N'User', 4, N'Xóa', 0)
SET IDENTITY_INSERT [dbo].[Wiki] OFF
GO
