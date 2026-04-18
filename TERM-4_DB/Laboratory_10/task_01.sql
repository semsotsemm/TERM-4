use UNIVER;
go

SELECT 
    t.name AS TableName,
    i.name AS IndexName,
    i.type_desc AS IndexType
FROM sys.indexes i
INNER JOIN sys.tables t ON i.object_id = t.object_id
WHERE t.is_ms_shipped = 0;

CREATE TABLE #StudentTest (
    ID INT,
    Name NVARCHAR(100),
    GroupNum INT,
    Score INT
);

WITH CYCLE AS (
    SELECT 1 as n
    UNION ALL
    SELECT n + 1 FROM CYCLE WHERE n < 2000
)
INSERT INTO #StudentTest (ID, Name, GroupNum, Score)
SELECT 
    n, 
    'StudentName' + CAST(n AS NVARCHAR(10)), 
    (n % 50) + 1, 
    ABS(CHECKSUM(NEWID()) % 10) + 1
FROM CYCLE
OPTION (MAXRECURSION 2000);
go

SET STATISTICS IO ON;
SET STATISTICS TIME ON;

SELECT * FROM #StudentTest 
WHERE GroupNum BETWEEN 10 AND 20;