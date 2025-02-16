// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.SemanticKernel;
using Utils;
using System.IO;

namespace GettingStarted
{
    public class Step6_Kusto_Agent
    {
        [KernelFunction("get_1000_kusto_logs")]
        [Description("Retrieves 1000 log entries from Kusto using a cache to preserve results.")]
        public async Task<string> GetKustoLogsAsync()
        {
            // Define the cache file path (using the system temporary folder)
            string cacheFile = Path.Combine(Path.GetTempPath(), "kusto_logs_cache.json");
            string logsJson = string.Empty;

            // Check if the cache file exists and contains data
            if (File.Exists(cacheFile) && new FileInfo(cacheFile).Length > 0)
            {
                logsJson = await File.ReadAllTextAsync(cacheFile);
                Console.WriteLine("Loaded logs from cache.");
            }
            else
            {
                // Invoke the PowerShellLogCollector (which uses default parameters)
                var collector = new PowerShellLogCollector();
                logsJson = await collector.GetLogsAsync();

                // Write to console as requested and cache the result if logs were retrieved
                if (!string.IsNullOrWhiteSpace(logsJson))
                {
                    Console.WriteLine("Successfully retrieved Kusto logs.");
                    await File.WriteAllTextAsync(cacheFile, logsJson);
                }
                else
                {
                    Console.WriteLine("No logs found.");
                }
            }

            return logsJson;
        }
    }
}
