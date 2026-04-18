CREATE TABLE #FillFactorDemo (
    ID INT IDENTITY(1,1),
    Code INT,
    Note CHAR(2000) DEFAULT 'Some data'
);

INSERT INTO #FillFactorDemo (Code)
SELECT n * 10 FROM (SELECT TOP 1000 ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS n FROM sys.all_columns) t;

CREATE NONCLUSTERED INDEX IX_Code_FillFactor 
ON #FillFactorDemo (Code)
WITH (FILLFACTOR = 70); 

INSERT INTO #FillFactorDemo (Code)
SELECT (n * 10) + 5 FROM (SELECT TOP 500 ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) AS n FROM sys.all_columns) t;

SELECT 
    avg_fragmentation_in_percent, 
    page_count
FROM sys.dm_db_index_physical_stats(DB_ID(), OBJECT_ID('tempdb..#FillFactorDemo'), NULL, NULL, 'DETAILED');