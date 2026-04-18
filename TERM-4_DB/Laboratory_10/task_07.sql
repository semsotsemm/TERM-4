USE A_MyBase;
GO

SELECT 
    t.name AS TableName, 
    i.name AS IndexName, 
    i.type_desc AS IndexType
FROM sys.indexes i
INNER JOIN sys.tables t ON i.object_id = t.object_id
WHERE t.is_ms_shipped = 0;
GO


CREATE CLUSTERED INDEX IX_Sales_Date ON Sales(Ordered_date);

SET STATISTICS IO ON;
SELECT * FROM Sales WHERE Ordered_date BETWEEN '2026-03-01' AND '2026-03-05';
SET STATISTICS IO OFF;
GO


CREATE NONCLUSTERED INDEX IX_Product_Name_Price 
ON Products (Product_name, Price);

SET STATISTICS IO ON;
SELECT Product_name, Price 
FROM Products 
WHERE Product_name LIKE 'Ваза%' AND Price > 30;
SET STATISTICS IO OFF;
GO


CREATE NONCLUSTERED INDEX IX_Customer_Covering
ON Customers (Customer)
INCLUDE (Phone_number);

SET STATISTICS IO ON;
SELECT Customer, Phone_number 
FROM Customers 
WHERE Customer = 'Иванов Артем Сергеевич';
SET STATISTICS IO OFF;
GO


CREATE NONCLUSTERED INDEX IX_Sales_LargeOrders
ON Sales (Product_code, Quantity_ordered_products)
WHERE Quantity_ordered_products > 5;

SET STATISTICS IO ON;
SELECT Product_code, Quantity_ordered_products 
FROM Sales 
WHERE Quantity_ordered_products = 10; 
SET STATISTICS IO OFF;
GO


SELECT 
    i.name AS IndexName,
    stats.avg_fragmentation_in_percent,
    stats.page_count
FROM sys.dm_db_index_physical_stats(DB_ID(), OBJECT_ID('Sales'), NULL, NULL, 'DETAILED') stats
JOIN sys.indexes i ON stats.object_id = i.object_id AND stats.index_id = i.index_id;

ALTER INDEX IX_Sales_Date ON Sales REORGANIZE;

ALTER INDEX IX_Sales_Date ON Sales REBUILD;
GO


CREATE NONCLUSTERED INDEX IX_Customer_FillDemo
ON Customers (Customer)
WITH (FILLFACTOR = 80, DROP_EXISTING = OFF);

SELECT name, fill_factor 
FROM sys.indexes 
WHERE object_id = OBJECT_ID('Customers') AND name = 'IX_Customer_FillDemo';
GO


CREATE TABLE #LargeSales (
    ID INT IDENTITY(1,1),
    PCode INT,
    SDate DATE
);

WITH Numbers AS (
    SELECT 1 AS n
    UNION ALL
    SELECT n + 1 FROM Numbers WHERE n < 5000
)
INSERT INTO #LargeSales (PCode, SDate)
SELECT 
    (n % 30) + 101, 
    DATEADD(day, -n, GETDATE())
FROM Numbers
OPTION (MAXRECURSION 5000);

CREATE INDEX IX_Temp_PCode ON #LargeSales(PCode);

SELECT PCode, COUNT(*) 
FROM #LargeSales 
WHERE PCode = 105
GROUP BY PCode;

DROP TABLE #LargeSales;
GO