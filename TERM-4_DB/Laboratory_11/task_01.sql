use UNIVER;
go

DECLARE ISIT_Subjects CURSOR for
	SELECT s.SUBJECT, s.PULPIT 
		FROM SUBJECT as s;	
DECLARE @subj char(20), @pulp char(20), @subjs char(300) = '';

OPEN ISIT_Subjects;
	FETCH ISIT_Subjects into @subj, @pulp;
	print('Дисциплины ИСИТа: ');
	while @@FETCH_STATUS = 0 begin
		if @pulp = 'ИСИТ' begin
			set @subjs = RTRIM(@subj) + ', ' + @subjs;
		end;
		FETCH ISIT_Subjects into @subj, @pulp;
		end;
	print(@subjs)
CLOSE ISIT_Subjects;

-- Удаление курсора
DEALLOCATE ISIT_Subjects;