using System.IdentityModel.Tokens;
using System.Linq;
using System.Web.Configuration;
using System.Web.Http;
using auh0_aspnet_owin_webapi_rs256;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.ActiveDirectory;
using Owin;

[assembly: OwinStartup(typeof(Startup))]
namespace auh0_aspnet_owin_webapi_rs256
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var auth0Issuer = WebConfigurationManager.AppSettings["Auth0Issuer"];
            var auth0Audience = WebConfigurationManager.AppSettings["Auth0Audience"];

            appBuilder.UseCors(CorsOptions.AllowAll);
            var activeDirectoryFederationServicesBearerAuthenticationOptions = new ActiveDirectoryFederationServicesBearerAuthenticationOptions
            {
                MetadataEndpoint = $"{auth0Issuer.TrimEnd('/')}/wsfed/FederationMetadata/2007-06/FederationMetadata.xml",
                TokenValidationParameters =
                    new TokenValidationParameters
                    {
                        ValidAudience = auth0Audience,
                        ValidIssuer = auth0Issuer,
                        IssuerSigningKeyResolver = (token, securityToken, identifier, parameters) => parameters.IssuerSigningTokens.FirstOrDefault()?.SecurityKeys?.FirstOrDefault()
                    }
            };

            appBuilder.UseActiveDirectoryFederationServicesBearerAuthentication(activeDirectoryFederationServicesBearerAuthenticationOptions);

            var httpConfiguration = new HttpConfiguration();
            httpConfiguration.MapHttpAttributeRoutes();
            httpConfiguration.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional });
            appBuilder.UseWebApi(httpConfiguration);
        }
    }
}