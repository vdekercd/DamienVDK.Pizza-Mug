namespace DamienVDK.Pizza_Mug.IntentHandler;

[Intent("^EndCommandIntent$")]
public class EndCommandIntentHandler : IIntentHandler
{
    private readonly OrderRepository _orderRepository;
    
    public EndCommandIntentHandler(IServiceProvider serviceProvider)
    {
        _orderRepository = serviceProvider.GetService<OrderRepository>() 
                           ?? throw new InvalidOperationException($"{nameof(OrderRepository)} is not registered in the service provider");
    }


    public async Task<WebhookResponse> GetResponseAsync(WebhookRequest request)
    {
        var session = request.Session;
        var order = await _orderRepository.GetOrderBySession(session);
        var price = order.OrderPizzas?.Sum(orderPizza => orderPizza.Quantity * orderPizza?.Pizza?.Price);
        var preparationInMinutes = order.OrderPizzas?.Sum(orderPizza => orderPizza.Quantity * orderPizza?.Pizza?.PreparationTimeInMinuutes);

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
            FulfillmentText = $"Le montant total de votre commande est de {price} euros. Elle sera prête dans {preparationInMinutes} minutes.",
            Source = "pizzamug.azurewebsites.net"
        }; 
    }
}