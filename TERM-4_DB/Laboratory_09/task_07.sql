CREATE TABLE #TempStudents (
    ID INT,
    StudentName NVARCHAR(50),
    TestScore INT
);

DECLARE @Counter INT = 1; 

WHILE @Counter <= 10
BEGIN
    INSERT INTO #TempStudents (ID, StudentName, TestScore)
    VALUES (
        @Counter, 
        'Студент ' + CAST(@Counter AS NVARCHAR(5)), 
        FLOOR(RAND() * 10) + 1 
    );

    SET @Counter = @Counter + 1;
END;

SELECT 
    ID AS [Порядковый номер], 
    StudentName AS [Имя студента], 
    TestScore AS [Балл за тест]
FROM #TempStudents;

DROP TABLE #TempStudents;