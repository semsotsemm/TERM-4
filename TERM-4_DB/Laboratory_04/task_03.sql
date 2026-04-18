USE UNIVER;
GO

SELECT 
    F.FACULTY_NAME AS [Факультет],
    P.PULPIT AS [Кафедра], 
    PR.PROFESSION_NAME AS [Специальность], 
    S.SUBJECT_NAME AS [Дисциплина], 
    ST.NAME AS [Студент], 
    CASE 
        WHEN PRG.NOTE = 6 THEN 'Шесть'
        WHEN PRG.NOTE = 7 THEN 'Семь'
        WHEN PRG.NOTE = 8 THEN 'Восемь'
        ELSE CAST(PRG.NOTE AS VARCHAR)
    END AS [Оценка]

FROM STUDENT ST INNER JOIN PROGRESS PRG 
        ON ST.IDSTUDENT = PRG.IDSTUDENT
                INNER JOIN SUBJECT S 
        ON PRG.SUBJECT = S.SUBJECT
                INNER JOIN PULPIT P 
        ON S.PULPIT = P.PULPIT
                INNER JOIN GROUPS G
        ON ST.IDGROUP = G.IDGROUP
                INNER JOIN PROFESSION PR
        ON G.PROFESSION = PR.PROFESSION
                INNER JOIN FACULTY F 
        ON G.FACULTY = F.FACULTY WHERE PRG.NOTE BETWEEN 6 AND 8; 