-- Неявная транзакция -- 

use UNIVER;
go

SET NOCOUNT ON;

if exists(SELECT * FROM sys.objects where OBJECT_ID = object_id(N'DBO.TEST_TABLE'))
	begin 
		DROP Table TEST_TABLE;
		print ('Таблица TEST_TABLE была удалена');
	end
else
	print ('Таблица TEST_TABLE не найдена');


declare @line_counts int, @flag char = 'c';

SET IMPLICIT_TRANSACTIONS ON;

CREATE Table TEST_TABLE(table_key int, table_value nvarchar(10));
INSERT TEST_TABLE(table_key, table_value)
		Values  (1, 'a'),
				(2, 'ala'),
				(3, 'alala'),
				(4, 'alalala');

set @line_counts = (SELECT count(*) FROM TEST_TABLE);
print('Количество строк в таблицу TEST_TABLE: ' + cast(@line_counts as varchar(5)));

if @flag = 'c'
	commit;
else
	rollback;
	
SET IMPLICIT_TRANSACTIONS OFF;

if exists(SELECT * FROM sys.objects where OBJECT_ID = object_id(N'DBO.TEST_TABLE'))
	print('Таблица TEST_TABLE существует')
else
	print('Таблицы TEST_TABLE не существует')