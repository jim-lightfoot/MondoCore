using Microsoft.VisualStudio.TestTools.UnitTesting;

using MondoCore.Common;
using MondoCore.Azure.Configuration;
using MondoCore.Azure.TestHelpers;

namespace MondoCore.Azure.Configuration.FunctionalTests
{
    [TestClass]
    public class AzureConfigurationTests
    {
        [TestMethod]
        public void AzureConfiguration_Get()
        {
            var config = CreateConfig();

            Assert.AreEqual(@"http:\\www.bedrock.us", config.Get("BedrockUrl"));
            Assert.AreEqual("bobsyouruncle",          config.Get("BedrockClientId"));
            Assert.AreEqual("wilmaflintstone",        config.Get("BedrockClientSecret"));
        }

        private ISettings CreateConfig(string folder = "")
        { 
            var config = TestConfiguration.Load();

            return new AzureConfiguration(config.ConfigConnectionString,
                                          config.KeyVaultTenantId, 
                                          config.KeyVaultClientId, 
                                          config.KeyVaultClientSecret);
        }
    }
}
