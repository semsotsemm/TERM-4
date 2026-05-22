-- Мульти триггер (инсерт, апдейт, делит) --
use UNIVER;
go

CREATE Trigger TR_TEACHER on TEACHER after INSERT, UPDATE, DELETE as 
	declare @teacher_code varchar(10), @teacher_name varchar(40), @teacher_gender varchar(10), @teacher_pulpit varchar(20), @in varchar(100) ;
	
	-- Количество вставленных и удаленных строк -- 
	declare @inserted int, @deleted int;


	-- Инсерт --
	if @inserted > 0 and @deleted = 0
		begin 
		print('Операция вставки (2)');
	
		set @teacher_code = (select TEACHER from inserted);
		set @teacher_name = (select TEACHER_NAME from inserted);
		set @teacher_gender = (select GENDER from inserted);
		set @teacher_pulpit = (select PULPIT from inserted);

		set @in =  'Код: ' + @teacher_code + ', имя: ' + @teacher_name + ', пол: ' + @teacher_gender + ', кафедра: ' + @teacher_pulpit + ';'

		insert into TR_AUDIT(STMT, TRNAME, CC)
					Values('INS', 'TR_TEACHER', @in)
	end;

	
	-- Делит --
	if @inserted = 0 and @deleted > 0
		begin 
		print('Операция удаления (2)');
	
		set @teacher_code = (select TEACHER from deleted);
		set @teacher_name = (select TEACHER_NAME from deleted);
		set @teacher_gender = (select GENDER from deleted);
		set @teacher_pulpit = (select PULPIT from deleted);

		set @in =  'Код: ' + @teacher_code + ', имя: ' + @teacher_name + ', пол: ' + @teacher_gender + ', кафедра: ' + @teacher_pulpit + ';'

		insert into TR_AUDIT(STMT, TRNAME, CC)
					Values('DEL', 'TR_TEACHER', @in);

	end;

	
	-- Апдейт --
	if @inserted > 0 and @deleted > 0
		begin 
		print('Операция обновления данных (2)');

		set @teacher_code = (select TEACHER from inserted);
		set @teacher_name = (select TEACHER_NAME from inserted);
		set @teacher_gender = (select GENDER from inserted);
		set @teacher_pulpit = (select PULPIT from inserted);

		set @in =  'Код: ' + @teacher_code + ', имя: ' + @teacher_name + ', пол: ' + @teacher_gender + ', кафедра: ' + @teacher_pulpit + ';'

		insert into TR_AUDIT(STMT, TRNAME, CC)
				Values('UPD', 'TR_TEACHER', @in);

	end;
	return;

	
-- Тест -- 

INSERT into TEACHER(TEACHER, TEACHER_NAME, GENDER, PULPIT)
			Values('КРЛВ1', 'Крылов Павел Павлович1', 'м', 'ИСиТ');

UPDATE TEACHER SET
	TEACHER_NAME = 'Крылов Павел Петрович2', 
    PULPIT = 'ИСиТ'                         
	WHERE TEACHER = 'КРЛВ1';

DELETE FROM TEACHER
	WHERE TEACHER.TEACHER = 'КРЛВ1';

SELECT *
	FROM TR_AUDIT;


-- Очистка --
DROP Trigger if exists TR_TEACHER;