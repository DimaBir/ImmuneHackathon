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
        [Description("Retrieves 1000 log entries from Kusto using a script.")]
        public async Task<string> GetKustoLogsAsync()
        {
            // Always invoke the PowerShellLogCollector to get logs
            var collector = new PowerShellLogCollector();
            string logsJson = await collector.GetLogsAsync();

            // Write to console as requested
            if (!string.IsNullOrWhiteSpace(logsJson))
            {
                Console.WriteLine("Successfully retrieved Kusto logs.");
            }
            else
            {
                Console.WriteLine("No logs found.");
            }

            return logsJson;
        }
    }
}
