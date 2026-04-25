-- Создание процедуры -- 

use UNIVER;
go

CREATE PROCEDURE PSUBJECT
as begin 
	declare @line_counter int = (SELECT count(*) FROM SUBJECT);
	SELECT * 
		FROM SUBJECT;
	return @line_counter;
end;
go

set nocount on;

declare @line_count int;
EXEC @line_count = PSUBJECT;
print('Количество строк в результирующем наборе: ' + cast(@line_count as nvarchar(3)));

-- Очистка -- 
DROP PROCEDURE PSUBJECT;