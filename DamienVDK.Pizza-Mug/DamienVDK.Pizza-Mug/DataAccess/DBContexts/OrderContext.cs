namespace DamienVDK.Pizza_Mug.DataAccess.DBContexts;

public sealed class OrderContext : DbContext
{
    public OrderContext(DbContextOptions<OrderContext> options) : base(options)
    {
    }
    
    public DbSet<Order> Orders { get; set; }
    public DbSet<Pizza> Pizzas { get; set; }
    public DbSet<OrderPizza> OrderPizzas { get; set; }
}