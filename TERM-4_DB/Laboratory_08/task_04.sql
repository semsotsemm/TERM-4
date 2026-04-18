use UNIVER;
go

CREATE VIEW [Лекционные аудитории] ([Код аудитории], [Тип аудитории], [Наименование аудитори])
	as SELECT a.AUDITORIUM, a.AUDITORIUM_TYPE, a.AUDITORIUM_NAME
		FROM AUDITORIUM as a
		WHERE a.AUDITORIUM_TYPE = 'ЛК';

SELECT * 
	FROM [Лекционные аудитории];

-- Очистка -- 

DROP VIEW [Лекционные аудитории];