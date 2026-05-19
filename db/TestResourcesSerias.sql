USE HoshikoDB;
GO

-- 1. Получаем необходимые ID (гарантируем, что UserId не будет NULL)
DECLARE @AdminUserId INT = (SELECT TOP 1 Id FROM Users ORDER BY Id ASC);
DECLARE @SeriesMediaTypeId INT = (SELECT Id FROM MediaContent WHERE Name = 'Series');
DECLARE @GenreId INT = (SELECT Id FROM Genres WHERE Name = N'Аниме' AND MediaContentId = @SeriesMediaTypeId);

DECLARE @SeriesTitle NVARCHAR(300) = N'Тестовый Сэмпл Сериал';
DECLARE @SeriesFolder NVARCHAR(1000) = N'C:\Users\2022\source\repos\Hoshiko\test-resources\';

DECLARE @SeriesId INT;

-- 2. Добавляем сам сериал (если его ещё нет)
IF NOT EXISTS (SELECT 1 FROM Series WHERE Title = @SeriesTitle)
BEGIN
    INSERT INTO Series (Title, FilePath, UploadedByUserId, GenreId)
    VALUES (@SeriesTitle, @SeriesFolder, @AdminUserId, @GenreId);
    
    SET @SeriesId = SCOPE_IDENTITY();
    PRINT 'Сериал "' + @SeriesTitle + '" успешно создан.';
END
ELSE
BEGIN
    SET @SeriesId = (SELECT Id FROM Series WHERE Title = @SeriesTitle);
    PRINT 'Сериал уже существует. Используем существующий ID.';
END

-- 3. Наполняем таблицу эпизодов (Episodes) для этого сериала
INSERT INTO Episodes (SeriesId, Title, EpisodeNumber, FilePath, UploadedByUserId)
SELECT @SeriesId, Source.Title, Source.EpNum, Source.FilePath, @AdminUserId
FROM (
    VALUES 
    (N'Сэмпл 10с (360p)', 1, N'C:\Users\2022\source\repos\Hoshiko\test-resources\sample-10s-360p.mp4'),
    (N'Сэмпл 10с (720p)', 2, N'C:\Users\2022\source\repos\Hoshiko\test-resources\sample-10s-720p.mp4'),
    (N'Сэмпл 10с (Оригинал)', 3, N'C:\Users\2022\source\repos\Hoshiko\test-resources\sample-10s.mp4'),
    (N'Сэмпл 15с (360p)', 4, N'C:\Users\2022\source\repos\Hoshiko\test-resources\sample-15s-360p.mp4'),
    (N'Сэмпл 15с (720p)', 5, N'C:\Users\2022\source\repos\Hoshiko\test-resources\sample-15s-720p.mp4'),
    (N'Сэмпл 20с (360p)', 6, N'C:\Users\2022\source\repos\Hoshiko\test-resources\sample-20s-360p.mp4'),
    (N'Сэмпл 20с (720p)', 7, N'C:\Users\2022\source\repos\Hoshiko\test-resources\sample-20s-720p.mp4'),
    (N'Сэмпл 5с (720p)',  8, N'C:\Users\2022\source\repos\Hoshiko\test-resources\sample-5s-720p.mp4'),
    (N'Сэмпл 5с (Оригинал)',  9, N'C:\Users\2022\source\repos\Hoshiko\test-resources\sample-5s.mp4')
) AS Source(Title, EpNum, FilePath)
WHERE NOT EXISTS (
    SELECT 1 FROM Episodes WHERE SeriesId = @SeriesId AND FilePath = Source.FilePath
);

PRINT 'Эпизоды успешно синхронизированы!';

-- 4. Проверяем результат
SELECT e.EpisodeNumber AS [Серия], e.Title AS [Название], e.FilePath AS [Путь к файлу]
FROM Episodes e
WHERE e.SeriesId = @SeriesId
ORDER BY e.EpisodeNumber;
GO
