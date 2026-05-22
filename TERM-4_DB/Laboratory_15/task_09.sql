USE UNIVER;
go

CREATE or ALTER Trigger DDL_TEACHER on database for DDL_DATABASE_LEVEL_EVENTS as 
	begin
		declare @t VARCHAR(100)  = EVENTDATA().value('(/EVENT_INSTANCE/EventType)[1]', 'varchar(100)');
		declare @obj VARCHAR(100) = EVENTDATA().value('(/EVENT_INSTANCE/ObjectName)[1]', 'varchar(100)');
		declare @type VARCHAR(100) = EVENTDATA().value('(/EVENT_INSTANCE/ObjectType)[1]', 'varchar(100)');

		if @obj = 'TEACHER'
		begin
			print ('Тип события: ' + @t);
			print ('Имя объекта: ' + @obj);
			print ('Тип объекта: ' + @type);
			RAISERROR(N'Операции с таблицей TEACHER запрещены', 16, 1); 
			rollback;
		end;
	end;

go


BEGIN TRY
	ALTER TABLE TEACHER 
		ADD TEST_COLUMN INT;
END TRY
BEGIN CATCH
	print(ERROR_MESSAGE());
END CATCH;

go

-- Очистка --
DROP TRIGGER DDL_TEACHER on DATABASE;