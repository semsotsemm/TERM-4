use UNIVER;
go

set nocount on;

DECLARE @beloded_count int = (select count(*) FROM TEACHER WHERE TEACHER_NAME like '%Белодед%');
DECLARE @all_count int = (select count(*) FROM TEACHER);

set transaction isolation level READ UNCOMMITTED;

print('Грязное чтение:');
print('Количество белодедов: ' + CAST(@beloded_count as nvarchar(3)));

print('Неповторяющееся чтение:');
SELECT * FROM TEACHER;
print('Фантомное чтение: ');
print('Количество преподавателей, до ролбека: ' + CAST(@all_count as nvarchar(3)));
WAITFOR DELAY '00:00:10';
print('Количество преподавателей, после ролбека: ' + CAST(@all_count as nvarchar(3)));