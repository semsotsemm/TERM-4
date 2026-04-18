use UNIVER;
go

SELECT st.IDSTUDENT[Индекс студента], st.IDGROUP[Номер группы], st.NAME[ФИО студента], st.BDAY[Дата рождения], st.STAMP[Штамп], st.INFO[Информация], st.FOTO[Фото]
	FROM STUDENT as st
	WHERE exists (
		SELECT *
			FROM STUDENT as inner_st
			WHERE DAY(inner_st.BDAY) = DAY(st.BDAY) and MONTH(inner_st.BDAY) = MONTH(st.BDAY) and inner_st.IDSTUDENT <> st.IDSTUDENT
	);