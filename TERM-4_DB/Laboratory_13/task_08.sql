USE UNIVER;
GO



CREATE OR ALTER PROCEDURE PRINT_REPORT
    @f CHAR(10) = NULL,
    @p CHAR(10) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @pulpit_count INT = 0;

    BEGIN TRY
        IF @f IS NULL AND @p IS NOT NULL
        BEGIN
            SELECT @f = FACULTY FROM PULPIT WHERE PULPIT = @p;
            IF @f IS NULL
            BEGIN
                THROW 50001, 'ошибка в параметрах: кафедра не найдена или не привязана к факультету', 1;
            END
        END

        SELECT 
            f.FACULTY [Код факультета],
            f.FACULTY_NAME [Название факультета],
            p.PULPIT [Код кафедры],
            p.PULPIT_NAME [Название кафедры]
        FROM FACULTY f
        INNER JOIN PULPIT p ON f.FACULTY = p.FACULTY
        WHERE 
            (f.FACULTY = @f OR @f IS NULL) AND
            (p.PULPIT = @p OR @p IS NULL)
        ORDER BY f.FACULTY, p.PULPIT;

        SET @pulpit_count = @@ROWCOUNT;

        RETURN @pulpit_count;

    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();

        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
        RETURN 0;
    END CATCH
END;
GO

-- Вызов процедуры -- 

DECLARE @rc INT;

BEGIN TRY
    PRINT '--- Тест 1: Отчет по факультету ИТ ---';
    EXEC @rc = PRINT_REPORT @f = 'ИТ';
    PRINT 'Количество кафедр в отчете: ' + CAST(@rc AS VARCHAR(10));

    PRINT CHAR(13) + '--- Тест 2: Отчет по конкретной кафедре (ИСиТ) ---';
    EXEC @rc = PRINT_REPORT @p = 'ИСиТ';
    PRINT 'Количество кафедр в отчете: ' + CAST(@rc AS VARCHAR(10));

    PRINT CHAR(13) + '--- Тест 3: Вызов с ошибкой (несуществующая кафедра) ---';
    EXEC @rc = PRINT_REPORT @p = 'НЕТ_ТАКОЙ';
    PRINT 'Количество кафедр в отчете: ' + CAST(@rc AS VARCHAR(10));

END TRY
BEGIN CATCH
    PRINT 'ЗАФИКСИРОВАНА ОШИБКА: ' + ERROR_MESSAGE();
END CATCH;
GO