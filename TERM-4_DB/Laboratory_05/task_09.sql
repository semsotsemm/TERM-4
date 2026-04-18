use A_MyBase;
go

-- 1. Задание (Инфа о оптовых заказчиках)--
SELECT cust.Customer[Фио], cust.Phone_number[Номер телефона], cust.Home_address[Домашний адрес]
	FROM Customers as cust
	where cust.Customer in (
	SELECT s.Customer_name 
		FROM Sales as s
		WHERE Quantity_ordered_products > 1
	);


-- 2. Тоже самое только inner join--
SELECT cust.Customer[Фио], cust.Phone_number[Номер телефона], cust.Home_address[Домашний адрес]
	FROM Customers as cust
	inner join (
		SELECT s.Customer_name 
		FROM Sales as s
		WHERE Quantity_ordered_products > 1
	) as wholesale 
	on cust.Customer = wholesale.Customer_name;

-- 3. Тоже самое только без подзапроса 
SELECT cust.Customer[ФИО], cust.Phone_number[Номер телефона], cust.Home_address[Домашний адрес]
	FROM Customers as cust
	inner join Sales as s
	on cust.Customer = s.Customer_name
	WHERE s.Quantity_ordered_products > 1;

-- 4. Коррелируемый подзапрос (самые большие заказы всех продуктов) -- 
SELECT s.Sales_number[Индекс продажи], Product_code[Код товара], Customer_name[ФИО покупателя], Ordered_date[Дата заказ] , Quantity_ordered_products[Количесттво заказаного товара]
	FROM Sales as s
	WHERE s.Sales_number = (
		SELECT top(1) inner_s.Sales_number 
			 FROM Sales as inner_s
			 WHERE inner_s.Product_code = s.Product_code
			 ORDER BY inner_s.Quantity_ordered_products desc
	) ORDER BY s.Quantity_ordered_products desc;

-- 5. Коррелируемый подзапрос (товары без продаж) --
SELECT *
	FROM Products as prod
	WHERE not exists(
		SELECT * 
			FROM Sales as s
			WHERE s.Product_code = prod.Code
	);

-- 6. В среднем покупок за раз --
SELECT AVG(CAST(s.Quantity_ordered_products as FLOAT)) [Среднее коичество покупок за раз]
	FROM Sales as s;

-- 7. Использование all -- 
SELECT s.Sales_number[Индекс продажи], s.Product_code[код товара], s.Customer_name [ФИО покупателя], s.Ordered_date [Дата заказа], s.Quantity_ordered_products [Количество заказанного товара] 
	FROM Sales as s 
	WHERE s.Quantity_ordered_products > all (
		SELECT AVG(CAST(inner_s.Quantity_ordered_products as float))
			FROM Sales as inner_s
	);

-- 8. Использоваение any --
SELECT s.Sales_number[Индекс продажи], s.Product_code[код товара], s.Customer_name [ФИО покупателя], s.Ordered_date [Дата заказа], s.Quantity_ordered_products [Количество заказанного товара] 
	FROM Sales as s 
	WHERE s.Quantity_ordered_products > any (
		SELECT inner_s.Quantity_ordered_products
			FROM Sales as inner_s
	);