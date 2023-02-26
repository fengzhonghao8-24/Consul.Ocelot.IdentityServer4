using IdentityServer4Center;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddIdentityServer()
    .AddDeveloperSigningCredential()
    .AddInMemoryIdentityResources(Config.GetIdentityResources())
    .AddInMemoryClients(Config.GetClients())//Client模式
    .AddInMemoryApiScopes(Config.ApiScopes)//作用域
    .AddInMemoryApiResources(Config.GetApiResources())//资源
    .AddTestUsers(Config.GetTestUsers());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseIdentityServer();//使用Id4

app.Run();
