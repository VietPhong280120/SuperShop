using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperShop.Data.EF
{
    public class SuperShopDbContextFactory : IDesignTimeDbContextFactory<SuperShopDbContext>
    {
        public SuperShopDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsetting.json")
                .Build();
            var conn = config.GetConnectionString("SuperShopDatabase");
            var optionsBuilder = new DbContextOptionsBuilder<SuperShopDbContext>();
            optionsBuilder.UseSqlServer(conn);

            return new SuperShopDbContext(optionsBuilder.Options);
        }
    }
}