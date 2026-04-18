USE UNIVER;
go

DECLARE navigation_cursor CURSOR SCROLL STATIC FOR
    SELECT AUDITORIUM, AUDITORIUM_TYPE, AUDITORIUM_CAPACITY
    FROM AUDITORIUM
    ORDER BY AUDITORIUM;

DECLARE @aud char(20), @type char(10), @cap int;

OPEN navigation_cursor;

FETCH NEXT FROM navigation_cursor INTO @aud, @type, @cap;
PRINT '1. NEXT (Первая): ' + @aud + ' | ' + @type;

FETCH LAST FROM navigation_cursor INTO @aud, @type, @cap;
PRINT '2. LAST (Последняя): ' + @aud + ' | ' + @type;

FETCH PRIOR FROM navigation_cursor INTO @aud, @type, @cap;
PRINT '3. PRIOR (Предпоследняя): ' + @aud + ' | ' + @type;

FETCH ABSOLUTE 3 FROM navigation_cursor INTO @aud, @type, @cap;
PRINT '4. ABSOLUTE 3: ' + @aud + ' | ' + @type;

FETCH RELATIVE 2 FROM navigation_cursor INTO @aud, @type, @cap;
PRINT '5. RELATIVE 2 (от 3-й стала 5-я): ' + @aud + ' | ' + @type;

FETCH RELATIVE -1 FROM navigation_cursor INTO @aud, @type, @cap;
PRINT '6. RELATIVE -1 (вернулись на 4-ю): ' + @aud + ' | ' + @type;

FETCH FIRST FROM navigation_cursor INTO @aud, @type, @cap;
PRINT '7. FIRST (Снова первая): ' + @aud + ' | ' + @type;

FETCH ABSOLUTE -2 FROM navigation_cursor INTO @aud, @type, @cap;
PRINT '8. ABSOLUTE -2 (Вторая с конца): ' + @aud + ' | ' + @type;

CLOSE navigation_cursor;
DEALLOCATE navigation_cursor;