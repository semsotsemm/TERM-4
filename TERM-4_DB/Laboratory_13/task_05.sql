-- Процедура для получения списка дисциплин на кафедре --

USE UNIVER;
GO

CREATE OR ALTER PROCEDURE SUBJECT_REPORT
    @p CHAR(10)
AS BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM PULPIT WHERE PULPIT = @p)
    BEGIN
        RAISERROR('Ошибка в параметрах: кафедра %s не найдена.', 16, 1, @p);
        RETURN 0;
    END

    DECLARE @subject_list NVARCHAR(MAX);
    DECLARE @subject_count INT;

    SELECT @subject_list = STRING_AGG(RTRIM(SUBJECT), ', ')
    FROM SUBJECT
    WHERE PULPIT = @p;

    SELECT @subject_count = COUNT(*)
    FROM SUBJECT
    WHERE PULPIT = @p;

    IF @subject_count > 0
    BEGIN
        PRINT 'Отчет по дисциплинам кафедры ' + RTRIM(@p) + ':';
        PRINT @subject_list;
    END
    ELSE
    BEGIN
        PRINT 'На кафедре ' + RTRIM(@p) + ' дисциплины отсутствуют.';
    END

    RETURN @subject_count;
END;
GO


DECLARE @count INT;
EXEC @count = SUBJECT_REPORT @p = 'ИСиТ';
PRINT 'Всего выведено дисциплин: ' + CAST(@count AS VARCHAR(10));