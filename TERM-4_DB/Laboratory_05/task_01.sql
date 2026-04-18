use UNIVER;
go

SELECT PULPIT_NAME [Наименование кафедры], fac.FACULTY_NAME [Название факультета]  
	FROM FACULTY as fac join PULPIT as pul
	on fac.FACULTY = pul.FACULTY
	WHERE fac.FACULTY in (
	SELECT FACULTY
		FROM PROFESSION as prof
		WHERE prof.PROFESSION_NAME like '%технологии%' or prof.PROFESSION_NAME like '%технология%' 
	);