use UNIVER;
go

set nocount on;

DECLARE @beloded_counts int = (select count(*) FROM TEACHER WHERE TEACHER_NAME like '%Белодед%');

set transaction isolation level READ UNCOMMITTED;

print('Количество белодедов: ' + CAST(@beloded_counts as nvarchar(3)));