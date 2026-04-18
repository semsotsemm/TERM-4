use UNIVER;
go

DECLARE @student_name char(30), @group int, @students char(300) = '';
DECLARE Group_22 CURSOR GLOBAL for
	SELECT s.NAME, s.IDGROUP
		FROM STUDENT as s;

OPEN Group_22;
	FETCH Group_22 into @student_name, @group;
	print('Студенты 22 группы:');

	while @@FETCH_STATUS = 0 begin
		if @group = 22 begin 
			set @students = rtrim(@student_name) + ', ' + @students;
		end;
		FETCH Group_22 into @student_name, @group;
	end;

	print(@students);
	go

	if CURSOR_STATUS('global', 'Group_22') >= 0 begin
		print ('Курсор Group_22 еще существует');
	end;
	else begin
		print ('Курсора Group_22 не существует');
	end;
CLOSE Group_22;

-- Удаление глобального курсора
DEALLOCATE Group_22;


-- Локальный курсор
print(char(13) + char(10) + '---- Локальный курсор ----')
DECLARE @student_name char(30), @group int, @students char(300) = '';
DECLARE Group_22 CURSOR local for
	SELECT s.NAME, s.IDGROUP
		FROM STUDENT as s;

OPEN Group_22;
	FETCH Group_22 into @student_name, @group;
	print('Студенты 22 группы:');

	while @@FETCH_STATUS = 0 begin
		if @group = 22 begin 
			set @students = rtrim(@student_name) + ', ' + @students;
		end;
		FETCH Group_22 into @student_name, @group;
	end;

	print(@students);
go

-- Закрывается после го (отправляем пакет)

if CURSOR_STATUS('local', 'Group_22') >= -1 begin
	print ('Курсор Group_22 еще существует');
end;
else begin
	print ('Курсора Group_22 не существует');
end;
-- Удаление не нужно