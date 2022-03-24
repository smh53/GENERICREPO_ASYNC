using DataAccess.Entities.Product;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Context
{
    public class GenericRepoContext : DbContext
    {
        public GenericRepoContext(DbContextOptions<GenericRepoContext> options): base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
       

    }
}
