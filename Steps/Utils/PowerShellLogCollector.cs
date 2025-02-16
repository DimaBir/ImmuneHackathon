using System.Diagnostics;
using System.Reflection;
using SemanticKernelExamples;

namespace Utils
{
    public class PowerShellLogCollector
    {
        public async Task<string> GetLogsAsync()
        {
            // Compute the path relative to the assembly location.
            string assemblyFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;
            string psScriptPath = Path.Combine(assemblyFolder, @"Steps\Utils\GetKustoLogs.ps1");

            // Pass the Kusto values read from appsettings.json as parameters.
            string arguments = $"-NoProfile -ExecutionPolicy Bypass -File \"{psScriptPath}\" " +
                                $"-KustoUri \"{AppConfig.Kusto.Uri}\" -BearerToken \"{AppConfig.Kusto.BearerToken}\"";

            var startInfo = new ProcessStartInfo
            {
                FileName = "powershell.exe",
                Arguments = arguments,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            };

            using var process = new Process { StartInfo = startInfo };
            process.Start();

            string output = await process.StandardOutput.ReadToEndAsync();
            string error = await process.StandardError.ReadToEndAsync();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                throw new Exception($"PowerShell script error: {error}");
            }

            return output;
        }
    }
}