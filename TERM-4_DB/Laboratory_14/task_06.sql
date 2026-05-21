use UNIVER;
go

CREATE function GET_COUNT_PULPITS(@faculty varchar(10)) returns int
	as begin
		declare @count_of_pulpits int;
		set @count_of_pulpits = (
			SELECT COUNT(pul.PULPIT)
				FROM PULPIT as pul 
				WHERE pul.FACULTY = @faculty);
		return @count_of_pulpits;
	end;

go

CREATE function GET_COUNT_GROUPS(@faculty varchar(10)) returns int
	as begin
		declare @count_of_groups int;
		set @count_of_groups = (
			SELECT COUNT(gr.IDGROUP)
				FROM GROUPS as gr 
				WHERE gr.FACULTY = @faculty);
		return @count_of_groups;
	end;

go

CREATE function GET_COUNT_PROFESSIONS(@faculty varchar(10)) returns int
	as begin
		declare @count_of_professions int;
		set @count_of_professions = (
			SELECT COUNT(prof.PROFESSION)
				FROM PROFESSION as prof 
				WHERE prof.FACULTY = @faculty);
		return @count_of_professions;
	end;

go

create function FACULTY_REPORT(@c int) returns @fr table
	([Факультет] varchar(50), [Количество кафедр] int, [Количество групп] int, [Количество студентов] int, [Количество специальностей] int )
	as begin 
		declare cc CURSOR static 
		for select FACULTY 
			from FACULTY 
			where dbo.COUNT_STUDENT(FACULTY, default) > @c; 
		declare @f varchar(30);

		open cc;  
		fetch cc into @f;
		while @@fetch_status = 0
		begin
			insert @fr values( @f,  
			dbo.GET_COUNT_PULPITS(@f),
			dbo.GET_COUNT_GROUPS(@f),
			dbo.COUNT_STUDENT(@f, default),
			dbo.GET_COUNT_PROFESSIONS(@f));
			fetch cc into @f;  
		end;   
		return; 
	end;

go 

SELECT *
	FROM FACULTY_REPORT(2);

go

DROP function if exists dbo.FACULTY_REPORT;
DROP function if exists dbo.GET_COUNT_PULPITS;
DROP function if exists dbo.GET_COUNT_GROUPS;
DROP function if exists dbo.GET_COUNT_PROFESSIONS;