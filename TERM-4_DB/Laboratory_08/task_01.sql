use UNIVER;
go

CREATE VIEW Преподаватели 
	as SELECT t.TEACHER as [Код преподавателя], t.TEACHER_NAME as [Имя преподавателя], t.GENDER as Пол, t.PULPIT[Код кафедры]
		FROM TEACHER as t;

SELECT *
	FROM Преподаватели;

-- Очистка -- 
DROP VIEW Преподаватели;