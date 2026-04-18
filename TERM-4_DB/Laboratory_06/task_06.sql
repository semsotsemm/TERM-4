use UNIVER;
go

SELECT fac.FACULTY_NAME [‘акультет], prof.PROFESSION_NAME[—пециальность], gr.Course [ урс], ROUND(AVG(CAST(progr.NOTE as float(4))), 2) [—редн€€ оценка]
	FROM PROGRESS as progr inner join STUDENT as st
	on progr.IDSTUDENT = st.IDSTUDENT
	inner join GROUPS as gr
	on gr.IDGROUP = st.IDGROUP
	inner join FACULTY as fac
	on fac.FACULTY = gr.FACULTY
	inner join PROFESSION as prof
	on prof.PROFESSION = gr.PROFESSION
	WHERE fac.FACULTY = '»Ё‘'
	GROUP BY gr.COURSE, fac.FACULTY_NAME, prof.PROFESSION_NAME
	ORDER BY ROUND(AVG(CAST(progr.NOTE as float(2))), 2) desc;