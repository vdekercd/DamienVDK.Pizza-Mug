using System.Diagnostics.CodeAnalysis;

namespace DamienVDK.Pizza_Mug.Repositories;

public sealed class OrderRepository
{
    private readonly OrderContext _context;
    
    public OrderRepository(OrderContext context)
    {
        _context = context;
    }
    
    public void Add(Order order)
    {
        _context.Orders.Add(order);
    }

    public async Task<Order> GetOrderBySession(string session)
    {
        var order = await _context.Orders
            .Include("OrderPizzas.Pizza")
            .FirstOrDefaultAsync(item => item.Session == session);
        
        return order ?? new Order() {Session = session, OrderPizzas = new List<OrderPizza>()};
    }
    
    public async Task<List<Pizza>> GetPizzasAsync()
    {
        return await _context.Pizzas.ToListAsync();
    }
    
    public async Task<bool> SaveChangesAsync()
    {
        return (await _context.SaveChangesAsync()) > 0;
    }
}