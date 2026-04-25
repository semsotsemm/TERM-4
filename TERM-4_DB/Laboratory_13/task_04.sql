-- Процедура для вставки аудиторий -- 

USE UNIVER;
GO

CREATE OR ALTER PROCEDURE PAUDITORIUM_INSERT
    @a CHAR(20),            
    @n VARCHAR(50),         
    @c INT = 0,             
    @t CHAR(10)            
AS
BEGIN
    BEGIN TRY
        INSERT INTO AUDITORIUM (AUDITORIUM, AUDITORIUM_NAME, AUDITORIUM_CAPACITY, AUDITORIUM_TYPE)
        VALUES (@a, @n, @c, @t);

        return 1;
    END TRY
    BEGIN CATCH
        PRINT '--- ОШИБКА ПРИ ВСТАВКЕ ---';
        PRINT 'Код ошибки: ' + CAST(ERROR_NUMBER() AS VARCHAR(10));
        PRINT 'Уровень серьезности: ' + CAST(ERROR_SEVERITY() AS VARCHAR(10));
        PRINT 'Сообщение: ' + ERROR_MESSAGE();
        
        return -1;
    END CATCH
END;
GO


-- Норм вставка -- 
CREATE OR ALTER PROCEDURE PAUDITORIUM_INSERT
    @a CHAR(20),            
    @n VARCHAR(50),         
    @c INT = 0,             
    @t CHAR(10)             
AS
BEGIN
    BEGIN TRY
        INSERT INTO AUDITORIUM (AUDITORIUM, AUDITORIUM_NAME, AUDITORIUM_CAPACITY, AUDITORIUM_TYPE)
        VALUES (@a, @n, @c, @t);

        RETURN 1;
    END TRY
    BEGIN CATCH
        PRINT '--- ОШИБКА ПРИ ВСТАВКЕ ---';
        PRINT 'Код ошибки: ' + CAST(ERROR_NUMBER() AS VARCHAR(10));
        PRINT 'Уровень серьезности: ' + CAST(ERROR_SEVERITY() AS VARCHAR(10));
        PRINT 'Сообщение: ' + ERROR_MESSAGE();
        
        return -1;
    END CATCH
END;
GO

-- Плохая вставка --

DECLARE @res INT;
EXEC @res = PAUDITORIUM_INSERT 
    @a = '202-2', 
    @n = 'Ошибка типа', 
    @c = 50, 
    @t = 'НЕТ-ТИПА';

PRINT 'Результат выполнения (ожидается -1): ' + CAST(@res AS VARCHAR(2));