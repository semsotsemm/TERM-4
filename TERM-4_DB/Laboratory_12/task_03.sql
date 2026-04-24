-- Контрольная точка в явной транзакции --

use UNIVER;
go

DECLARE @SavePoint nvarchar(20) = 'SavePoint';

set nocount on;

begin try
	begin tran
		INSERT into TEACHER(TEACHER, TEACHER_NAME, GENDER, PULPIT)
					Values('БПЛБ', 'Белодед Николай Иванович', null, 'ИСиТ');
		save tran @SavePoint;
		INSERT into TEACHER(TEACHER, TEACHER_NAME, GENDER, PULPIT)
					Values('БПЛБ', 1, null, 'ПИ');
	commit tran;	
end try
begin catch
    print('Ошибка ' + cast(error_number() as varchar(5)) + ': ' + error_message())
    print('Откат к контрольной точке')
    
    if @@trancount > 0 begin
        ROLLBACK TRANSACTION @SavePoint;
        COMMIT TRANSACTION;             
    end;

    DECLARE @beloded_count int = (SELECT count(*)
        FROM TEACHER as t
        WHERE t.TEACHER_NAME = 'Белодед Николай Иванович');
    print('Количество Белодедов: ' + CAST(@beloded_count as varchar(3)));
    DELETE FROM TEACHER where TEACHER_NAME = 'Белодед Николай Иванович'
end catch;