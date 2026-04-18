use UNIVER;

DECLARE @t int = 1,
        @x int = 2,
        @z numeric(5,2);

IF @t > @x
    SET @z = SIN(@t) * SIN(@t)
ELSE IF @t < @x 
    SET @z = 4 * (@t + @x)
ELSE 
    SET @z = 1 - (EXP(@x - 2));

PRINT '1. Результат z = ' + CAST(@z AS VARCHAR);
PRINT '-----------------------------------------';

DECLARE @originalName NVARCHAR(100),
        @shortenedName NVARCHAR(100);

SELECT TOP 1 @originalName = NAME FROM STUDENT;

SET @shortenedName = 
    SUBSTRING(@originalName, 1, CHARINDEX(' ', @originalName)) + 
    SUBSTRING(@originalName, CHARINDEX(' ', @originalName) + 1, 1) + '. ' + 
    SUBSTRING(@originalName, CHARINDEX(' ', @originalName, CHARINDEX(' ', @originalName) + 1) + 1, 1) + '.'; 

PRINT + @shortenedName;
PRINT '-----------------------------------------';

PRINT '3. Студенты у которых др в следующем месяце:';

SELECT 
    NAME AS [Студент], 
    BDAY AS [Дата рождения],
    DATEDIFF(YEAR, BDAY, GETDATE()) AS [Возраст]
FROM STUDENT

WHERE MONTH(BDAY) = MONTH(DATEADD(MONTH, 1, GETDATE()));
PRINT '-----------------------------------------';

DECLARE @TargetGroupID INT = 25; 

SELECT 
    S.NAME AS [Студент],
    G.IDGROUP AS [Группа],
    SUB.SUBJECT_NAME AS [Предмет],
    P.PDATE AS [Дата экзамена],
    DATENAME(WEEKDAY, P.PDATE) AS [День недели]
FROM PROGRESS P
JOIN STUDENT S ON P.IDSTUDENT = S.IDSTUDENT
JOIN GROUPS G ON S.IDGROUP = G.IDGROUP
JOIN SUBJECT SUB ON P.SUBJECT = SUB.SUBJECT
WHERE G.IDGROUP = @TargetGroupID 
  AND SUB.SUBJECT = 'БД'; 
