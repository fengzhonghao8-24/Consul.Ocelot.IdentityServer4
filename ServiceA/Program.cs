using IdentityModel.Client;
using Newtonsoft.Json.Linq;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<ConsulRegisterOptions>(builder.Configuration.GetSection("ConsulRegisterOptions"));
builder.Services.AddConsulRegister();

var app = builder.Build();

app.Services.GetService<IConsulRegister>()!.ConsulRegistAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHealthCheckMiddleware();

app.UseHttpsRedirection();

app.MapGet("/testA", (IConfiguration configuration) =>
{
    return $"{Assembly.GetExecutingAssembly().FullName};当前时间：{DateTime.Now:G};Port：{configuration["ConsulRegisterOptions:Port"]}";
});

app.MapGet("/GetIdentityResource",async () =>
{
    var client = new HttpClient();
    var disco = await client.GetDiscoveryDocumentAsync("http://localhost:5269/");

    if (disco.IsError)
    {
        Console.WriteLine(disco.Error);
    }

    // request access token
    //var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
    //{
    //    Address = disco.TokenEndpoint,
    //    ClientId = "web_client",
    //    ClientSecret = "Mamba24",
    //    Scope = "serviceA openid",
    //    UserName= "Mamba24",
    //    Password="666666"
    //});

    var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
    {
        Address = disco.TokenEndpoint,
        ClientId = "web_client",
        ClientSecret = "Mamba24",
        Scope = "serviceA"
    });

    if (tokenResponse.IsError)
    {
        Console.WriteLine(tokenResponse.Error);
    }

    // call Identity Resource API
    var apiClient = new HttpClient();
    apiClient.SetBearerToken(tokenResponse.AccessToken);
    var response = await apiClient.GetAsync(disco.UserInfoEndpoint);
    return response;
});

app.Run();

