use UNIVER;
go

SELECT aud_t.AUDITORIUM_TYPENAME[Тип аудитории], MIN(aud.AUDITORIUM_CAPACITY) [Минимальная вместимость], MAX(aud.AUDITORIUM_CAPACITY) [Максимальная вместимость], AVG(aud.AUDITORIUM_CAPACITY) [Средняя вместимость], SUM(aud.AUDITORIUM_CAPACITY) [Суммарная вместимость], COUNT(*)[Количество аудиторий данного типа]
	FROM AUDITORIUM as aud inner join AUDITORIUM_TYPE as aud_t
	on aud.AUDITORIUM_TYPE = aud_t.AUDITORIUM_TYPE
	GROUP BY aud_t.AUDITORIUM_TYPENAME;