USE UNIVER;
GO

SELECT MAX(pt.[22]) as [2], MAX(pt.[23]) as [3], MAX(pt.[28]) as [8], pt.[Номер пары], pt.[День недели]
	FROM (
		SELECT CONCAT(t.SUBJECT, t.TEACHER, t.AUDITORIUM) as [Информация], t.IDGROUP as [Группа], t.DAY_OF_WEEK[День недели], t.LESSON_TIME[Номер пары]
			FROM TIMETABLE as t
			) as st
		pivot (
			MAX(st.Информация)
			FOR [Группа] IN ([22], [23], [28])
			) as pt
		GROUP BY [День недели], [Номер пары]
		ORDER BY [День недели], [Номер пары];