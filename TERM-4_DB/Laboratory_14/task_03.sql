-- Табличная функция для вывода таблицы факультета x кафедры y -- 
use UNIVER;
go

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

SELECT *
	FROM dbo.FFACPUL(NULL, 'ИСиТ');

SELECT *
	FROM dbo.FFACPUL('ИТ', NULL);
	
SELECT *
	FROM dbo.FFACPUL(NULL, NULL);

SELECT *
	FROM dbo.FFACPUL('ТОВ', 'ХПД');

go
DROP function if exists dbo.FFACPUL;