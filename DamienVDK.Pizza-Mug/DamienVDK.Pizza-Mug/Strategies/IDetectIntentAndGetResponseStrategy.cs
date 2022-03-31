namespace DamienVDK.Pizza_Mug.Strategies;

public interface IDetectIntentAndGetResponseStrategy
{
    Task<WebhookResponse> DetectIntentAndReturnResponseAsync(WebhookRequest request);
}