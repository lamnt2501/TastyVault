using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using TastyVault.Models;

namespace TastyVault
{
    public class DbConfig
    {
        public const string connectionString = "Data Source=VUAN\\SQLEXPRESS;Initial Catalog=tastyvaultdb;User Id=sa;Password=123456;Encrypt=False";
        public AppDbContext CreateContext() 
            => new AppDbContext(
                new DbContextOptionsBuilder<AppDbContext>().UseSqlServer(connectionString).Options);
        
    }
}
