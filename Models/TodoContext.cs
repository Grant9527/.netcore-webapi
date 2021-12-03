using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace TodoApi.Models
{
  public class TodoContext : DbContext
  {
    public TodoContext(DbContextOptions<TodoContext> options)
        : base(options)
    {

      // DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder();
      // var config = new ConfigurationBuilder()
      //                       .SetBasePath(System.IO.Directory.GetCurrentDirectory())
      //                       .AddJsonFile("appsettings.json")
      //                       .Build();

      // optionsBuilder.UseSqlServer(config.GetConnectionString("SqlServerConnection"));
    }

    public DbSet<Users> Users { get; set; }
  }
}