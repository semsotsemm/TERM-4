CREATE TABLE #StudentStats (
    RecordID INT IDENTITY(1,1),
    StudentID INT,
    SubjectCode CHAR(10),
    ExamScore INT,
    ExamDate DATE
);

WITH Numbers AS (
    SELECT 1 AS n
    UNION ALL
    SELECT n + 1 FROM Numbers WHERE n < 10000
)
INSERT INTO #StudentStats (StudentID, SubjectCode, ExamScore, ExamDate)
SELECT 
    ABS(CHECKSUM(NEWID()) % 5000) + 1000,   
    CASE (n % 3) 
        WHEN 0 THEN 'ясад' 
        WHEN 1 THEN 'нюХо' 
        ELSE 'ад' 
    END,                    
    (ABS(CHECKSUM(NEWID())) % 10) + 1,  
    DATEADD(DAY, -(n % 365), GETDATE())    
FROM Numbers
OPTION (MAXRECURSION 10000);

SET STATISTICS IO ON;


SELECT SubjectCode, ExamScore, StudentID
FROM #StudentStats
WHERE SubjectCode = 'ясад' AND ExamScore > 8;


CREATE NONCLUSTERED INDEX IX_Subject_Score 
ON #StudentStats (SubjectCode, ExamScore);