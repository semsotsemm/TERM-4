use UNIVER;
go

CREATE function FSUBJECTS(@p varchar(20)) returns varchar(300)
	as begin
		declare @subject varchar(20);
		declare @subjects_of_pulpit varchar(300) = 'Дисциплины: ';
		declare CSubject CURSOR LOCAL
			FOR SELECT subj.SUBJECT
				FROM SUBJECT as subj
				WHERE subj.PULPIT = @p;
		
		open CSubject;
		fetch CSubject into @subject;
		while @@FETCH_STATUS = 0
		begin
			set @subjects_of_pulpit = @subjects_of_pulpit + RTRIM(@subject) + ', ';
			fetch CSubject into @subject;
		end;
		return @subjects_of_pulpit;
	end;

go


ALTER function FSUBJECTS(@p varchar(20)) returns varchar(300)
	as begin
		declare @subject varchar(20);
		declare @subjects_of_pulpit varchar(300) = 'Дисциплины: ';
		declare CSubject CURSOR LOCAL
			FOR SELECT subj.SUBJECT
				FROM SUBJECT as subj
				WHERE subj.PULPIT = @p;
		
		open CSubject;
		fetch CSubject into @subject;
		while @@FETCH_STATUS = 0
		begin
			set @subjects_of_pulpit = @subjects_of_pulpit + RTRIM(@subject) + ', ';
			fetch CSubject into @subject;
		end;
		return @subjects_of_pulpit;
	end;
go

SELECT DISTINCT subj.PULPIT as 'Факультет', dbo.FSUBJECTS(subj.PULPIT) as 'Изучаемые дисциплины'
		FROM SUBJECT as subj;

DROP function if exists dbo.FSUBJECTS;
