using Hoshiko.Context;
using Hoshiko.Models.Entity;
using Hoshiko.Repository;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Collections.Generic;
using System.Linq;

namespace Hoshiko.Repository.Implementation
{
    public class SeriesRepository : IRepository<SeriesEntity>
    {
        public int Add(SeriesEntity item)
        {
            using (var db = new HoshikoDbContext())
            {
                db.Series.Add(item);
                db.SaveChanges();
                return item.Id;
            }
        }

        public bool Update(SeriesEntity item)
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
                var series = db.Series.Find(id);
                if (series == null)
                    return false;

                db.Series.Remove(series);
                return db.SaveChanges() > 0;
            }
        }

        public SeriesEntity GetById(int id)
        {
            using (var db = new HoshikoDbContext())
            {
                return db.Series
                         .Include(s => s.Episodes)
                         .FirstOrDefault(s => s.Id == id);
            }
        }

        public List<SeriesEntity> GetAll()
        {
            using (var db = new HoshikoDbContext())
            {
                return db.Series
                         .Include(s => s.Episodes)
                         .ToList();
            }
        }

        public List<SeriesEntity> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<SeriesEntity>();

            using (var db = new HoshikoDbContext())
            {
                return db.Series
                         .Include(s => s.Episodes)
                         .Where(x => x.Title.Contains(query) || x.SourcePath.Contains(query))
                         .ToList();
            }
        }
    }
}