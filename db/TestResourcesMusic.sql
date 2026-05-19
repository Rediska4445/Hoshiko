USE HoshikoDB;
GO

-- 2. Получаем ID пользователя и медиа-контента
DECLARE @AdminUserId INT = 1;
DECLARE @MusicTypeId INT = (SELECT Id FROM MediaContent WHERE Name = 'Music');

-- 3. Получаем ID конкретных музыкальных жанров
DECLARE @GenreRap INT = (SELECT Id FROM Genres WHERE Name = N'Rap' AND MediaContentId = @MusicTypeId);
DECLARE @GenreCloud INT = (SELECT Id FROM Genres WHERE Name = N'Cloud Rap' AND MediaContentId = @MusicTypeId);
DECLARE @GenreHorror INT = (SELECT Id FROM Genres WHERE Name = N'Horrorcore' AND MediaContentId = @MusicTypeId);
DECLARE @GenreIndie INT = (SELECT Id FROM Genres WHERE Name = N'Industrial Hip Hop' AND MediaContentId = @MusicTypeId);

-- 4. Заполнение таблицы Music
INSERT INTO Music (Title, FilePath, UploadedByUserId, GenreId)
SELECT Source.Title, Source.FilePath, @AdminUserId, Source.GenreId
FROM (
    VALUES 
    -- $uicideboy$ & Getter (Horrorcore / Cloud Rap)
    (N'$uicideboy$ - I''m Done', N'C:\Users\2022\source\repos\Hoshiko\test-resources\$uicideboy$ - I''m Done.mp3', @GenreCloud),
    (N'$uicideboy$, Getter - Champion of Death', N'C:\Users\2022\source\repos\Hoshiko\test-resources\$uicideboy$, Getter - Champion of Death.mp3', @GenreHorror),
    (N'$uicideboy$, Getter - Stop Calling Us Horrorcore', N'C:\Users\2022\source\repos\Hoshiko\test-resources\$uicideboy$, Getter - Stop Calling Us Horrorcore.mp3', @GenreHorror),
    
    -- Baker & Cursed (Phonk / Memphis / Rap)
    (N'BAKER - MINDSET', N'C:\Users\2022\source\repos\Hoshiko\test-resources\BAKER - MINDSET.mp3', @GenreRap),
    (N'CURSED - Red Dot (Prod. PYRVMXDZ)', N'C:\Users\2022\source\repos\Hoshiko\test-resources\CURSED - Red Dot (Prod. PYRVMXDZ).mp3', @GenreRap),
    
    -- DVRST (Electronic / Rap / Phonk)
    (N'DVRST - Scape', N'C:\Users\2022\source\repos\Hoshiko\test-resources\DVRST - Scape.mp3', @GenreIndie),
    (N'DVRST - Close Eyes', N'C:\Users\2022\source\repos\Hoshiko\test-resources\DVRST_-_Close_Eyes_73006469.mp3', @GenreIndie),
    (N'DVRST - Dream Space', N'C:\Users\2022\source\repos\Hoshiko\test-resources\DVRST_-_Dream_Space_(musmore.com).mp3', @GenreIndie),
    
    -- Freddie Dredd & Ghostemane (Horrorcore / Industrial)
    (N'Freddie Dredd - WTH', N'C:\Users\2022\source\repos\Hoshiko\test-resources\Freddie Dredd - WTH.mp3', @GenreHorror),
    (N'Ghostemane - D(R)Own', N'C:\Users\2022\source\repos\Hoshiko\test-resources\Ghostemane - D(R)Own.mp3', @GenreIndie),
    (N'GHOSTEMANE - Dread', N'C:\Users\2022\source\repos\Hoshiko\test-resources\GHOSTEMANE - Dread.mp3', @GenreHorror),
    (N'Ghostemane - Hades', N'C:\Users\2022\source\repos\Hoshiko\test-resources\Ghostemane - Hades.mp3', @GenreHorror),
    (N'Ghostemane - Hexada', N'C:\Users\2022\source\repos\Hoshiko\test-resources\Ghostemane - Hexada.mp3', @GenreHorror),
    (N'Ghostemane - Venom', N'C:\Users\2022\source\repos\Hoshiko\test-resources\Ghostemane - Venom.mp3', @GenreIndie),
    
    -- Остальные Memphis/Phonk треки
    (N'JUNTMANE — CORRUPT III', N'C:\Users\2022\source\repos\Hoshiko\test-resources\JUNTMANE — CORRUPT III.mp3', @GenreRap),
    (N'LIL KAINE & OCCVLT - OUT MA LEASH', N'C:\Users\2022\source\repos\Hoshiko\test-resources\LIL KAINE & OCCVLT - OUT MA LEASH.mp3', @GenreHorror),
    (N'LXST CXNTURY, Kingpin Skinny Pimp - ODIUM', N'C:\Users\2022\source\repos\Hoshiko\test-resources\LXST CXNTURY, Kingpin Skinny Pimp - ODIUM.mp3', @GenreIndie),
    (N'Memphis Cult, Groove Dealers, SPLYXER - 9mm', N'C:\Users\2022\source\repos\Hoshiko\test-resources\Memphis Cult, Groove Dealers, SPLYXER - 9mm.mp3', @GenreRap)
) AS Source(Title, FilePath, GenreId)
WHERE NOT EXISTS (
    SELECT 1 FROM Music WHERE Title = Source.Title OR FilePath = Source.FilePath
);

PRINT 'Все музыкальные треки успешно добавлены!';

-- 5. Проверка заполнения
SELECT m.Id, m.Title, g.Name AS [Жанр], m.FilePath
FROM Music m
INNER JOIN Genres g ON m.GenreId = g.Id
ORDER BY [Жанр], m.Title;
GO