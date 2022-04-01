namespace DamienVDK.Pizza_Mug.DataAccess.DBContexts;

public sealed class OrderContext : DbContext
{
    public OrderContext(DbContextOptions<OrderContext> options) : base(options)
    {
    }

    public DbSet<Order> Orders => Set<Order>();
    public DbSet<Pizza> Pizzas => Set<Pizza>();
}