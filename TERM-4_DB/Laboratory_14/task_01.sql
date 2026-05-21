-- Функция считает количество студентов на факультете x специальности y --

use UNIVER;
go

create function COUNT_STUDENT(@faculty varchar(20)) returns int
	as begin
	declare @count_of_student int = 0;
	set @count_of_student = (
		SELECT count(*)
			FROM STUDENT as st 
			inner join GROUPS as gr 
			on st.IDGROUP = gr.IDGROUP
			inner join FACULTY as fac 
			on fac.FACULTY = gr.FACULTY
			where fac.FACULTY = @faculty);
	return @count_of_student;
	end;

go

alter function COUNT_STUDENT(@faculty varchar(20) = null, @prof varchar(20) = null) returns int
	as begin
	declare @count_of_student int = 0;
	set @count_of_student = (
		SELECT count(*)
			FROM STUDENT as st 
			inner join GROUPS as gr 
			on st.IDGROUP = gr.IDGROUP
			inner join FACULTY as fac 
			on fac.FACULTY = gr.FACULTY
			where 
				(@faculty is null or fac.FACULTY = @faculty)
					and
				(@prof is null or gr.PROFESSION = @prof));
	return @count_of_student;
	end;

go

DECLARE @count_of_student_ief int = dbo.COUNT_STUDENT('ИЭФ');
PRINT('Количество студентов ИЭФ: ' + CAST(@count_of_student_ief AS varchar(4))); 


DECLARE @count_of_student_lxf1 int = dbo.COUNT_STUDENT('ЛХФ', NULL);
DECLARE @count_of_student_lxf2 int = dbo.COUNT_STUDENT('ЛХФ', '1-89 02 02');

PRINT('Общее количество студентов ЛХФ: ' + CAST(@count_of_student_lxf1 AS varchar(4)));
PRINT('Количество студентов ЛФХ с кодом специальности 1-89 02 02: ' + CAST(@count_of_student_lxf2 AS varchar(4))); 

SELECT *
	FROM STUDENT as st 
	inner join GROUPS as gr 
	on st.IDGROUP = gr.IDGROUP
	inner join FACULTY as fac 
	on fac.FACULTY = gr.FACULTY;

DROP function if exists dbo.COUNT_STUDENT;