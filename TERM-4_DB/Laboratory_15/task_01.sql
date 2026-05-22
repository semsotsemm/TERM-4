-- Инсерт триггер для таблицы teacher -- 
USE UNIVER;
go

CREATE Table TR_AUDIT
(
	ID int identity, 
	STMT varchar(20) check (STMT in ('INS', 'DEL', 'UPD')),
	TRNAME varchar(50),
	CC varchar(300)
);

go

CREATE Trigger TR_TEACHER_INS on TEACHER after INSERT as
	print('Операция вставки');

	insert into TR_AUDIT(STMT, TRNAME, CC)
				Values('INS', 'TR_TEACHER_INS', '')
	return;

go

ALTER Trigger TR_TEACHER_INS on TEACHER after INSERT as 
	declare @teacher_code varchar(10), @teacher_name varchar(40), @teacher_gender varchar(10), @teacher_pulpit varchar(20), @in varchar(100) ;
	
	print('Операция вставки');
	
	set @teacher_code = (select TEACHER from inserted);
	set @teacher_name = (select TEACHER_NAME from inserted);
	set @teacher_gender = (select GENDER from inserted);
	set @teacher_pulpit = (select PULPIT from inserted);

	set @in =  'Код: ' + @teacher_code + ', имя: ' + @teacher_name + ', пол: ' + @teacher_gender + ', кафедра: ' + @teacher_pulpit + ';'

	insert into TR_AUDIT(STMT, TRNAME, CC)
				Values('INS', 'TR_TEACHER_INS', @in)
	return;

go

-- Тест --

INSERT into TEACHER(TEACHER, TEACHER_NAME, GENDER, PULPIT)
			Values('КРЛВ1', 'Крылов Павел Павлович1', 'м', 'ИСиТ');

SELECT *
	FROM TEACHER;

SELECT *
	FROM TR_AUDIT;

-- Очистка --
DROP Trigger if exists TR_TEACHER_INS;