CREATE TABLE #Attendance (
    ID INT IDENTITY(1,1),
    StudentID INT,
    LessonDate DATE,
    IsAbsent BIT 
);

WITH Numbers AS (
    SELECT 1 AS n UNION ALL SELECT n + 1 FROM Numbers WHERE n < 10000
)
INSERT INTO #Attendance (StudentID, LessonDate, IsAbsent)
SELECT 
    ABS(CHECKSUM(NEWID()) % 1000) + 1,
    DATEADD(DAY, -(n % 30), GETDATE()),
    CASE WHEN n % 100 = 0 THEN 1 ELSE 0 END
FROM Numbers
OPTION (MAXRECURSION 10000);

SET STATISTICS IO ON;
GO

SELECT StudentID, LessonDate 
FROM #Attendance 
WHERE IsAbsent = 1;

CREATE NONCLUSTERED INDEX IX_Attendance_Absenteeism
ON #Attendance (StudentID, LessonDate)
WHERE IsAbsent = 1; 


SELECT StudentID, LessonDate 
FROM #Attendance 
WHERE IsAbsent = 1;