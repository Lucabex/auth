using Microsoft.EntityFrameworkCore;
using auth.Models;
namespace auth.Data;

public class AppDbContext :DbContext
{   
    public AppDbContext(DbContextOptions<AppDbContext>options) : base(options)
    {
        
    }
   public DbSet<User> Users {get; set;}
}