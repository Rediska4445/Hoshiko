# Hoshiko Media

Настольное приложение на платформе WPF (Windows Presentation Foundation) с архитектурой MVVM для управления медиаконтентом (музыка, фильмы, сериалы) и отображения программы телепередач. Проект использует Entity Framework для взаимодействия с СУБД SQL Server.

---

## Архитектура базы данных

Ниже представлена структура таблиц и связей в формате Mermaid.

```mermaid
erDiagram
    Users {
        int Id PK
        nvarchar Username
        nvarchar PasswordHash
    }
    MediaContent {
        int Id PK
        nvarchar Name
    }
    Genres {
        int Id PK
        nvarchar Name
        int MediaContentId FK
    }
    Movies {
        int Id PK
        nvarchar Title
        nvarchar FilePath
        datetime2 UploadDate
        int UploadedByUserId FK
        int GenreId FK
    }
    Series {
        int Id PK
        nvarchar Title
        nvarchar FilePath
        datetime2 UploadDate
        int UploadedByUserId FK
        int GenreId FK
    }
    Episodes {
        int Id PK
        int SeriesId FK
        nvarchar Title
        int EpisodeNumber
        nvarchar FilePath
        datetime2 UploadDate
        int UploadedByUserId FK
    }
    Music {
        int Id PK
        nvarchar Title
        nvarchar FilePath
        datetime2 UploadDate
        int UploadedByUserId FK
        int GenreId FK
    }
    UserFavoriteGenres {
        int UserId PK, FK
        int GenreId PK, FK
    }
    TvPrograms {
        int Id PK
        nvarchar Title
        nvarchar ChannelName
        datetime2 StartTime
    }

    MediaContent ||--o{ Genres : "содержит"
    Genres ||--o{ Movies : "классифицирует"
    Genres ||--o{ Series : "классифицирует"
    Genres ||--o{ Music : "классифицирует"
    Users ||--o{ Movies : "загружает"
    Users ||--o{ Series : "загружает"
    Users ||--o{ Music : "загружает"
    Users ||--o{ Episodes : "загружает"
    Series ||--o{ Episodes : "содержит"
    Users ||--o{ UserFavoriteGenres : "выбирает"
    Genres ||--o{ UserFavoriteGenres : "включается в"
```

---

## Установка и запуск

### Сборка из исходного кода
Для ручной сборки необходима среда разработки Visual Studio 2022 с установленной рабочей нагрузкой "Разработка настольных приложений .NET".

### Скачивание готовой версии
Чтобы скачать готовую сборку приложения:
1. Перейдите в раздел **Releases** на GitHub.
2. Скачайте последнюю доступную версию (там будет представлен ZIP-архив).
3. Распакуйте содержимое архива в удобную папку на жестком диске.

---

## Инструкция по эксплуатации

1. **Подготовка базы данных**  
   Обязательно перед работой выполнить все скрипты из папки `db` на вашем экземпляре SQL Server. Скрипты автоматически развернут базу данных HoshikoDB, создадут таблицы и заполнят их тестовыми жанрами и файлами.

2. **Запуск программы**  
   Запустите файл `Hoshiko.exe` из папки с приложением.

3. **Авторизация**  
   Для входа в систему используйте следующие демонстрационные учетные данные:
   * **Логин:** byak
   * **Пароль:** 123456

4. **Просмотр контента**  
   Все функции приложения находятся во вкладках («Музыка», «Фильмы», «Сериалы», «Телепередачи», «Жанры»). При переключении между ними на экране будут отображаться интерактивные таблицы со списками контента, кнопками управления и блоками рекомендаций.
