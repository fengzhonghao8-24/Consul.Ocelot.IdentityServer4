using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<ConsulRegisterOptions>(builder.Configuration.GetSection("ConsulRegisterOptions"));
builder.Services.AddConsulRegister();

var authenticationProviderKey = "serviceB";
builder.Services.AddAuthentication(authenticationProviderKey)
              .AddJwtBearer(authenticationProviderKey, options =>
              {
                  options.Authority = "http://localhost:5269";//id4�����ַ
                  options.Audience = "serviceB";//id4 api��Դ���apiname
                  options.RequireHttpsMetadata = false; //��ʹ��https
                  options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
              });

builder.Services.AddAuthorization();

var app = builder.Build();

app.Services.GetService<IConsulRegister>()!.ConsulRegistAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHealthCheckMiddleware();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/testB", [Authorize] (IConfiguration configuration) =>
{
    return $"{Assembly.GetExecutingAssembly().FullName}����ǰʱ�䣺{DateTime.Now:G};Port��{configuration["ConsulRegisterOptions:Port"]}";
});

app.Run();
