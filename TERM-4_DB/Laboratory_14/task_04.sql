use UNIVER;
go

CREATE function FCTEACHER(@pulpit_code varchar(10) = NULL) returns int
	as begin
		declare @teacher_count int;
		set @teacher_count = (
			SELECT count(*) 
				FROM TEACHER as teach
				WHERE (@pulpit_code is null or teach.PULPIT = @pulpit_code));
		return @teacher_count;
	end;
go

SELECT pul.PULPIT_NAME as 'Кафедра', dbo.FCTEACHER(pul.PULPIT) as 'Количество преподавателей на кафедре'
	FROM PULPIT as pul;

SELECT dbo.FCTEACHER(NULL) as 'Всего преподавателей';

go
DROP function if exists dbo.FCTEACHER;