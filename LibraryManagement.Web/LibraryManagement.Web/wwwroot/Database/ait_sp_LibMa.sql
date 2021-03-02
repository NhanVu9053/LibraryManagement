USE [LibraryManagementDB]
GO
/****** Object:  StoredProcedure [dbo].[sp_ChangeStatusBook]    Script Date: 3/2/2021 7:23:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		 Tùng Nguyễn
-- Create date: 19/11/2020
-- Description:	Change status Book
-- Status: 1: stochking, 2: over, 3: pending, 4: deleted
-- =============================================
CREATE PROCEDURE [dbo].[sp_ChangeStatusBook] 
	@BookId INT,
	@StatusId INT,
	@ModifiedBy NVARCHAR(100)
AS
BEGIN
	DECLARE @Message NVARCHAR(100) = N'Thao tác thay đổi Status không thành công, vui lòng thử lại sau'
	IF(EXISTS(SELECT BookId FROM Book WHERE BookId = @BookId))
	BEGIN
		DECLARE @BookName NVARCHAR(200) = (SELECT TOP(1) BookName FROM Book WHERE BookId = @BookId AND StatusId <> 4)
		IF(EXISTS(SELECT StatusId FROM Wiki WHERE TableId = 1 AND StatusId = @StatusId))
		BEGIN
			IF(ISNULL(@ModifiedBy,'0') <> '0' AND @ModifiedBy <> '')
			BEGIN
				IF(@StatusId = 4 AND (NOT EXISTS(SELECT BookId FROM BookArchive WHERE BookId = @BookId AND Quantity = 0)))
				BEGIN
					SET @Message = N'Không thể xóa sách vì Sách vẫn còn trong kho'
					SET @BookId = 0
				END
				ELSE
				BEGIN
					IF(@StatusId = 4 AND (EXISTS(SELECT LC.LoanCardId FROM LoanCardBook AS LCB INNER JOIN Book AS B ON LCB.BookId = B.BookId
																				INNER JOIN LoanCard AS LC ON LCB.LoanCardId = LC.LoanCardId
																				WHERE B.BookId = @BookId AND LC.StatusId <> 5)))
					BEGIN
						SET @Message = N'Không thể xóa sách vì Sách vẫn còn lưu trong các thẻ mượn'
						SET @BookId = 0
					END
					ELSE
					BEGIN
						DECLARE @StatusName NVARCHAR(50) = (SELECT TOP(1) StatusName FROM Wiki WHERE TableId = 1 AND StatusId = @StatusId)
						UPDATE [dbo].[Book]
						   SET [StatusId] = @StatusId
							  ,[ModifiedDate] = GETDATE()
							  ,[ModifiedBy] = @ModifiedBy
							  ,@BookName = [BookName]
						 WHERE BookId = @BookId	
						 SET @Message = N'Thay đổi trạng thái sách: ' + @BookName + ' qua ' + @StatusName + ' thành công'
					END
				END	
			END
			ELSE
			BEGIN
				SET @Message = N'Mã người cập nhật Sách không được để trống'
				SET @BookId = 0
			END
		END
		ELSE
		BEGIN
			SET @Message = N'Trạng thái không tồn tại'
			SET @BookId = 0
		END
	END
	ELSE
	BEGIN
		SET @Message = N'Mã sách không tồn tại'
		SET @BookId = 0
	END
	SELECT @Message AS [Message], @BookId AS BookId
END
GO
/****** Object:  StoredProcedure [dbo].[sp_ChangeStatusLoanCard]    Script Date: 3/2/2021 7:23:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		 Tùng Nguyễn
-- Create date: 24/11/2020
-- Description:	Change status LoanCard
-- Status: 1: loanting, 2: extend, 3: overdue, 4: completed, 5: deleted
-- =============================================
CREATE PROCEDURE [dbo].[sp_ChangeStatusLoanCard]  
	@LoanCardId INT,
	@StatusId INT,
	@ModifiedBy NVARCHAR(100)
AS
BEGIN
	DECLARE @Message NVARCHAR(100) = N'Thao tác thay đổi trạng thái không thành công, vui lòng thử lại sau'
	DECLARE @StudentId INT = (SELECT TOP(1) StudentId FROM LoanCard WHERE LoanCardId = @LoanCardId)
	IF(EXISTS(SELECT LoanCardId FROM LoanCard WHERE LoanCardId = @LoanCardId AND StatusId <> 5))
	BEGIN
		IF(EXISTS(SELECT StatusId FROM Wiki WHERE TableId = 3 AND StatusId = @StatusId AND IsDeleted = 0))
		BEGIN
			IF(ISNULL(@ModifiedBy,'0') <> '0' AND @ModifiedBy <> '')
			BEGIN
				IF(@StatusId = 5 AND (NOT EXISTS(SELECT * FROM LoanCard WHERE LoanCardId = @LoanCardId AND StatusId = 4)))
				BEGIN
					SET @Message = N'Không thể xóa Thẻ mượn khi nó chưa có trạng thái Hoàn thành'
					SET @LoanCardId = 0
				END
				ELSE
				BEGIN
					DECLARE @StatusName NVARCHAR(50) = (SELECT TOP(1) StatusName FROM Wiki WHERE TableId = 3 AND StatusId = @StatusId)
						UPDATE [dbo].[LoanCard]
						   SET [StatusId] = @StatusId
							  ,[ModifiedDate] = GETDATE()
							  ,[ModifiedBy] = @ModifiedBy
						 WHERE LoanCardId = @LoanCardId	
						 IF(@StatusId = 4)
						 BEGIN
							UPDATE [dbo].[Student]
							   SET [StatusId] = 1
							 WHERE StudentId = @StudentId
						 END
						 SET @Message = N'Thay đổi trạng thái Thẻ mượn có ID: ' + CONVERT(NVARCHAR, @LoanCardId) + ' qua ' + @StatusName + ' thành công'
				END	
			END
			ELSE
			BEGIN
				SET @Message = N'Mã người cập nhật Thẻ mượn không được để trống'
				SET @LoanCardId = 0
			END
		END
		ELSE
		BEGIN
			SET @Message = N'Trạng thái chuyển không tồn tại'
			SET @LoanCardId = 0
		END
	END
	ELSE
	BEGIN
		SET @Message = N'Mã thẻ mượn không tồn tại'
		SET @LoanCardId = 0
	END

	SELECT @Message AS [Message], @LoanCardId AS LoanCardId
END





GO
/****** Object:  StoredProcedure [dbo].[sp_ChangeStatusStudent]    Script Date: 3/2/2021 7:23:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		 Tùng Nguyễn
-- Create date: 23/11/2020
-- Description:	Change status Student
-- Status: 1: studying, 2: postponed, 3: blocked, 4: deleted
-- =============================================
CREATE PROCEDURE [dbo].[sp_ChangeStatusStudent] 
	@StudentId INT,
	@StatusId INT,
	@ModifiedBy NVARCHAR(100)
AS
BEGIN
	DECLARE @Message NVARCHAR(100) = N'Thao tác thay đổi Status không thành công, vui lòng thử lại sau'
	IF(EXISTS(SELECT StudentId FROM Student WHERE StudentId = @StudentId))
	BEGIN
		IF(EXISTS(SELECT StatusId FROM Wiki WHERE TableId = 2 AND StatusId = @StatusId AND IsDeleted = 0))
		BEGIN
			IF(ISNULL(@ModifiedBy,'0') <> '0' AND @ModifiedBy <> '')
			BEGIN
				IF(@StatusId = 4 AND (NOT EXISTS(SELECT StudentId FROM Student WHERE StudentId = @StudentId AND StatusId <> 2)))
				BEGIN
					SET @Message = N'Học sinh đang mượn sách nên không thể xóa được'
					SET @StudentId = 0
				END
				ELSE
				BEGIN
					IF(@StatusId = 4 AND (EXISTS(SELECT StudentId FROM LoanCard WHERE StudentId = @StudentId AND StatusId <> 5)))
					BEGIN
						SET @Message = N'Học sinh vẫn còn được lưu thông tin trong các Thẻ mượn nên không thể xóa'
						SET @StudentId = 0
					END
					ELSE
					BEGIN
						DECLARE @StatusName NVARCHAR(50) = (SELECT TOP(1) StatusName FROM Wiki WHERE TableId = 2 AND StatusId = @StatusId)
						UPDATE [dbo].[Student]
						   SET [StatusId] = @StatusId
							  ,[ModifiedDate] = GETDATE()
							  ,[ModifiedBy] = @ModifiedBy
						 WHERE StudentId = @StudentId	
						 SET @Message = N'Thay đổi trạng thái học sinh có ID: ' + CONVERT(NVARCHAR, @StudentId) + ' qua ' + @StatusName + ' thành công'
					END
				END
			END
			ELSE
			BEGIN
				SET @Message = N'Mã người cập nhật học sinh không được để trống'
				SET @StudentId = 0
			END
		END
		ELSE
		BEGIN
			SET @Message = N'Trạng thái thay đổi không tồn tại'
			SET @StudentId = 0
		END
	END
	ELSE
	BEGIN
		SET @Message = N'Mã học sinh không tồn tại'
		SET @StudentId = 0
	END

	SELECT @Message AS [Message], @StudentId AS StudentId
END
GO
/****** Object:  StoredProcedure [dbo].[sp_ChangeStatusUser]    Script Date: 3/2/2021 7:23:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		 Tùng Nguyễn
-- Create date: 12/12/2020
-- Description:	Change status user
-- =============================================
CREATE PROCEDURE [dbo].[sp_ChangeStatusUser]
	@UserId NVARCHAR(450),
	@StatusId INT,
	@ModifiedBy NVARCHAR(100)
AS
BEGIN
	DECLARE @Message NVARCHAR(100) = N'Thao tác thay đổi trạng thái không thành công, vui lòng thử lại sau'
	DECLARE @Check INT = 0
	IF(EXISTS(SELECT Id FROM [dbo].[AspNetUsers] WHERE Id = @UserId AND StatusId <> 4))
	BEGIN
		IF(EXISTS(SELECT StatusId FROM Wiki WHERE TableId = 8 AND @StatusId = StatusId AND IsDeleted = 0))
		BEGIN
			IF(EXISTS(SELECT Id FROM [dbo].[AspNetUsers] WHERE Id = @ModifiedBy AND StatusId = 1))
			BEGIN
				IF(@StatusId = 4 AND (EXISTS(SELECT * FROM LoanCard WHERE CreatedBy = @UserId OR ModifiedBy = @UserId AND StatusId <> 5)))
				BEGIN
					SET @Message = N'Thông tin tài khoản vẫn còn được lưu thông tin trong các thẻ mượn nên không thể xóa'
				END
				ELSE
				BEGIN
					DECLARE @StatusName NVARCHAR(50) = (SELECT TOP(1) StatusName FROM Wiki WHERE TableId = 8 AND StatusId = @StatusId)
					DECLARE @Email NVARCHAR(256) = (SELECT TOP(1) Email FROM [dbo].[AspNetUsers] WHERE Id = @UserId)
					UPDATE [dbo].[AspNetUsers]
					   SET [ModifiedBy] = @ModifiedBy
						  ,[ModifiedDate] = GETDATE()
						  ,[StatusId] = @StatusId
					 WHERE Id = @UserId
					 SET @Message = N'Thay đổi trạng thái tài khoản: ' + @Email + ' qua ' + @StatusName + ' thành công'
					 SET @Check = 1
				END
			END
			ELSE
			BEGIN
				SET @Message = N'Tài khoản người cập nhật không hợp lệ'
			END
		END
		ELSE
		BEGIN
			SET @Message = N'Trạng thái muốn thay đổi không tồn tại'
		END
	END
	ELSE
	BEGIN
		SET @Message = N'Tài khoản không tồn tại hoặc đã bị xóa'
	END
	IF(@Check = 0)
	BEGIN
		SET @UserId = NULL
	END
	SELECT @Message AS [Message] , @UserId AS UserId
END
GO
/****** Object:  StoredProcedure [dbo].[sp_CheckAndChangeOverToStochking]    Script Date: 3/2/2021 7:23:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Tùng Nguyễn
-- Create date: 08/12/2020
-- Description:	Check and change status Over to Stochking from Book table
-- =============================================
CREATE PROCEDURE [dbo].[sp_CheckAndChangeOverToStochking] 
	@BookId INT
AS
BEGIN
	DECLARE @QuantityRemain INT = (SELECT TOP(1) QuantityRemain  FROM Book AS B INNER JOIN BookArchive AS BA ON B.BookId = BA.BookId
																WHERE B.BookId = @BookId)
	DECLARE @StatusId INT = (SELECT TOP(1) StatusId FROM Book WHERE @BookId = @BookId)
	IF(@QuantityRemain > 0 AND @StatusId = 2)
	BEGIN
		UPDATE [dbo].[Book]
		   SET [StatusId] = 1
		 WHERE BookId = @BookId
	END
END
GO
/****** Object:  StoredProcedure [dbo].[sp_CheckAndChangeStatusOverdueLoanCard]    Script Date: 3/2/2021 7:23:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		 Tùng Nguyễn
-- Create date: 21/12/2020
-- Description:	Check all Loan Card overdue and change status Overdue
-- =============================================
CREATE PROCEDURE [dbo].[sp_CheckAndChangeStatusOverdueLoanCard]  
AS
BEGIN
	DECLARE @tbLoanCardId TABLE(LoanCardId INT)
	DECLARE @LoanCardId INT
	INSERT INTO @tbLoanCardId 
	SELECT LoanCardId FROM LoanCard AS LC
	WHERE DATEDIFF(DAY, CONVERT(DATE,LC.ReturnOfDate), GETDATE()) > 0 AND StatusId IN (1,2,3)

	WHILE(EXISTS(SELECT * FROM @tbLoanCardId))
	BEGIN
		SET @LoanCardId = (SELECT TOP(1) LoanCardId FROM @tbLoanCardId)
		UPDATE [dbo].[LoanCard]
			SET [StatusId] = 3
			WHERE LoanCardId = @LoanCardId
		DELETE FROM @tbLoanCardId WHERE LoanCardId = @LoanCardId
	END
END
GO
/****** Object:  StoredProcedure [dbo].[sp_CheckSaveRole]    Script Date: 3/2/2021 7:23:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		 Tùng Nguyễn
-- Create date: 11/12/2020
-- Description:	Check validity request role
-- =============================================
CREATE PROCEDURE [dbo].[sp_CheckSaveRole] 
	@RoleId NVARCHAR(450),
	@RoleName NVARCHAR(256)
AS
BEGIN
	DECLARE @Message NVARCHAR(100) = N'Đã xảy ra lỗi, xin vui lòng liên hệ Admin'
		IF(ISNULL(@RoleId,'0') = '0' OR @RoleId = '')
		BEGIN
			--CHECK CREATE ROLE
			IF(NOT EXISTS(SELECT [Name] FROM [dbo].[AspNetRoles]
				WHERE LOWER(RTRIM(LTRIM([Name]))) = LOWER(RTRIM(LTRIM(@RoleName)))))
			BEGIN
				SET @Message = N'Thao tác tạo mới Role có thể bắt đầu'
			END
			ELSE
			BEGIN
				SET @Message = N'Tên Role đã tồn tại'
				SET @RoleName = NULL
			END
		END
		ELSE
		BEGIN
			--CHECK UPDATE ROLE
			IF(EXISTS(SELECT Id FROM [dbo].[AspNetRoles] WHERE Id = @RoleId))
			BEGIN
				IF(NOT EXISTS(SELECT Id FROM [dbo].[AspNetRoles]
					WHERE Id != @RoleId 
					AND LOWER(RTRIM(LTRIM(Name))) = LOWER(RTRIM(LTRIM(@RoleName)))))
					BEGIN
						SET @Message = N'Thao tác cập nhật Role có thể bắt đầu'
					END
					ELSE
					BEGIN
						SET @Message = N'Tên Role đã tồn tại'
						SET @RoleName = NULL
					END
			END
			ELSE
			BEGIN
				SET @Message = N'Mã ID Role không tồn tại'
				SET @RoleName = NULL
			END
		END

	SELECT @Message AS [Message], @RoleName AS RoleName
END
GO
/****** Object:  StoredProcedure [dbo].[sp_CheckSaveUser]    Script Date: 3/2/2021 7:23:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		 Tùng Nguyễn
-- Create date: 11/12/2020
-- Description:	Check validity request user
-- =============================================
CREATE PROCEDURE [dbo].[sp_CheckSaveUser]  
	@UserId NVARCHAR(450),
	@Email NVARCHAR(256),
	@FullName NVARCHAR(100),
	@PhoneNumber NVARCHAR,
	@Dob DATE,
	@HireDate DATE,
	@ProvinceId INT,
	@DistrictId INT,
	@WardId INT,
	@Address NVARCHAR(200),
	@AvatarPath NVARCHAR(200),
	@ModifiedBy NVARCHAR(100),
	@RoleId NVARCHAR(450)
AS
BEGIN
	DECLARE @Message NVARCHAR(100) = N'Đã xảy ra lỗi, xin vui lòng liên hệ Admin'
	DECLARE @Check INT = 0
	IF(ISNULL(@Email,'0') <> '0' OR @Email <> '')
	BEGIN
		IF(ISNULL(@FullName,'0') <> '0' OR @FullName <> '')
		BEGIN
			IF(ISNULL(@PhoneNumber,'0') <> '0' OR @PhoneNumber <> '')
			BEGIN
				IF(DATEDIFF(DAY, CONVERT(DATE,@HireDate), GETDATE()) >= 0)
				BEGIN
					IF(DATEDIFF(YEAR, CONVERT(DATE,@Dob), GETDATE()) >= 18)
					BEGIN
						IF(@ProvinceId > 0)
						BEGIN
							IF(@DistrictId > 0)
							BEGIN
								IF(@WardId > 0)
								BEGIN
									IF(ISNULL(@Address,'0') <> '0' OR @Address <> '')
									BEGIN
										IF(ISNULL(@AvatarPath,'0') <> '0' OR @AvatarPath <> '')
										BEGIN
											IF(EXISTS(SELECT * FROM [dbo].[AspNetRoles] WHERE Id = @RoleId))
											BEGIN
												IF(ISNULL(@UserId,'0') = '0' OR @UserId = '')
												BEGIN
												--CHECK CREATE USER
													IF(NOT EXISTS(SELECT Id FROM [dbo].[AspNetUsers] WHERE Email = @Email))
													BEGIN
														IF(ISNULL(@ModifiedBy,'0') <> '0' OR @ModifiedBy <> '')
														BEGIN
															SET @Message = N'Thao tác tạo mới Tài khoản có thể bắt đầu'
															SET @Check = 1
														END
														ELSE
														BEGIN
															SET @Message = N'Mã ID người tạo không được để trống'
														END
													END
													ELSE
													BEGIN
														SET @Message = N'Email đã tồn tại'
													END
												END
												ELSE
												--CHECK UPDATE USER
												BEGIN
													IF(EXISTS(SELECT Id FROM [dbo].[AspNetUsers] WHERE Id = @UserId))
													BEGIN
														IF(NOT EXISTS(SELECT Id FROM [dbo].[AspNetUsers] WHERE Email = @Email AND Id <> @UserId))
														BEGIN
															IF(ISNULL(@ModifiedBy,'0') <> '0' OR @ModifiedBy <> '')
															BEGIN
																SET @Message = N'Thao tác cập nhật Tài khoản có thể bắt đầu'
																SET @Check = 1
															END
															ELSE
															BEGIN
																SET @Message = N'Mã ID người sửa không được để trống'
															END
														END
														ELSE
														BEGIN
															SET @Message = N'Email đã tồn tại'
														END
													END
													ELSE
													BEGIN
														SET @Message = N'Mã ID của tài khoản không tồn tại'
													END
												END
											END
											ELSE
											BEGIN
												SET @Message = N'Mã ID Role không tồn tại'
											END
										END
										ELSE
										BEGIN
											SET @Message = N'File ảnh đại diện không được để trống'
										END
									END
									ELSE
									BEGIN
										SET @Message = N'Thông tin Địa chỉ không được để trống'
									END
								END
								ELSE
								BEGIN
									SET @Message = N'Thông tin Phường/xã không hợp lệ'
								END
							END
							ELSE
							BEGIN
								SET @Message = N'Thông tin Quận/huyện không hợp lệ'
							END
						END
						ELSE
						BEGIN
							SET @Message = N'Thông tin Tỉnh/thành không hợp lệ'
						END
						END
					ELSE
					BEGIN
						SET @Message = N'Ngày sinh không hợp lệ (số tuổi phải lớn hơn bằng 18 )'
					END
				END
				ELSE
				BEGIN
					SET @Message = N'Ngày bắt đầu làm việc không được lớn hơn hiện tại'
				END
			END
			ELSE
			BEGIN
				SET @Message = N'Số điện thoại không được để trống'
			END
		END
		ELSE
		BEGIN
			SET @Message = N'Họ và tên không được để trống'
		END
	END
	ELSE
	BEGIN
		SET @Message = N'Email không được để trống'
	END

	IF(@Check = 0)
	BEGIN
		SET @Email = NULL
	END

	SELECT @Message AS [Message], @Email AS Email
END
GO
/****** Object:  StoredProcedure [dbo].[sp_CheckStatusBookIsOver]    Script Date: 3/2/2021 7:23:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		 Tùng Nguyễn
-- Create date: 19/11/2020
-- Description:	Check Status Book is Over (StatusId = 4)
-- =============================================
CREATE PROCEDURE [dbo].[sp_CheckStatusBookIsOver]
	@BookId INT
AS
BEGIN
	DECLARE @QuantityRemain INT = (SELECT TOP(1) QuantityRemain  FROM Book AS B INNER JOIN BookArchive AS BA ON B.BookId = BA.BookId
																WHERE B.BookId = @BookId)
	IF(@QuantityRemain = 0)
	BEGIN
		UPDATE [dbo].[Book]
		   SET [StatusId] = 2
		 WHERE BookId = @BookId
	END
END
GO
/****** Object:  StoredProcedure [dbo].[sp_CheckStatusUserIsActive]    Script Date: 3/2/2021 7:23:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		 Tùng Nguyễn
-- Create date: 12/12/2020
-- Description:	Check status user is active
-- =============================================
CREATE PROCEDURE [dbo].[sp_CheckStatusUserIsActive]   
	@Email NVARCHAR(256)
AS
BEGIN
	DECLARE @Message NVARCHAR(100) = N'Đã xảy ra lỗi, xin vui lòng liên hệ Admin'
	IF(EXISTS(SELECT Id FROM [dbo].[AspNetUsers] WHERE Email = @Email))
	BEGIN
		IF(EXISTS(SELECT Id FROM [dbo].[AspNetUsers] WHERE Email = @Email AND StatusId = 1))
		BEGIN
			SET @Message = N'Tài khoản đang ở trạng thái hoạt động bình thường'
		END
		ELSE
		BEGIN
			SET @Message = N'Tài khoản đã bị vô hiệu'
			SET @Email = NULL
		END
	END
	ELSE
	BEGIN
		SET @Message = N'Tài khoản không tồn tại'
		SET @Email = NULL
	END
		
	SELECT @Message AS [Message], @Email AS Email
END
GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteBookArchive]    Script Date: 3/2/2021 7:23:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_DeleteBookArchive]
	@BookArchiveId INT,
	@ModifiedBy NVARCHAR(100)
AS
BEGIN
	DECLARE @Message NVARCHAR(200) = N'Thao tác thay đổi trạng thái không thành công, vui lòng thử lại sau'
	DECLARE @BookId INT = (SELECT TOP(1) BookId FROM BookArchive WHERE BookArchiveId = @BookArchiveId)
	IF(NOT EXISTS(SELECT BookArchiveId FROM [dbo].[BookArchive] WHERE BookArchiveId = @BookArchiveId))
	BEGIN
		SET @Message = N'Mã ID danh mục kho không tồn tại'	
		SET @BookArchiveId = 0
	END
	ELSE 
	BEGIN
			IF(EXISTS(SELECT COUNT(Quantity)FROM BookArchive  WHERE Quantity > 0 ))
			BEGIN
				SET @Message =N'Không thể xóa Kho sách này vì trong kho còn sách'
				SET @BookArchiveId = 0
			END
			ELSE
			BEGIN
				IF(NOT EXISTS(SELECT BookId FROM Book WHERE BookId = @BookId AND StatusId = 4))
				BEGIN
					SET @Message =N'Không thể xóa Kho sách này vì Sách vẫn còn tồn tại'
					SET @BookArchiveId = 0
				END
				ELSE
				BEGIN
					UPDATE [dbo].[BookArchive]
					   SET [StatusId] = 4    
						  ,[ModifiedDate] = GETDATE()
						  ,[ModifiedBy] = @ModifiedBy
					 WHERE BookArchiveId  = @BookArchiveId
		 
					DECLARE @BookName NVARCHAR(100) = (SELECT TOP(1) B.BookName FROM Book AS B INNER JOIN BookArchive AS BA
																ON B.BookId = BA.BookId
																WHERE BA.BookArchiveId = @BookArchiveId)
					SET @Message = N'Sách: ' + @BookName + 'đã xóa thành công trong Kho sách'
				END
			END
	END
	SELECT @BookArchiveId AS BookArchiveId, @Message AS [Message]
END
GO
/****** Object:  StoredProcedure [dbo].[sp_DeleteCategory]    Script Date: 3/2/2021 7:23:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_DeleteCategory]
	@CategoryId INT,
	@ModifiedBy NVARCHAR(100)
AS
BEGIN
	DECLARE @Message NVARCHAR(200) = N'Thao tác không thành công, vui lòng thử lại sau'
	IF EXISTS
	(
		SELECT BookId
		FROM Book
		WHERE CategoryId = @CategoryId AND StatusId <> 4
	)
	BEGIN
		SET @Message = N'Danh mục không thể xóa do có Sách đang sử dụng danh mục này'
		SET @CategoryId = 0
	END
	ELSE
	BEGIN
		IF NOT EXISTS
			(
				SELECT CategoryId FROM Category WHERE CategoryId = @CategoryId AND StatusId <> 4
			)
		BEGIN
			SET @Message = N'Mã ID Thể loại sách không tìm thấy'
			SET @CategoryId = 0
		END
		ELSE
		BEGIN
			DECLARE @CategoryName NVARCHAR(200)= (SELECT TOP(1) CategoryName FROM Category WHERE CategoryId = @CategoryId)
			UPDATE Category
			   SET StatusId = 4    
				  ,ModifiedDate = GETDATE()
				  ,ModifiedBy = @ModifiedBy
			 WHERE CategoryId  = @CategoryId
			 SET @Message = N'Thể loại sách ' + @CategoryName + N' đã được xóa'
		END
	END

	SELECT @CategoryId AS CategoryId, @Message AS [Message]
END

GO
/****** Object:  StoredProcedure [dbo].[sp_ExtendLoanCard]    Script Date: 3/2/2021 7:23:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		 Tùng Nguyễn
-- Create date: 03/12/2020
-- Description:	Extend Loan Card
-- =============================================
CREATE PROCEDURE [dbo].[sp_ExtendLoanCard] 
	@dayNumber INT,
	@LoanCardId INT,
	@ModifiedBy NVARCHAR(100)
AS
BEGIN
	DECLARE @Message NVARCHAR(100) = N'Thao tác thay gia hạn thời gian mượn sách không thành công, vui lòng thử lại sau'
	IF(EXISTS(SELECT LoanCardId FROM LoanCard WHERE LoanCardId = @LoanCardId))
	BEGIN
		IF(@dayNumber > 0)
		BEGIN
			IF(ISNULL(@ModifiedBy,'0') <> '0' AND @ModifiedBy <> '')
			BEGIN
				UPDATE [dbo].[LoanCard]
				   SET [ReturnOfDate] = DATEADD(DAY, @dayNumber, [ReturnOfDate])
					  ,[StatusId] = 2
					  ,[ModifiedDate] = GETDATE()
					  ,[ModifiedBy] = @ModifiedBy
				 WHERE LoanCardId = @LoanCardId
				 SET @Message = N'Thao tác thay gia hạn thời gian mượn sách của Thẻ mượn có ID: ' + CONVERT(NVARCHAR, @LoanCardId) + ' thành công'
			END
			ELSE
			BEGIN
				SET @Message = N'ID người gia hạn Thẻ mượn không được để trống'
			END
		END
		ELSE
		BEGIN
			SET @Message = N'Số ngày gia hạn thêm không hợp lệ'
		END
	END
	ELSE
	BEGIN
		SET @Message = N'LoanCardId không tồn tại'
	END
	SELECT @Message AS [Message], @LoanCardId AS LoanCardId
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllBookArchive]    Script Date: 3/2/2021 7:23:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		 Nhân Vũ
-- Create date: 23/11/2020
-- Description:	Get all BookArchive
--Status: 1 : Active ; 4: Isdeleted
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetAllBookArchive] 
AS
BEGIN
SELECT
		BA.BookArchiveId
		,BA.BookId
		,(SELECT BookName FROM [dbo].[Book] B WHERE B.BookId = BA.BookId ) AS 'BookName'
		,(SELECT CategoryName FROM Category C WHERE C.CategoryId = B.CategoryId) AS 'CategoryName'
		,Quantity
		,QuantityRemain
		,BA.StatusId
		,(SELECT StatusName FROM Wiki as W WHERE TableId = 7 AND W.StatusId = BA.StatusId AND IsDeleted = 0) AS 'StatusName'
		,BA.CreatedDate
		,FORMAT(BA.CreatedDate,'dd-MM-yyyy hh:mm tt') AS CreatedDateStr
		,BA.CreatedBy
		,BA.ModifiedBy
		,BA.ModifiedDate
		,FORMAT(BA.ModifiedDate,'dd-MM-yyyy hh:mm tt') AS ModifiedDateStr
		
	FROM [dbo].[BookArchive] BA INNER JOIN Book B ON BA.BookId = B.BookId
	WHERE BA.StatusId IN (1)
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllBookArchiveById]    Script Date: 3/2/2021 7:23:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		 Nhân Vũ
-- Create date: 23/11/2020
-- Description:	Get all BookArchive
--Status: 1 : Active ; 4: Isdeleted
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetAllBookArchiveById] 
@BookArchiveId INT
AS
BEGIN
SELECT
		BA.BookArchiveId
		,BA.BookId
		,(SELECT TOP(1) BookName FROM [dbo].[Book] B WHERE B.BookId = BA.BookId ) AS 'BookName'
		,(SELECT TOP(1) CategoryName FROM Category C WHERE C.CategoryId = B.CategoryId) AS 'CategoryName'
		,Quantity
		,QuantityRemain
		,BA.StatusId
		,(SELECT StatusName FROM Wiki as W WHERE TableId = 7 AND W.StatusId = BA.StatusId AND IsDeleted = 0) AS 'StatusName'
		,BA.CreatedDate
		,FORMAT(BA.CreatedDate,'dd-MM-yyyy hh:mm tt') AS CreatedDateStr
		,BA.CreatedBy
		,(SELECT TOP(1) ANU.UserName FROM [dbo].[AspNetUsers] AS ANU WHERE ANU.Id = BA.[CreatedBy]) AS CreatedByName
		,BA.ModifiedDate
		,FORMAT(BA.ModifiedDate,'dd-MM-yyyy hh:mm tt') AS ModifiedDateStr
		,BA.ModifiedBy
		,(SELECT TOP(1) ANU.UserName FROM [dbo].[AspNetUsers] AS ANU WHERE ANU.Id = BA.[ModifiedBy]) AS ModifiedByName
		,(SELECT TOP(1) ImagePath FROM [dbo].[Book] AS B WHERE B.BookId = BA.BookId ) AS 'ImagePath'
	FROM [dbo].[BookArchive] BA INNER JOIN Book B ON BA.BookId = B.BookId
	WHERE BA.BookArchiveId = @BookArchiveId
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetAllLoanCard]    Script Date: 3/2/2021 7:23:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		 Nhân Vũ
-- Create date: 23/11/2020
-- Description:	Get all LoanCard
--Status: 1 : Loanting ; 2: Extend ; 3 : Overdue; 4: Completed ; 5: IsDeleted
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetAllLoanCard] 
AS
BEGIN
SELECT
		L.[LoanCardId]
		,L.LoanOfDate
		,FORMAT(L.LoanOfDate,'MMM dd yyyy') AS LoanOfDateStr
		,L.ReturnOfDate
		,FORMAT(L.ReturnOfDate,'MMM dd yyyy')  AS ReturnOfDateStr
		,(SELECT StudentName FROM [dbo].[Student] s WHERE L.StudentId=s.StudentId) AS 'StudentName'
		,(SELECT StatusName FROM Wiki WHERE TableId = 4 AND StatusId = L.StatusId AND IsDeleted = 0) AS 'StatusName'
		,(SELECT COUNT(*) FROM [dbo].[LoanCardBook] AS LC
							WHERE L.LoanCardId= LC.LoanCardId) AS Books
		,L.CreatedDate
		,FORMAT(L.CreatedDate,'MMM dd yyyy') AS CreatedDateStr
		,L.CreatedBy

	FROM [dbo].[LoanCard] L
	WHERE StatusId IN (1,2,3,4)
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetBook]    Script Date: 3/2/2021 7:23:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		 Tùng Nguyễn
-- Create date: 20/11/2020
-- Description:	Get book by BookId
-- Status: 1: stochking, 2: over, 3: pending, 4: deleted
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetBook]  
	@BookId INT
AS
BEGIN
	SELECT B.[BookId]
		  ,B.[BookName]
		  ,B.[Dop]
		  ,FORMAT(B.[Dop],'dd-MM-yyyy') AS DopStr
		  ,B.[PublishCompany]
		  ,B.[Author]
		  ,B.[Page]
		  ,B.[Description]
		  ,B.[CategoryId]
		  ,(SELECT TOP(1) C.CategoryName	FROM [dbo].[Category] AS C WHERE C.CategoryId = B.[CategoryId]) AS CategoryName
		  ,B.[StatusId]
		  ,(SELECT TOP(1) W.StatusName FROM Wiki AS W WHERE W.TableId = 1 AND W.StatusId = B.[StatusId]) AS StatusName
		  ,B.[CreatedDate]
		  ,FORMAT(B.[CreatedDate],'dd-MM-yyyy hh:mm tt') AS CreatedDateStr
		  ,B.[CreatedBy]
		  ,(SELECT TOP(1) ANU.UserName FROM [dbo].[AspNetUsers] AS ANU WHERE ANU.Id = B.[CreatedBy]) AS CreatedByName
		  ,B.[ModifiedDate]
		  ,FORMAT(B.[ModifiedDate],'dd-MM-yyyy hh:mm tt') AS ModifiedDateStr
		  ,B.[ModifiedBy]
		  ,(SELECT TOP(1) ANU.UserName FROM [dbo].[AspNetUsers] AS ANU WHERE ANU.Id = B.[ModifiedBy]) AS ModifiedByName
		  ,B.[ImagePath]
		  ,(SELECT TOP(1) BA.QuantityRemain FROM [dbo].[BookArchive] AS BA WHERE BA.BookId = B.BookId) AS QuanityRemain
		  ,(SELECT TOP(1) BA.Quantity FROM [dbo].[BookArchive] AS BA WHERE BA.BookId = B.BookId) AS Quantity
	  FROM [dbo].[Book] AS B
	  WHERE B.BookId = @BookId AND B.StatusId <> 4
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetBookByCategoryId]    Script Date: 3/2/2021 7:23:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		 Nhân Vũ
-- Create date: 02/12/2020
-- Description:	Get BookList by CategoryId
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetBookByCategoryId] 
	@CategoryId INT
AS
BEGIN
	SELECT B.BookId AS BookId
		  ,B.BookName AS BookName		 
		  ,B.ImagePath AS ImagePath
		  ,(SELECT TOP(1) C.CategoryName FROM Category AS C WHERE B.CategoryId = C.CategoryId ) AS CategoryName
		  ,B.StatusId
		  ,(SELECT TOP(1) W.StatusName FROM Wiki AS W WHERE W.TableId = 1 AND B.StatusId = W.StatusId) AS StatusName
	FROM Book AS B
	WHERE B.CategoryId  = @CategoryId AND B.StatusId <> 4
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetBookList]    Script Date: 3/2/2021 7:23:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		 Tùng Nguyễn
-- Create date: 24/11/2020
-- Description:	Get BookList by LoanCardId
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetBookList] 
	@LoanCardId INT
AS
BEGIN
	SELECT B.BookId AS BookId
		  ,B.BookName AS BookName
		  ,(SELECT TOP(1) C.CategoryName FROM Category AS C WHERE C.CategoryId = B.CategoryId) AS CategoryName
		  ,B.Author
		  ,B.Dop
		  ,FORMAT(B.[Dop],'dd-MM-yyyy') AS DopStr
		  ,B.ImagePath AS ImagePath
	FROM [dbo].[LoanCardBook] AS LCB INNER JOIN LoanCard AS LC ON LCB.LoanCardId = LC.LoanCardId
													  INNER JOIN Book AS B ON LCB.BookId = B.BookId
													  WHERE LC.LoanCardId = @LoanCardId
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetBooks]    Script Date: 3/2/2021 7:23:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		 Tùng Nguyễn
-- Create date: 20/11/2020
-- Description:	Get all book
-- =============================================
CREATE PROCEDURE  [dbo].[sp_GetBooks] 
AS
BEGIN
	SELECT B.[BookId]
		  ,B.[BookName]
		  ,B.[Dop]
		  ,FORMAT(B.[Dop],'dd-MM-yyyy') AS DopStr
		  ,B.[PublishCompany]
		  ,B.[Author]
		  ,B.[Page]
		  ,B.[Description]
		  ,B.[CategoryId]
		  ,(SELECT TOP(1) C.CategoryName FROM [dbo].[Category] AS C WHERE C.CategoryId = b.[CategoryId]) AS CategoryName
		  ,B.[StatusId]
		  ,(SELECT TOP(1) W.StatusName FROM Wiki AS W WHERE W.TableId = 1 AND W.StatusId = B.[StatusId]) AS StatusName
		  ,B.[CreatedDate]
		  ,FORMAT(B.[CreatedDate],'dd-MM-yyyy hh:mm tt') AS CreatedDateStr
		  ,B.[CreatedBy]
		  ,B.[ModifiedDate]
		  ,FORMAT(B.[ModifiedDate],'dd-MM-yyyy hh:mm tt') AS ModifiedDateStr
		  ,B.[ModifiedBy]
		  ,B.[ImagePath]
	  FROM [dbo].[Book] AS B WHERE B.StatusId <> 4
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetCategories]    Script Date: 3/2/2021 7:23:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		 Nhan Vu
-- Create date: 18/11/2020
-- Description:	Get all module
-- status: 1: rest, 4: deleted
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetCategories] 
AS
BEGIN
	SELECT	c.CategoryId  
			,c.CategoryName	  
			,c.StatusId
			,(SELECT TOP(1) w.[StatusName] FROM[dbo].[Wiki] AS w WHERE w.TableId = 6 AND w.[StatusId] = c.StatusId AND w.IsDeleted = 0) AS 'StatusName'
			,c.[CreatedDate]
			,FORMAT(c.[CreatedDate],'dd-MM-yyyy hh:mm tt') AS CreatedDateStr
			,c.[ModifiedDate]
			,FORMAT(c.[ModifiedDate],'dd-MM-yyyy hh:mm tt') AS ModifiedDateStr
			,c.[ModifiedBy]
			,(SELECT TOP(1) ANU.UserName FROM [dbo].[AspNetUsers] AS ANU WHERE ANU.Id = c.[ModifiedBy]) AS ModifiedByName
			,c.[CreatedBy]
			,(SELECT TOP(1) ANU.UserName FROM [dbo].[AspNetUsers] AS ANU WHERE ANU.Id = c.[CreatedBy]) AS CreatedByName
	FROM [dbo].[Category] AS c
	WHERE c.[StatusId] = 1
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetCategory]    Script Date: 3/2/2021 7:23:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetCategory]	
	 @CategoryId INT
AS
BEGIN
		SELECT	c.CategoryId  
				,c.CategoryName	  
				,c.StatusId
				,c.[CreatedDate]
				,FORMAT(c.[CreatedDate],'dd-MM-yyyy hh:mm tt') AS CreatedDateStr
				,c.[ModifiedBy]
				,(SELECT TOP(1) ANU.UserName FROM [dbo].[AspNetUsers] AS ANU WHERE ANU.Id = c.[ModifiedBy]) AS ModifiedByName
				,c.[CreatedBy]
				,(SELECT TOP(1) ANU.UserName FROM [dbo].[AspNetUsers] AS ANU WHERE ANU.Id = c.[CreatedBy]) AS CreatedByName
				,c.[ModifiedDate]
				,FORMAT(c.[ModifiedDate],'dd-MM-yyyy hh:mm tt') AS ModifiedDateStr
				,(SELECT TOP(1) w.[StatusName] FROM[dbo].[Wiki] AS w WHERE w.TableId = 6 AND w.[StatusId] = c.StatusId AND w.IsDeleted = 0) AS 'StatusName'
		FROM [dbo].[Category] AS c
		WHERE CategoryId = @CategoryId
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetDistricts]    Script Date: 3/2/2021 7:23:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		 Tùng Nguyễn
-- Create date: 25/11/2020
-- Description:	Get all District
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetDistricts] 
	@ProvinceId INT
AS
BEGIN
	SELECT	[id] AS DistrictId
			,CONCAT([_prefix],' ',[_name]) AS DistrictName
	FROM [dbo].[district]
	WHERE [_province_id] = @ProvinceId
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetLibaryian]    Script Date: 3/2/2021 7:23:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		 Tùng Nguyễn
-- Create date: 25/11/2020
-- Description:	Get Libaryian by LibaryianId
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetLibaryian] 
	@LibaryianId INT
AS
BEGIN
	SELECT [LibaryianId]
		  ,[LibaryianName]
		  ,[AccountId]
		  ,[Dob]
		  ,FORMAT([Dob],'dd-MM-yyyy') AS DobStr
		  ,[Gender]
		  ,[Email]
		  ,[PhoneNumber]
		  ,[StatusId]
		  ,(SELECT TOP(1) W.StatusName FROM Wiki AS W WHERE W.TableId = 5 AND W.StatusId = [StatusId]) AS StatusName
		  ,[ProvinceId]
		  ,(SELECT TOP(1) P._name FROM province AS P WHERE P.id = [ProvinceId]) AS ProvinceName
		  ,[DistrictId]
		  ,(SELECT TOP(1) CONCAT(D._prefix,' ', D._name) FROM district AS D WHERE D.id = [DistrictId]) AS DistrictName
		  ,[WardId]
		  ,(SELECT TOP(1) CONCAT(WA._prefix,' ', WA._name ) FROM ward AS WA WHERE WA.id = [WardId]) AS WardName
		  ,[Address]
		  ,[HireDate]
		  ,FORMAT([HireDate],'dd-MM-yyyy') AS HireDateStr
		  ,[CreatedDate]
		  ,FORMAT([CreatedDate],'dd-MM-yyyy hh:mm tt') AS CreatedDateStr
		  ,[CreatedBy]
		  ,[ModifiedDate]
		  ,FORMAT([ModifiedDate],'dd-MM-yyyy hh:mm tt') AS ModifiedDateStr
		  ,[ModifiedBy]
	  FROM [dbo].[Libaryian]
	  WHERE [LibaryianId] = @LibaryianId
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetLibaryians]    Script Date: 3/2/2021 7:23:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		 Tùng Nguyễn
-- Create date: 25/11/2020
-- Description:	Get all Libaryian
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetLibaryians] 
AS
BEGIN
	SELECT [LibaryianId]
		  ,[LibaryianName]
		  ,[AccountId]
		  ,[Dob]
		  ,FORMAT([Dob],'dd-MM-yyyy') AS DobStr
		  ,[Gender]
		  ,[Email]
		  ,[PhoneNumber]
		  ,[StatusId]
		  ,(SELECT TOP(1) W.StatusName FROM Wiki AS W WHERE W.TableId = 5 AND W.StatusId = [StatusId]) AS StatusName
		  ,[ProvinceId]
		  ,(SELECT TOP(1) P._name FROM province AS P WHERE P.id = [ProvinceId]) AS ProvinceName
		  ,[DistrictId]
		  ,(SELECT TOP(1) CONCAT(D._prefix,' ', D._name) FROM district AS D WHERE D.id = [DistrictId]) AS DistrictName
		  ,[WardId]
		  ,(SELECT TOP(1) CONCAT(WA._prefix,' ', WA._name ) FROM ward AS WA WHERE WA.id = [WardId]) AS WardName
		  ,[Address]
		  ,[HireDate]
		  ,FORMAT([HireDate],'dd-MM-yyyy') AS HireDateStr
		  ,[CreatedDate]
		  ,FORMAT([CreatedDate],'dd-MM-yyyy hh:mm tt') AS CreatedDateStr
		  ,[CreatedBy]
		  ,[ModifiedDate]
		  ,FORMAT([ModifiedDate],'dd-MM-yyyy hh:mm tt') AS ModifiedDateStr
		  ,[ModifiedBy]
	  FROM [dbo].[Libaryian]
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetLoanCard]    Script Date: 3/2/2021 7:23:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		 Tùng Nguyễn
-- Create date: 24/11/2020
-- Description:	Get Loan card by LoanCardId
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetLoanCard]
	@LoanCardId INT
AS
BEGIN
	SELECT LC.[LoanCardId]
		  ,LC.[LoanOfDate]
		  ,FORMAT(LC.[LoanOfDate],'dd-MM-yyyy') AS LoanOfDateStr
		  ,LC.[ReturnOfDate]
		  ,FORMAT(LC.[ReturnOfDate],'dd-MM-yyyy') AS ReturnOfDateStr
		  ,LC.[StudentId]
		  ,(SELECT TOP(1) S.StudentName FROM Student AS S WHERE LC.[StudentId] = S.StudentId) AS StudentName
		  ,(SELECT TOP(1) S.CourseName FROM Student AS S WHERE LC.[StudentId] = S.StudentId) AS CourseName
		  ,(SELECT TOP(1) S.AvatarPath FROM Student AS S WHERE LC.StudentId = S.StudentId) AS AvatarPath
		  ,LC.[StatusId]
		  ,(SELECT TOP(1) W.StatusName FROM Wiki AS W WHERE W.TableId = 3 AND W.StatusId = LC.[StatusId]) AS StatusName
		  ,LC.[CreatedDate]
		  ,FORMAT(LC.[CreatedDate],'dd-MM-yyyy hh:mm tt') AS CreatedDateStr
		  ,LC.[CreatedBy]
		  ,(SELECT TOP(1) ANU.UserName FROM [dbo].[AspNetUsers] AS ANU WHERE ANU.Id = LC.[CreatedBy]) AS CreatedByName
		  ,LC.[ModifiedDate]
		  ,FORMAT(LC.[ModifiedDate],'dd-MM-yyyy hh:mm tt') AS ModifiedDateStr
		  ,LC.[ModifiedBy]
		  ,(SELECT TOP(1) ANU.UserName FROM [dbo].[AspNetUsers] AS ANU WHERE ANU.Id = LC.[ModifiedBy]) AS ModifiedByName
	  FROM [dbo].[LoanCard] AS LC
	  WHERE LC.[LoanCardId] = @LoanCardId
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetLoanCards]    Script Date: 3/2/2021 7:23:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		 Tùng Nguyễn
-- Create date: 24/11/2020
-- Description:	Get all Loan card
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetLoanCards]
AS
BEGIN
	EXEC sp_CheckAndChangeStatusOverdueLoanCard
	SELECT LC.[LoanCardId]
		  ,LC.[LoanOfDate]
		  ,FORMAT(LC.[LoanOfDate],'dd-MM-yyyy') AS LoanOfDateStr
		  ,LC.[ReturnOfDate]
		  ,FORMAT(LC.[ReturnOfDate],'dd-MM-yyyy') AS ReturnOfDateStr
		  ,LC.[StudentId]
		  ,(SELECT TOP(1) S.StudentName FROM Student AS S WHERE LC.[StudentId] = S.StudentId) AS StudentName
		  ,(SELECT TOP(1) S.CourseName FROM Student AS S WHERE LC.[StudentId] = S.StudentId) AS CourseName
		  ,LC.[StatusId]
		  ,(SELECT TOP(1) W.StatusName FROM Wiki AS W WHERE W.TableId = 3 AND W.StatusId = LC.[StatusId]) AS StatusName
		  ,LC.[CreatedDate]
		  ,FORMAT(LC.[CreatedDate],'dd-MM-yyyy hh:mm tt') AS CreatedDateStr
		  ,LC.[CreatedBy]
		  ,LC.[ModifiedDate]
		  ,FORMAT(LC.[ModifiedDate],'dd-MM-yyyy hh:mm tt') AS ModifiedDateStr
		  ,LC.[ModifiedBy]
		  ,(SELECT COUNT(*) FROM [dbo].[LoanCardBook] AS LCB WHERE LC.[LoanCardId] = LCB.LoanCardId) AS Books
	  FROM [dbo].[LoanCard] AS LC
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetProvinces]    Script Date: 3/2/2021 7:23:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		 Tùng Nguyễn
-- Create date: 25/11/2020
-- Description:	Get all Province
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetProvinces] 
AS
BEGIN
	SELECT	[id] AS ProvinceId
			,[_name] AS ProvinceName
	FROM	[dbo].[province]
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetRandomBook]    Script Date: 3/2/2021 7:23:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[sp_GetRandomBook]
AS
BEGIN
SELECT TOP (3)
		   B.[BookId]
		  ,B.[BookName]
		  ,B.[Dop]
		  ,FORMAT(B.[Dop],'dd-MM-yyyy') AS DopStr
		  ,B.[PublishCompany]
		  ,B.[Author]
		  ,B.[Page]
		  ,B.[Description]
		  ,B.[CategoryId]
		  ,(SELECT TOP(1) C.CategoryName FROM [dbo].[Category] AS C WHERE C.CategoryId = [CategoryId]) AS CategoryName
		  ,B.[StatusId]
		  ,(SELECT TOP(1) W.StatusName FROM Wiki AS W WHERE W.TableId = 1 AND W.StatusId = B.[StatusId]) AS StatusName
		  ,B.[CreatedDate]
		  ,FORMAT(B.[CreatedDate],'dd-MM-yyyy hh:mm tt') AS CreatedDateStr
		  ,B.[CreatedBy]
		  ,B.[ModifiedDate]
		  ,FORMAT(B.[ModifiedDate],'dd-MM-yyyy hh:mm tt') AS ModifiedDateStr
		  ,B.[ModifiedBy]
		  ,B.[ImagePath]
	  FROM [dbo].[Book] AS B
	  WHERE StatusId <>4
ORDER BY NEWID()
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetRole]    Script Date: 3/2/2021 7:23:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		 Tùng Nguyễn
-- Create date: 11/12/2020
-- Description:	Get Role by Id
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetRole] 
	@RoleId NVARCHAR(450)
AS
BEGIN
	SELECT [Id] AS RoleId
		  ,[Name] AS RoleName
	  FROM [dbo].[AspNetRoles]
	  WHERE Id = @RoleId
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetRoles]    Script Date: 3/2/2021 7:23:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		 Tùng Nguyễn
-- Create date: 11/12/2020
-- Description:	Get all Role
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetRoles] 
AS
BEGIN
	SELECT [Id] AS RoleId
		  ,[Name] AS RoleName
	  FROM [dbo].[AspNetRoles]
	  WHERE [Name] <> 'System Admin'
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetStatus]    Script Date: 3/2/2021 7:23:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		 Tùng Nguyễn
-- Create date: 25/11/2020
-- Description:	Get all Statu by TableId + StatusId
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetStatus] 
	@TableId	INT,
	@StatusId	INT
AS
BEGIN
	SELECT TOP(1) [StatusId] AS Id
				  ,StatusName AS [Name]
	FROM	[dbo].[Wiki]
	WHERE	TableId = @TableId AND StatusId = @StatusId
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetStatuses]    Script Date: 3/2/2021 7:23:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		 Tùng Nguyễn
-- Create date: 25/11/2020
-- Description:	Get all Status by TableId
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetStatuses] 
	@TableId	INT,
	@IsUpdate	BIT
AS
BEGIN
	DECLARE @Conditions NVARCHAR(20) = ''
	IF(@IsUpdate = 0)
	BEGIN
		SET @Conditions = 'Deleted'
	END
	ELSE
	BEGIN
		SET @Conditions = ''
	END
	SELECT	[StatusId] 
			,StatusName 
	FROM	[dbo].[Wiki]
	WHERE	TableId = @TableId AND IsDeleted = 0 AND StatusName != @Conditions
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetStudent]    Script Date: 3/2/2021 7:23:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		 Tùng Nguyễn
-- Create date: 23/11/2020
-- Description:	Get student by StudentId
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetStudent] 
	@StudentId INT
AS
BEGIN
	SELECT S.[StudentId]
		  ,S.[StudentName]
		  ,S.[CourseName]
		  ,S.[Dob]
		  ,FORMAT(S.[Dob],'dd-MM-yyyy') AS DobStr
		  ,S.[Gender]
		  ,S.[Email]
		  ,S.[PhoneNumber]
		  ,S.[AvatarPath]
		  ,S.[StatusId]
		  ,(SELECT TOP(1) W.StatusName FROM Wiki AS W WHERE W.TableId = 2 AND W.StatusId = S.[StatusId]) AS StatusName
		  ,S.[ProvinceId]
		  ,(SELECT TOP(1) P._name FROM province AS P WHERE P.id = S.[ProvinceId]) AS ProvinceName
		  ,S.[DistrictId]
		  ,(SELECT TOP(1) CONCAT(D._prefix,' ', D._name) FROM district AS D WHERE D.id = S.[DistrictId]) AS DistrictName
		  ,S.[WardId]
		  ,(SELECT TOP(1) CONCAT(WA._prefix,' ', WA._name ) FROM ward AS WA WHERE WA.id = S.[WardId]) AS WardName
		  ,S.[Address]
		  ,S.[CreatedDate]
		  ,FORMAT(S.[CreatedDate],'dd-MM-yyyy hh:mm tt') AS CreatedDateStr
		  ,S.[CreatedBy]
		  ,(SELECT TOP(1) ANU.UserName FROM [dbo].[AspNetUsers] AS ANU WHERE ANU.Id = S.[CreatedBy]) AS CreatedByName
		  ,S.[ModifiedDate]
		  ,FORMAT(S.[ModifiedDate],'dd-MM-yyyy hh:mm tt') AS ModifiedDateStr
		  ,S.[ModifiedBy]
		  ,(SELECT TOP(1) ANU.UserName FROM [dbo].[AspNetUsers] AS ANU WHERE ANU.Id = S.[ModifiedBy]) AS ModifiedByName
	  FROM [dbo].[Student] AS S
	  WHERE [StudentId] = @StudentId AND StatusId <> 4
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetStudents]    Script Date: 3/2/2021 7:23:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		 Tùng Nguyễn
-- Create date: 23/11/2020
-- Description:	Get all student
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetStudents] 
AS
BEGIN
	SELECT S.[StudentId]
		  ,S.[StudentName]
		  ,S.[CourseName]
		  ,S.[Dob]
		  ,FORMAT(S.[Dob],'dd-MM-yyyy') AS DobStr
		  ,S.[Gender]
		  ,S.[Email]
		  ,S.[PhoneNumber]
		  ,S.[AvatarPath]
		  ,S.[StatusId]
		  ,(SELECT TOP(1) W.StatusName FROM Wiki AS W WHERE W.TableId = 2 AND W.StatusId = S.[StatusId]) AS StatusName
		  ,S.[ProvinceId]
		  ,(SELECT TOP(1) P._name FROM province AS P WHERE P.id = S.[ProvinceId]) AS ProvinceName
		  ,S.[DistrictId]
		  ,(SELECT TOP(1) CONCAT(D._prefix,' ', D._name) FROM district AS D WHERE D.id = S.[DistrictId]) AS DistrictName
		  ,S.[WardId]
		  ,(SELECT TOP(1) CONCAT(WA._prefix,' ', WA._name ) FROM ward AS WA WHERE WA.id = S.[WardId]) AS WardName
		  ,S.[Address]
		  ,S.[CreatedDate]
		  ,FORMAT(S.[CreatedDate],'dd-MM-yyyy hh:mm tt') AS CreatedDateStr
		  ,S.[CreatedBy]
		  ,S.[ModifiedDate]
		  ,FORMAT(S.[ModifiedDate],'dd-MM-yyyy hh:mm tt') AS ModifiedDateStr
		  ,S.[ModifiedBy]
	  FROM [dbo].[Student] AS S
	  WHERE S.StatusId <> 4
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetTopLoanBook]    Script Date: 3/2/2021 7:23:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		 Nhân Vũ
-- Create date: 04/12/2020
-- Description:	Get Top 3 Best Loan
-- =============================================
CREATE PROCEDURE  [dbo].[sp_GetTopLoanBook] 
AS
BEGIN
SELECT TOP(3) COUNT(LCB.BookId) AS 'Top'
			  ,B.[BookId]
			  ,B.[BookName]
			  ,B.[ImagePath]
			  ,B.[Dop]
			  ,B.[StatusId]
			  ,(SELECT TOP(1) W.StatusName FROM Wiki AS W WHERE W.TableId = 1 AND B.StatusId = W.StatusId) AS StatusName
FROM LoanCardBook AS LCB INNER JOIN Book AS B ON LCB.BookId = B.BookId
WHERE LCB.StatusId <> 4 AND B.StatusId <> 4
GROUP BY LCB.BookId, B.BookId, B.BookName, B.ImagePath, B.Dop, B.StatusId
ORDER BY COUNT(LCB.BookId) DESC
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetTopNew]    Script Date: 3/2/2021 7:23:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		 Nhân Vũ
-- Create date: 04/12/2020
-- Description:	Get Top 3 NEW
-- =============================================
CREATE PROCEDURE  [dbo].[sp_GetTopNew] 
AS
BEGIN
SELECT TOP(3)  B.[BookId]
			  ,B.[BookName]
			  ,B.[ImagePath]
			  ,B.[Dop]
			  ,B.StatusId
			   ,(SELECT TOP(1) W.StatusName FROM Wiki AS W WHERE W.TableId = 1 AND B.StatusId = W.StatusId) AS StatusName
FROM Book as B
WHERE   B.StatusId <> 4
GROUP BY B.BookId, B.BookName, B.ImagePath, B.Dop,B.StatusId
ORDER BY B.Dop DESC
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetUser]    Script Date: 3/2/2021 7:23:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		 Tùng Nguyễn
-- Create date: 11/12/2020
-- Description:	Get info user by Id
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetUser] 
	@UserId NVARCHAR(450)
AS
BEGIN
	SELECT U.[Id] AS UserId
		  ,U.[UserName]
		  ,U.[Email]
		  ,U.[PhoneNumber]
		  ,U.[FullName]
		  ,U.[Gender]
		  ,U.[HireDate]
		  ,FORMAT(U.[HireDate],'dd-MM-yyyy') AS HireDateStr
		  ,U.[Dob]
		  ,FORMAT(U.[Dob],'dd-MM-yyyy') AS DobStr
		  ,U.[ProvinceId]
		  ,(SELECT TOP(1) P._name FROM province AS P WHERE P.id = U.[ProvinceId]) AS ProvinceName
		  ,U.[DistrictId]
		  ,(SELECT TOP(1) CONCAT(D._prefix,' ', D._name) FROM district AS D WHERE D.id = U.[DistrictId]) AS DistrictName
		  ,U.[WardId]
		  ,(SELECT TOP(1) CONCAT(WA._prefix,' ', WA._name ) FROM ward AS WA WHERE WA.id = U.[WardId]) AS WardName
		  ,U.[Address]
		  ,U.[StatusId]
		  ,(SELECT TOP(1) W.StatusName FROM Wiki AS W WHERE W.TableId = 8 AND W.StatusId = U.[StatusId]) AS StatusName
		  ,U.[AvatarPath]
		  ,U.[CreatedDate]
		  ,FORMAT(U.[CreatedDate],'dd-MM-yyyy hh:mm tt') AS CreatedDateStr
		  ,U.[CreatedBy]
		  ,(SELECT TOP(1) ANU.UserName FROM [dbo].[AspNetUsers] AS ANU WHERE ANU.Id = U.[CreatedBy]) AS CreatedByName
		  ,U.[ModifiedBy]
		  ,(SELECT TOP(1) ANU.UserName FROM [dbo].[AspNetUsers] AS ANU WHERE ANU.Id = U.[ModifiedBy]) AS ModifiedByName
		  ,U.[ModifiedDate]
		  ,FORMAT(U.[ModifiedDate],'dd-MM-yyyy hh:mm tt') AS ModifiedDateStr
		  ,(SELECT TOP(1) UR.RoleId FROM [dbo].[AspNetUserRoles] AS UR WHERE UR.UserId = U.[Id]) AS RoleId
		  ,(SELECT TOP(1) R.[Name] FROM [dbo].[AspNetUserRoles] AS UR INNER JOIN [dbo].[AspNetRoles] AS R ON UR.RoleId = R.Id
																WHERE UR.UserId = U.Id) AS RoleName
	  FROM [dbo].[AspNetUsers] AS U
	  WHERE U.StatusId <> 4 AND U.Id = @UserId
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetUsers]    Script Date: 3/2/2021 7:23:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		 Tùng Nguyễn
-- Create date: 11/12/2020
-- Description:	Get all info user
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetUsers] 
AS
BEGIN
	EXEC sp_CheckAndChangeStatusOverdueLoanCard
	SELECT U.[Id] AS UserId
		  ,U.[UserName]
		  ,U.[Email]
		  ,U.[PhoneNumber]
		  ,U.[FullName]
		  ,U.[Gender]
		  ,U.[StatusId]
		  ,(SELECT TOP(1) W.StatusName FROM Wiki AS W WHERE W.TableId = 8 AND W.StatusId = U.[StatusId]) AS StatusName
		  ,U.[AvatarPath]
		  ,(SELECT TOP(1) UR.RoleId FROM [dbo].[AspNetUserRoles] AS UR WHERE UR.UserId = U.[Id]) AS RoleId
		  ,(SELECT TOP(1) R.[Name] FROM [dbo].[AspNetUserRoles] AS UR INNER JOIN [dbo].[AspNetRoles] AS R ON UR.RoleId = R.Id
																WHERE UR.UserId = U.Id) AS RoleName
	  FROM [dbo].[AspNetUsers] AS U
	  WHERE U.StatusId <> 4 AND U.Id <> '16d1aa0b-e8ea-4cf4-bfbb-9d556883224c'
END
GO
/****** Object:  StoredProcedure [dbo].[sp_GetWards]    Script Date: 3/2/2021 7:23:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		 Tùng Nguyễn
-- Create date: 25/11/2020
-- Description:	Get all Ward
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetWards] 
	@DistrictId INT
AS
BEGIN
	SELECT	[id] AS WardId
			,CONCAT([_prefix],' ', [_name]) AS WardName
	FROM [dbo].[ward] 
	WHERE [_district_id] = @DistrictId
END
GO
/****** Object:  StoredProcedure [dbo].[sp_SaveBook]    Script Date: 3/2/2021 7:23:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		 Tùng Nguyễn
-- Create date: 19/11/2020
-- Description:	Save book
-- Status: 1: stochking, 2: over, 3: pending, 4: deleted
-- =============================================
CREATE PROCEDURE [dbo].[sp_SaveBook] 
	@BookId INT,
	@BookName NVARCHAR(200),
	@Dop DATE,
	@PublishCompany NVARCHAR(200),
	@Author NVARCHAR(200),
	@Page INT,
	@Description NVARCHAR(2000),
	@CategoryId INT,
	@ImagePath NVARCHAR(200),
	@Quantity INT,
	@CreatedBy NVARCHAR(100) = '',
	@ModifiedBy NVARCHAR(100) = ''
AS
BEGIN
	DECLARE @Message NVARCHAR(100) = N'Thao tác không thành công, vui lòng thử lại sau'
	IF(ISNULL(@BookName,'0') <> '0' AND @BookName <> '')
	BEGIN
		IF(NOT EXISTS(SELECT BookName FROM Book
					 WHERE BookId != @BookId 
							AND LOWER(RTRIM(LTRIM(BookName))) = LOWER(RTRIM(LTRIM(@BookName)))))
		BEGIN
			IF(DATEDIFF(DAY, CONVERT(DATETIME,@Dop), GETDATE()) > 0)
			BEGIN
				IF(ISNULL(@PublishCompany,'0') <> '0' AND @PublishCompany <> '')
				BEGIN
					IF(ISNULL(@Author,'0') <> '0' AND @Author <> '')
					BEGIN
						IF(@Page > 0)
						BEGIN
							IF(ISNULL(@Description,'0') <> '0' AND @Description <> '')
							BEGIN
								IF(@CategoryId IN (SELECT CategoryId FROM Category WHERE StatusId = 1))
								BEGIN
										IF(ISNULL(@ImagePath,'0') <> '0' AND @ImagePath <> '')
										BEGIN
											IF(ISNULL(@BookId,0) = 0)
											BEGIN
											-- CREATE BOOK
												IF(@Quantity > 0)
												BEGIN
													IF(ISNULL(@CreatedBy,'0') <> '0' AND @CreatedBy <> '')
													BEGIN
														INSERT INTO [dbo].[Book]
															   ([BookName]
															   ,[Dop]
															   ,[PublishCompany]
															   ,[Author]
															   ,[Page]
															   ,[Description]
															   ,[CategoryId]
															   ,[StatusId]
															   ,[CreatedDate]
															   ,[CreatedBy]
															   ,[ModifiedDate]
															   ,[ModifiedBy]
															   ,[ImagePath])
														 VALUES
															   (@BookName
															   ,@Dop
															   ,@PublishCompany
															   ,@Author
															   ,@Page
															   ,@Description
															   ,@CategoryId
															   ,3
															   ,GETDATE()
															   ,@CreatedBy
															   ,GETDATE()
															   ,@CreatedBy
															   ,@ImagePath)
													
														SET @BookId = SCOPE_IDENTITY()
													INSERT INTO [dbo].[BookArchive]
															   ([BookId]
															   ,[Quantity]
															   ,[QuantityRemain]
															   ,[StatusId]
															   ,[CreatedDate]
															   ,[CreatedBy]
															   ,[ModifiedDate]
															   ,[ModifiedBy])
														 VALUES
															   (@BookId
															   ,@Quantity
															   ,@Quantity
															   ,1
															   ,GETDATE()
															   ,@CreatedBy
															   ,GETDATE()
															   ,@CreatedBy)
														SET @Message = N'Thao tác tạo mới sách thành công'
													END
													ELSE
													BEGIN
														SET @Message = N'ID người tạo mới sách không được để trống'
														SET @BookId = 0
													END
												END
												ELSE
												BEGIN
													SET @Message = N'Số lượng sách nhập vào không hợp lệ'
													SET @BookId = 0
												END
											END
											ELSE
											----UPDATE BOOK
											BEGIN
												IF(ISNULL(@ModifiedBy,'0') <> '0' AND @ModifiedBy <> '')
												BEGIN
													IF(EXISTS(SELECT BookId FROM Book WHERE BookId = @BookId))
													BEGIN
													   UPDATE [dbo].[Book]
													   SET [BookName] = @BookName
														  ,[Dop] = @Dop
														  ,[PublishCompany] = @PublishCompany
														  ,[Author] = @Author
														  ,[Page] = @Page
														  ,[Description] = @Description
														  ,[CategoryId] = @CategoryId
														  ,[ModifiedDate] = GETDATE()
														  ,[ModifiedBy] = @ModifiedBy
														  ,[ImagePath] = @ImagePath
													   WHERE BookId = @BookId
													   SET @Message = N'Thao tác cập nhật sách thành công'
													END
													ELSE
													BEGIN
														SET @Message = N'ID sách không tồn tại'
														SET @BookId = 0
													END
												END
												ELSE
												BEGIN
													SET @Message = N'ID người cập nhật Book không được để trống'
													SET @BookId = 0
												END
											END					
										END
										ELSE
										BEGIN
											SET @Message = N'Tên file Hình ảnh mô tả không được để trống'
											SET @BookId = 0
										END
								END
								ELSE
								BEGIN
									SET @Message = N'Thể loại sách không hợp lệ'
									SET @BookId = 0
								END
							END
							ELSE
							BEGIN
								SET @Message = N'Mô tả sách không được để trống'
								SET @BookId = 0
							END
						END
						ELSE
						BEGIN
							SET @Message = N'Số trang sách không hợp lệ'
							SET @BookId = 0
						END
					END
					ELSE
					BEGIN
						SET @Message = N'Tác giả không được để trống'
						SET @BookId = 0
					END
				END
				ELSE
				BEGIN
					SET @Message = N'Nhà xuất bản không được để trống'
					SET @BookId = 0
				END
			END
			ELSE
			BEGIN
				SET @Message = N'Ngày xuất bản không hợp lệ'
				SET @BookId = 0
			END
		END
		ELSE
		BEGIN
			SET @Message = N'Tên Sách đã tồn tại'
			SET @BookId = 0
		END
	END
	ELSE
	BEGIN
		SET @Message = N'Tên Sách không được để trống'
		SET @BookId = 0
	END

	SELECT @Message AS [Message], @BookId AS BookId
END



GO
/****** Object:  StoredProcedure [dbo].[sp_SaveCategory]    Script Date: 3/2/2021 7:23:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Nhân Vũ
-- Create date: 19/11/2020
-- Description:	Create or update Course
-- Status : 1 - Actived;  4-Deleted
-- =============================================
CREATE PROCEDURE [dbo].[sp_SaveCategory]
	  @CategoryId INT,
	  @CategoryName NVARCHAR(50),
	  @CreatedBy NVARCHAR(100),
	  @ModifiedBy NVARCHAR(100)
      
AS
BEGIN
	DECLARE @Message NVARCHAR(100) = N'Đã xảy ra lỗi, xin vui lòng liên hệ Admin'
	BEGIN TRY
		-- Create
		IF(ISNULL(@CategoryId,0) = 0)
		BEGIN
			IF(NOT EXISTS(SELECT * FROM [dbo].[Category] WHERE LOWER(RTRIM(LTRIM(CategoryName))) = LOWER(RTRIM(LTRIM(@CategoryName)))))
			BEGIN
					IF(ISNULL(@CategoryName,'0') <> '0' AND @CategoryName <> '')
					BEGIN
						IF(ISNULL(@CreatedBy,'0') <> '0' AND @CreatedBy <> '')
						BEGIN
							INSERT INTO [dbo].[Category]
									   ([CategoryName]
									   ,[StatusId]
									   ,[CreatedDate]
									   ,[CreatedBy]
									   ,[ModifiedDate]
									   ,[ModifiedBy])
								 VALUES
									   (@CategoryName
									   ,1
									   ,GETDATE()
									   ,@CreatedBy
									   ,GETDATE()
									   ,@CreatedBy)

							SET @CategoryId = SCOPE_IDENTITY()
							SET @Message = N'Thể loại sách có ID: ' + CONVERT(NVARCHAR, @CategoryId) + N' được tạo thành công'
						END
						ELSE
						BEGIN
							SET @Message = N'ID người tạo Thể loại sách không được để trống'
							SET @CategoryId = 0
						END	
					END
					ELSE
					BEGIN
						SET @Message = N'Tên không được rỗng'
						SET @CategoryId = 0
					END
			END
			ELSE
			BEGIN
				SET @Message = N'Tên Thể loại sách đã tồn tại'
				SET @CategoryId = 0
			END
	END
		ELSE --Update
		BEGIN
			IF(NOT EXISTS(SELECT * FROM [dbo].[Category] WHERE CategoryId = @CategoryId))
			BEGIN
				SET @Message = N'Mã ID Thể loại sách không tồn tại'
				SET @CategoryId = 0
			END
			ELSE
			BEGIN
				IF(NOT EXISTS(SELECT CategoryName  FROM [dbo].[Category]
					 WHERE CategoryId != @CategoryId AND
					 LOWER(RTRIM(LTRIM(CategoryName))) = LOWER(RTRIM(LTRIM(@CategoryName)))))
				BEGIN
					IF(ISNULL(@CategoryName,'0') <> '0' AND @CategoryName <> '')
					BEGIN
						IF(ISNULL(@ModifiedBy,'0') <> '0' AND @ModifiedBy <> '')
						BEGIN
							UPDATE [dbo].[Category]
								SET [CategoryName] = @CategoryName
									,[ModifiedDate] = GETDATE()
									,[ModifiedBy] = @ModifiedBy
								
							WHERE CategoryId = @CategoryId
							SET @Message = N'Thể loại sách có ID: ' + CONVERT(NVARCHAR, @CategoryId) + N' được sửa thành công'
						END
						ELSE
						BEGIN
							SET @Message = N'ID người tạo Thể loại sách không được để trống'
							SET @CategoryId = 0
						END
					END
					ELSE
					BEGIN
						SET @Message = N'Tên thể loại sách không được để trống'
						SET @CategoryId = 0
					END
					END
				ELSE
				BEGIN
					SET @Message = N'Tên Thể loại sách đã tồn tại'
					SET @CategoryId = 0
				END
			END
		END
		SELECT @Message AS [Message], @CategoryId AS CategoryId
	END TRY
	BEGIN CATCH
		SELECT @Message AS [Message], @CategoryId AS CategoryId
	END CATCH
END
GO
/****** Object:  StoredProcedure [dbo].[sp_SaveLoanCard]    Script Date: 3/2/2021 7:23:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		 Tùng Nguyễn
-- Create date: 24/11/2020
-- Description:	Save Loan card 
-- =============================================
CREATE PROCEDURE [dbo].[sp_SaveLoanCard]
	@LoanCardId INT,
	@StudentId INT,
	@LoanOfDate DATE,
	@ReturnOfDate DATE,
	@CreatedBy NVARCHAR(100) = '',
	@ModifiedBy NVARCHAR(100) = '',
	@BookIds	NVARCHAR(100)
AS
BEGIN
	DECLARE @Message NVARCHAR(100) = N'Thao tác không thành công, vui lòng thử lại sau'
	DECLARE @tbBookId TABLE(BookId INT)
	DECLARE @tbBookIdOld TABLE(BookId INT)
	DECLARE @BookIdOld INT
	DECLARE @BookId INT
	DECLARE @AmountBookMax INT = 3
	IF(ISNULL(@StudentId,0) <> 0 AND @StudentId > 0)
	BEGIN
		IF(EXISTS (SELECT StudentId FROM Student WHERE StudentId = @StudentId AND StatusId <> 4))
		BEGIN
			IF(ISNULL(@BookIds,'0') <> '0' AND @BookIds <> '')
			BEGIN
			--Check BookIds valid
				DECLARE @CheckBookId BIT = 1
				INSERT INTO @tbBookId SELECT value FROM STRING_SPLIT (@BookIds , ',' ) 
				WHILE(EXISTS(SELECT * FROM @tbBookId))
				BEGIN
					SET @BookId = (SELECT TOP(1) BookId FROM @tbBookId)
					IF(NOT EXISTS (SELECT BookId FROM Book WHERE @BookId = BookId AND StatusId <> 4))
					BEGIN
						SET @CheckBookId = 0
						BREAK  
					END
					DELETE FROM @tbBookId WHERE BookId = @BookId
				END
				IF(@CheckBookId = 1)
				BEGIN
					IF(ISNULL(@LoanCardId,0) = 0)
					BEGIN
					--CREATE LOANCARD
						IF(EXISTS(SELECT * FROM Student WHERE StudentId = @StudentId AND StatusId = 1))
						BEGIN
							IF(NOT EXISTS(SELECT LoanCardId FROM LoanCard WHERE StudentId = @StudentId AND StatusId <> 4 AND StatusId <> 5))
							BEGIN
								IF(ISNULL(@CreatedBy,'0') <> '0' AND @CreatedBy <> '')
								BEGIN
								--Check Quantity Remain book in BookIds
									DECLARE @CheckAmountCreate INT = 0
									DECLARE @CheckQuantityRemainBook BIT = 1
									DECLARE @StatusBook BIT = 1
									INSERT INTO @tbBookId SELECT value FROM STRING_SPLIT (@BookIds , ',' ) 
									WHILE(EXISTS(SELECT * FROM @tbBookId))
									BEGIN
										SET @BookId = (SELECT TOP(1) BookId FROM @tbBookId)
										IF(NOT EXISTS (SELECT QuantityRemain FROM BookArchive WHERE @BookId = BookId AND QuantityRemain > 0 AND StatusId <> 4))
										BEGIN
											SET @CheckQuantityRemainBook = 0
											BREAK  
										END
										IF(NOT EXISTS (SELECT BookId FROM Book WHERE @BookId = BookId AND StatusId = 1))
										BEGIN
											SET @StatusBook = 0
											BREAK  
										END
										SET @CheckAmountCreate += 1
										DELETE FROM @tbBookId WHERE BookId = @BookId
									END
									IF(@CheckQuantityRemainBook = 1)
									BEGIN
										IF(@StatusBook = 1)
										BEGIN
											IF(@CheckAmountCreate <= @AmountBookMax)
											BEGIN
											--Insert data to LoanCard table
												INSERT INTO [dbo].[LoanCard]
															([LoanOfDate]
															,[ReturnOfDate]
															,[StudentId]
															,[StatusId]
															,[CreatedDate]
															,[CreatedBy]
															,[ModifiedDate]
															,[ModifiedBy])
														VALUES
															(GETDATE()
															,DATEADD(DAY, 7, GETDATE())
															,@StudentId
															,1
															,GETDATE()
															,@CreatedBy
															,GETDATE()
															,@CreatedBy)

												--Insert data to LoanCardBook and Update data BookArchive table
													SET @LoanCardId = SCOPE_IDENTITY()
													INSERT INTO @tbBookId SELECT value FROM STRING_SPLIT (@BookIds , ',' ) 
													WHILE(EXISTS(SELECT * FROM @tbBookId))
													BEGIN
														SET @BookId = (SELECT TOP(1) BookId FROM @tbBookId)
														--Insert data to LoanCardBook table
														INSERT INTO [dbo].[LoanCardBook]
																	([LoanCardId]
																	,[BookId]
																	,[StatusId])
																VALUES (@LoanCardId
																		,@BookId
																		,1)
														--Update data BookArchive table
															UPDATE [dbo].[BookArchive]
																SET [QuantityRemain] -= 1
																WHERE BookId = @BookId
														--Check QuantityRemain. If QuantityRemain = 0 change Status to Over
														UPDATE [dbo].[Student]
															SET [StatusId] = 2
															WHERE StudentId = @StudentId

														EXEC sp_CheckStatusBookIsOver @BookId

														DELETE FROM @tbBookId WHERE BookId = @BookId
													END
															
													SET @Message = N'Thao tác tạo mới Thẻ mượn sách thành công'
											END
											ELSE
											BEGIN
												SET @Message = N'Danh sách mượn vượt quá số lượng sách mượn quy định (3 sách)'
												SET @LoanCardId = 0
											END
										END
										ELSE
										BEGIN
											SET @Message = N'Danh sách mượn có Sách đang ở trạng thái không cho mượn'
											SET @LoanCardId = 0
										END
									END
									ELSE
									BEGIN
										SET @Message = N'Danh sách mượn có Sách hết số lượng trong kho'
										SET @LoanCardId = 0
									END
								END
								ELSE
								BEGIN
									SET @Message = N'Mã người tạo mới Thẻ mượn sách không được để trống'
									SET @LoanCardId = 0
								END
							END
							ELSE
							BEGIN
								SET @Message = N'Học sinh vẫn chưa hoàn thành việc mượn trả sách'
								SET @LoanCardId = 0
							END
						END
						ELSE
						BEGIN
							SET @Message = N'Học sinh đang ở trạng thái không được mượn sách'
							SET @LoanCardId = 0
						END
					END
					ELSE
					--UPDATE LOANCARD
					BEGIN
						IF(NOT EXISTS (SELECT LoanCardId FROM LoanCard WHERE LoanCardId = @LoanCardId AND StatusId = 5))
						BEGIN
							IF(ISNULL(@ModifiedBy,'0') <> '0' AND @ModifiedBy <> '')
							BEGIN
								IF(EXISTS(SELECT LoanCardId FROM LoanCard WHERE LoanCardId = @LoanCardId))
								BEGIN
									IF(DATEDIFF(DAY, CONVERT(DATETIME,@LoanOfDate), GETDATE()) >= 0)
									BEGIN
										IF(DATEDIFF(DAY, CONVERT(DATETIME,@LoanOfDate), CONVERT(DATETIME,@ReturnOfDate)) > 0)
										BEGIN

		--Check Quantity Remain book in BookIds
		DECLARE @CheckAmount INT = 0
		DECLARE @CheckQuantityBookNew BIT = 1
		INSERT INTO @tbBookId SELECT value FROM STRING_SPLIT (@BookIds , ',' ) 
		WHILE(EXISTS(SELECT * FROM @tbBookId))
		BEGIN
			SET @BookId = (SELECT TOP(1) BookId FROM @tbBookId)
			IF(NOT EXISTS (SELECT QuantityRemain FROM BookArchive WHERE @BookId = BookId AND QuantityRemain > 0 AND StatusId <> 4))
			BEGIN
				SET @CheckQuantityBookNew = 0
				BREAK  
			END
			SET @CheckAmount += 1
			DELETE FROM @tbBookId WHERE BookId = @BookId
		END
		IF(@CheckQuantityBookNew = 1)
		BEGIN
			IF(@CheckAmount <= @AmountBookMax)
			BEGIN
			--Process old data BookIds
			INSERT INTO @tbBookIdOld SELECT BookId FROM LoanCardBook WHERE LoanCardId = @LoanCardId
			WHILE(EXISTS(SELECT * FROM @tbBookIdOld))
			BEGIN
				SET @BookIdOld = (SELECT TOP(1) BookId FROM @tbBookIdOld)
				UPDATE	[dbo].[BookArchive]
					SET [QuantityRemain] += 1
				WHERE	BookId = @BookIdOld
				EXEC sp_CheckAndChangeOverToStochking @BookIdOld
				DELETE FROM @tbBookIdOld WHERE BookId = @BookIdOld
			END
			--Check status list book edit
			SET @StatusBook = 1
			INSERT INTO @tbBookId SELECT value FROM STRING_SPLIT (@BookIds , ',' ) 
			WHILE(EXISTS(SELECT * FROM @tbBookId))
			BEGIN
				SET @BookId = (SELECT TOP(1) BookId FROM @tbBookId)
				IF(NOT EXISTS (SELECT BookId FROM Book WHERE @BookId = BookId AND StatusId = 1))
				BEGIN
					SET @StatusBook = 0
					BREAK  
				END
				DELETE FROM @tbBookId WHERE BookId = @BookId
			END
			IF(@StatusBook = 1)
			BEGIN
											--Update data to LoanCard table
												UPDATE [dbo].[LoanCard]
													SET [LoanOfDate] = @LoanOfDate
														,[ReturnOfDate] = @ReturnOfDate
														,[StudentId] = @StudentId
														,[ModifiedDate] = GETDATE()
														,[ModifiedBy] = @ModifiedBy
													WHERE LoanCardId = @LoanCardId
											--Update data to LoanCardBook table
												--First delete data LoanCardBook table by LoanCardId
												DELETE FROM [dbo].[LoanCardBook] WHERE LoanCardId = @LoanCardId;

												INSERT INTO @tbBookId SELECT value FROM STRING_SPLIT (@BookIds , ',' ) 
												WHILE(EXISTS(SELECT * FROM @tbBookId))
												BEGIN
												--Insert data to LoanCardBook table
													SET @BookId = (SELECT TOP(1) BookId FROM @tbBookId)
													INSERT INTO [dbo].[LoanCardBook]
																([LoanCardId]
																,[BookId]
																,[StatusId])
															VALUES (@LoanCardId
																	,@BookId
																	,1)
													--Update data BookArchive table
														UPDATE	[dbo].[BookArchive]
															SET	[QuantityRemain] -= 1
														WHERE	BookId = @BookId
													--Check QuantityRemain. If QuantityRemain = 0 change Status to Over
														EXEC sp_CheckStatusBookIsOver @BookId

													DELETE FROM @tbBookId WHERE BookId = @BookId
												END
												SET @Message = N'Thao tác cập nhật Thẻ mượn sách thành công'
				END
				ELSE
				BEGIN
					INSERT INTO @tbBookIdOld SELECT BookId FROM LoanCardBook WHERE LoanCardId = @LoanCardId
					WHILE(EXISTS(SELECT * FROM @tbBookIdOld))
					BEGIN
						SET @BookIdOld = (SELECT TOP(1) BookId FROM @tbBookIdOld)
						UPDATE	[dbo].[BookArchive]
							SET [QuantityRemain] -= 1
						WHERE	BookId = @BookIdOld
						EXEC sp_CheckAndChangeOverToStochking @BookIdOld
						DELETE FROM @tbBookIdOld WHERE BookId = @BookIdOld
					END
					SET @Message = N'Danh sách mượn có sách không ở trạng thái cho mượn'
					SET @LoanCardId = 0
				END
			END
			ELSE
			BEGIN
				SET @Message = N'Danh sách mượn vượt quá số lượng sách mượn quy định (3 sách)'
				SET @LoanCardId = 0
			END
		END
		ELSE
		BEGIN
			SET @Message = N'Danh sách mượn có Sách hết số lượng trong kho'
			SET @LoanCardId = 0
		END
										END
										ELSE
										BEGIN
											SET @Message = N'Ngày cho mượn không đượn lớn hơn hoặc bằng ngày trả sách'
											SET @LoanCardId = 0
										END
									END
									ELSE
									BEGIN
										SET @Message = N'Ngày cho mượn không được lớn hơn ngày hiện tại'
										SET @LoanCardId = 0
									END
								END
								ELSE
								BEGIN
									SET @Message = N'Mã thẻ mượn không tồn tại'
									SET @LoanCardId = 0
								END
							END
							ELSE
							BEGIN
								SET @Message = N'Mã người cập nhật Thẻ mượn sách không được để trống'
								SET @LoanCardId = 0
							END
						END
						ELSE
						BEGIN
							SET @Message = N'Mã thẻ mượn đã bị xóa hoặc không tồn tại'
							SET @LoanCardId = 0
						END
					END
				END
				ELSE
				BEGIN
					SET @Message = N'Danh sách mượn sách có Mã sách không hợp lệ'
					SET @LoanCardId = 0
				END
			END
			ELSE
			BEGIN
				SET @Message = N'Danh sách mượn sách không được để trống'
				SET @LoanCardId = 0
			END
		END
		ELSE
		BEGIN
			SET @Message = N'Mã học sinh đã bị xóa hoặc không tồn tại'
			SET @LoanCardId = 0
		END
	END
	ELSE
	BEGIN
		SET @Message = N'Mã học sinh không hợp lệ'
		SET @LoanCardId = 0
	END
	SELECT @Message AS [Message], @LoanCardId AS LoanCardId
END
GO
/****** Object:  StoredProcedure [dbo].[sp_SaveStudent]    Script Date: 3/2/2021 7:23:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		 Tùng Nguyễn
-- Create date: 23/11/2020
-- Description:	Save student
-- Status: 1: studying, 2: suspend, 3: blocked, 4: deleted
-- =============================================
CREATE PROCEDURE [dbo].[sp_SaveStudent]  
	@StudentId INT,
	@StudentName NVARCHAR(50),
	@CourseName NVARCHAR(50),
	@Dob DATE,
	@Gender BIT,
	@Email NVARCHAR(100),
	@PhoneNumber NVARCHAR(20),
	@ProvinceId INT,
	@DistrictId INT,
	@WardId INT,
	@Address NVARCHAR(100),
	@CreatedBy NVARCHAR(100) = '',
	@ModifiedBy NVARCHAR(100) = '',
	@AvatarPath NVARCHAR(100)
AS
BEGIN
	DECLARE @Message NVARCHAR(100) = N'Thao tác không thành công, vui lòng thử lại sau'
	IF(ISNULL(@StudentName,'0') <> '0' AND @StudentName <> '')
	BEGIN
		IF(ISNULL(@CourseName,'0') <> '0' AND @CourseName <> '')
		BEGIN
			IF(DATEDIFF(YEAR, CONVERT(DATETIME,@Dob), GETDATE()) > 0)
			BEGIN
				IF(ISNULL(@Email,'0') <> '0' AND @Email <> '')
				BEGIN
					IF(NOT EXISTS (SELECT Email FROM Student WHERE StudentId <> @StudentId AND Email = @Email))
					BEGIN
						IF(ISNULL(@PhoneNumber,'0') <> '0' AND @PhoneNumber <> '')
						BEGIN
							IF(NOT EXISTS (SELECT PhoneNumber FROM Student WHERE StudentId <> @StudentId AND PhoneNumber = @PhoneNumber))
							BEGIN
								IF(ISNULL(@AvatarPath,'0') <> '0' AND @AvatarPath <> '')
								BEGIN
									IF(ISNULL(@ProvinceId,0) <> 0  AND @ProvinceId > 0)
									BEGIN
										IF(ISNULL(@DistrictId,0) <> 0 AND @DistrictId > 0)
										BEGIN
											IF(ISNULL(@WardId,0) <> 0 AND @WardId > 0)
											BEGIN
												IF(ISNULL(@Address,'0') <> '0' AND @Address <> '')
												BEGIN
													IF(ISNULL(@StudentId,0) = 0)
													BEGIN
													-- CREATE STUDENT
														IF(ISNULL(@CreatedBy,'0') <> '0' AND @CreatedBy <> '')
														BEGIN
															INSERT INTO [dbo].[Student]
																		([StudentName]
																		,[CourseName]
																		,[Dob]
																		,[Gender]
																		,[Email]
																		,[PhoneNumber]
																		,[StatusId]
																		,[ProvinceId]
																		,[DistrictId]
																		,[WardId]
																		,[Address]
																		,[CreatedDate]
																		,[CreatedBy]
																		,[ModifiedDate]
																		,[ModifiedBy]
																		,[AvatarPath])
																	VALUES
																		(@StudentName
																		,@CourseName
																		,@Dob
																		,@Gender
																		,@Email
																		,@PhoneNumber
																		,1
																		,@ProvinceId
																		,@DistrictId
																		,@WardId
																		,@Address
																		,GETDATE()
																		,@CreatedBy
																		,GETDATE()
																		,@CreatedBy
																		,@AvatarPath)
															SET @StudentId = SCOPE_IDENTITY()
															SET @Message = N'Thao tác tạo mới Student thành công'
														END
														ELSE
														BEGIN
															SET @Message = N'ID người tạo mới Student không được để trống'
															SET @StudentId = 0
														END
													END
													ELSE
													BEGIN
														-- UPDATE STUDENT
														IF(ISNULL(@ModifiedBy,'0') <> '0' AND @ModifiedBy <> '')
														BEGIN
															IF(EXISTS(SELECT StudentId FROM Student WHERE StudentId = @StudentId))
															BEGIN
																UPDATE [dbo].[Student]
																	SET [StudentName] = @StudentName
																		,[CourseName] = @CourseName
																		,[Dob] = @Dob
																		,[Gender] = @Gender
																		,[Email] = @Email
																		,[PhoneNumber] = @PhoneNumber
																		,[ProvinceId] = @ProvinceId
																		,[DistrictId] = @DistrictId
																		,[WardId] = @WardId
																		,[Address] = @Address
																		,[ModifiedDate] = GETDATE()
																		,[ModifiedBy] = @ModifiedBy
																		,[AvatarPath] = @AvatarPath
																	WHERE StudentId = @StudentId
																	SET @Message = N'Thao tác cập nhật Student thành công'
															END
															ELSE
															BEGIN
																SET @Message = N'StudentId không tồn tại'
																SET @StudentId = 0
															END
														END
														ELSE
														BEGIN
															SET @Message = N'ID người cập nhật Student không được để trống'
															SET @StudentId = 0
														END
													END
												END
												ELSE
												BEGIN
													SET @Message = N'Thông tin Địa chỉ không hợp lệ'
													SET @StudentId = 0
												END
											END
											ELSE
											BEGIN
												SET @Message = N'Thông tin Phường/xã không hợp lệ'
												SET @StudentId = 0
											END
										END
										ELSE
										BEGIN
											SET @Message = N'Thông tin Quận/huyện không hợp lệ'
											SET @StudentId = 0
										END
									END
									ELSE
									BEGIN
										SET @Message = N'Thông tin Tỉnh/thành không hợp lệ'
										SET @StudentId = 0
									END
								END
								ELSE
								BEGIN
									SET @Message = N'Ảnh đại diện không được để trống'
									SET @StudentId = 0
								END
							END
							ELSE
							BEGIN
								SET @Message = N'Số điện đã tồn tại'
								SET @StudentId = 0
							END
						END
						ELSE
						BEGIN
							SET @Message = N'Số điện không được để trống'
							SET @StudentId = 0
						END
					END
					ELSE
					BEGIN
						SET @Message = N'Email đã đã được sử dụng'
						SET @StudentId = 0
					END
				END
				ELSE
				BEGIN
					SET @Message = N'Email không được để trống'
					SET @StudentId = 0
				END
			END
			ELSE
			BEGIN
				SET @Message = N'Ngày sinh không hợp lệ'
				SET @StudentId = 0
			END
		END
		ELSE
		BEGIN
			SET @Message = N'Tên Lớp học không được để trống'
			SET @StudentId = 0
		END
	END
	ELSE
	BEGIN
		SET @Message = N'Tên Student không được để trống'
		SET @StudentId = 0
	END

	SELECT @Message AS [Message], @StudentId AS StudentId
END






GO
/****** Object:  StoredProcedure [dbo].[sp_SearchBook]    Script Date: 3/2/2021 7:23:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		 NHÂN VŨ
-- Create date: 03/12/2020
-- Description:	SearchBook
-- =============================================
CREATE PROCEDURE  [dbo].[sp_SearchBook] 
@Search NVARCHAR(50)
AS
BEGIN

	SELECT B.[BookId]
		  ,B.[BookName]
		  ,B.[Dop]
		  ,FORMAT(B.[Dop],'dd-MM-yyyy') AS DopStr
		  ,B.[PublishCompany]
		  ,B.[Author]
		  ,B.[Page]
		  ,B.[Description]
		  ,B.[CategoryId]
		  ,(SELECT	C.CategoryName	FROM [dbo].[Category] AS C WHERE C.CategoryId = [CategoryId]) AS CategoryName
		  ,B.[StatusId]
		  ,(SELECT TOP(1) W.StatusName FROM Wiki AS W WHERE W.TableId = 1 AND W.StatusId = B.[StatusId]) AS StatusName	
		  ,B.[ImagePath]
	  FROM [dbo].[Book] AS B 
	  INNER JOIN Category AS C ON B.CategoryId = C.CategoryId
	  WHERE B.BookName LIKE '%'+@Search+'%' OR C.CategoryName LIKE '%'+@Search+'%' AND B.StatusId <> 4
	  ORDER BY B.BookName
END
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateBookArchive]    Script Date: 3/2/2021 7:23:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		 Nhân Vũ
-- Create date: 23/11/2020
-- Description:	Update BookArchive
--Status: 1 : Active ; 4: Isdeleted
-- =============================================
CREATE PROCEDURE [dbo].[sp_UpdateBookArchive] 
	@BookArchiveId INT,
	@Value INT,
	@IsPlus BIT,
	@ModifiedBy NVARCHAR(100)
AS
BEGIN
DECLARE @Message NVARCHAR(100) = N'Đã xảy ra lỗi, xin vui lòng liên hệ Admin'
		IF(NOT EXISTS(SELECT * FROM BookArchive WHERE BookArchiveId = @BookArchiveId AND StatusId <> 4))
		BEGIN
			SET @Message = N'Không tìm thấy thông tin Kho sách'	
		END
		ELSE
		BEGIN
			IF(ISNULL (@value,0)<> 0)
			BEGIN
				IF(EXISTS(SELECT Quantity FROM [dbo].[BookArchive] AS BA
									WHERE BA.Quantity >= BA.QuantityRemain))
				BEGIN
					IF(@value >= 0)
					BEGIN
						IF(@isPlus = 0)
						BEGIN
						---Cập nhật trừ-
							DECLARE @QuantityRemain INT = (SELECT TOP(1) QuantityRemain FROM BookArchive WHERE BookArchiveId = @BookArchiveId)
							IF((@QuantityRemain - @value) >= 0)
							BEGIN
								UPDATE [dbo].[BookArchive] 
									SET [Quantity] = [Quantity] - @value
										,[QuantityRemain] = [QuantityRemain] - @value			
										,[ModifiedDate] = GETDATE()
										,[ModifiedBy] = @ModifiedBy
									WHERE BookArchiveId =@BookArchiveId
									SET @Message = N'Cập nhật giảm số lượng Sách có ID: ' + CONVERT(NVARCHAR, @BookArchiveId) + ' thành công'
							END
							ELSE
							BEGIN
								SET @Message = N'Số lượng sách nhập vào lớn hơn số lượng sách còn trong kho'
								SET @BookArchiveId = 0
							END
						END
						ELSE
						BEGIN
							--Cập nhật cộng--
							UPDATE [dbo].[BookArchive] 
								SET [Quantity] = [Quantity] + @value
									,[QuantityRemain] = [QuantityRemain] + @value				
									,[ModifiedDate] = GETDATE()
									,[ModifiedBy] = @ModifiedBy
								WHERE BookArchiveId =@BookArchiveId
								SET @Message = N'Cập nhật tăng số lượng Sách có ID: ' + CONVERT(NVARCHAR, @BookArchiveId) + ' thành công'
						END
					END
					ELSE
					BEGIN
						SET @Message = N'Không được nhập giá trị âm hoặc bằng 0'
						SET @BookArchiveId = 0
					END
				END
				ELSE			
				BEGIN
					SET @Message = N'Số lượng còn không được lớn hơn số lượng thực tế'
					SET @BookArchiveId = 0
				END
			END
		END

	DECLARE @BookId INT = (SELECT TOP(1) BookId FROM BookArchive WHERE BookArchiveId = @BookArchiveId)
	EXEC sp_CheckStatusBookIsOver @BookId
	EXEC sp_CheckAndChangeOverToStochking @BookId

	SELECT @Message AS [Message], @BookArchiveId AS BookArchiveId
END
GO
/****** Object:  StoredProcedure [dbo].[sp_UpdateUser]    Script Date: 3/2/2021 7:23:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		 Tùng Nguyễn
-- Create date: 12/12/2020
-- Description:	Update user
-- =============================================
CREATE PROCEDURE [dbo].[sp_UpdateUser]
	@UserId NVARCHAR(450),
	@Email NVARCHAR(256),
	@FullName NVARCHAR(100),
	@Gender BIT,
	@PhoneNumber NVARCHAR,
	@Dob DATE,
	@HireDate DATE,
	@ProvinceId INT,
	@DistrictId INT,
	@WardId INT,
	@Address NVARCHAR(200),
	@AvatarPath NVARCHAR(200),
	@ModifiedBy NVARCHAR(100),
	@RoleId NVARCHAR(450)
AS
BEGIN
	DECLARE @Message NVARCHAR(100) = N'Đã xảy ra lỗi, xin vui lòng liên hệ Admin'
	DECLARE @Check INT = 0
	IF(EXISTS(SELECT Id FROM [dbo].[AspNetUsers] WHERE Id = @ModifiedBy AND StatusId <> 4))
	BEGIN
		UPDATE [dbo].[AspNetUsers]
			SET [UserName] = @Email
				,[NormalizedUserName] = UPPER(@Email)
				,[Email] = @Email
				,[NormalizedEmail] = UPPER(@Email)
				,[PhoneNumber] = @PhoneNumber
				,[FullName] = @FullName
				,[Gender] = @Gender
				,[Dob] = @Dob
				,[HireDate] = @HireDate
				,[ProvinceId] = @ProvinceId
				,[DistrictId] = @DistrictId
				,[WardId] = @WardId
				,[Address] = @Address
				,[AvatarPath] = @AvatarPath
				,[ModifiedBy] = @ModifiedBy
				,[ModifiedDate] = GETDATE()
			WHERE Id = @UserId

			UPDATE [dbo].[AspNetUserRoles]
			SET [RoleId] = @RoleId
			WHERE UserId = @UserId AND RoleId = @RoleId
			SET @Message = N'Cập nhật tài khoản: ' + @Email + N' thành công'
			SET @Check = 1
	END
	ELSE
	BEGIN
		SET @Message = N'Mã ID người cập nhật không tồn tại hoặc đã bị xóa'
	END
	IF(@Check = 0)
	BEGIN
		SET @Email = NULL
	END
	 SELECT @Message AS [Message], @Email AS Email
END




GO
