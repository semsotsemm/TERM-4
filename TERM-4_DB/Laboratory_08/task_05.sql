use UNIVER;
go

CREATE VIEW Дисциплины ([Код предмета], [Предмет], [Кафедра])
	as SELECT top(150) s.SUBJECT, s.SUBJECT_NAME, s.PULPIT
		FROM SUBJECT as s
		ORDER BY  s.SUBJECT_NAME;

SELECT *
	FROM Дисциплины;

-- Очистка --
DROP VIEW Дисциплины;