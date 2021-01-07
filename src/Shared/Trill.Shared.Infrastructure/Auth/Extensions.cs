using System;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Trill.Shared.Abstractions.Auth;

namespace Trill.Shared.Infrastructure.Auth
{
    internal static class Extensions
    {
        private const string SectionName = "jwt";

        public static IServiceCollection AddJwt(this IServiceCollection services, string sectionName = SectionName,
            Action<JwtBearerOptions> optionsFactory = null)
        {
            if (string.IsNullOrWhiteSpace(sectionName))
            {
                sectionName = SectionName;
            }

            var options = services.GetOptions<JwtOptions>(sectionName);
            services.AddSingleton<IJwtHandler, JwtHandler>();
            services.AddSingleton<IAccessTokenService, InMemoryAccessTokenService>();
            services.AddScoped<AccessTokenValidatorMiddleware>();

            if (options.AuthenticationDisabled)
            {
                services.AddSingleton<IPolicyEvaluator, DisabledAuthenticationPolicyEvaluator>();
            }

            var tokenValidationParameters = new TokenValidationParameters
            {
                RequireAudience = options.RequireAudience,
                ValidIssuer = options.ValidIssuer,
                ValidIssuers = options.ValidIssuers,
                ValidateActor = options.ValidateActor,
                ValidAudience = options.ValidAudience,
                ValidAudiences = options.ValidAudiences,
                ValidateAudience = options.ValidateAudience,
                ValidateIssuer = options.ValidateIssuer,
                ValidateLifetime = options.ValidateLifetime,
                ValidateTokenReplay = options.ValidateTokenReplay,
                ValidateIssuerSigningKey = options.ValidateIssuerSigningKey,
                SaveSigninToken = options.SaveSigninToken,
                RequireExpirationTime = options.RequireExpirationTime,
                RequireSignedTokens = options.RequireSignedTokens,
                ClockSkew = TimeSpan.Zero
            };

            if (!string.IsNullOrWhiteSpace(options.AuthenticationType))
            {
                tokenValidationParameters.AuthenticationType = options.AuthenticationType;
            }

            var hasCertificate = false;
            if (options.Certificate is {})
            {
                X509Certificate2 certificate = null;
                var password = options.Certificate.Password;
                var hasPassword = !string.IsNullOrWhiteSpace(password);
                if (!string.IsNullOrWhiteSpace(options.Certificate.Location))
                {
                    certificate = hasPassword
                        ? new X509Certificate2(options.Certificate.Location, password)
                        : new X509Certificate2(options.Certificate.Location);
                    var keyType = certificate.HasPrivateKey ? "with private key" : "with public key only";
                    Console.WriteLine($"Loaded X.509 certificate from location: '{options.Certificate.Location}' {keyType}.");
                }
                
                if (!string.IsNullOrWhiteSpace(options.Certificate.RawData))
                {
                    var rawData = Convert.FromBase64String(options.Certificate.RawData);
                    certificate = hasPassword
                        ? new X509Certificate2(rawData, password)
                        : new X509Certificate2(rawData);
                    var keyType = certificate.HasPrivateKey ? "with private key" : "with public key only";
                    Console.WriteLine($"Loaded X.509 certificate from raw data {keyType}.");
                }

                if (certificate is {})
                {
                    if (string.IsNullOrWhiteSpace(options.Algorithm))
                    {
                        options.Algorithm = SecurityAlgorithms.RsaSha256;
                    }

                    hasCertificate = true;
                    tokenValidationParameters.IssuerSigningKey = new X509SecurityKey(certificate);
                    var actionType = certificate.HasPrivateKey ? "issuing" : "validating";
                    Console.WriteLine($"Using X.509 certificate for {actionType} tokens.");
                }
            }

            if (!string.IsNullOrWhiteSpace(options.IssuerSigningKey) && !hasCertificate)
            {
                if (string.IsNullOrWhiteSpace(options.Algorithm) || hasCertificate)
                {
                    options.Algorithm = SecurityAlgorithms.HmacSha256;
                }

                var rawKey = Encoding.UTF8.GetBytes(options.IssuerSigningKey);
                tokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(rawKey);
                Console.WriteLine("Using symmetric encryption for issuing tokens.");
            }

            if (!string.IsNullOrWhiteSpace(options.NameClaimType))
            {
                tokenValidationParameters.NameClaimType = options.NameClaimType;
            }

            if (!string.IsNullOrWhiteSpace(options.RoleClaimType))
            {
                tokenValidationParameters.RoleClaimType = options.RoleClaimType;
            }

            services
                .AddAuthentication(o =>
                {
                    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(o =>
                {
                    o.Authority = options.Authority;
                    o.Audience = options.Audience;
                    o.MetadataAddress = options.MetadataAddress;
                    o.SaveToken = options.SaveToken;
                    o.RefreshOnIssuerKeyNotFound = options.RefreshOnIssuerKeyNotFound;
                    o.RequireHttpsMetadata = options.RequireHttpsMetadata;
                    o.IncludeErrorDetails = options.IncludeErrorDetails;
                    o.TokenValidationParameters = tokenValidationParameters;
                    if (!string.IsNullOrWhiteSpace(options.Challenge))
                    {
                        o.Challenge = options.Challenge;
                    }

                    optionsFactory?.Invoke(o);
                });

            services.AddSingleton(options);
            services.AddSingleton(tokenValidationParameters);

            return services;
        }

        public static IApplicationBuilder UseAccessTokenValidator(this IApplicationBuilder app)
            => app.UseMiddleware<AccessTokenValidatorMiddleware>();
    }
}