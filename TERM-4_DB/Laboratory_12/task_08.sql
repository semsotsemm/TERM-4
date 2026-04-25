use UNIVER;
go

set nocount on;

print('Уровень вложенности c начала: ' + CAST(@@TRANCOUNT as varchar));

BEGIN TRANSACTION OuterTran;
    print('Внешняя транзакция: ' + CAST(@@TRANCOUNT as varchar));

    INSERT INTO PULPIT (PULPIT, PULPIT_NAME, FACULTY)
                Values ('ИТС', 'Информационные технологии и системы', 'ИТ');

    BEGIN TRANSACTION InnerTran;
        print('Открыта вложенная транзакция: ' + CAST(@@TRANCOUNT as varchar));

        INSERT INTO AUDITORIUM (AUDITORIUM, AUDITORIUM_TYPE, AUDITORIUM_CAPACITY, AUDITORIUM_NAME)
                        Values ('100-1', 'ЛК', 50, 'Вложенная аудитория');

        COMMIT TRANSACTION InnerTran;
        print('Выполнен commit вложенной транзакции: ' + CAST(@@TRANCOUNT as varchar));

    SELECT * 
        FROM PULPIT WHERE PULPIT = 'ИТС';
    SELECT * 
        FROM AUDITORIUM WHERE AUDITORIUM = '100-1';

print('Откатываем внешнюю транзакцию');
ROLLBACK TRANSACTION OuterTran; 

print('После rollback внешней транзакции: ' + CAST(@@TRANCOUNT as varchar));

SELECT 'Кафедра' as Entity, PULPIT 
    FROM PULPIT WHERE PULPIT = 'ИТС'
UNION ALL
SELECT 'Аудитория', AUDITORIUM 
    FROM AUDITORIUM WHERE AUDITORIUM = '100-1';