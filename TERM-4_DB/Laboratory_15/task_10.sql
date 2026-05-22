USE A_MyBase;
go

CREATE TABLE SALES_AUDIT (
    ID   INT IDENTITY PRIMARY KEY,
    STMT VARCHAR(20) CHECK (STMT IN ('INS', 'DEL', 'UPD')),
    TRNAME VARCHAR(50),
    CC   VARCHAR(500)
);
go


-- 1 --
CREATE OR ALTER TRIGGER TR_SALES_INS ON Sales AFTER INSERT AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @sales_number      INT,
            @product_code      INT,
            @customer_name     VARCHAR(50),
            @ordered_date      DATE,
            @qty               INT,
            @cc                VARCHAR(500);

    SET @sales_number  = (SELECT Sales_number            FROM inserted);
    SET @product_code  = (SELECT Product_code            FROM inserted);
    SET @customer_name = (SELECT Customer_name           FROM inserted);
    SET @ordered_date  = (SELECT Ordered_date            FROM inserted);
    SET @qty           = (SELECT Quantity_ordered_products FROM inserted);

    SET @cc = 'Номер: '    + CAST(@sales_number  AS VARCHAR) +
              ', товар: '  + CAST(@product_code  AS VARCHAR) +
              ', клиент: ' + @customer_name +
              ', дата: '   + CAST(@ordered_date  AS VARCHAR) +
              ', кол-во: ' + CAST(@qty           AS VARCHAR) + ';';

    PRINT('Операция вставки в Sales');

    INSERT INTO SALES_AUDIT (STMT, TRNAME, CC)
    VALUES ('INS', 'TR_SALES_INS', @cc);
END;
GO

-- Тест --
INSERT INTO Sales (Product_code, Customer_name, Ordered_date, Quantity_ordered_products)
VALUES (101, 'Иванов Артем Сергеевич', '2026-05-01', 2);

SELECT * FROM Sales        WHERE Customer_name = 'Иванов Артем Сергеевич';
SELECT * FROM SALES_AUDIT;

-- Очистка --
DROP TRIGGER IF EXISTS TR_SALES_INS;
GO


-- 2 --
CREATE OR ALTER TRIGGER TR_SALES_DEL ON Sales AFTER DELETE AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @sales_number      INT,
            @product_code      INT,
            @customer_name     VARCHAR(50),
            @ordered_date      DATE,
            @qty               INT,
            @cc                VARCHAR(500);

    SET @sales_number  = (SELECT Sales_number            FROM deleted);
    SET @product_code  = (SELECT Product_code            FROM deleted);
    SET @customer_name = (SELECT Customer_name           FROM deleted);
    SET @ordered_date  = (SELECT Ordered_date            FROM deleted);
    SET @qty           = (SELECT Quantity_ordered_products FROM deleted);

    SET @cc = 'Номер: '    + CAST(@sales_number  AS VARCHAR) +
              ', товар: '  + CAST(@product_code  AS VARCHAR) +
              ', клиент: ' + @customer_name +
              ', дата: '   + CAST(@ordered_date  AS VARCHAR) +
              ', кол-во: ' + CAST(@qty           AS VARCHAR) + ';';

    PRINT('Операция удаления из Sales');

    INSERT INTO SALES_AUDIT (STMT, TRNAME, CC)
    VALUES ('DEL', 'TR_SALES_DEL', @cc);
END;
GO

-- Тест --
INSERT INTO Sales (Product_code, Customer_name, Ordered_date, Quantity_ordered_products)
VALUES (102, 'Петрова Елена Викторовна', '2026-05-02', 1);

DELETE FROM Sales
WHERE Customer_name = 'Петрова Елена Викторовна'
  AND Ordered_date  = '2026-05-02';

SELECT * FROM SALES_AUDIT;

-- Очистка --
DROP TRIGGER IF EXISTS TR_SALES_DEL;
GO


-- 3 --
CREATE OR ALTER TRIGGER TR_SALES_UPD ON Sales AFTER UPDATE AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @sales_number      INT,
            @product_code      INT,
            @customer_name     VARCHAR(50),
            @ordered_date      DATE,
            @qty               INT,
            @cc                VARCHAR(500);

    SET @sales_number  = (SELECT Sales_number            FROM inserted);
    SET @product_code  = (SELECT Product_code            FROM inserted);
    SET @customer_name = (SELECT Customer_name           FROM inserted);
    SET @ordered_date  = (SELECT Ordered_date            FROM inserted);
    SET @qty           = (SELECT Quantity_ordered_products FROM inserted);

    SET @cc = 'Номер: '    + CAST(@sales_number  AS VARCHAR) +
              ', товар: '  + CAST(@product_code  AS VARCHAR) +
              ', клиент: ' + @customer_name +
              ', дата: '   + CAST(@ordered_date  AS VARCHAR) +
              ', кол-во: ' + CAST(@qty           AS VARCHAR) + ';';

    PRINT('Операция обновления Sales');

    INSERT INTO SALES_AUDIT (STMT, TRNAME, CC)
    VALUES ('UPD', 'TR_SALES_UPD', @cc);
END;
GO

-- 3 --
UPDATE Sales SET Quantity_ordered_products = 5
WHERE Sales_number = 1;

SELECT * FROM Sales      WHERE Sales_number = 1;
SELECT * FROM SALES_AUDIT;

-- Очистка --
DROP TRIGGER IF EXISTS TR_SALES_UPD;
GO


-- 4 --
CREATE OR ALTER TRIGGER TR_SALES ON Sales AFTER INSERT, UPDATE, DELETE AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @cc VARCHAR(500);

    DECLARE @inserted INT = (SELECT COUNT(*) FROM inserted);
    DECLARE @deleted  INT = (SELECT COUNT(*) FROM deleted);

    IF @inserted > 0 AND @deleted = 0
    BEGIN
        PRINT('Мульти-триггер: вставка в Sales');
        INSERT INTO SALES_AUDIT (STMT, TRNAME, CC)
        SELECT 'INS', 'TR_SALES',
               CONCAT('Номер: ', Sales_number,
                      ', товар: ', Product_code,
                      ', клиент: ', Customer_name,
                      ', кол-во: ', Quantity_ordered_products, ';')
        FROM inserted;
    END;

    IF @inserted = 0 AND @deleted > 0
    BEGIN
        PRINT('Мульти-триггер: удаление из Sales');
        INSERT INTO SALES_AUDIT (STMT, TRNAME, CC)
        SELECT 'DEL', 'TR_SALES',
               CONCAT('Номер: ', Sales_number,
                      ', товар: ', Product_code,
                      ', клиент: ', Customer_name,
                      ', кол-во: ', Quantity_ordered_products, ';')
        FROM deleted;
    END;

    IF @inserted > 0 AND @deleted > 0
    BEGIN
        PRINT('Мульти-триггер: обновление Sales');
        INSERT INTO SALES_AUDIT (STMT, TRNAME, CC)
        SELECT 'UPD', 'TR_SALES',
               CONCAT('Номер: ', Sales_number,
                      ', товар: ', Product_code,
                      ', клиент: ', Customer_name,
                      ', кол-во: ', Quantity_ordered_products, ';')
        FROM inserted;
    END;
END;
GO

-- Тест --
INSERT INTO Sales (Product_code, Customer_name, Ordered_date, Quantity_ordered_products)
VALUES (103, 'Богданов Станислав Львович', '2026-05-03', 1);

UPDATE Sales SET Quantity_ordered_products = 99
WHERE Customer_name = 'Богданов Станислав Львович'
  AND Ordered_date  = '2026-05-03';

DELETE FROM Sales
WHERE Customer_name = 'Богданов Станислав Львович'
  AND Ordered_date  = '2026-05-03';

SELECT * FROM SALES_AUDIT;

-- Очистка --
DROP TRIGGER IF EXISTS TR_SALES;
GO


-- 5 -- 
CREATE OR ALTER TRIGGER TR_PRODUCTS_UPD ON Products AFTER UPDATE AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO SALES_AUDIT (STMT, TRNAME, CC)
    SELECT 'UPD', 'TR_PRODUCTS_UPD',
           CONCAT('Код: ', Code, ', товар: ', Product_name, ', цена: ', Price, ';')
    FROM inserted;
END;
GO

-- Тест --
BEGIN TRY
    UPDATE Products SET Price = -1 WHERE Code = 101;
END TRY
BEGIN CATCH
    PRINT('Ошибка CHECK: ' + ERROR_MESSAGE());
END CATCH;

SELECT * FROM SALES_AUDIT;

-- Очистка --
DROP TRIGGER IF EXISTS TR_PRODUCTS_UPD;
GO


-- 6 --

CREATE OR ALTER TRIGGER TR_SALES_DEL1 ON Sales AFTER DELETE AS
BEGIN
    SET NOCOUNT ON;
    PRINT('Sales DEL триггер 1');
    INSERT INTO SALES_AUDIT (STMT, TRNAME, CC)
    SELECT 'DEL', 'TR_SALES_DEL1',
           CONCAT('Номер: ', Sales_number, ', клиент: ', Customer_name, ';')
    FROM deleted;
END;
GO

CREATE OR ALTER TRIGGER TR_SALES_DEL2 ON Sales AFTER DELETE AS
BEGIN
    SET NOCOUNT ON;
    PRINT('Sales DEL триггер 2');
    INSERT INTO SALES_AUDIT (STMT, TRNAME, CC)
    SELECT 'DEL', 'TR_SALES_DEL2',
           CONCAT('Номер: ', Sales_number, ', клиент: ', Customer_name, ';')
    FROM deleted;
END;
GO

CREATE OR ALTER TRIGGER TR_SALES_DEL3 ON Sales AFTER DELETE AS
BEGIN
    SET NOCOUNT ON;
    PRINT('Sales DEL триггер 3');
    INSERT INTO SALES_AUDIT (STMT, TRNAME, CC)
    SELECT 'DEL', 'TR_SALES_DEL3',
           CONCAT('Номер: ', Sales_number, ', клиент: ', Customer_name, ';')
    FROM deleted;
END;
GO

SELECT t.name, e.type_desc
FROM sys.triggers t
JOIN sys.trigger_events e ON t.object_id = e.object_id
WHERE OBJECT_NAME(t.parent_id) = 'Sales'
  AND e.type_desc = 'DELETE';
GO

EXEC sp_settriggerorder
    @triggername = 'TR_SALES_DEL3',
    @order       = 'First',
    @stmttype    = 'DELETE';
GO

EXEC sp_settriggerorder
    @triggername = 'TR_SALES_DEL1',
    @order       = 'Last',
    @stmttype    = 'DELETE';
GO

-- 6 --
DELETE FROM SALES_AUDIT;

INSERT INTO Sales (Product_code, Customer_name, Ordered_date, Quantity_ordered_products)
VALUES (104, 'Морозова Анна Валерьевна', '2026-05-04', 1);

DELETE FROM Sales
WHERE Customer_name = 'Морозова Анна Валерьевна'
  AND Ordered_date  = '2026-05-04';

SELECT * FROM SALES_AUDIT; 


-- 7 -- 
CREATE OR ALTER TRIGGER TR_SALES_DEL_TRAN ON Sales AFTER DELETE AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO SALES_AUDIT (STMT, TRNAME, CC)
    SELECT 'DEL', 'TR_SALES_DEL_TRAN',
           CONCAT('Удалён номер: ', Sales_number, ', клиент: ', Customer_name, ';')
    FROM deleted;
END;
GO

DELETE FROM SALES_AUDIT;
INSERT INTO Sales (Product_code, Customer_name, Ordered_date, Quantity_ordered_products)
VALUES (105, 'Набор стаканов', '2026-05-05', 1);

-- 7 --
DECLARE @test_id INT = (SELECT MAX(Sales_number) FROM Sales);

BEGIN TRANSACTION;
    DELETE FROM Sales WHERE Sales_number = @test_id;
    SELECT 'Внутри транзакции — строка удалена:' AS info;
    SELECT * FROM Sales      WHERE Sales_number = @test_id;  -- пусто
    SELECT * FROM SALES_AUDIT;                               -- запись есть
ROLLBACK TRANSACTION;
GO

SELECT 'После ROLLBACK — строка восстановлена:' AS info;
SELECT * FROM Sales       WHERE Customer_name = 'Набор стаканов';
SELECT 'После ROLLBACK — аудит пуст:' AS info;
SELECT * FROM SALES_AUDIT;
GO


-- 8 -- 
CREATE OR ALTER TRIGGER TR_PRODUCTS_INSTEAD_OF ON Products INSTEAD OF DELETE AS
BEGIN
    SET NOCOUNT ON;
    RAISERROR(N'Удаление товаров запрещено', 16, 1);
END;
GO

-- Тест --
BEGIN TRY
    DELETE FROM Products WHERE Code = 101;
END TRY
BEGIN CATCH
    PRINT('Ошибка: ' + ERROR_MESSAGE());
END CATCH;

SELECT * FROM Products WHERE Code = 101; 
GO


-- 9 --
CREATE OR ALTER TRIGGER DDL_CUSTOMERS ON DATABASE FOR DDL_DATABASE_LEVEL_EVENTS AS
BEGIN
    DECLARE @evt  VARCHAR(100) = EVENTDATA().value('(/EVENT_INSTANCE/EventType)[1]', 'varchar(100)');
    DECLARE @obj  VARCHAR(100) = EVENTDATA().value('(/EVENT_INSTANCE/ObjectName)[1]', 'varchar(100)');
    DECLARE @type VARCHAR(100) = EVENTDATA().value('(/EVENT_INSTANCE/ObjectType)[1]', 'varchar(100)');

    IF @obj = 'Customers'
    BEGIN
        PRINT('Тип события: ' + @evt);
        PRINT('Имя объекта: ' + @obj);
        PRINT('Тип объекта: ' + @type);
        RAISERROR(N'Структурные изменения таблицы Customers запрещены', 16, 1);
        ROLLBACK;
    END;
END;
GO

-- Тест --
BEGIN TRY
    ALTER TABLE Customers ADD TEST_COLUMN INT;
END TRY
BEGIN CATCH
    PRINT(ERROR_MESSAGE());
END CATCH;
GO

-- Очистка --
DROP TRIGGER IF EXISTS DDL_CUSTOMERS ON DATABASE;
GO