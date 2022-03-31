namespace DamienVDK.Pizza_Mug.IntentHandler;

public interface IIntentHandler
{
    Task<WebhookResponse> GetResponseAsync(WebhookRequest request);
}