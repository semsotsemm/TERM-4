USE UNIVER;
GO

PRINT 'Начальный уровень вложенности: ' + CAST(@@TRANCOUNT AS VARCHAR);

BEGIN TRANSACTION OuterTran;
    PRINT 'Открыта внешняя транзакция. @@TRANCOUNT = ' + CAST(@@TRANCOUNT AS VARCHAR);

    INSERT INTO PULPIT (PULPIT, PULPIT_NAME, FACULTY)
    VALUES ('ИТС', 'Информационные технологии в строительстве', 'ИТ');

    BEGIN TRANSACTION InnerTran;
        PRINT 'Открыта вложенная транзакция. @@TRANCOUNT = ' + CAST(@@TRANCOUNT AS VARCHAR);

        INSERT INTO AUDITORIUM (AUDITORIUM, AUDITORIUM_TYPE, AUDITORIUM_CAPACITY, AUDITORIUM_NAME)
        VALUES ('100-1', 'ЛК', 50, 'Вложенная аудитория');

        COMMIT TRANSACTION InnerTran;
        PRINT 'Выполнен COMMIT вложенной транзакции. @@TRANCOUNT = ' + CAST(@@TRANCOUNT AS VARCHAR);

    SELECT * FROM PULPIT WHERE PULPIT = 'ИТС';
    SELECT * FROM AUDITORIUM WHERE AUDITORIUM = '100-1';

PRINT 'Откатываем ВНЕШНЮЮ транзакцию...';
ROLLBACK TRANSACTION OuterTran; 

PRINT 'После ROLLBACK внешней транзакции. @@TRANCOUNT = ' + CAST(@@TRANCOUNT AS VARCHAR);

SELECT 'Кафедра' as Entity, PULPIT FROM PULPIT WHERE PULPIT = 'ИТС'
UNION ALL
SELECT 'Аудитория', AUDITORIUM FROM AUDITORIUM WHERE AUDITORIUM = '100-1';