-- Создать несколько триггеров делит и изменить порядок их выполения --
USE UNIVER;
GO

-- 1 -- 
CREATE or ALTER TRIGGER TR_TEACHER_DEL1 on TEACHER after DELETE as 
BEGIN
    SET NOCOUNT ON;
    print('делит 1');

    INSERT into TR_AUDIT(STMT, TRNAME, CC)
    SELECT 
        'DEL', 
        'TR_TEACHER_DEL1', 
        CONCAT('Код: ', TEACHER, ', имя: ', TEACHER_NAME, ', пол: ', GENDER, ', кафедра: ', PULPIT, ';')
    FROM deleted;
END;
go

-- 2 --
CREATE or ALTER Trigger TR_TEACHER_DEL2 on TEACHER after DELETE as
BEGIN
    SET NOCOUNT ON;
    print('делит 2');

    INSERT into TR_AUDIT(STMT, TRNAME, CC)
    SELECT 
        'DEL', 
        'TR_TEACHER_DEL2', 
        CONCAT('Код: ', TEACHER, ', имя: ', TEACHER_NAME, ', пол: ', GENDER, ', кафедра: ', PULPIT, ';')
    FROM deleted;
END;
go

-- 3 --
CREATE or ALTER Trigger TR_TEACHER_DEL3 on TEACHER after DELETE as
BEGIN
    SET NOCOUNT ON;
    print('делит 3');

    INSERT into TR_AUDIT(STMT, TRNAME, CC)
    SELECT 
        'DEL', 
        'TR_TEACHER_DEL3', 
        CONCAT('Код: ', TEACHER, ', имя: ', TEACHER_NAME, ', пол: ', GENDER, ', кафедра: ', PULPIT, ';')
    FROM deleted;
END;
go 

-- Все делит триггеры -- 
SELECT t.name, e.type_desc 
    FROM sys.triggers t 
    JOIN sys.trigger_events e 
    on t.object_id = e.object_id  
    WHERE OBJECT_NAME(t.parent_id) = 'TEACHER' and e.type_desc = 'DELETE';
    go


-- Изменяем порядок триггеров -- 
EXEC sp_settriggerorder 
    @triggername = 'TR_TEACHER_DEL3', 
    @order = 'First',                 
    @stmttype = 'DELETE';             
go

EXEC sp_settriggerorder 
    @triggername = 'TR_TEACHER_DEL1',
    @order = 'Last',                 
    @stmttype = 'DELETE';            