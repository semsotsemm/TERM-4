use A_MyBase;
go

-- 1 -- 
CREATE VIEW [Продажи в первую неделю Марта]
	as SELECT s.Product_code as [Код товара], s.Customer_name as [Покупатель], s.Ordered_date as [Дата заказа], s.Quantity_ordered_products as [Количество продукции]
		FROM Sales as s
		WHERE DAY(s.Ordered_date ) BETWEEN 1 and 7;

SELECT * 
	FROM [Продажи в первую неделю Марта];

-- Очистка --
DROP VIEW [Продажи в первую неделю Марта];

-- 2 -- 
CREATE VIEW [Количество продаж]
	as SELECT s.Ordered_date as [Дата заказа], SUM(s.Quantity_ordered_products)  as [Количество продукции]
		FROM SALES as s
		GROUP by s.Ordered_date;

SELECT *
	FROM [Количество продаж];

-- Очистка --
DROP VIEW [Количество продаж];

-- 3 -- 
CREATE VIEW [Заказы дешевых товаров] ([Код товара], [Покупатель], [Дата заказа], [Количество заказанной продукции], [Сумма заказа])
	as SELECT s.Product_code, s.Customer_name, s.Ordered_date, s.Quantity_ordered_products, p.Price * s.Quantity_ordered_products
		FROM Sales as s 
		inner join Products as p
		on s.Product_code = p.Code
		WHERE p.Price < 30;

SELECT * 
	FROM [Заказы дешевых товаров];

-- Очистка -- 
DROP VIEW [Заказы дешевых товаров];

-- 4 --	
CREATE VIEW [Заказы дешевых товаров] ([Код товара], [Покупатель], [Дата заказа], [Количество заказанной продукции], [Сумма заказа])
	as SELECT s.Product_code, s.Customer_name, s.Ordered_date, s.Quantity_ordered_products, p.Price * s.Quantity_ordered_products
		FROM Sales as s 
		inner join Products as p
		on s.Product_code = p.Code
		WHERE p.Price < 30
		WITH CHECK OPTION;

SELECT * 
	FROM [Заказы дешевых товаров];

-- Очистка -- 
DROP VIEW [Заказы дешевых товаров];

-- 5 -- 
CREATE VIEW [Количество продаж]
	as SELECT s.Ordered_date as [Дата заказа], SUM(s.Quantity_ordered_products)  as [Количество продукции]
		FROM SALES as s
		GROUP by s.Ordered_date;

SELECT *
	FROM [Количество продаж];

-- Очистка --
DROP VIEW [Количество продаж];


-- 6 -- 
ALTER VIEW [Количество продаж] WITH SCHEMABINDING
	as SELECT s.Ordered_date as [Дата заказа], SUM(s.Quantity_ordered_products)  as [Количество продукции]
		FROM dbo.SALES as s
		GROUP by s.Ordered_date;

SELECT *
	FROM [Количество продаж];

-- Очистка --
DROP VIEW [Количество продаж];
