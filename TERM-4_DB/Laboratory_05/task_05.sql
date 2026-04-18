use UNIVER;
go

SELECT fac.FACULTY_NAME [Наименование факультета без кафедр] 
	FROM FACULTY as fac
	WHERE not exists (
	SELECT *  FROM PULPIT as pul
		WHERE pul.FACULTY = fac.FACULTY
	)