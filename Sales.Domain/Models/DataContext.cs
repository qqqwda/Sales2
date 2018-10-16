using Sales.Common.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sales.Domain.Models
{
    public class DataContext : DbContext
    {
        public DataContext() : base("DefaultConnection")
        {

        }

        public DbSet<Product> Products { get; set; }//Mapea en la bd y llama a la clase "Product" "Products" en una tabla en la BD
        public DbSet<Category> Categories { get; set; }


        
    }
}
