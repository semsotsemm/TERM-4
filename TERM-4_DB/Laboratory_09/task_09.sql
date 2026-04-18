USE UNIVER;
GO

BEGIN TRY
    DECLARE @Num1 INT = 100,
            @Num2 INT = 0,
            @Result INT;

    PRINT 'Попытка выполнить деление...';
    
    SET @Result = @Num1 / @Num2;

    PRINT 'Результат деления: ' + CAST(@Result AS VARCHAR);
END TRY

BEGIN CATCH
    PRINT 'ошибка';
    PRINT '-----------------------------------------';
    
    SELECT 
        ERROR_NUMBER()    AS [Код ошибки],
        ERROR_MESSAGE()   AS [Сообщение],
        ERROR_LINE()      AS [Строка],
        ERROR_PROCEDURE() AS [Процедура],
        ERROR_SEVERITY()  AS [Серьезность],
        ERROR_STATE()     AS [Состояние];

    PRINT 'Сообщение: ' + ERROR_MESSAGE();
    PRINT 'Номер строки: ' + CAST(ERROR_LINE() AS VARCHAR);
    PRINT 'Уровень серьезности: ' + CAST(ERROR_SEVERITY() AS VARCHAR);
    PRINT '-----------------------------------------';
END CATCH;