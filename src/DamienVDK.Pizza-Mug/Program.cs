var jsonParser = new JsonParser(JsonParser.Settings.Default.WithIgnoreUnknownFields(true));

// Configure services
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .Configure<KestrelServerOptions>(options => options.AllowSynchronousIO = true)
    .AddTransient<IDetectIntentAndGetResponseStrategy, DefaultDetectIntentAndGetResponseStrategy>()
    .AddScoped<OrderRepository>()
    .AddDbContext<OrderContext>(
        options => options.UseSqlServer(builder.Configuration.GetConnectionString("OrderDatabase"), 
  providerOptions => providerOptions.EnableRetryOnFailure()))
    ;

// Configure app
var app = builder.Build();
app.UseSwagger()
   .UseSwaggerUI();

// Map POST request
app.MapPost("/DialogFlow", async (HttpRequest httpRequest, IDetectIntentAndGetResponseStrategy detectIntentAndGetResponseStrategy) =>
{
    WebhookRequest request;
    using (var reader = new StreamReader(httpRequest.Body))
    {
        request = jsonParser.Parse<WebhookRequest>(reader);
    }

    var jsonResponse = (await detectIntentAndGetResponseStrategy.DetectIntentAndReturnResponseAsync(request)).ToString();
    return new ContentResult { Content = jsonResponse, ContentType = "application/json" }.Content;
});

// And of course...
app.Run();