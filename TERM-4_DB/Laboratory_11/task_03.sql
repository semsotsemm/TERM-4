use UNIVER;
go

-- Статический курсор
print(char(13) + char(10) + '---- Статический курсор ----')
DECLARE @student_name char(30), @group int, @students char(300) = '';
DECLARE Group_22 CURSOR local static for
	SELECT s.NAME, s.IDGROUP
		FROM STUDENT as s;

OPEN Group_22;
	FETCH Group_22 into @student_name, @group;
	print('Студенты 22 группы:');

	-- Добавляем нового студента
	INSERT into STUDENT (NAME, IDGROUP, BDAY)
				 Values ('Алексей Навальный', 22, '2005-01-01');
	print ('(Добавлен Алексей Навальный)')

	while @@FETCH_STATUS = 0 begin
		if @group = 22 begin 
			set @students = rtrim(@student_name) + ', ' + @students;
		end;
		FETCH Group_22 into @student_name, @group;
	end;

	-- Удаляем чтобы не мешал
	print(@students);
	DELETE FROM STUDENT WHERE NAME = 'Алексей Навальный';
	print('(Удален Алексей Навальный)');
go

-- Динамический курсор
print(char(13) + char(10) + '---- Динамический курсор ----')
DECLARE @student_name char(30), @group int, @students char(300) = '';
DECLARE Group_22 CURSOR local dynamic for
	SELECT s.NAME, s.IDGROUP
		FROM STUDENT as s;


OPEN Group_22;
	FETCH Group_22 into @student_name, @group;
	print('Студенты 22 группы:');

	-- Добавляем нового студента
	INSERT into STUDENT (NAME, IDGROUP, BDAY)
				 Values ('Алексей Навальный', 22, '2005-01-01');
	print ('(добавлен Алексей Навальный)')

	while @@FETCH_STATUS = 0 begin
		if @group = 22 begin 
			set @students = rtrim(@student_name) + ', ' + @students;
		end;
		FETCH Group_22 into @student_name, @group;
	end;

	-- Удаляем чтобы не мешал
	print(@students);
	DELETE FROM STUDENT WHERE NAME = 'Алексей Навальный';
	print('(Удален Алексей Навальный)');
go