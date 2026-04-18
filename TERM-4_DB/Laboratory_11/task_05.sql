USE UNIVER;
GO

DECLARE @name nvarchar(100), @group int;

DECLARE stud_cursor CURSOR LOCAL DYNAMIC FOR
    SELECT NAME, IDGROUP
    FROM STUDENT
    FOR UPDATE; 

OPEN stud_cursor;

FETCH NEXT FROM stud_cursor INTO @name, @group;

WHILE @@FETCH_STATUS = 0
BEGIN
    IF @name = 'Пугач Михаил Трофимович'
    BEGIN
        UPDATE STUDENT 
        SET NAME = 'Пугач М.Т. (Обновлено)'
        WHERE CURRENT OF stud_cursor; 
        
        PRINT 'Обновлено: ' + @name;
    END
    ELSE IF @group = 22
    BEGIN
        DELETE FROM STUDENT 
        WHERE CURRENT OF stud_cursor; 
        
        PRINT 'Удален студент группы 22: ' + @name;
    END

    FETCH NEXT FROM stud_cursor INTO @name, @group;
END

CLOSE stud_cursor;
DEALLOCATE stud_cursor;

SELECT * FROM STUDENT;