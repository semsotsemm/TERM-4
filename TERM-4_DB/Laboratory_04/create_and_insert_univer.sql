USE master;
CREATE Database UNIVER;
GO

USE UNIVER;
CREATE Table AUDITORIUM_TYPE
(
	AUDITORIUM_TYPE char(10) not null primary key,
	AUDITORIUM_TYPENAME varchar(40)
);
CREATE Table AUDITORIUM
(
	AUDITORIUM char(20) not null primary key,
	AUDITORIUM_TYPE char (10) foreign key references AUDITORIUM_TYPE(AUDITORIUM_TYPE),
	AUDITORIUM_CAPACITY int default 1 check (AUDITORIUM_CAPACITY between 1 and 300),
	AUDITORIUM_NAME varchar(50)
);

CREATE Table FACULTY
(
	FACULTY char(10) not null primary key,
	FACULTY_NAME varchar(50) default '???'
);
CREATE Table PROFESSION
(
	PROFESSION char(20) not null primary key,
	FACULTY char(10) not null foreign key references FACULTY(FACULTY),
	PROFESSION_NAME varchar(100),
	QUALIFICATION varchar(50)
);

CREATE Table GROUPS
(
	IDGROUP int not null primary key, 
	FACULTY char(10) not null foreign key references FACULTY(FACULTY),
	PROFESSION char(20) not null foreign key references PROFESSION(PROFESSION),
	YEAR_FIRST smallint check (YEAR_FIRST <= YEAR(GETDATE()) + 2), 
	COURSE as (cast((YEAR(GETDATE()) - YEAR_FIRST) as tinyint))
);
CREATE Table STUDENT
(
	IDSTUDENT int identity(1000, 1) primary key,
	IDGROUP int not null foreign key references GROUPS(IDGROUP),
	NAME nvarchar(100),
	BDAY date, 
	STAMP timestamp,
	INFO xml default(null),
	FOTO varbinary(max) default(null)
);
CREATE Table PULPIT
(
	PULPIT char(20) not null primary key,
	PULPIT_NAME varchar(100),
	FACULTY char(10) not null foreign key references FACULTY(FACULTY)
);
CREATE Table SUBJECT
(
	SUBJECT char(10) not null primary key,
	SUBJECT_NAME varchar(100) unique,
	PULPIT char(20) not null foreign key references PULPIT(PULPIT) 
);
CREATE Table PROGRESS
(
	SUBJECT char(10) foreign key references SUBJECT(SUBJECT), 
	IDSTUDENT int foreign key references STUDENT(IDSTUDENT),
	PDATE date, 
	NOTE int check (NOTE between 1 and 10),
	CONSTRAINT PK_PROGRESS PRIMARY KEY (SUBJECT, IDSTUDENT, PDATE)
);

CREATE Table TEACHER
(
	TEACHER char(20) primary key not null,
	TEACHER_NAME varchar(100) default null,
	GENDER char(1) default 'м' check (GENDER in('м', 'ж')),
	PULPIT char(20) not null foreign key references PULPIT(PULPIT)
);
GO


INSERT INTO AUDITORIUM_TYPE(AUDITORIUM_TYPE, AUDITORIUM_TYPENAME)
					  Values('ЛБ-Х', 'Химическая лаборатория'),
							('ЛБ-К', 'Компьютерный класс'),
							('ЛБ-СК', 'Спец. компьютерный класс'),
							('ЛК', 'Лекционная'),
							('ЛК-К', 'Лекционная с уст. проектором');

INSERT INTO AUDITORIUM(AUDITORIUM, AUDITORIUM_TYPE, AUDITORIUM_CAPACITY, AUDITORIUM_NAME)
				Values('301-1', 'ЛБ-К', 15, '301-1'), 
					  ('304-4', 'ЛБ-К', 90, '304-4'),
					  ('313-1', 'ЛБ-К', 60, '313-1'),
					  ('314-4', 'ЛК', 90, '314-4'),
					  ('320-4', 'ЛК', 90, '320-4'),
					  ('324-1', 'ЛБ-К', 50, '324-1'),
					  ('413-1', 'ЛБ-К', 15, '413-1'),
					  ('423-1', 'ЛБ-К', 90, '423-1');

INSERT INTO FACULTY (FACULTY, FACULTY_NAME) 
			Values  ('ТТЛП', 'Технологии и техника лесной промышленности'),
					('ТОВ', 'Технологии органических веществ'),
					('ХТиТ', 'Химические технологии и техника'),
					('ИЭФ', 'Инженерно-экономический'),
					('ЛХФ', 'Лесохозяйственный'),
					('ИДиП', 'Издательское дело и полиграфия'),
					('ИТ', 'Информационных технологий');

INSERT INTO PULPIT (PULPIT, PULPIT_NAME, FACULTY)
			Values ('РИТ', 'Редакционно-издательских технологий', 'ИДиП'),
				   ('СБУАиА', 'Статистики, бух. учета, анализа и аудита', 'ИЭФ'),
				   ('ТДП', 'Технологий деревообрабатывающих производств', 'ТТЛП'),
				   ('ТиП', 'Туризма и природопользования', 'ЛХФ'),
				   ('ТЛ', 'Транспорта леса', 'ТТЛП'),
				   ('ТНВиОХТ', 'Технологии неорганических веществ и общей химической технологии', 'ХТиТ'),
				   ('ТНХСиППМ', 'Технологии нефтехимического синтеза и переработки полимерных материалов', 'ТОВ'),
				   ('ХПД', 'Химической переработки древесины', 'ТОВ'),
				   ('ХТЭПиМЭЕ', 'Химии, техн. электрохим. производств', 'ХТиТ'),
				   ('ЭТиМ', 'Экономической теории и маркетинга', 'ИЭФ'),
				   ('ИСиТ', 'Информационных систем и технологий', 'ИТ'),
				   ('ЛМиЛЗ', 'Лесных машин и технологии лесозаготовок', 'ТТЛП'),
				   ('МиЭП', 'Менеджмента и экономики природопользования', 'ИЭФ');

INSERT INTO PROFESSION (PROFESSION, FACULTY, PROFESSION_NAME, QUALIFICATION) 
				Values ('1-36 06 01', 'ИДиП', 'Полиграфическое оборудование и системы обработки информации', 'инженер-электромеханик'),
					   ('1-36 07 01', 'ХТиТ', 'Машины и аппараты химических производств и предприятий строительных материалов', 'инженер-электромеханик'),
					   ('1-40 01 02', 'ИТ', 'Информационные системы и технологии', 'инженер-программист'),
					   ('1-46 01 01', 'ТТЛП', 'Лесоинженерное дело', 'инженер-технолог'),
					   ('1-47 01 01', 'ИДиП', 'Издательское дело', 'редактор-технолог'),
					   ('1-48 01 02', 'ТОВ', 'Химическая технология органических веществ, материалов и изделий', 'инженер-химик-технолог'),
					   ('1-48 01 05', 'ТОВ', 'Химическая технология переработки древесины', 'инженер-химик-технолог'),
					   ('1-54 01 03', 'ТОВ', 'Физико-химические методы и приборы контроля качества продукции', 'инженер по сертификации'),
					   ('1-75 01 01', 'ЛХФ', 'Лесное хозяйство', 'инженер лесного хозяйства'),
					   ('1-36 05 01', 'ТТЛП', 'Машины и механизмы', 'инженер'),
					   ('1-25 01 07', 'ИЭФ', 'Экономика и управление', 'экономист'),
					   ('1-25 01 08', 'ИЭФ', 'Бухгалтерский учет', 'бухгалтер'),
					   ('1-75 02 01', 'ЛХФ', 'Садово-парковое строительство', 'инженер садово-паркового строительства'),
					   ('1-89 02 02', 'ЛХФ', 'Туризм и природопользование', 'специалист в сфере туризма');

INSERT INTO GROUPS (IDGROUP, FACULTY, PROFESSION, YEAR_FIRST) 
			Values (22, 'ЛХФ',  '1-75 02 01', 2011),
				   (23, 'ЛХФ',  '1-89 02 02', 2012),
				   (24, 'ЛХФ',  '1-89 02 02', 2011),
				   (25, 'ТТЛП', '1-36 05 01', 2013),
				   (26, 'ТТЛП', '1-36 05 01', 2012),
				   (27, 'ТТЛП', '1-46 01 01', 2012),
				   (28, 'ИЭФ',  '1-25 01 07', 2013),
				   (29, 'ИЭФ',  '1-25 01 07', 2012),
				   (30, 'ИЭФ',  '1-25 01 07', 2010),
				   (31, 'ИЭФ',  '1-25 01 08', 2013),
				   (32, 'ИЭФ',  '1-25 01 08', 2012);

INSERT INTO STUDENT (IDGROUP, NAME, BDAY) 
			Values  (22, 'Пугач Михаил Трофимович', '1996-01-12'),
					(23, 'Авдеев Николай Иванович', '1996-07-19'),
					(24, 'Белова Елена Степановна', '1996-05-22'),
					(25, 'Вилков Андрей Петрович', '1996-12-08'),
					(26, 'Грушин Леонид Николаевич', '1995-11-11'),
					(27, 'Дунаев Дмитрий Михайлович', '1996-08-24'),
					(28, 'Клуни Иван Владиславович', '1996-09-15'),
					(29, 'Крылов Олег Павлович', '1996-10-16');

INSERT INTO SUBJECT (SUBJECT, SUBJECT_NAME, PULPIT) 
			Values	('ПЗ',   'Представление знаний в компьютерных системах', 'ИСиТ'),
					('ПИС',  'Проектирование информационных систем', 'ИСиТ'),
					('ПСП',  'Программирование сетевых приложений', 'ИСиТ'),
					('ПЭХ',  'Прикладная электрохимия', 'ХТЭПиМЭЕ'),
					('СУБД', 'Системы управления базами данных', 'ИСиТ'),
					('ТиОЛ', 'Технология и оборудование лесозаготовок', 'ЛМиЛЗ'),
					('ТРИ',  'Технология резиновых изделий', 'ТНХСиППМ'),
					('ЭП',   'Экономика природопользования', 'МиЭП'),
					('ЭТ',   'Экономическая теория', 'ЭТиМ'),
					('ОАиП', 'Основы алгоритмизации и программирования', 'ИСиТ'),
					('БД', 'Базы данных', 'ИСиТ');

INSERT INTO PROGRESS (SUBJECT, IDSTUDENT, PDATE, NOTE) 
			   Values('ОАиП', 1000, '2014-01-12', 4),
					 ('ОАиП', 1001, '2014-01-19', 5),
					 ('ОАиП', 1003, '2014-01-08', 9),
					 ('БД',   1004, '2014-01-11', 8),
					 ('БД',   1005, '2014-01-15', 4),
					 ('СУБД', 1006, '2014-01-16', 7),
					 ('СУБД', 1007, '2014-01-27', 6);

INSERT INTO TEACHER (TEACHER, TEACHER_NAME, GENDER, PULPIT)
					Values  ('ПРКП', 'Прокопенко Николай Иванович', NULL, 'ТНХСиППМ'),
							('МРЗВ', 'Морозова Елена Степановна', NULL, 'ИСиТ'),
							('РМНВ', 'Романов Дмитрий Михайлович', NULL, 'ИСиТ'),
							('СМЛВ', 'Смелов Владимир Владиславович', NULL, 'ИСиТ'),
							('КРЛВ', 'Крылов Павел Павлович', NULL, 'ИСиТ'),
							('ЧРН', 'Чернова Анна Викторовна', NULL, 'ХПД');