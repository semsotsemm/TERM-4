USE master;
GO 

CREATE DATABASE A_MyDataBase;
GO 

USE A_MyDataBase;
GO 

CREATE TABLE Покупатели (
	Покупатель nvarchar(30) primary key,
	Номер_телефона nvarchar(20) not null,
	Адрес nvarchar(50) not null
);

CREATE table Товары(
	Код_товара int primary key,
	Наименование_товара nvarchar(20) not null,
	Цена int not null,
	Описание nvarchar(200),
	Количество_на_складе int not null
);

CREATE table Продажи(
	Номер_продажи int primary key,
	Код_товара int FOREIGN KEY REFERENCES Товары(Код_товара),
	Покупатель nvarchar(30) FOREIGN KEY REFERENCES Покупатели(Покупатель),
	Дата date not null,
	Количество_заказанного_товара int not null
);