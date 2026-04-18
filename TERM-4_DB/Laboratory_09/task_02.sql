use UNIVER;

DECLARE @capacity_of_auditoriums int,
		@quantity_of_auditoriums int,
		@average_capacity int,
		@quantity_small_aud int;
		
SELECT @capacity_of_auditoriums = SUM(a.AUDITORIUM_CAPACITY) FROM AUDITORIUM as a;
SELECT @quantity_of_auditoriums = COUNT(a.AUDITORIUM_CAPACITY) FROM AUDITORIUM as a;
SELECT @average_capacity = AVG(a.AUDITORIUM_CAPACITY) FROM AUDITORIUM as a;

if @capacity_of_auditoriums > 200
	begin 
	print('Суммарная вместимость аудиторий больше 200')
	print('Количество аудиторий: ' + cast(@quantity_of_auditoriums as VARCHAR))
	print('Средняя вместимость аудитории: ' + cast(@average_capacity as VARCHAR))
	print('Суммарная вместимость всех аудиторий: ' + cast(@capacity_of_auditoriums as VARCHAR))
	print('Список аудиторий, вместимостью ниже средней (' + cast(@average_capacity as VARCHAR) + ') (в результате)')
	SELECT @quantity_small_aud = COUNT(*) FROM AUDITORIUM WHERE AUDITORIUM.AUDITORIUM_CAPACITY < @average_capacity 
	print('Процент аудиторий, чья вместимость меньше 200: ' + cast(cast((@quantity_small_aud * 100.0 / @quantity_of_auditoriums) as numeric(4,1)) as VARCHAR) + '%')
	SELECT a.AUDITORIUM as [Аудитория], a.AUDITORIUM_TYPE as [Тип аудитории], a.AUDITORIUM_CAPACITY as [Вместимость аудитории]
		FROM AUDITORIUM as a
		WHERE a.AUDITORIUM_CAPACITY < 200;
end
else
begin
	print('Суммарная вместимость аудиторий меньше 200')
end;