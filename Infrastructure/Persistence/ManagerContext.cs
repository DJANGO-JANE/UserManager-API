using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence
{
    public class ManagerContext:DbContext
    {
        public ManagerContext(DbContextOptions<ManagerContext> options) : base(options) 
        {

        }
/*        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                                    .HasData(
                new User { Id=1, FirstName = "Django", LastName = "Jane", Email = "django@jane.com", DateJoined = DateTime.Now,Username="django" }
                );
        }*/

        public DbSet<User> UserRegistry { get; set; }
    }
}
