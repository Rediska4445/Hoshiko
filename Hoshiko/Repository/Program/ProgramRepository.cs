using Hoshiko.Context;
using Hoshiko.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoshiko.Repository.Program
{
    public class ProgramRepository : IRepository<TvProgramEntity>
    {
        public int Add(TvProgramEntity item)
        {
            using (var db = new HoshikoDbContext())
            {
                db.TvPrograms.Add(item);
                db.SaveChanges();
                return item.Id;
            }
        }

        public bool Update(TvProgramEntity item)
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
                var program = db.TvPrograms.Find(id);
                if (program == null)
                    return false;

                db.TvPrograms.Remove(program);
                return db.SaveChanges() > 0;
            }
        }

        public TvProgramEntity GetById(int id)
        {
            using (var db = new HoshikoDbContext())
            {
                return db.TvPrograms.Find(id);
            }
        }

        public List<TvProgramEntity> GetAll()
        {
            using (var db = new HoshikoDbContext())
            {
                return db.TvPrograms.ToList();
            }
        }

        public List<TvProgramEntity> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<TvProgramEntity>();

            using (var db = new HoshikoDbContext())
            {
                return db.TvPrograms
                    .Where(p => p.Title.Contains(query) || p.ChannelName.Contains(query))
                    .ToList();
            }
        }
    }
}
