use UNIVER;
go

print 'Версия SQL Server: ' + @@VERSION ;
print 'id процессора: ' + CAST(@@SPID as VARCHAR);
SELECT * FROM  AUDITORIUM;
print 'Последнее количество обработанных строк: ' + CAST(@@ROWCOUNT as VARCHAR);
print 'Код последней ошибки: ' + CAST(@@ERROR as VARCHAR);
print 'Имя сервера: ' + @@SERVERNAME
print 'Уровень текущей транзакции: ' + CAST(@@TRANCOUNT as VARCHAR);
print 'Статус курсора: ' + CASE @@FETCH_STATUS 
	WHEN 0 THEN 'Последняя сторка успешно прочитана'
	WHEN  -1 THEN 'Строка была последней'
	WHEN  -2 THEN 'Ошибка чтения (строка удалена)'
	END;
print 'Уровень вложенности текущей процедуры: ' + CAST(@@NESTLEVEL as VARCHAR);