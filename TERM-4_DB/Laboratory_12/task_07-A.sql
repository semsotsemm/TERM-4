-- SERIALIZABLE -- 
use UNIVER;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

DECLARE @quantity_aud int;

BEGIN TRAN;
    set @quantity_aud = (SELECT COUNT(*) FROM AUDITORIUM WHERE AUDITORIUM_TYPE = 'ЛК');
    print('Количество лекционных аудиторий до изменений: ' +  cast(@quantity_aud as varchar(3)));

    WAITFOR DELAY '00:00:10';

    set @quantity_aud = (SELECT COUNT(*) FROM AUDITORIUM WHERE AUDITORIUM_TYPE = 'ЛК');
    print('Количество лекционных аудиторий после изменений: ' +  cast(@quantity_aud as varchar(3)));
commit;