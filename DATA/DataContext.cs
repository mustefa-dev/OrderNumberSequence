using Microsoft.EntityFrameworkCore;
using OrderNumberSequence.Entities;

namespace OrderNumberSequence.DATA;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }


    public DbSet<AppUser> Users { get; set; }
    


    // here to add
public DbSet<OrderProduct> OrderProducts { get; set; }
public DbSet<Order> Orders { get; set; }
public DbSet<Product> Products { get; set; }
public DbSet<Message> Messages { get; set; }
    public DbSet<Notifications> Notifications { get; set; }


    public virtual async Task<int> SaveChangesAsync(Guid? userId = null)
    {
        // await OnBeforeSaveChanges(userId);
        var result = await base.SaveChangesAsync();
        return result;
    }
}

public class DbContextOptions<T>
{
}
