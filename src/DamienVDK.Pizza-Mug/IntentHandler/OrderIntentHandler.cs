using Microsoft.Extensions.Caching.Memory;

namespace DamienVDK.Pizza_Mug.IntentHandler;

[Intent("^OrderIntent$")]
public sealed class OrderIntentHandler : IIntentHandler
{
    private readonly OrderRepository _orderRepository;
    private readonly IMemoryCache _memoryCache;
    
    public OrderIntentHandler(IServiceProvider serviceProvider)
    {
        _orderRepository = serviceProvider.GetService<OrderRepository>()
                           ?? throw new InvalidOperationException($"{nameof(OrderRepository)} is not registered in the service provider");
        _memoryCache = serviceProvider.GetService<IMemoryCache>() 
                       ?? throw new InvalidOperationException($"{nameof(IMemoryCache)} is not registered in the service provider");
    }
    
    public async Task<WebhookResponse> GetResponseAsync(WebhookRequest request)
    {
        var pizzaNames = request.QueryResult.GetStringFieldList("PizzaName").Select(item => item.Replace("\"", "")).ToList();
        var pizzaQuantities = request.QueryResult.GetIntFieldList("number-integer");

        if (pizzaNames.Count == 0 || pizzaNames.Count != pizzaQuantities.Count)
            return ReturnCannotUnderstandResponse();
        
        
        var order = await _orderRepository.GetOrderBySession(request.Session);
        var isNewSession = order.OrderId == 0;

        var knownPizzaNames = await GetPizzaNamesAsync();
        
        var pizzas = new List<Pizza>();

        foreach (var pizzaName in pizzaNames)
        {
            var pizza = knownPizzaNames.FirstOrDefault(p => p.Name.Equals(pizzaName));
        
            if (pizza == null) return NoPizzaWithNameResponse(pizzaName);

            pizzas.Add(pizza);
        }

        for (int i = 0; i < pizzaNames.Count; i++)
        {
            order.OrderPizzas.Add(new OrderPizza()
            {
                Order = order,
                Pizza = pizzas[i],
                Quantity = (int)pizzaQuantities[i]
            });
        }
 
        if (order.OrderId == 0) _orderRepository.Add(order);
        await _orderRepository.SaveChangesAsync();

        string responseText = "Autre chose?";

        if (isNewSession)
        {
            responseText = $"Votre numéro de commande est le {order.OrderId}! " + responseText;
        }
        
        return new WebhookResponse
        {
            FulfillmentText = responseText,
            Source = "pizzamug.azurewebsites.net",
        };
    }

    private static WebhookResponse NoPizzaWithNameResponse(string pizzaName)
    {
        return new WebhookResponse
        {
            FulfillmentText = $"Nous ne faisons pas de pizza {pizzaName}",
            Source = "pizzamug.azurewebsites.net"
        };
    }

    private static WebhookResponse ReturnCannotUnderstandResponse()
    {
        return new WebhookResponse
        {
            FulfillmentText = $"Désolé, je n'ai pas compris. Veuillez répéter votre commande.",
            Source = "pizzamug.azurewebsites.net"
        };
    }
    
    private async Task<List<Pizza>> GetPizzaNamesAsync()
    {
        if (_memoryCache.TryGetValue("Pizzas", out List<Pizza> cacheValue)) return cacheValue;
        
        cacheValue = await _orderRepository.GetPizzasAsync();;

        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromHours(3));

        _memoryCache.Set("Pizzas", cacheValue, cacheEntryOptions);

        return cacheValue;
    }
}