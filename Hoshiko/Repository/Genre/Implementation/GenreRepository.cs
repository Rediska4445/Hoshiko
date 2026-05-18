using Hoshiko.Context;
using Hoshiko.Models;
using Hoshiko.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoshiko.Repository.Genre
{
    public class GenreRepository : IGenreRepository
    {
        public int Add(GenreEntity item)
        {
            using (var db = new HoshikoDbContext())
            {
                db.Genres.Add(item);
                db.SaveChanges();
                return item.Id;
            }
        }

        public bool Update(GenreEntity item)
        {
            using (var db = new HoshikoDbContext())
            {
                db.Entry(item).State = EntityState.Modified;
                try
                {
                    return db.SaveChanges() > 0;
                }
                catch
                {
                    return false;
                }
            }
        }

        public bool Delete(int id)
        {
            using (var db = new HoshikoDbContext())
            {
                var genre = db.Genres
                              .Include(g => g.MediaContent)
                              .FirstOrDefault(g => g.Id == id);
                if (genre == null)
                    return false;

                db.Genres.Remove(genre);
                return db.SaveChanges() > 0;
            }
        }

        public GenreEntity GetById(int id)
        {
            using (var db = new HoshikoDbContext())
            {
                return db.Genres
                         .Include(g => g.MediaContent)
                         .FirstOrDefault(g => g.Id == id);
            }
        }

        public List<GenreEntity> GetAll()
        {
            using (var db = new HoshikoDbContext())
            {
                return db.Genres
                         .Include(g => g.MediaContent)
                         .ToList();
            }
        }

        public List<GenreEntity> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<GenreEntity>();

            using (var db = new HoshikoDbContext())
            {
                return db.Genres
                         .Include(g => g.MediaContent)
                         .Where(g => g.Name.Contains(query))
                         .ToList();
            }
        }

        public bool AddFavoriteGenres(UserEntity user, List<GenreEntity> genres)
        {
            if (user == null || genres == null || !genres.Any())
                return false;

            using (var db = new HoshikoDbContext())
            {
                var genreIds = genres.Select(g => g.Id).ToList();

                var currentFav = db.UserFavoriteGenres
                                   .Where(ufg => ufg.UserId == user.Id)
                                   .Select(ufg => ufg.GenreId)
                                   .ToHashSet();

                foreach (var genreId in genreIds.Where(id => !currentFav.Contains(id)))
                {
                    db.UserFavoriteGenres.Add(new UserFavoriteGenreEntity
                    {
                        UserId = user.Id,
                        GenreId = genreId
                    });
                }

                return db.SaveChanges() > 0;
            }
        }

        public bool RemoveFavoriteGenres(UserEntity user, List<GenreEntity> genres)
        {
            if (user == null || genres == null || !genres.Any())
                return false;

            using (var db = new HoshikoDbContext())
            {
                var genreIds = genres.Select(g => g.Id).ToList();

                var favorites = db.UserFavoriteGenres
                                  .Where(ufg => ufg.UserId == user.Id && genreIds.Contains(ufg.GenreId));

                db.UserFavoriteGenres.RemoveRange(favorites);

                return db.SaveChanges() > 0;
            }
        }

        public List<GenreEntity> GetGenresByMediaType(string mediaTypeName)
        {
            using (var db = new HoshikoDbContext())
            {
                return db.Genres
                         .Include(g => g.MediaContent)
                         .Where(g => g.MediaContent.Name == mediaTypeName)
                         .ToList();
            }
        }

        public List<GenreEntity> GetAllGenres() => GetAll();

        public GenreEntity GetGenreById(int id) => GetById(id);

        public List<GenreEntity> GetAllFavoriteGenres(UserEntity user)
        {
            if (user == null) return new List<GenreEntity>();

            using (var context = new HoshikoDbContext())
            {
                return context.Set<UserFavoriteGenreEntity>()
                              .Where(ufg => ufg.UserId == user.Id)
                              .Select(ufg => ufg.Genre)
                              .ToList();
            }
        }
    }
}
