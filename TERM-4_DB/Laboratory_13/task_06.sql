-- Вставка в 2 таблицы -- 

USE UNIVER;
GO

CREATE OR ALTER PROCEDURE PAUDITORIUM_INSERTX
    @a  CHAR(20),      
    @n  VARCHAR(50),   
    @c  INT = 0,       
    @t  CHAR(10),      
    @tn VARCHAR(50)    
AS
BEGIN
    SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

    BEGIN TRY
        BEGIN TRANSACTION;

        INSERT INTO AUDITORIUM_TYPE (AUDITORIUM_TYPE, AUDITORIUM_TYPENAME)
        VALUES (@t, @tn);

        DECLARE @inner_res INT;
        EXEC @inner_res = PAUDITORIUM_INSERT @a, @n, @c, @t;

        IF @inner_res = -1
        BEGIN
            THROW 50001, 'Ошибка при вызове вложенной процедуры PAUDITORIUM_INSERT.', 1;
        END

        COMMIT TRANSACTION;
        RETURN 1;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;

        PRINT '--- ОШИБКА В ПРОЦЕДУРЕ PAUDITORIUM_INSERTX ---';
        PRINT 'Текст ошибки: ' + ERROR_MESSAGE();
        PRINT 'Код ошибки: ' + CAST(ERROR_NUMBER() AS VARCHAR(10));
        
        RETURN -1;
    END CATCH
END;
GO

-- Норм вставка --
DECLARE @res INT;
EXEC @res = PAUDITORIUM_INSERTX 
    @a = '500-1', 
    @n = 'Новая Лаборатория', 
    @c = 20, 
    @t = 'ЛБ-Н', 
    @tn = 'Научная лаборатория';

PRINT 'Результат: ' + CAST(@res AS VARCHAR(2));
SELECT * FROM AUDITORIUM_TYPE WHERE AUDITORIUM_TYPE = 'ЛБ-Н';
SELECT * FROM AUDITORIUM WHERE AUDITORIUM = '500-1';

-- Плохая вставка --
DECLARE @res INT;
EXEC @res = PAUDITORIUM_INSERTX 
    @a = '501-1', 
    @n = 'Тест', 
    @c = 10, 
    @t = 'ЛК',
    @tn = 'Лекционная';

PRINT 'Результат: ' + CAST(@res AS VARCHAR(2));