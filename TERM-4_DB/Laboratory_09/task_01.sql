DECLARE @char_param char(3) = 'ФИТ',
		@varchar_param varchar(4) = 'БГТУ',
		@datetime_param datetime,
		@time_param time,
		@int_param int,
		@small_int_param smallint,
		@tiny_int_param tinyint,
		@numeric_param numeric(12, 5);

SET @datetime_param = GETDATE();
SELECT @time_param = '17:00';
SET @int_param = -49;
SELECT @small_int_param = -10;
SET @tiny_int_param = 3;
SELECT @numeric_param = 134.23;

SELECT @char_param as [Переменная char],
	FORMAT(@datetime_param, 'dd-MM-yyyy') [Переменная date time], 
	@small_int_param as [Переменная small int],
	@numeric_param as [Переменная numberic];

print 'Переменная var char = ' + @varchar_param; 
print 'Переменная time = ' + CAST(FORMAT(CAST(@time_param AS DATETIME), 'HH:mm') as VARCHAR);
print 'Переменная int =' + CAST(@int_param as VARCHAR);
print 'Переменная tiny int = ' + CAST(@tiny_int_param as VARCHAR);
