using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System.Security.Claims;

namespace IdentityServer4Center
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new IdentityResource[] {
                new IdentityResources.OpenId(),
                 new IdentityResources.Email(),
                 new IdentityResources.Profile(),
            };
        }

        /// <summary>
        /// 定义API范围
        /// https://www.cnblogs.com/Mamba8-24
        /// </summary>
        public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
           new ApiScope("serviceA"),
           new ApiScope("serviceB")
        };

        /// <summary>
        /// 定义API资源
        /// https://www.cnblogs.com/Mamba8-24
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>()
            {
                new ApiResource("serviceA","serviceA"){ Scopes={ "serviceA" } },
                new ApiResource("serviceB","serviceB"){ Scopes={ "serviceB" } }
            };
        }

        /// <summary>
        /// 定义客户端
        /// https://www.cnblogs.com/Mamba8-24
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
               new Client{
                   ClientId="web_client",//客户端唯一标识
                   ClientName="AuthCenter",
                   AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                   //AllowedGrantTypes=GrantTypes.ClientCredentials,
                   ClientSecrets=new[]{new Secret("Mamba24".Sha256()) },//客户端密码，进行了加密
                   AccessTokenLifetime=3600,
                   AllowedScopes=new List<string>//允许访问的资源
                   {
                        "serviceA",
                        "serviceB",
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.Profile,
                   },
               }
            };
        }

        /// <summary>
        /// In-memory user
        /// </summary>
        /// <returns></returns>
        public static List<TestUser> GetTestUsers()
        {
            return new List<TestUser>
            {
                new TestUser()
                {
                     SubjectId = "11111111",

                     Username = "Mamba24",
                     Password = "666666",
                       Claims=new List<Claim>(){
                   new Claim(IdentityModel.JwtClaimTypes.Email,"Admin"),
                   new Claim(IdentityModel.JwtClaimTypes.Profile,"Mamba24"),
                }
                }
            };
        }
    }
}
