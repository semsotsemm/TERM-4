use UNIVER;
go

SELECT fac.FACULTY_NAME [Факультет], prof.PROFESSION_NAME[Специальность], gr.Course [Курс], ROUND(AVG(CAST(progr.NOTE as float(4))), 2) [Средняя оценка]
	FROM PROGRESS as progr inner join STUDENT as st
	on progr.IDSTUDENT = st.IDSTUDENT
	inner join GROUPS as gr
	on gr.IDGROUP = st.IDGROUP
	inner join FACULTY as fac
	on fac.FACULTY = gr.FACULTY
	inner join PROFESSION as prof
	on prof.PROFESSION = gr.PROFESSION
	GROUP BY gr.COURSE, fac.FACULTY_NAME, prof.PROFESSION_NAME
	ORDER BY ROUND(AVG(CAST(progr.NOTE as float(2))), 2) desc;