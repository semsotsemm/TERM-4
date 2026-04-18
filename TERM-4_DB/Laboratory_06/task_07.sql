use UNIVER;
go

SELECT progr.SUBJECT[Дисциплина], count(progr.NOTE)[Количество оценок (8 или 9)]
	FROM PROGRESS as progr
	GROUP BY progr.SUBJECT, progr.NOTE 
	HAVING progr.NOTE in (8, 9);