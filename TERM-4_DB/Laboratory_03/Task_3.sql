USE A_MyDataBase;
ALTER Table Покупатели ADD Id_Покупателя int;
GO

ALTER Table Покупатели ALTER Column Id_Покупателя int not null;
GO

ALTER Table Покупатели DROP Column Id_Покупателя;