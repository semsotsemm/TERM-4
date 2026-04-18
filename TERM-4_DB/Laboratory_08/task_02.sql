use UNIVER;
go

CREATE VIEW [Количество кафедр]
	as SELECT f.FACULTY_NAME as Факультет, COUNT(p.PULPIT) as [Количество кафедр]
		FROM FACULTY as f
		inner join PULPIT as p
		on f.FACULTY = p.FACULTY
		GROUP BY f.FACULTY_NAME;
go

SELECT *
	FROM [Количество кафедр] ORDER BY [Количество кафедр] desc;

-- Очистка -- 
DROP VIEW [Количество кафедр];