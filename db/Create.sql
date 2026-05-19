-- Убираем БД, если есть (и она не используется)
USE master;
GO

-- Если база уже есть и включена, отключаем пользователей и удаляем
IF DB_ID('HoshikoDB') IS NOT NULL
BEGIN
    ALTER DATABASE HoshikoDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE HoshikoDB;
END
GO

-- Создаём новую БД с коллацией для кириллицы
CREATE DATABASE HoshikoDB
COLLATE Cyrillic_General_CI_AS;
GO

USE HoshikoDB;
GO

-- Пользователи (логин + хэш пароля)
CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(100) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(256) NOT NULL
);
GO

-- Справочник: тип медиа (Movies, Series, Music, ...)
CREATE TABLE MediaContent (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(20) NOT NULL UNIQUE  -- "Movie", "Series", "Music"
);

-- Общая таблица жанров (связь с MediaContent)
CREATE TABLE Genres (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    MediaContentId INT NOT NULL,

    UNIQUE(Name, MediaContentId),
    CONSTRAINT FK_Genres_MediaContent FOREIGN KEY (MediaContentId) REFERENCES MediaContent(Id)
);

-- Фильмы
CREATE TABLE Movies (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(300) NOT NULL,
    FilePath NVARCHAR(1000) NOT NULL,
    UploadDate DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    UploadedByUserId INT NOT NULL,
	GenreId INT NOT NULL,

    CONSTRAINT FK_Movies_Users FOREIGN KEY (UploadedByUserId) REFERENCES Users(Id),
	CONSTRAINT FK_Movies_Genres FOREIGN KEY (GenreId) REFERENCES Genres(Id)
);
GO

-- Сериалы
CREATE TABLE Series (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(300) NOT NULL,
    FilePath NVARCHAR(1000) NOT NULL,
    UploadDate DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    UploadedByUserId INT NOT NULL,
	GenreId INT NOT NULL,

    CONSTRAINT FK_Series_Users FOREIGN KEY (UploadedByUserId) REFERENCES Users(Id),
	CONSTRAINT FK_Series_Genres FOREIGN KEY (GenreId) REFERENCES Genres(Id)
);
GO

-- Эпизоды сериалов
CREATE TABLE Episodes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    SeriesId INT NOT NULL,
    Title NVARCHAR(300) NULL,
    EpisodeNumber INT NOT NULL,
    FilePath NVARCHAR(1000) NOT NULL,
    UploadDate DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    UploadedByUserId INT NOT NULL,

    CONSTRAINT FK_Episodes_Series        FOREIGN KEY (SeriesId)        REFERENCES Series(Id),
    CONSTRAINT FK_Episodes_Users_Upload  FOREIGN KEY (UploadedByUserId) REFERENCES Users(Id)
);
GO

-- Музыкальные треки
CREATE TABLE Music (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Title NVARCHAR(300) NOT NULL,
    FilePath NVARCHAR(1000) NOT NULL,
    UploadDate DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
    UploadedByUserId INT NOT NULL,
	GenreId INT NOT NULL,

    CONSTRAINT FK_Music_Users FOREIGN KEY (UploadedByUserId) REFERENCES Users(Id),
	CONSTRAINT FK_Music_Genres FOREIGN KEY (GenreId) REFERENCES Genres(Id)
);
GO

CREATE TABLE UserFavoriteGenres (
    UserId INT NOT NULL,
    GenreId INT NOT NULL,

    CONSTRAINT PK_UserFavoriteGenres PRIMARY KEY (UserId, GenreId),
    CONSTRAINT FK_UserFavoriteGenres_Users  FOREIGN KEY (UserId)  REFERENCES Users(Id),
    CONSTRAINT FK_UserFavoriteGenres_Genres FOREIGN KEY (GenreId) REFERENCES Genres(Id)
);