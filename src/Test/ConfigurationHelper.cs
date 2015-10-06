using Microsoft.Dnx.Runtime;
using Microsoft.Dnx.Runtime.Infrastructure;
using Microsoft.Framework.Configuration;

namespace Avalara.Avatax.Rest.Test
{
    public class ConfigurationHelper
    {
        public static IConfigurationSection GetConfiguration()
        {
            var applicationEnvironment =
                (IApplicationEnvironment)
                    CallContextServiceLocator.Locator.ServiceProvider.GetService(typeof (IApplicationEnvironment));
            var configurationBuilder = new ConfigurationBuilder(applicationEnvironment.ApplicationBasePath)
                .AddJsonFile("config.json");
            var configuration = configurationBuilder.Build();
            var configSection = configuration.GetSection("app:avatax");
            return configSection;
        }
    }
}