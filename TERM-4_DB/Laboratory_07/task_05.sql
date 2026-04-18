use UNIVER;
go

SELECT fac.FACULTY, prof.PROFESSION_NAME, prog.SUBJECT, AVG(prog.NOTE)
	FROM GROUPS as gr
	inner join STUDENT as st
	on st.IDGROUP = gr.IDGROUP
	inner join PROGRESS as prog
	on prog.IDSTUDENT = st.IDSTUDENT
	inner join FACULTY as fac 
	on fac.FACULTY = gr.FACULTY
	inner join PROFESSION as prof
	on prof.PROFESSION = gr.PROFESSION
	WHERE fac.FACULTY = 'кут'
	GROUP BY fac.FACULTY, gr.PROFESSION, prog.SUBJECT, prof.PROFESSION_NAME
	EXCEPT
SELECT fac.FACULTY, prof.PROFESSION_NAME, prog.SUBJECT, AVG(prog.NOTE)
	FROM GROUPS as gr
	inner join STUDENT as st
	on st.IDGROUP = gr.IDGROUP
	inner join PROGRESS as prog
	on prog.IDSTUDENT = st.IDSTUDENT
	inner join FACULTY as fac 
	on fac.FACULTY = gr.FACULTY
	inner join PROFESSION as prof
	on prof.PROFESSION = gr.PROFESSION
	WHERE fac.FACULTY = 'ррко'
	GROUP BY fac.FACULTY, gr.PROFESSION, prog.SUBJECT, prof.PROFESSION_NAME;