using Hoshiko.Context;
using Hoshiko.Models.Entity;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;

namespace Hoshiko.Repository.Implementation
{
    public class MusicRepository : IRepository<MusicEntity>
    {
        private Logger logger = new Logger();

        public int Add(MusicEntity item)
        {
            using (var db = new HoshikoDbContext())
            {
                db.Music.Add(item);
                db.SaveChanges();
                return item.Id;
            }
        }

        public bool Update(MusicEntity item)
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
                var music = db.Music.Find(id);
                if (music == null)
                    return false;

                db.Music.Remove(music);
                return db.SaveChanges() > 0;
            }
        }

        public MusicEntity GetById(int id)
        {
            using (var db = new HoshikoDbContext())
            {
                return db.Music.FirstOrDefault(x => x.Id == id);
            }
        }

        public List<MusicEntity> GetAll()
        {
            using (var db = new HoshikoDbContext())
            {
                try
                {
                    return db.Music.ToList();
                }
                catch (System.Data.Entity.Core.EntityCommandExecutionException ex)
                {
                    logger.Info("InnerException: " + ex.InnerException?.Message);
                    throw;
                }
            }
        }

        public List<MusicEntity> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<MusicEntity>();

            using (var db = new HoshikoDbContext())
            {
                return db.Music
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
                return db.Music.Any(x => x.SourcePath == sourcePath);
            }
        }
    }
}