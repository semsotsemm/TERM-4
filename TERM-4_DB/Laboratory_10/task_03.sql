CREATE TABLE #ExamResults (
    ID INT IDENTITY(1,1),
    StudentName NVARCHAR(100),
    SubjectName NVARCHAR(50),
    Mark INT,
    ExamDate DATE
);

WITH CYCLE AS (
    SELECT 1 as n
    UNION ALL
    SELECT n + 1 FROM CYCLE WHERE n < 10000
)

INSERT INTO #ExamResults (StudentName, SubjectName, Mark, ExamDate)
SELECT 
    'Ñòóäåíò ' + CAST(n AS NVARCHAR(10)),
    CASE WHEN n % 2 = 0 THEN 'ÁÄ' ELSE 'ÑÓÁÄ' END,
    (n % 10) + 1,
    DATEADD(day, -n, GETDATE())
FROM CYCLE
OPTION (MAXRECURSION 10000);

SET STATISTICS IO ON;
GO

SELECT StudentName, Mark
FROM #ExamResults
WHERE SubjectName = 'ÑÓÁÄ';

CREATE NONCLUSTERED INDEX IX_Subject_Covering
ON #ExamResults (SubjectName) 
INCLUDE (StudentName, Mark);

SELECT StudentName, Mark
FROM #ExamResults
WHERE SubjectName = 'ÑÓÁÄ';