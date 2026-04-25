use UNIVER;
go

set nocount on;

UPDATE AUDITORIUM SET AUDITORIUM_CAPACITY = 20 WHERE AUDITORIUM = '301-1';
INSERT INTO AUDITORIUM (AUDITORIUM, AUDITORIUM_TYPE, AUDITORIUM_CAPACITY, AUDITORIUM_NAME)
				Values ('555-5', 'ЛК', 100, 'Новая аудитория');

WAITFOR DELAY '00:00:10';

-- Очистка --
UPDATE AUDITORIUM SET AUDITORIUM_CAPACITY = 15 WHERE AUDITORIUM = '301-1';
DELETE FROM AUDITORIUM WHERE AUDITORIUM_NAME = 'Новая аудитория';

-- B --
DECLARE @capacity_301 int;
DECLARE @quantity_aud int;

SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
BEGIN TRAN;
	set @capacity_301 = (SELECT AUDITORIUM_CAPACITY FROM AUDITORIUM WHERE AUDITORIUM = '301-1'); 
	set @quantity_aud = (SELECT count(*) FROM AUDITORIUM); 
	print('Вместимость аудитории 301-1 до изменения ' + cast(@capacity_301 as varchar(3)))
	print('Количество аудиторий до изменения: ' + cast(@quantity_aud as varchar(3)))
COMMIT;