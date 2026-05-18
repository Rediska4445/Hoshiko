using Hoshiko.Context;
using Hoshiko.Models.Entity;
using System.Collections.Generic;

namespace Hoshiko.Repository.Implementaion
{
    using System.Data.Entity;
    using System.Data.Entity.Validation;
    using System.Linq;

    public class MovieRepository : IMediaRepository<MovieEntity>
    {
        public int Add(MovieEntity item)
        {
            using (var db = new HoshikoDbContext())
            {
                db.Movies.Add(item);
                db.SaveChanges();
                return item.Id;
            }
        }

        public bool Update(MovieEntity item)
        {
            using (var db = new HoshikoDbContext())
            {
                db.Entry(item).State = EntityState.Modified;
                try
                {
                    return db.SaveChanges() > 0;
                }
                catch (DbEntityValidationException)
                {
                    return false;
                }
            }
        }

        public bool Delete(int id)
        {
            using (var db = new HoshikoDbContext())
            {
                var movie = db.Movies.Find(id);
                if (movie == null)
                    return false;

                db.Movies.Remove(movie);
                return db.SaveChanges() > 0;
            }
        }

        public MovieEntity GetById(int id)
        {
            using (var db = new HoshikoDbContext())
            {
                return db.Movies.FirstOrDefault(x => x.Id == id);
            }
        }

        public List<MovieEntity> GetAll()
        {
            using (var db = new HoshikoDbContext())
            {
                return db.Movies.ToList();
            }
        }

        public List<MovieEntity> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<MovieEntity>();

            using (var db = new HoshikoDbContext())
            {
                return db.Movies
                         .Where(x => x.Title.Contains(query) || x.SourcePath.Contains(query))
                         .ToList();
            }
        }

        public bool ExistsBySource(string sourcePath)
        {
            if (string.IsNullOrWhiteSpace(sourcePath))
                return false;

            using (var db = new HoshikoDbContext())
            {
                return db.Movies.Any(x => x.SourcePath == sourcePath);
            }
        }
    }
}
