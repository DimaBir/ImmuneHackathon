// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;
using Microsoft.SemanticKernel;
using SemanticKernelExamples;

using CosmosContainer = Microsoft.Azure.Cosmos.Container;

namespace GettingStarted
{
    public class Step5_Cosmos_Agent
    {
        [KernelFunction("get_logs_summary")]
        [Description("Retrieves log entries from Cosmos DB and returns a summary grouped by log level.")]
        public async Task<string> GetLogsSummaryAsync()
        {
            CosmosClient cosmosClient = new CosmosClient(AppConfig.CosmosDB.ConnectionString, new CosmosClientOptions()
            {
                UseSystemTextJsonSerializerWithOptions = JsonSerializerOptions.Default,
            });
            Database database = await cosmosClient.CreateDatabaseIfNotExistsAsync(AppConfig.CosmosDB.DatabaseName);
            CosmosContainer container = await database.CreateContainerIfNotExistsAsync(AppConfig.CosmosDB.ContainerName, "/logLevel");

            List<LogEntry> logs = new List<LogEntry>();
            using (FeedIterator<LogEntry> iterator = container.GetItemQueryIterator<LogEntry>("SELECT * FROM c"))
            {
                while (iterator.HasMoreResults)
                {
                    FeedResponse<LogEntry> response = await iterator.ReadNextAsync();
                    logs.AddRange(response);
                }
            }

            if (logs.Count == 0)
            {
                return "No logs found.";
            }

            var summary = logs.GroupBy(log => log.LogLevel)
                              .Select(g => new { LogLevel = g.Key, Count = g.Count() })
                              .ToList();

            string result = "Here's the summary of the logs:\n\n";
            foreach (var group in summary)
            {
                result += $"- **{group.LogLevel}**: {group.Count} entries\n";
            }
            return result;
        }

        [KernelFunction("get_logs_by_level")]
        [Description("Retrieves log entries for a specific log level from Cosmos DB.")]
        public async Task<string> GetLogsByLevelAsync([Description("Represents Log Level")] string logLevel)
        {
            CosmosClient cosmosClient = new CosmosClient(AppConfig.CosmosDB.ConnectionString, new CosmosClientOptions()
            {
                UseSystemTextJsonSerializerWithOptions = JsonSerializerOptions.Default,
            });
            Database database = await cosmosClient.CreateDatabaseIfNotExistsAsync(AppConfig.CosmosDB.DatabaseName);
            CosmosContainer container = await database.CreateContainerIfNotExistsAsync(AppConfig.CosmosDB.ContainerName, "/logLevel");

            List<LogEntry> logs = new List<LogEntry>();
            string query = $"SELECT * FROM c WHERE c.logLevel = '{logLevel}'";
            using (FeedIterator<LogEntry> iterator = container.GetItemQueryIterator<LogEntry>(query))
            {
                while (iterator.HasMoreResults)
                {
                    FeedResponse<LogEntry> response = await iterator.ReadNextAsync();
                    logs.AddRange(response);
                }
            }

            if (logs.Count == 0)
            {
                return $"There are no {logLevel} logs found in the database.";
            }

            string result = $"Found {logs.Count} {logLevel} logs:\n";
            foreach (var log in logs)
            {
                result += $"- {log.Message} (at {log.Timestamp:O})\n";
            }
            return result;
        }

        [KernelFunction("query_logs")]
        [Description("Executes a custom Cosmos DB query provided by the LLM and returns matching log entries.")]
        public async Task<string> ExecuteQueryAsync([Description("Cosmos DB query generated by the LLM")] string query)
        {
            CosmosClient cosmosClient = new CosmosClient(AppConfig.CosmosDB.ConnectionString, new CosmosClientOptions()
            {
                UseSystemTextJsonSerializerWithOptions = JsonSerializerOptions.Default,
            });
            Database database = await cosmosClient.CreateDatabaseIfNotExistsAsync(AppConfig.CosmosDB.DatabaseName);
            CosmosContainer container = await database.CreateContainerIfNotExistsAsync(AppConfig.CosmosDB.ContainerName, "/logLevel");

            List<LogEntry> logs = new List<LogEntry>();
            using (FeedIterator<LogEntry> iterator = container.GetItemQueryIterator<LogEntry>(query))
            {
                while (iterator.HasMoreResults)
                {
                    FeedResponse<LogEntry> response = await iterator.ReadNextAsync();
                    logs.AddRange(response);
                }
            }

            if (logs.Count == 0)
            {
                return "No logs found for the given query.";
            }

            string result = "Query Results:\n";
            foreach (var log in logs)
            {
                result += $"- {log.Message} (Level: {log.LogLevel}, Timestamp: {log.Timestamp:O})\n";
            }
            return result;
        }
    }

    public class LogEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;
        [JsonPropertyName("logLevel")]
        public string LogLevel { get; set; } = string.Empty;
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
    }
}
