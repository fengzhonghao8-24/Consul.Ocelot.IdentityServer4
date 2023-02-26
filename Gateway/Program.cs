using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);

var authenticationProviderKey = "serviceA";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddIdentityServerAuthentication(authenticationProviderKey, options =>
              {
                  options.Authority = "http://localhost:5269";//id4服务地址
                  options.ApiName = "serviceA";//id4 api资源里的apiname
                  options.RequireHttpsMetadata = false; //不使用https
                  options.SupportedTokens = SupportedTokens.Both;
              });

authenticationProviderKey = "serviceB";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddIdentityServerAuthentication(authenticationProviderKey, options =>
              {
                  options.Authority = "http://localhost:5269";//id4服务地址
                  options.ApiName = "serviceB";//id4 api资源里的apiname
                  options.RequireHttpsMetadata = false; //不使用https
                  options.SupportedTokens = SupportedTokens.Both;
              });

builder.Services.AddOcelot().AddConsul();

builder.Services.AddLogging(builder =>
{
    builder.AddFilter("Ocelot", LogLevel.Warning);
    builder.AddLog4Net();
});

var app = builder.Build();

app.UseOcelot();

app.Run();
