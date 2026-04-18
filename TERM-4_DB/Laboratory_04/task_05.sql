SELECT P.PULPIT_NAME[Кафедра], isnull(T.TEACHER_NAME, '?')[ФИО преподавателя]
	FROM PULPIT P full outer join TEACHER T
	on P.PULPIT = T.PULPIT;

SELECT P.PULPIT_NAME[Кафедра], isnull(T.TEACHER_NAME, '?')[ФИО преподавателя]
	FROM TEACHER T full outer join PULPIT P
	on P.PULPIT = T.PULPIT;



SELECT P.PULPIT_NAME[Кафедра], isnull(T.TEACHER_NAME, '?')[ФИО преподавателя]
	FROM PULPIT P left outer join TEACHER T
	on T.PULPIT = P.PULPIT
	WHERE T.TEACHER_NAME is null



SELECT P.PULPIT_NAME[Кафедра], T.TEACHER_NAME[ФИО преподавателя]
	FROM PULPIT P right outer join  TEACHER T
	on T.PULPIT = P.PULPIT
	WHERE P.PULPIT is null



SELECT P.PULPIT_NAME[Кафедра], T.TEACHER_NAME[ФИО преподавателя]
	FROM PULPIT P full outer join TEACHER T 
	on T.PULPIT = P.PULPIT
	WHERE T.TEACHER_NAME is not null 