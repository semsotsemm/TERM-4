-- Изменение процедуры для вставки в временную таблицу результата --

USE [UNIVER]
GO

ALTER PROCEDURE [dbo].[PSUBJECT]
    @p varchar(20) = NULL
AS BEGIN 
    SET NOCOUNT ON;

    IF @p IS NULL begin
        SELECT * 
            FROM SUBJECT;
    END
    ELSE begin
        SELECT * 
            FROM SUBJECT WHERE PULPIT = @p;
    END
END;
GO

set nocount on;

IF OBJECT_ID('tempdb..#SUBJECT') IS NOT NULL
    DROP TABLE #SUBJECT; 

CREATE TABLE #SUBJECT
(
    SUBJECT      CHAR(10),
    SUBJECT_NAME VARCHAR(100),
    PULPIT       CHAR(20)
);

INSERT INTO #SUBJECT
    EXEC [dbo].[PSUBJECT] @p = NULL; 
    
print('Содержимое временной таблицы SUBJECT:');
SELECT * 
    FROM #SUBJECT;