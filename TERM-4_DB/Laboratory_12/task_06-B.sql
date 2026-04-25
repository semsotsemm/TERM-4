use UNIVER;
go

set nocount on;

INSERT into AUDITORIUM (AUDITORIUM, AUDITORIUM_TYPE, AUDITORIUM_CAPACITY, AUDITORIUM_NAME)
				Values ('777-7', 'ЛК', 99, 'Новая');

WAITFOR DELAY '00:00:10';

-- Очистка -- 
DELETE FROM AUDITORIUM  WHERE AUDITORIUM_NAME = 'Новая';

-- B -- 
SET TRANSACTION ISOLATION LEVEL READ COMMITTED ;

DECLARE @quantity_aud int;

BEGIN TRAN;
    set @quantity_aud = (SELECT COUNT(*) FROM AUDITORIUM WHERE AUDITORIUM_TYPE = 'ЛК');
    print('Количество лекционных аудиторий до изменений: ' +  cast(@quantity_aud as varchar(3)));

    WAITFOR DELAY '00:00:10';

    set @quantity_aud = (SELECT COUNT(*) FROM AUDITORIUM WHERE AUDITORIUM_TYPE = 'ЛК');
    print('Количество лекционных аудиторий после изменений: ' +  cast(@quantity_aud as varchar(3)));
COMMIT;