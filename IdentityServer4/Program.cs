using IdentityServer4Center;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddIdentityServer()
    .AddDeveloperSigningCredential()
    .AddInMemoryIdentityResources(Config.GetIdentityResources())
    .AddInMemoryClients(Config.GetClients())//Clientģʽ
    .AddInMemoryApiScopes(Config.ApiScopes)//������
    .AddInMemoryApiResources(Config.GetApiResources())//��Դ
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

app.UseIdentityServer();//ʹ��Id4

app.Run();
