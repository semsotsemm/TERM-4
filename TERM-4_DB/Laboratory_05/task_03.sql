use UNIVER;
go

SELECT DISTINCT PULPIT_NAME [Наименование кафедры], fac.FACULTY_NAME [Название факультета]  
	FROM FACULTY as fac inner join PULPIT as pul
	on pul.FACULTY = fac.FACULTY
	inner join PROFESSION as prof
	on prof.FACULTY = fac.FACULTY
	where prof.PROFESSION_NAME like '%технологии%' or prof.PROFESSION_NAME like '%технология%';