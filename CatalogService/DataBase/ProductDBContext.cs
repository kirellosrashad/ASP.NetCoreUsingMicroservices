using CatalogService.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogService.DataBase
{
    public class ProductDBContext : DbContext
    {
        // this will create Product DB
        public DbSet<Product> products { get; set; }

        // This is the connectionString
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"data source=localhost;initial catalog=ProductMicroService;integrated security=True;");
        }

        // for the first time we should initiate this command "add-migration initial" in Package Manager Console to create the db file in server
        // this will create the DB "update-database -verbose" in Package Manager Console
    }
}
