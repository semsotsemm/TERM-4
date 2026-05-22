use UNIVER;
go

CREATE or ALTER Trigger Teacher_INSTEAD_OF on TEACHER instead of DELETE as
	SET NOCOUNT ON;
	RAISERROR(N'Удаление учителей запрещено', 16, 1)
	return;
go

DELETE FROM TEACHER
	WHERE TEACHER = 'КРЛВ'

SELECT *
	FROM TEACHER;