using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Cryptocop.Software.API.Services.Interfaces;

namespace Cryptocop.Software.API.Middleware
{
    public static class JwtAuthenticationMiddleware
    {
        public static AuthenticationBuilder AddJwtTokenAuthentication(this AuthenticationBuilder builder, IConfiguration config)
        {
            var jwtConfig = config.GetSection("JwtConfig");
            var secret = jwtConfig.GetSection("secret").Value;
            var issuer = jwtConfig.GetSection("issuer").Value;
            var audience = jwtConfig.GetSection("audience").Value;
            var key = Encoding.ASCII.GetBytes(secret);

            builder.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, x => {
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    NameClaimType = "name" // User.Identity.Name
                };
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        var claim = context.Principal.Claims.FirstOrDefault(c => c.Type == "tokenId")?.Value;
                        int.TryParse(claim, out var tokenId);
                        var JwtTokenService = context.HttpContext.RequestServices.GetService<IJwtTokenService>();

                        if (JwtTokenService.IsTokenBlacklisted(tokenId))
                        {
                            context.Response.StatusCode = 401;
                            await context.Response.WriteAsync("JWT token provided is invalid");
                        }
                    }
                };
            });
            return builder;
        }
    }
}