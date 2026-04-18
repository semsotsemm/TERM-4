use UNIVER;
go

SELECT prof.PROFESSION_NAME as [—пециальность], fac.FACULTY_NAME as [‘акультет], prog.SUBJECT as [ƒисциплина], AVG(prog.NOTE) as [—редн€€ оценка]
	FROM PROGRESS as prog inner join STUDENT as st
	on prog.IDSTUDENT = st.IDSTUDENT
	inner join GROUPS as gr
	on gr.IDGROUP = st.IDGROUP
	inner join FACULTY as fac
	on fac.FACULTY = gr.FACULTY
	inner join PROFESSION as prof
	on prof.FACULTY = fac.FACULTY
	WHERE fac.FACULTY_NAME = 'Ћесохоз€йственный'
	GROUP BY ROLLUP(prof.PROFESSION_NAME, fac.FACULTY_NAME, prog.SUBJECT);