declare @Book_Quantity nvarchar(500) = '1-2,3-4,5-6'
DECLARE @tbBookQuantity TABLE(BookI_Quantity NVARCHAR(100))
DECLARE @tbBook_Quantity TABLE(BookId INT, Quantity INT)
DECLARE @Book_Quantity_Val NVARCHAR(50)
DECLARE @BookId INT
DECLARE @BookName NVARCHAR(100)
DECLARE @Quantity INT
DECLARE @checkBookQuantity BIT = 0
DECLARE @Message NVARCHAR(100) = N'Thao tác không thành công, vui lòng thử lại sau'

--Kiểm tra các giá trị bookId và số lượng quantity
INSERT INTO @tbBookQuantity SELECT value FROM STRING_SPLIT (@Book_Quantity , ',' ) 

WHILE(EXISTS(SELECT * FROM @tbBookQuantity))
BEGIN
	SET @Book_Quantity_Val = (SELECT TOP(1) BookI_Quantity FROM @tbBookQuantity)

	INSERT INTO @tbBook_Quantity SELECT value FROM STRING_SPLIT (@Book_Quantity_Val , '-' )

	WHILE(EXISTS(SELECT * FROM @tbBook_Quantity))
	BEGIN
		SET @BookId = (SELECT TOP(1) BookId FROM @tbBook_Quantity)
		SET @BookName = (SELECT TOP(1) BookName FROM Book WHERE BookId = @BookId)
		SET @Quantity = (SELECT TOP(1) Quantity FROM @tbBook_Quantity)
		IF(NOT EXISTS (SELECT BookId FROM Book WHERE @BookId = BookId AND StatusId <> 4))
		BEGIN
			DECLARE @checkQuantity INT = (SELECT TOP(1) Quantity FROM Book WHERE BookId = @BookId)
			IF(@checkQuantity >= @Quantity AND @checkQuantity >= 0)
			BEGIN
				SET @checkBookQuantity = 1
			END
			ELSE
			BEGIN
				SET @checkBookQuantity = 0
				SET @Message = N'Số lượng sách ' + @BookName + N' không đủ để nhận đơn !'
			END
		END
		ELSE
		BEGIN
			SET @checkBookQuantity = 0
			SET @Message = N'Mã sách nhập vào có sách không hợp lệ'
		END
		DELETE FROM @tbBook_Quantity WHERE BookId = @BookId
	END
	DELETE FROM @tbBookQuantity WHERE @Book_Quantity_Val = BookI_Quantity
END

--Bắt đầu Update
IF(@checkBookQuantity = 1)
BEGIN
	SET @Book_Quantity_Val = (SELECT TOP(1) BookI_Quantity FROM @tbBookQuantity)

	INSERT INTO @tbBook_Quantity SELECT value FROM STRING_SPLIT (@Book_Quantity_Val , '-' )

	WHILE(EXISTS(SELECT * FROM @tbBook_Quantity))
	BEGIN
		SET @BookId = (SELECT TOP(1) BookId FROM @tbBook_Quantity)
		SET @Quantity = (SELECT TOP(1) Quantity FROM @tbBook_Quantity)

		--Code UPDATE vô các bảng OrderDetails dựa vào @BookId và @Quantity, @OrderId

		DELETE FROM @tbBook_Quantity WHERE BookId = @BookId
	END
	DELETE FROM @tbBookQuantity WHERE @Book_Quantity_Val = BookI_Quantity
END