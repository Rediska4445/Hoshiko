using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoshiko.Repository
{
    public interface IRepository<T>
    {
        int Add(T item);
        bool Update(T item);
        bool Delete(int id);

        T GetById(int id);
        List<T> GetAll();

        List<T> Search(string query);
    }
}
