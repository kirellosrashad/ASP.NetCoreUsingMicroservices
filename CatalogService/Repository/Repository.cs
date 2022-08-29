using CatalogService.DataBase;
using CatalogService.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogService.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        ProductDBContext dbContext;
        DbSet<T> table;

        ProductDBContext _catalogDBContext;
        public Repository()
        {
            dbContext = new ProductDBContext();
            table = dbContext.Set<T>();
        }
        public Repository(ProductDBContext _dbcontext)
        {
            dbContext = _dbcontext;
            table = dbContext.Set<T>();
        }
        public IEnumerable<T> GetAll()
        {
            return table.ToList();
        }

        public void AddNew(T obj)
        {
            table.Add(obj);
        }

        public T GetByID(int ID)
        {
            return table.Find(ID);
        }

        public void Save()
        {
            dbContext.SaveChanges();
        }

        public Task<int> SaveAsync()
        {
            return dbContext.SaveChangesAsync();
        }

        public void Update(T obj)
        {
            table.Attach(obj);
            dbContext.Entry(obj).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }

        public void Delete(int ID)
        {
            T obj = table.Find(ID);
            table.Remove(obj);
        }
    }
}
