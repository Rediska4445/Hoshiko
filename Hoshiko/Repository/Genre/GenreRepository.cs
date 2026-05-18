using Hoshiko.Context;
using Hoshiko.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoshiko.Repository.Genre
{
    public class GenreRepository : IRepository<GenreEntity>
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
                var genre = db.Genres.Find(id);
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
                         .FirstOrDefault(x => x.Id == id);
            }
        }

        public List<GenreEntity> GetAll()
        {
            using (var db = new HoshikoDbContext())
            {
                return db.Genres.ToList();
            }
        }

        public List<GenreEntity> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<GenreEntity>();

            using (var db = new HoshikoDbContext())
            {
                return db.Genres
                         .Where(x => x.Name.Contains(query))
                         .ToList();
            }
        }
    }
}
