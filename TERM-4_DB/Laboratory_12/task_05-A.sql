use UNIVER;
go

DECLARE @capacity_301 int;
DECLARE @quantity_aud int;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
BEGIN TRAN;
	set @capacity_301 = (SELECT AUDITORIUM_CAPACITY FROM AUDITORIUM WHERE AUDITORIUM = '301-1'); 
	set @quantity_aud = (SELECT count(*) FROM AUDITORIUM); 
	print('Вместимость аудитории 301-1 до изменения ' + cast(@capacity_301 as varchar(3)))
	print('Количество аудиторий до изменения: ' + cast(@quantity_aud as varchar(3)))

	WAITFOR DELAY '00:00:10';

	set @capacity_301 = (SELECT AUDITORIUM_CAPACITY FROM AUDITORIUM WHERE AUDITORIUM = '301-1');
	set @quantity_aud = (SELECT count(*) FROM AUDITORIUM); 
	print('Вместимость аудитории 301-1 после изменения: ' + cast(@capacity_301 as varchar(3)))
	print('Количество аудиторий после изменения: ' + cast(@quantity_aud as varchar(3)))
COMMIT;