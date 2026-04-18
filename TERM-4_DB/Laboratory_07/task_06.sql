use A_MyBase;
go

SELECT s.Ordered_date as [Дата продажи], SUM(prod.Price) as[Дневная выручка], SUM(s.Quantity_ordered_products) as [Количество проданного товара]
	FROM Sales as s
	inner join Products as prod
	on prod.Code = s.Product_code
	inner join Customers as cust
	on cust.Customer = s.Customer_name
	GROUP BY ROLLUP(s.Ordered_date)
	ORDER BY SUM(s.Quantity_ordered_products);


SELECT s.Ordered_date as [Дата продажи], SUM(prod.Price) as[Дневная выручка], SUM(s.Quantity_ordered_products) as [Количество проданного товара]
	FROM Sales as s
	inner join Products as prod
	on prod.Code = s.Product_code
	inner join Customers as cust
	on cust.Customer = s.Customer_name
	GROUP BY CUBE(s.Ordered_date)
	ORDER BY SUM(s.Quantity_ordered_products);
	

SELECT *
	FROM Products as prod
	WHERE prod.Price < 20
	UNION
SELECT *
	FROM Products as prod
	WHERE prod.Price > 20 and prod.Price < 30;


SELECT *
	FROM Products as prod
	WHERE prod.Price < 20
	UNION ALL 
SELECT *
	FROM Products as prod
	WHERE prod.Price < 30
	ORDER BY prod.Code;


SELECT *
	FROM Products as prod
	WHERE prod.Price < 20
	INTERSECT 
SELECT *
	FROM Products as prod
	WHERE prod.Price < 30
	ORDER BY prod.Code;


SELECT *
	FROM Products as prod
	WHERE prod.Price < 20
	EXCEPT 
SELECT *
	FROM Products as prod
	WHERE prod.Price < 30 and prod.Price > 10
	ORDER BY prod.Code;