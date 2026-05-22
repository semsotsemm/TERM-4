-- Чек выполняется до триггера -- 
use UNIVER;
go

update TEACHER 
	SET GENDER = 'SS' 
	WHERE TEACHER = 'КРЛВ';


SELECT *
	FROM TR_AUDIT;