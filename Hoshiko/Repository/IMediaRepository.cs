using Hoshiko.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hoshiko.Repository
{
    public interface IMediaRepository<T> where T : MediaItem, new()
    {
        int Add(T item);
        bool Update(T item);
        bool Delete(int id);

        T GetById(int id);
        List<T> GetAll();

        List<T> Search(string query);

        bool ExistsBySource(string sourcePath); 
    }
}
