SELECT P.PULPIT_NAME [Кафедра], isnull(T.TEACHER_NAME, '**') [Преподаватель]
	FROM PULPIT P Left Outer Join TEACHER T 
	ON P.PULPIT = T.PULPIT;
