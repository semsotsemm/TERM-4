use UNIVER;

SELECT * 
	FROM PROGRESS as progr
	WHERE progr.NOTE >= ANY (
		SELECT avg(inner_progr.NOTE) 
			FROM PROGRESS as inner_progr
	)