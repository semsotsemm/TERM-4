use A_MyBase;
go

-- 1. Группировка и агрегатная функция --
SELECT s.Ordered_date[Дата заказа], MIN(s.Quantity_ordered_products)[Минимальный опт], MAX(s.Quantity_ordered_products)[Максимальный опт], ROUND(AVG(CAST(s.Quantity_ordered_products as float(4))), 2)[Средний опт], COUNT(s.Quantity_ordered_products)[Количество заказов]
	FROM Sales as s
	GROUP BY s.Ordered_date;

	
-- 2. Группировка и агрегатная функция --
SELECT s.Ordered_date[Дата заказа], MIN(s.Quantity_ordered_products)[Минимальный опт], MAX(s.Quantity_ordered_products)[Максимальный опт], ROUND(AVG(CAST(s.Quantity_ordered_products as float(4))), 2)[Средний опт], COUNT(s.Quantity_ordered_products)[Количество заказов]
	FROM Sales as s
	GROUP BY s.Ordered_date;
	

-- 3. Группировка и агрегатная функция --
SELECT result.Date_interval[Временной интервал], SUM(result.Orders_in_interval)[Количество продаж]
	FROM (
		SELECT COUNT(inner_s.Quantity_ordered_products)[Orders_in_interval], inner_s.Ordered_date[Order_Date] ,
			CASE 
				WHEN DAY(inner_s.Ordered_date) between 1 and 5 THEN '1-5'
				WHEN DAY(inner_s.Ordered_date) between 6 and 10 THEN '6-10'
				WHEN DAY(inner_s.Ordered_date) between 11 and 15 THEN '11-15'
				ELSE '0'
				end as Date_interval
		FROM Sales as inner_s
		GROUP BY inner_s.Ordered_date
	) as result
	GROUP BY result.Date_interval
	ORDER BY MAX(result.Order_Date);

-- 4. Средняя цена каждого заказа в временной интервал --
SELECT result.Date_interval[Временной интервал], SUM(result.Average_sum)[Выручка], (SUM(result.Average_sum) / SUM(result.Count_saled_product)) [Средний чек], SUM(result.Count_saled_product)[Продано товаров], SUM(result.Count_customers) [Количестов клиентов]
	FROM(
		SELECT 
			CASE 
				WHEN DAY(inner_s.Ordered_date) between 1 and 5 THEN '1-5'
				WHEN DAY(inner_s.Ordered_date) between 6 and 10 THEN '6-10'
				WHEN DAY(inner_s.Ordered_date) between 11 and 15 THEN '11-15'
				ELSE '0'
				END [Date_interval], 
				SUM(inner_s.Quantity_ordered_products * prod.Price)[Average_sum], SUM(inner_s.Quantity_ordered_products)[Count_saled_product], COUNT(inner_s.Quantity_ordered_products)[Count_customers], inner_s.Ordered_date[Date_of_order]
			FROM Sales as inner_s
			inner join Products as prod
			on prod.Code = inner_s.Product_code
			GROUP BY inner_s.Ordered_date
	) as result
	GROUP BY result.Date_interval
	ORDER BY MAX(result.Date_of_order);


-- 5. Тоже самое только с фильтром -- 
SELECT result.Date_interval[Временной интервал], SUM(result.Average_sum)[Выручка], (SUM(result.Average_sum) / SUM(result.Count_saled_product)) [Средний чек], SUM(result.Count_saled_product)[Продано товаров], SUM(result.Count_customers) [Количестов клиентов]
	FROM(
		SELECT 
			CASE 
				WHEN DAY(inner_s.Ordered_date) between 1 and 5 THEN '1-5'
				WHEN DAY(inner_s.Ordered_date) between 6 and 10 THEN '6-10'
				WHEN DAY(inner_s.Ordered_date) between 11 and 15 THEN '11-15'
				ELSE '0'
				END [Date_interval], 
				SUM(inner_s.Quantity_ordered_products * prod.Price)[Average_sum], SUM(inner_s.Quantity_ordered_products)[Count_saled_product], COUNT(inner_s.Quantity_ordered_products)[Count_customers], inner_s.Ordered_date[Date_of_order]
			FROM Sales as inner_s
			inner join Products as prod
			on prod.Code = inner_s.Product_code
			WHERE inner_s.Quantity_ordered_products != 1
			GROUP BY inner_s.Ordered_date
	) as result
	GROUP BY result.Date_interval
	ORDER BY MAX(result.Date_of_order);


-- 6. Количество продаж наборов --
SELECT s.Ordered_date[Дата заказа], SUM(s.Quantity_ordered_products)[Количество заказанного товара], COUNT(*)[Количство покупателей], ROUND(AVG(s.Quantity_ordered_products * prod.Price), 2)[Средний чек]
	FROM Sales as s
	inner join Products as prod
	on prod.Code = s.Product_code
	inner join Customers as cust 
	on cust.Customer = s.Customer_name
	WHERE prod.Product_name like '%Набор%'
	GROUP BY s.Ordered_date;


-- 7. Использование having -- 
SELECT s.Ordered_date[Дата заказа], SUM(s.Quantity_ordered_products)[Количество заказанного товара], COUNT(*)[Количство покупателей], ROUND(AVG(s.Quantity_ordered_products * prod.Price), 2)[Средний чек]
	FROM Sales as s
	inner join Products as prod
	on prod.Code = s.Product_code
	inner join Customers as cust 
	on cust.Customer = s.Customer_name
	WHERE prod.Product_name like '%Набор%'
	GROUP BY s.Ordered_date
	HAVING SUM(s.Quantity_ordered_products) > 1;