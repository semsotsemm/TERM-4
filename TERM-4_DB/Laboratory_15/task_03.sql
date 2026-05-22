-- Инсерт триггер -
use UNIVER;
go

CREATE Trigger TR_TEACHER_UPD on TEACHER after UPDATE as
	declare @teacher_code varchar(10), @teacher_name varchar(40), @teacher_gender varchar(10), @teacher_pulpit varchar(20), @in varchar(100) ;
	
	print('Операция обновления данных');

	set @teacher_code = (select TEACHER from inserted);
	set @teacher_name = (select TEACHER_NAME from inserted);
	set @teacher_gender = (select GENDER from inserted);
	set @teacher_pulpit = (select PULPIT from inserted);

	set @in =  'Код: ' + @teacher_code + ', имя: ' + @teacher_name + ', пол: ' + @teacher_gender + ', кафедра: ' + @teacher_pulpit + ';'

	insert into TR_AUDIT(STMT, TRNAME, CC)
			Values('UPD', 'TR_TEACHER_UPD', @in);

	return;

go
	
-- Тест -- 

UPDATE TEACHER SET
	TEACHER_NAME = 'Крылов Павел Петрович2', 
    PULPIT = 'ИСиТ'                         
WHERE TEACHER = 'КРЛВ1';

SELECT *
	FROM TEACHER;

SELECT *
	FROM TR_AUDIT;

-- Очистка --
DROP Trigger if exists TR_TEACHER_UPD;