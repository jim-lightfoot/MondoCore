using System;

using Azure.Identity;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;

using MondoCore.Common;

namespace MondoCore.Azure.Configuration
{
    /// <summary>
    /// Access configuration entries in Azure App Configuration
    /// </summary>
    public class AzureConfiguration : ISettings
    {
        private readonly IConfiguration _config;

        public AzureConfiguration(string endPoint)
        {
            var builder = new ConfigurationBuilder();

            builder.AddAzureAppConfiguration(options =>
            {
                options.Connect(new Uri(endPoint), new DefaultAzureCredential());

                options.ConfigureKeyVault(kv =>
                {
                    kv.SetCredential(new ManagedIdentityCredential());
                });
            });

            _config = builder.Build();
        }

        public AzureConfiguration(string connectionString, string tenantId, string clientId, string secret)
        {
            var builder = new ConfigurationBuilder();

            builder.AddAzureAppConfiguration(options =>
            {
                options.Connect(connectionString);

                options.ConfigureKeyVault(kv =>
                {
                    kv.SetCredential(new ClientSecretCredential(tenantId, clientId, secret));
                });
            });

            _config = builder.Build();
        }

        #region ISettings

        public string Get(string key)
        {
            return _config[key];
        }

        #endregion
    }
}
