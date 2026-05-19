USE HoshikoDB;
GO

-- 2. Объявляем переменные для динамического поиска ID типов медиа
DECLARE @MovieTypeId INT = (SELECT Id FROM MediaContent WHERE Name = 'Movie');
DECLARE @SeriesTypeId INT = (SELECT Id FROM MediaContent WHERE Name = 'Series');
DECLARE @MusicTypeId INT = (SELECT Id FROM MediaContent WHERE Name = 'Music');

-- 3. Добавляем тестовые жанры для ФИЛЬМОВ (Movie)
INSERT INTO Genres (Name, MediaContentId)
SELECT Source.Name, @MovieTypeId
FROM (
    VALUES 
    (N'Боевик'),
    (N'Комедия'),
    (N'Фантастика'),
    (N'Ужасы'),
    (N'Драма'),
    (N'Триллер'),
    (N'Детектив'),
    (N'Мелодрама'),
    (N'Исторический')
) AS Source(Name)
WHERE NOT EXISTS (
    SELECT 1 FROM Genres 
    WHERE Name = Source.Name AND MediaContentId = @MovieTypeId
);

-- 4. Добавляем тестовые жанры для СЕРИАЛОВ (Series)
INSERT INTO Genres (Name, MediaContentId)
SELECT Source.Name, @SeriesTypeId
FROM (
    VALUES 
    (N'Аниме'),
    (N'Дорама'),
    (N'Ситком'),
    (N'Мультсериал'),
    (N'Фэнтези'),
    (N'Документальный'),
    (N'Криминал'),
    (N'Приключения')
) AS Source(Name)
WHERE NOT EXISTS (
    SELECT 1 FROM Genres 
    WHERE Name = Source.Name AND MediaContentId = @SeriesTypeId
);

-- 5. Добавляем тестовые жанры для МУЗЫКИ (Music)
INSERT INTO Genres (Name, MediaContentId)
SELECT Source.Name, @MusicTypeId
FROM (
    VALUES 
    (N'Rap'),
    (N'Cloud Rap'),
    (N'Horrorcore'),
    (N'Industrial Hip Hop'),
    (N'Rock'),
    (N'Metal'),
    (N'Pop'),
    (N'Electronic'),
    (N'Lo-Fi'),
    (N'Jazz')
) AS Source(Name)
WHERE NOT EXISTS (
    SELECT 1 FROM Genres 
    WHERE Name = Source.Name AND MediaContentId = @MusicTypeId
);

-- 6. Выводим итоговую таблицу для проверки заполнения
SELECT g.Id AS GenreId, g.Name AS GenreName, mc.Name AS MediaTypeName
FROM Genres g
INNER JOIN MediaContent mc ON g.MediaContentId = mc.Id
ORDER BY MediaTypeName, g.Id;
GO
