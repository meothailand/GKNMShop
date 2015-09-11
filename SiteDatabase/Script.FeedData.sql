/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

--IF OBJECT_ID (N'dbo.fn_VNSignedToUnsignedString', N'FN') IS NOT NULL
--	DROP FUNCTION [dbo].[fn_VNSignedToUnsignedString]
--GO

--CREATE FUNCTION [dbo].[fn_VNSignedToUnsignedString](@inputVar NVARCHAR(MAX))
--RETURNS NVARCHAR(MAX)
--AS
--BEGIN    
--    IF (@inputVar IS NULL OR @inputVar = '')  RETURN ''
       
--    DECLARE @RT NVARCHAR(MAX)
--    DECLARE @SIGN_CHARS NCHAR(256)
--    DECLARE @UNSIGN_CHARS NCHAR (256)
     
--    SET @SIGN_CHARS = N'ăâđêôơưàảãạáằẳẵặắầẩẫậấèẻẽẹéềểễệếìỉĩịíòỏõọóồổỗộốờởỡợớùủũụúừửữựứỳỷỹỵýĂÂĐÊÔƠƯÀẢÃẠÁẰẲẴẶẮẦẨẪẬẤÈẺẼẸÉỀỂỄỆẾÌỈĨỊÍÒỎÕỌÓỒỔỖỘỐỜỞỠỢỚÙỦŨỤÚỪỬỮỰỨỲỶỸỴÝ' + NCHAR(272) + NCHAR(208)
--    SET @UNSIGN_CHARS = N'aadeoouaaaaaaaaaaaaaaaeeeeeeeeeeiiiiiooooooooooooooouuuuuuuuuuyyyyyAADEOOUAAAAAAAAAAAAAAAEEEEEEEEEEIIIIIOOOOOOOOOOOOOOOUUUUUUUUUUYYYYYDD'
     
--    DECLARE @COUNTER int
--    DECLARE @COUNTER1 int
       
--    SET @COUNTER = 1
--    WHILE (@COUNTER <= LEN(@inputVar))
--    BEGIN  
--        SET @COUNTER1 = 1
--        WHILE (@COUNTER1 <= LEN(@SIGN_CHARS) + 1)
--        BEGIN
--            IF UNICODE(SUBSTRING(@SIGN_CHARS, @COUNTER1,1)) = UNICODE(SUBSTRING(@inputVar,@COUNTER ,1))
--            BEGIN          
--                IF @COUNTER = 1
--                    SET @inputVar = SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@inputVar, @COUNTER+1,LEN(@inputVar)-1)      
--                ELSE
--                    SET @inputVar = SUBSTRING(@inputVar, 1, @COUNTER-1) +SUBSTRING(@UNSIGN_CHARS, @COUNTER1,1) + SUBSTRING(@inputVar, @COUNTER+1,LEN(@inputVar)- @COUNTER)
--                BREAK
--            END
--            SET @COUNTER1 = @COUNTER1 +1
--        END
--        SET @COUNTER = @COUNTER +1
--    END
--    SET @inputVar = replace(@inputVar,' ','-')
--    RETURN @inputVar
--END
--GO
DELETE [Order]
DBCC CHECKIDENT([Order], RESEED, 0);

DELETE [Expense]
DBCC CHECKIDENT([Expense], RESEED,0);

DELETE [User];
DBCC CHECKIDENT([User], RESEED, 0);
INSERT INTO [User] ([Email], [IsActive], [Password], [Role], [UserName])
VALUES ('ngocmai.vo@hotmail.com', 1, 'maivop@ss', 'Admin', N'Mai Võ');

DELETE [Category];
DBCC CHECKIDENT([Category], RESEED, 0);
GO
INSERT INTO [Category] ([CategoryName], [UrlName], [CategoryDesciption]) VALUES 
(N'Nước Ép Trái Cây', 'nuoc-ep-trai-cay', 'Vestibulum posuere tincidunt nulla, id euismod lorem sagittis at. Etiam viverra ullamcorper diam, eget tempus nulla venenatis sed. Nunc volutpat, lacus sed consectetur tempor, nulla elit dapibus dui, et tempor urna est et ante. Etiam quis feugiat mi. Nulla facilisi. Pellentesque id ultrices risus. Etiam sollicitudin tincidunt adipiscing. Class aptent taciti sociosqu ad litora.'),
(N'Trà - Trà Sữa', 'tra-tra-sua', 'Vestibulum posuere tincidunt nulla, id euismod lorem sagittis at. Etiam viverra ullamcorper diam, eget tempus nulla venenatis sed. Nunc volutpat, lacus sed consectetur tempor, nulla elit dapibus dui, et tempor urna est et ante. Etiam quis feugiat mi. Nulla facilisi. Pellentesque id ultrices risus. Etiam sollicitudin tincidunt adipiscing. Class aptent taciti sociosqu ad litora.'),
(N'Sinh Tố', 'sinh-to', 'Vestibulum posuere tincidunt nulla, id euismod lorem sagittis at. Etiam viverra ullamcorper diam, eget tempus nulla venenatis sed. Nunc volutpat, lacus sed consectetur tempor, nulla elit dapibus dui, et tempor urna est et ante. Etiam quis feugiat mi. Nulla facilisi. Pellentesque id ultrices risus. Etiam sollicitudin tincidunt adipiscing. Class aptent taciti sociosqu ad litora.');

DELETE [Item];
DBCC CHECKIDENT([Item], RESEED, 0);
INSERT INTO [Item] ([ItemName],[UrlName], [Price], [CategoryId], [Unit], [IsTopping], [IsFeatured]) VALUES
(N'Cam - Cà rốt', 'cam-ca-rot', 20000, 1, 'Chai 400ml',0,1),
(N'Dưa Hấu', 'dua-hau', 17000, 1,'Ly',0,1),
(N'Bưởi', 'buoi', 18000, 1, 'Ly',0,1),
(N'Trà đào', 'tra-dao', 12000, 2, 'Ly',0,1),
(N'Trà sữa đào', 'tra-sua-dao', 15000, 2, 'Ly',0,1),
(N'Trà socola', 'tra-socola', 17000, 2, 'Ly',0,0),
(N'Sầu riêng', 'sau-rieng', 20000, 3, N'Ly nhỏ',0,0),
(N'Bơ', 'bo', 17000, 3, N'Ly nhỏ',0,0),
(N'Dứa [Thơm]', 'dua-[-thom-]', 17000, 3, N'Ly lớn',0,1);

DELETE [ShipmentFee];
DBCC CHECKIDENT([ShipmentFee], RESEED, 0);
INSERT INTO [ShipmentFee]([District], [Fee], [FreeThreshold])
VALUES (N'Quận 1', 0, 0), (N'Quận 3', 10000, 200000),(N'Quận 7', 15000, 200000),(N'Quận 10', 15000, 200000);

DELETE [SiteNews]
DBCC CHECKIDENT([SiteNews], RESEED, 0);
INSERT INTO [SiteNews]([Title],[UrlTitle], [Content])
VALUES (N'Hướng dẫn mua hàng','huong-dan-mua-hang', ''), (N'Giới thiệu Giải khát Ngọc Mai','gioi-thieu-giai-khat-ngoc-mai', ''), 
       (N'Chính sách giao hàng','chinh-sach-giao-hang', '');

DELETE [Unit]
DBCC CHECKIDENT([Unit], RESEED, 0);
INSERT INTO [Unit]([UnitName])
VALUES (N'Chai 400ML'),( N'Ly'),( N'Ly lớn');
