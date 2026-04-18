USE UNIVER;
GO

DECLARE @TargetFaculty CHAR(10) = 'ИЭФ';

SELECT 
    S.NAME AS [Студент],
    SUB.SUBJECT_NAME AS [Предмет],
    P.NOTE AS [Балл],
    CASE 
        WHEN P.NOTE BETWEEN 9 AND 10 THEN 'Хорошо'
        WHEN P.NOTE BETWEEN 7 AND 8  THEN 'Нормально'
        WHEN P.NOTE BETWEEN 5 AND 6  THEN 'Плохо'
        WHEN P.NOTE BETWEEN 1 AND 4  THEN 'Неудовлетворительно'
        ELSE 'Оценка не выставлена'
    END AS [Результат анализа]

FROM PROGRESS P
JOIN STUDENT S ON P.IDSTUDENT = S.IDSTUDENT
JOIN GROUPS G ON S.IDGROUP = G.IDGROUP
JOIN SUBJECT SUB ON P.SUBJECT = SUB.SUBJECT
WHERE G.FACULTY = @TargetFaculty
ORDER BY P.NOTE DESC; 
