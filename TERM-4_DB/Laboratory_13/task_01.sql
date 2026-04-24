use UNIVER;
go

set nocount on

CREATE PROCEDURE PSUBJECT
as begin 
	declare @line_counter int = (SELECT count(*) FROM SUBJECT);
	SELECT * 
		FROM SUBJECT;
	return @line_counter;
end;

declare @line_count int;
EXEC @line_count = PSUBJECT;
print('Количество строк в результирующем наборе: ' + cast(@line_count as nvarchar(3)));