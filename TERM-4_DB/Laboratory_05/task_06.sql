USE UNIVER;

SELECT top(1)(
	SELECT avg(progr.NOTE) 
		FROM PROGRESS as progr
		WHERE progr.SUBJECT = 'ад'), 
	(SELECT avg(progr.NOTE) 
		FROM PROGRESS as progr
		WHERE progr.SUBJECT = 'нюхо'),
	(SELECT avg(progr.NOTE) 
		FROM PROGRESS as progr
		WHERE progr.SUBJECT = 'ясад')
	FROM PROGRESS;