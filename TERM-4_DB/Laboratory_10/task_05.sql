CREATE TABLE #DefragTest (
    ID INT IDENTITY(1,1),
    GUID UNIQUEIDENTIFIER DEFAULT NEWID(),
    Data CHAR(1000) DEFAULT 'Initial'
);

INSERT INTO #DefragTest (Data)
SELECT TOP 1000 'BaseData' FROM sys.all_columns a, sys.all_columns b;

CREATE NONCLUSTERED INDEX IX_Defrag_GUID ON #DefragTest(GUID);

SELECT 
    index_id, 
    avg_fragmentation_in_percent, 
    fragment_count, 
    page_count
FROM sys.dm_db_index_physical_stats(DB_ID(), OBJECT_ID('tempdb..#DefragTest'), NULL, NULL, 'DETAILED');

DECLARE @i INT = 0;
WHILE @i < 5000
BEGIN
    INSERT INTO #DefragTest (GUID, Data) VALUES (NEWID(), 'NewData');
    SET @i = @i + 1;
END;

SELECT avg_fragmentation_in_percent 
FROM sys.dm_db_index_physical_stats(DB_ID(), OBJECT_ID('tempdb..#DefragTest'), NULL, NULL, 'DETAILED');

ALTER INDEX IX_Defrag_GUID ON #DefragTest REORGANIZE;

SELECT avg_fragmentation_in_percent 
FROM sys.dm_db_index_physical_stats(DB_ID(), OBJECT_ID('tempdb..#DefragTest'), NULL, NULL, 'DETAILED');


ALTER INDEX IX_Defrag_GUID ON #DefragTest REBUILD;

SELECT avg_fragmentation_in_percent 
FROM sys.dm_db_index_physical_stats(DB_ID(), OBJECT_ID('tempdb..#DefragTest'), NULL, NULL, 'DETAILED');