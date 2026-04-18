use UNIVER;

SELECT * 
	FROM PROGRESS as progr
	WHERE progr.NOTE >= ALL (
		SELECT avg(inner_progr.NOTE) 
			FROM PROGRESS as inner_progr
	)