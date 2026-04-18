USE A_MyDataBase;
SELECT * From Покупатели;

SELECT Покупатель, Номер_телефона From Покупатели;

SELECT count(*) From Покупатели;

UPDATE Товары SET Цена = Цена + 777 WHERE Код_товара = 101;

SELECT Код_Товара, Наименование_товара, Цена From Товары