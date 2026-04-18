use UNIVER;
go

SELECT result.interval [Интервал оценок], COUNT(result.quantity)[Количество оценок в интервале]
	FROM(
		SELECT 
			CASE 
				WHEN progr.NOTE between 1 and 3 THEN 'От 1 до 3'
				WHEN progr.NOTE between 4 and 6 THEN 'От 4 до 6'
				WHEN progr.NOTE between 7 and 10 THEN 'От 7 до 10'
				else '0'
				end as interval, 
			COUNT(progr.NOTE) as quantity, progr.NOTE as notes
			FROM PROGRESS as progr
			GROUP BY progr.NOTE
	) as result
	GROUP BY result.interval
	ORDER BY MAX(result.notes) desc;