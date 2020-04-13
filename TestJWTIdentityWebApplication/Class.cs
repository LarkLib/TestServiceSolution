using IdentityServer4.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestJWTIdentityWebApplication
{
    public static class Config
    {
        public static IEnumerable<ApiResource> Apis
            => new List<ApiResource>
        {
            new ApiResource("api1","My API")
        };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {
                new Client
                {
                    ClientId="client",
                    AllowedGrantTypes =GrantTypes.ClientCredentials,
                    ClientSecrets={
                    new Secret("aju".Sha256())
                    },
                    AllowedScopes={ "api1"}
                }
            };
    }
}
