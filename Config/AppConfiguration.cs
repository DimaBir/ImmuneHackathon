using System;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace SemanticKernelExamples
{
    public static class AppConfig
    {
        private static readonly IConfigurationRoot s_config;

        static AppConfig()
        {
            s_config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();
        }

        public static class OpenAI
        {
            public static string DeploymentName =>
                s_config["OpenAI:DeploymentName"] ?? throw Missing("OpenAI:DeploymentName");

            public static string ModelId =>
                s_config["OpenAI:ModelId"] ?? throw Missing("OpenAI:ModelId");

            public static string Endpoint =>
                s_config["OpenAI:Endpoint"] ?? throw Missing("OpenAI:Endpoint");

            public static string ApiKey =>
                s_config["OpenAI:ApiKey"] ?? throw Missing("OpenAI:ApiKey");
        }

        public static class CosmosDB
        {
            public static string ConnectionString =>
                s_config["Cosmos:ConnectionString"] ?? throw Missing("Cosmos:ConnectionString");

            public static string DatabaseName =>
                s_config["Cosmos:DatabaseName"] ?? throw Missing("Cosmos:DatabaseName");

            public static string ContainerName =>
                s_config["Cosmos:ContainerName"] ?? throw Missing("Cosmos:ContainerName");
        }
        
        public static class Kusto
        {
            public static string Uri =>
                s_config["Kusto:Uri"] ?? throw Missing("Kusto:Uri");

            public static string BearerToken =>
                s_config["Kusto:BearerToken"] ?? throw Missing("Kusto:BearerToken");
        }

        // Helper to throw a clear exception if a config value is missing.
        private static Exception Missing(string key)
        {
            return new InvalidOperationException($"Configuration value for '{key}' was not found.");
        }
    }
}
