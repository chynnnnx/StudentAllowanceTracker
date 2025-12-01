using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace StudentAllowanceTracker.Infrastructure.Persistence.Data
{
    public class AppDbContextFactory:IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
            

            optionsBuilder.UseSqlServer(
              "Server=.;Database=AllowanceTrackerDB;Trusted_Connection=True;TrustServerCertificate=true;MultipleActiveResultSets=true"
            );

            return new AppDbContext(optionsBuilder.Options);
        }
    }
}
