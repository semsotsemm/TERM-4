USE master;
GO


CREATE Database A_MyDataBase
ON PRIMARY
(
	NAME = N'A_MyDataBase_mdf', 
	FILENAME = N'C:\SQLData\A_MyDataBase_mdf.mdf',
	SIZE =10240KB, 
	MAXSIZE = UNLIMITED,
	FILEGROWTH = 1024KB
)

LOG ON
(
	NAME = N'A_MyDataBase_log', 
	FILENAME = N'C:\SQLData\A_MyDataBase_log.ldf',
	SIZE =10240Kb, 
	MAXSIZE = 2048Gb, 
	FILEGROWTH = 10%
);
GO


USE A_MyDataBase;
GO
CREATE Table Покупатели(
			Покупатель nvarchar(30) primary key,
			Номер_телефона nvarchar(20) not null,
			Адрес nvarchar(50) not null);

CREATE Table Товары(
			Код_товара int primary key,
			Наименование_товара nvarchar(20) not null,
			Цена int not null,
			Описание nvarchar(200),
			Количество_на_складе int not null);

CREATE Table Продажи(
			Номер_продажи int primary key,
			Код_товара int foreign key references Товары(Код_товара),
			Покупатель nvarchar(30) foreign key references Покупатели(Покупатель),
			Дата date not null,
			Количество_заказанного_товара int not null);