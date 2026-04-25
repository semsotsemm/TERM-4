USE A_MyBase;
GO

SET NOCOUNT ON;

-- 1 -- 
CREATE OR ALTER PROCEDURE P_GET_PRODUCT_COUNT
AS BEGIN
    DECLARE @cnt int = (SELECT COUNT(*) FROM Products);
    SELECT * FROM Products;
    RETURN @cnt;
END;
GO

DECLARE @total_p int;
EXEC @total_p = P_GET_PRODUCT_COUNT;
PRINT 'Количество товаров: ' + CAST(@total_p AS VARCHAR(10));
GO


-- 2 -- 
CREATE OR ALTER PROCEDURE P_GET_PRODUCTS_FILTER
    @min_price DECIMAL(10,2) = NULL,
    @res_count INT OUTPUT
AS 
BEGIN
    IF @min_price IS NULL
    BEGIN
        SELECT @res_count = COUNT(*) FROM Products;
        SELECT * FROM Products;
    END
    ELSE
    BEGIN
        SELECT @res_count = COUNT(*) FROM Products WHERE Price >= @min_price;
        SELECT * FROM Products WHERE Price >= @min_price;
    END
    RETURN 0;
END;
GO

DECLARE @c INT, @stat INT;
EXEC @stat = P_GET_PRODUCTS_FILTER @min_price = 50, @res_count = @c OUTPUT;
PRINT 'Найдено товаров: ' + CAST(@c AS VARCHAR(10));
GO


-- 3 -- 
CREATE OR ALTER PROCEDURE P_FOR_INSERT_EXEC
AS 
BEGIN
    SELECT Code, Product_name, Price FROM Products;
END;
GO

IF OBJECT_ID('tempdb..#PROD_TEMP') IS NOT NULL DROP TABLE #PROD_TEMP;
CREATE TABLE #PROD_TEMP (id int, name varchar(100), cost decimal(10,2));

INSERT INTO #PROD_TEMP
    EXEC P_FOR_INSERT_EXEC;

SELECT * FROM #PROD_TEMP;
GO


-- 4 -- 
CREATE OR ALTER PROCEDURE P_INSERT_CUSTOMER_SAFE
    @name nvarchar(255),
    @phone nvarchar(20),
    @addr nvarchar(255)
AS
BEGIN
    BEGIN TRY
        INSERT INTO Customers (Customer, Phone_number, Home_address)
        VALUES (@name, @phone, @addr);
        RETURN 1;
    END TRY
    BEGIN CATCH
        PRINT 'Код ошибки: ' + CAST(ERROR_NUMBER() AS VARCHAR(10));
        PRINT 'Сообщение: ' + ERROR_MESSAGE();
        RETURN -1;
    END CATCH
END;
GO

DECLARE @r INT;
EXEC @r = P_INSERT_CUSTOMER_SAFE 'Иванов Артем Сергеевич', '123', 'Тест'; -- Ошибка PK
PRINT 'Результат: ' + CAST(@r AS VARCHAR(2));
GO


-- 5 -- 
CREATE OR ALTER PROCEDURE P_CUSTOMER_REPORT
    @c_name NVARCHAR(255)
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Customers WHERE Customer = @c_name)
    BEGIN
        RAISERROR('Покупатель не найден', 16, 1);
        RETURN 0;
    END

    DECLARE @list NVARCHAR(MAX);
    SELECT @list = STRING_AGG(RTRIM(Product_name), ', ')
    FROM Products p JOIN Sales s ON p.Code = s.Product_code
    WHERE s.Customer_name = @c_name;

    PRINT 'Товары клиента ' + @c_name + ': ' + ISNULL(@list, 'нет покупок');
    RETURN (SELECT COUNT(*) FROM Sales WHERE Customer_name = @c_name);
END;
GO

DECLARE @sc INT;
EXEC @sc = P_CUSTOMER_REPORT 'Иванов Артем Сергеевич';
GO

-- 6 --
CREATE OR ALTER PROCEDURE P_COMPLEX_SALE
    @p_code INT,
    @c_name NVARCHAR(255),
    @qty INT
AS
BEGIN
    SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
    BEGIN TRY
        BEGIN TRANSACTION;
            
            INSERT INTO Sales (Product_code, Customer_name, Ordered_date, Quantity_ordered_products)
            VALUES (@p_code, @c_name, GETDATE(), @qty);

            IF @qty > 100 THROW 50005, 'Слишком большой заказ', 1;

        COMMIT TRANSACTION;
        RETURN 1;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION;
        PRINT ERROR_MESSAGE();
        RETURN -1;
    END CATCH
END;
GO


SET IMPLICIT_TRANSACTIONS ON;
INSERT INTO Products (Code, Product_name, Price, Quantity) VALUES (777, 'Транз-1', 10, 1);
COMMIT;
SET IMPLICIT_TRANSACTIONS OFF;

BEGIN TRAN;
    INSERT INTO Products (Code, Product_name, Price, Quantity) VALUES (888, 'Транз-2', 10, 1);
    SAVE TRAN S1;
    INSERT INTO Products (Code, Product_name, Price, Quantity) VALUES (101, 'Ошибка', 0, 0); 
    IF @@ERROR <> 0 ROLLBACK TRAN S1;
COMMIT;

PRINT 'Уровень: ' + CAST(@@TRANCOUNT AS VARCHAR);
BEGIN TRAN;
    BEGIN TRAN;
        INSERT INTO Products (Code, Product_name, Price, Quantity) VALUES (999, 'Транз-3', 5, 1);
    COMMIT;
ROLLBACK; 
PRINT 'Итог уровня: ' + CAST(@@TRANCOUNT AS VARCHAR);
GO