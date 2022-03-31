namespace DamienVDK.Pizza_Mug.Strategies;

public sealed class DefaultDetectIntentAndGetResponseStrategy : IDetectIntentAndGetResponseStrategy
{
    private readonly IServiceProvider _serviceProvider;

    public DefaultDetectIntentAndGetResponseStrategy(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<WebhookResponse> DetectIntentAndReturnResponseAsync(WebhookRequest request)
    {
        var type = typeof(IIntentHandler);
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => type.IsAssignableFrom(p));

        foreach (var currentType in types)
        {
            var attributes = currentType.GetCustomAttributes(true);
            foreach (var attribute in attributes)
            {
                if (attribute is not IntentAttribute intentAttribute) continue;
                if (!Regex.IsMatch(request.QueryResult?.Intent?.DisplayName ?? string.Empty, intentAttribute.Name)) continue;

                var instance = (IIntentHandler)Activator.CreateInstance(currentType, new object[] { _serviceProvider })!;
                return await instance.GetResponseAsync(request);
            }
        }

        return null!;
    }
}