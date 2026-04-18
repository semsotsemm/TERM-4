use master;
go

CREATE Database A_MyBase;
go

use A_MyBase;
go

CREATE Table Customers(
	Customer varchar(50) primary key,
	Phone_number varchar(20) not null,
	Home_address varchar(50)not null,
	);

CREATE Table Products(
	Code int primary key,
	Product_name varchar(40) not null,
	Price decimal(7,2) not null check (Price > 0),
	Product_description varchar(200) not null,
	Quantity int not null check (Quantity > 0),
	);

CREATE Table Sales(
	Sales_number int identity(1, 1) primary key,
	Product_code int foreign key references Products(code),
	Customer_name varchar(50) foreign key references Customers(Customer),
	Ordered_date date default GETDATE(),
	Quantity_ordered_products int not null
	);