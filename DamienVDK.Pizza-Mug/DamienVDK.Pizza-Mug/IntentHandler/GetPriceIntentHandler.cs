namespace DamienVDK.Pizza_Mug.IntentHandler;

[Intent("^GetPriceIntent$")]
public sealed class GetPriceIntentHandler : IIntentHandler
{
    private readonly OrderRepository _orderRepository;
    
    public GetPriceIntentHandler(IServiceProvider serviceProvider)
    {
        _orderRepository = serviceProvider.GetService<OrderRepository>();
    }

    public async Task<WebhookResponse> GetResponseAsync(WebhookRequest request)
    {
        var session = request.Session;
        var order = await _orderRepository.GetOrderBySession(session);
        var price = order.OrderPizzas?.Sum(orderPizza => orderPizza.Quantity * orderPizza?.Pizza?.Price);

        if (order.OrderId == 0)
        {
            return new WebhookResponse
            {
                FulfillmentText = $"Vous n'avez pas de commande en cours.",
                Source = "pizzamug.azurewebsites.net"
            }; 
        }

        return new WebhookResponse
        {
            FulfillmentText = $"Le montant de votre commande est de {price} euros.",
            Source = "pizzamug.azurewebsites.net"
        }; 
    }
}