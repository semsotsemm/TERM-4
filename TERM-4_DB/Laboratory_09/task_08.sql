USE UNIVER;
GO


DECLARE @StudentID INT = 1005,
        @StudentExists INT;

SELECT @StudentExists = COUNT(*) FROM STUDENT WHERE IDSTUDENT = @StudentID;

IF @StudentExists = 0
BEGIN
    PRINT 'Ошибка: студент с id ' + CAST(@StudentID AS VARCHAR) + ' не найден.';
    RETURN; 
END

DECLARE @AvgNote FLOAT;
SELECT @AvgNote = AVG(CAST(NOTE AS FLOAT)) FROM PROGRESS WHERE IDSTUDENT = @StudentID;

IF @AvgNote IS NULL
BEGIN
    PRINT 'инфо: у студента id ' + CAST(@StudentID AS VARCHAR) + ' пока нет оценок.';
    RETURN; 
END

PRINT 'анализ завершен успешно.';
PRINT 'средний балл студента: ' + CAST(@AvgNote AS VARCHAR);
GO