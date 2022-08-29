using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogService.Repository
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetByID(int ID);
        void AddNew(T obj);
        void Update(T obj);
        void Delete(int ID);
        void Save();
        Task<int> SaveAsync();
    }
}
