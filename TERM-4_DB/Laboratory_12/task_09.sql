USE A_MyBase;
GO

SET NOCOUNT ON;

-- 1 --
IF EXISTS(SELECT * FROM sys.objects WHERE OBJECT_ID = OBJECT_ID(N'DBO.TEMP_PRODUCTS'))
	BEGIN 
		DROP TABLE TEMP_PRODUCTS;
		PRINT 'Таблица TEMP_PRODUCTS была удалена';
	END
ELSE
	PRINT 'Таблица TEMP_PRODUCTS не найдена';

DECLARE @prod_count int, @flag char = 'c';

SET IMPLICIT_TRANSACTIONS ON;

CREATE TABLE TEMP_PRODUCTS(id int, name nvarchar(50));
INSERT TEMP_PRODUCTS(id, name)
		VALUES  (1, 'Ваза Тест'),
				(2, 'Бокал Тест');

SET @prod_count = (SELECT COUNT(*) FROM TEMP_PRODUCTS);
PRINT('Количество строк в таблице TEMP_PRODUCTS: ' + CAST(@prod_count AS VARCHAR(5)));

IF @flag = 'c'
	COMMIT;
ELSE
	ROLLBACK;
	
SET IMPLICIT_TRANSACTIONS OFF;

IF EXISTS(SELECT * FROM sys.objects WHERE OBJECT_ID = OBJECT_ID(N'DBO.TEMP_PRODUCTS'))
	PRINT('Результат: Таблица TEMP_PRODUCTS зафиксирована в БД')
ELSE
	PRINT('Результат: Таблицы TEMP_PRODUCTS не существует (откат)');

-- 2 --
BEGIN TRY
	BEGIN TRAN
		INSERT INTO Products(Code, Product_name, Price, Product_description, Quantity)
					VALUES(999, 'Временный товар', 10.00, 'Описание', 1);
		
		INSERT INTO Products(Code, Product_name, Price, Product_description, Quantity)
					VALUES(101, 'Дубликат кода', 5.00, 'Сломает транзакцию', 1);
	COMMIT TRAN;	
END TRY
BEGIN CATCH
	PRINT('Ошибка ' + CAST(ERROR_NUMBER() AS VARCHAR(5)) + ': ' + ERROR_MESSAGE())
	
	IF @@TRANCOUNT > 0 ROLLBACK TRAN;

	DECLARE @temp_check int = (SELECT COUNT(*) FROM Products WHERE Code = 999);
	PRINT('Количество вставленных временных товаров после ошибки: ' + CAST(@temp_check AS VARCHAR(3)));
END CATCH;

-- 3 --
DECLARE @SavePoint nvarchar(20) = 'ProductSave';

BEGIN TRY
	BEGIN TRAN
		INSERT INTO Products(Code, Product_name, Price, Product_description, Quantity)
					VALUES(888, 'Товар до точки', 20.00, 'Сохранится', 5);
		
		SAVE TRAN @SavePoint;

		INSERT INTO Products(Code, Product_name, Price, Product_description, Quantity)
					VALUES(102, 'Ошибочный дубль', 1.00, 'Умрет', 1);
					
	COMMIT TRAN;	
END TRY
BEGIN CATCH
    PRINT('Ошибка перехвачена. Откат к контрольной точке.')
    
    IF @@TRANCOUNT > 0 BEGIN
        ROLLBACK TRANSACTION @SavePoint;
        COMMIT TRANSACTION; -- Фиксируем то, что было до точки             
    END;

    DECLARE @check_888 int = (SELECT COUNT(*) FROM Products WHERE Code = 888);
    PRINT('Товар 888 в базе: ' + CAST(@check_888 AS VARCHAR(3)));
    
    DELETE FROM Products WHERE Code = 888;
END CATCH;

-- 8 --
PRINT('Уровень вложенности в начале: ' + CAST(@@TRANCOUNT AS VARCHAR));

BEGIN TRANSACTION OuterTran;
    PRINT('Внешняя транзакция открыта: ' + CAST(@@TRANCOUNT AS VARCHAR));

    INSERT INTO Customers(Customer, Phone_number, Home_address)
                VALUES ('Тестовый Клиент', '+375 (00) 000-00-00', 'Адрес');

    BEGIN TRANSACTION InnerTran;
        PRINT('Внутренняя транзакция открыта: ' + CAST(@@TRANCOUNT AS VARCHAR));

        INSERT INTO Products (Code, Product_name, Price, Product_description, Quantity)
                    VALUES (777, 'Вложенный товар', 7.00, 'Из внутренней транзакции', 7);

        COMMIT TRANSACTION InnerTran;
        PRINT('Внутренний COMMIT выполнен: ' + CAST(@@TRANCOUNT AS VARCHAR));

    SELECT Customer FROM Customers WHERE Customer = 'Тестовый Клиент';
    SELECT Product_name FROM Products WHERE Code = 777;

PRINT('Откатываем ВНЕШНЮЮ транзакцию...');
ROLLBACK TRANSACTION OuterTran; 

PRINT('После полного отката: ' + CAST(@@TRANCOUNT AS VARCHAR));

SELECT 'Клиент' AS Тип, Customer FROM Customers WHERE Customer = 'Тестовый Клиент'
UNION ALL
SELECT 'Товар', Product_name FROM Products WHERE Code = 777;