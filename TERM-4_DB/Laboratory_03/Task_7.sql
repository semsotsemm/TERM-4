USE master;
CREATE Database A_MyDataBase
ON PRIMARY 
(
	NAME = N'A_MyDataBase_mdf',
	FILENAME = N'C:\SQLData\A_MyDataBase_mdf.mdf',
	SIZE = 10240KB,
	MAXSIZE = UNLIMITED,
	FILEGROWTH = 1024 KB
),
(
	NAME = N'A_MyDataBase_ndf',
	FILENAME = N'C:\SQLData\A_MyDataBase_ndf.ndf',
	SIZE = 10240KB,
	MAXSIZE = 1GB,
	FILEGROWTH = 25%
),
FILEGROUP FG1
(
	NAME = N'A_MyDataBase_fg1_1',
	FILENAME = N'C:\SQLData\A_MyDataBase_fg-1.ndf',
	SIZE = 10240KB,
	MAXSIZE = 1GB,
	FILEGROWTH = 25%
),
(
	NAME = N'A_MyDataBase_fg1_2',
	FILENAME = N'C:\SQLData\A_MyDataBase_fg-2.ndf',
	SIZE = 10240KB,
	MAXSIZE = 1GB,
	FILEGROWTH = 25%
)
log on 
(
	NAME = N'A_MyDataBase_log',
	FILENAME = N'C:\SQLData\A_MyDataBase_log.log',
	SIZE = 10240KB,
	MAXSIZE = 2048GB,
	FILEGROWTH = 15%
);
GO

USE A_MyDataBase;
CREATE Table Покупатели(
			 Покупатель nvarchar(30) primary key,
			 Номер_телефона nvarchar(20),
			 Адрес nvarchar(50) not null
			 ) on FG1;
			 
CREATE Table Товары(
			 Код_товара int primary key,
			 Наименование_товара nvarchar(20),
			 Цена int not null,
			 Описание nvarchar(200),
			 Количество_на_складе int not null
			 ) on FG1;

CREATE Table Продажи(
			 Номер_продажи int primary key,
			 Код_товара int foreign key references Товары(Код_товара),
			 Покупатель nvarchar(30) foreign key references Покупатели(Покупатель),
			 Дата date not null,
			 Количество int not null
			 ) on FG1;
			 