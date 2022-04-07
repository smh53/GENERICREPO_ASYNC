using DataAccess.Entities.Product;
using DataAccess.Entities.Section;
using DataAccess.Entities.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Context
{
    public class GenericRepoContext : IdentityDbContext<User>
    {
        public GenericRepoContext(DbContextOptions<GenericRepoContext> options): base(options)
        {

        }

        public DbSet<Product>? Products { get; set; }
        public DbSet<Section>? Sections { get; set; }




    }
}
