USE A_MyBase;
go

-- 1 --
CREATE FUNCTION dbo.FN_GET_CUSTOMER_SALES_COUNT(
    @customer_name varchar(100) = NULL
) 
RETURNS INT
AS 
BEGIN
    DECLARE @total_quantity INT = 0;
    
    SET @total_quantity = (
        SELECT SUM(s.Quantity_ordered_products)
        FROM Sales AS s
        WHERE (@customer_name IS NULL OR s.Customer_name = @customer_name)
    );
    
    RETURN ISNULL(@total_quantity, 0);
END;
GO

PRINT '--- Демонстрация работы скалярной функции ---';
DECLARE @cnt int = dbo.FN_GET_CUSTOMER_SALES_COUNT('Сидоров Дмитрий Николаевич');
PRINT('Штук товара куплено Сидоровым: ' + CAST(@cnt AS varchar(10)));

PRINT('Всего товаров продано магазином: ' + CAST(dbo.FN_GET_CUSTOMER_SALES_COUNT(NULL) AS varchar(10)));
GO


-- 2 -- 
CREATE FUNCTION dbo.FN_GET_CUSTOMER_PRODUCTS_LIST(
    @customer_name varchar(100)
) 
RETURNS VARCHAR(500)
AS 
BEGIN
    DECLARE @product_name varchar(100);
    DECLARE @result_string varchar(500) = 'Куплено: ';
    
    DECLARE CProducts CURSOR LOCAL FOR 
        SELECT p.Product_name
        FROM Sales AS s
        INNER JOIN Products AS p ON s.Product_code = p.Code
        WHERE s.Customer_name = @customer_name;
        
    OPEN CProducts;
    FETCH CProducts INTO @product_name;
    
    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @result_string = @result_string + RTRIM(@product_name) + '; ';
        FETCH CProducts INTO @product_name;
    END;
    
    CLOSE CProducts;
    DEALLOCATE CProducts;
    
    IF @result_string = 'Куплено: ' SET @result_string = 'Покупок не обнаружено';
    
    RETURN @result_string;
END;
GO

SELECT 
    c.Customer AS [ФИО Клиента], 
    dbo.FN_GET_CUSTOMER_PRODUCTS_LIST(c.Customer) AS [Список покупок]
FROM Customers AS c
WHERE c.Customer IN ('Иванов Артем Сергеевич', 'Сидоров Дмитрий Николаевич', 'Сазонова Таисия Львовна');
GO


-- 3 -- 
CREATE FUNCTION dbo.FN_GET_PRODUCTS_BY_PRICE(
    @min_price DECIMAL(10,2) = 0,
    @max_price DECIMAL(10,2) = 99999.99
) 
RETURNS TABLE
AS 
RETURN 
(
    SELECT 
        p.Code AS [Код товара],
        p.Product_name AS [Наименование],
        p.Price AS [Цена],
        p.Quantity AS [Остаток на складе]
    FROM Products AS p
    WHERE p.Price BETWEEN @min_price AND @max_price
);
GO

PRINT '--- Товары в ценовом диапазоне от 30 до 50 рублей ---';
SELECT * FROM dbo.FN_GET_PRODUCTS_BY_PRICE(30.00, 50.00);
GO


-- 4 --
CREATE FUNCTION dbo.FN_GET_CUSTOMER_INVOICE(
    @customer_name varchar(100)
)
RETURNS @InvoiceTable TABLE (
    [ФИО Покупателя] varchar(100),
    [Дата заказа] date,
    [Товар] varchar(100),
    [Цена за шт] decimal(10,2),
    [Кол-во] int,
    [Стоимость позиции] decimal(10,2)
)
AS
BEGIN
    INSERT INTO @InvoiceTable
    SELECT 
        s.Customer_name,
        s.Ordered_date,
        p.Product_name,
        p.Price,
        s.Quantity_ordered_products,
        (p.Price * s.Quantity_ordered_products) -- Расчет стоимости
    FROM Sales AS s
    INNER JOIN Products AS p ON s.Product_code = p.Code
    WHERE s.Customer_name = @customer_name;
    
    RETURN;
END;
GO

PRINT '--- Демонстрация генерации чеков для выбранных клиентов ---';
SELECT cust.Customer, inv.*
FROM Customers AS cust
CROSS APPLY dbo.FN_GET_CUSTOMER_INVOICE(cust.Customer) AS inv
WHERE cust.Customer IN ('Сидоров Дмитрий Николаевич', 'Кузнецова Марина Игоревна');
GO

-- Удаление --
DROP FUNCTION IF EXISTS dbo.FN_GET_CUSTOMER_SALES_COUNT;
DROP FUNCTION IF EXISTS dbo.FN_GET_CUSTOMER_PRODUCTS_LIST;
DROP FUNCTION IF EXISTS dbo.FN_GET_PRODUCTS_BY_PRICE;
DROP FUNCTION IF EXISTS dbo.FN_GET_CUSTOMER_INVOICE;
GO