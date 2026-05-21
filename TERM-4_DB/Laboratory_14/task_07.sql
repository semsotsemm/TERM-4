use UNIVER;
go
-- FSUBJECTS --

CREATE function FSUBJECTS(@p varchar(20)) returns varchar(300)
	as begin
		declare @subject varchar(20);
		declare @subjects_of_pulpit varchar(300) = 'Дисциплины: ';
		declare CSubject CURSOR LOCAL
			FOR SELECT subj.SUBJECT
				FROM SUBJECT as subj
				WHERE subj.PULPIT = @p;
		
		open CSubject;
		fetch CSubject into @subject;
		while @@FETCH_STATUS = 0
		begin
			set @subjects_of_pulpit = @subjects_of_pulpit + RTRIM(@subject) + ', ';
			fetch CSubject into @subject;
		end;
		return @subjects_of_pulpit;
	end;

go
-- FFACPUL --

CREATE function FFACPUL(@faculty varchar(20) = null, @pulpit varchar(20) = null)
	returns table
	as return 
		SELECT fac.FACULTY as 'Код Факультета', fac.FACULTY_NAME 'Факультет',
				pul.PULPIT as 'Код Кафедры', pul.PULPIT_NAME as 'Кафедра'
			FROM FACULTY as fac 
			left join PULPIT as pul
			on fac.FACULTY = pul.FACULTY
			WHERE
				(@faculty is null or fac.FACULTY = @faculty)
				and
				(@pulpit is null or pul.PULPIT = @pulpit);
go
-- FCTEACHER --

CREATE function FCTEACHER(@pulpit_code varchar(10) = NULL) returns int
	as begin
		declare @teacher_count int;
		set @teacher_count = (
			SELECT count(*) 
				FROM TEACHER as teach
				WHERE (@pulpit_code is null or teach.PULPIT = @pulpit_code));
		return @teacher_count;
	end;
go

-- Процедура -- 
CREATE or ALTER PROCEDURE PRINT_REPORTX(@f varchar(10) = NULL, @p varchar(10) = NULL)
as begin
	SET NOCOUNT ON;
    declare @pulpit_count INT = 0;

    BEGIN TRY
        if @f IS NULL AND @p IS NOT NULL
            begin
                SELECT @f = FACULTY 
                    FROM PULPIT 
                    WHERE PULPIT = @p;
                if @f is null
                    begin THROW 50001, 'ошибка в параметрах: кафедра не найдена или не привязана к факультету', 1;
            end
        end

        SELECT  f.FACULTY [Код факультета], f.FACULTY_NAME [Название факультета], p.PULPIT [Код кафедры], p.PULPIT_NAME [Название кафедры]
            FROM FACULTY f
            inner join PULPIT p 
            on f.FACULTY = p.FACULTY
            WHERE 
                (f.FACULTY = @f or @f is null) 
                and
                (p.PULPIT = @p or @p is null)
            ORDER BY f.FACULTY, p.PULPIT;

        SET @pulpit_count = @@ROWCOUNT;

        return @pulpit_count;

    END TRY
    BEGIN CATCH
        declare @ErrorMessage nvarchar(4000) = ERROR_MESSAGE();
        declare @ErrorSeverity int = ERROR_SEVERITY();
        declare @ErrorState int = ERROR_STATE();

        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
        return 0;
    END CATCH
end;
go

-- Тесты --

DECLARE @rc INT;

BEGIN TRY
    PRINT '--- Тест 1: Отчет по факультету ИТ ---';
    EXEC @rc = PRINT_REPORT @f = 'ИТ';
    PRINT 'Количество кафедр в отчете: ' + CAST(@rc AS VARCHAR(10));

    PRINT CHAR(13) + '--- Тест 2: Отчет по конкретной кафедре (ИСиТ) ---';
    EXEC @rc = PRINT_REPORT @p = 'ИСиТ';
    PRINT 'Количество кафедр в отчете: ' + CAST(@rc AS VARCHAR(10));

    PRINT CHAR(13) + '--- Тест 3: Вызов с ошибкой (несуществующая кафедра) ---';
    EXEC @rc = PRINT_REPORT @p = 'НЕТ_ТАКОЙ';
    PRINT 'Количество кафедр в отчете: ' + CAST(@rc AS VARCHAR(10));

END TRY
BEGIN CATCH
    PRINT 'ЗАФИКСИРОВАНА ОШИБКА: ' + ERROR_MESSAGE();
END CATCH;
go

-- Очистка --
DROP function if exists dbo.FSUBJECTS;
DROP function if exists dbo.FFACPUL;
DROP function if exists dbo.FCTEACHER;