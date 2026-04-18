USE UNIVER;
GO

DECLARE @GroupId INT = 28,
        @SubjCode CHAR(10) = 'СУБД',
        @ExcellentCount INT;

SELECT @ExcellentCount = COUNT(*)
FROM PROGRESS P
JOIN STUDENT S ON P.IDSTUDENT = S.IDSTUDENT
WHERE S.IDGROUP = @GroupId 
  AND P.SUBJECT = @SubjCode
  AND P.NOTE > 8;

IF @ExcellentCount > 0
BEGIN
    PRINT 'В группе ' + CAST(@GroupId AS VARCHAR) + ' есть отличники';
    PRINT 'Количество: ' + CAST(@ExcellentCount AS VARCHAR);
END
ELSE
BEGIN
    PRINT 'В группе ' + CAST(@GroupId AS VARCHAR) + ' отличников по предмету ' + @SubjCode + ' нет.';
    
    DECLARE @AvgNote NUMERIC(4,2);
    
    SELECT @AvgNote = AVG(CAST(NOTE AS FLOAT))
    FROM PROGRESS P
    JOIN STUDENT S ON P.IDSTUDENT = S.IDSTUDENT
    WHERE S.IDGROUP = @GroupId AND P.SUBJECT = @SubjCode;

    IF @AvgNote IS NOT NULL
        PRINT 'Средний балл: ' + CAST(@AvgNote AS VARCHAR);
    ELSE
        PRINT 'Данные отсутствуют.';
END