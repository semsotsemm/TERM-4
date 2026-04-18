use UNIVER;
go

CREATE VIEW Аудитории
	as SELECT a.AUDITORIUM as [Код аудитории], a.AUDITORIUM_TYPE as [Тип аудитории], a.AUDITORIUM_NAME as [Наименование аудитори]
		FROM AUDITORIUM as a
		WHERE a.AUDITORIUM_TYPE = 'ЛК';

SELECT * 
	FROM Аудитории;

-- Демонстранция удаления --

DELETE FROM Аудитории
	WHERE Аудитории.[Код аудитории] = '101';

-- Очистка -- 

DROP VIEW Аудитории;