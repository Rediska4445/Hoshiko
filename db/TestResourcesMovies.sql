USE HoshikoDB;
GO

-- Удаляем все записи из таблицы фильмов
DELETE FROM Movies;

-- Сбрасываем счетчик автоинкремента (Identity) обратно на 1,
-- чтобы новые фильмы снова начинались с Id = 1
DBCC CHECKIDENT ('Movies', RESEED, 0);
GO



USE HoshikoDB;
GO

-- 1. Гарантируем наличие типа медиа и жанра "Детектив" для фильмов
DECLARE @MovieTypeId INT = (SELECT Id FROM MediaContent WHERE Name = 'Movie');

IF NOT EXISTS (SELECT 1 FROM Genres WHERE Name = N'Детектив' AND MediaContentId = @MovieTypeId)
BEGIN
    INSERT INTO Genres (Name, MediaContentId) VALUES (N'Детектив', @MovieTypeId);
END
GO

-- 2. Получаем ID самого первого доступного пользователя (чтобы точно избежать ошибки NULL)
DECLARE @AdminUserId INT = (SELECT TOP 1 Id FROM Users ORDER BY Id ASC);
DECLARE @MovieTypeId INT = (SELECT Id FROM MediaContent WHERE Name = 'Movie');
DECLARE @GenreId INT = (SELECT Id FROM Genres WHERE Name = N'Детектив' AND MediaContentId = @MovieTypeId);

-- Переменные с вашими точными данными
DECLARE @Title NVARCHAR(300) = N'Тестовый фильм';
DECLARE @Path NVARCHAR(1000) = N'C:\Users\2022\source\repos\Hoshiko\test-resources\sample-20s-360p.mp4';

-- 3. Безопасное добавление фильма
IF NOT EXISTS (SELECT 1 FROM Movies WHERE Title = @Title OR FilePath = @Path)
BEGIN
    INSERT INTO Movies (Title, FilePath, UploadedByUserId, GenreId)
    VALUES (@Title, @Path, @AdminUserId, @GenreId);
    
    PRINT 'фильм успешно добавлен в базу!';
END
ELSE
BEGIN
    PRINT 'Этот фильм или путь уже существуют в базе данных.';
END
GO

-- Проверяем результат
SELECT m.Id, m.Title, g.Name AS GenreName, m.FilePath 
FROM Movies m
INNER JOIN Genres g ON m.GenreId = g.Id;
GO
