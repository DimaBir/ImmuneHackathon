// Copyright (c) Microsoft. All rights reserved.

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using GettingStarted;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.SemanticKernel.ChatCompletion;
using SemanticKernelExamples;

namespace SemanticKernelSample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Select a step to run:");
            Console.WriteLine("1: Run Step1_Create_Kernel example (using AppConfiguration)");
            Console.WriteLine("2: Run Step2_Add_Plugins example (using AppConfiguration)");
            Console.WriteLine("3: Run Step3_Chat_Prompt example (using AppConfiguration)");
            Console.WriteLine("4: Run Step4_Chat_Agent example (using AppConfiguration)");
            Console.WriteLine("5: Run Step5_Cosmos_Agent example (using AppConfiguration)");
            Console.WriteLine("6: Run Step6_Kusto_Agent example (using AppConfiguration)");
            Console.WriteLine("q: Quit");
            while (true)
            {
                Console.Write("Enter selection: ");
                var selection = Console.ReadLine()?.Trim();

                if (selection == "1")
                {
                    var step1 = new Step1_Create_Kernel();
                    await step1.CreateKernelAsync();
                }
                else if (selection == "2")
                {
                    var step2 = new Step2_Add_Plugins();
                    await step2.AddPluginsAsync();
                }
                else if (selection == "3")
                {
                    var step3 = new Step3_Chat_Prompt();
                    await step3.InvokeChatPromptAsync();
                }
                else if (selection == "4")
                {
                    var step4 = new Step4_Chat_Agent();
                    await step4.InvokeYodaTranslatorAsync();
                }
                else if (selection == "5")
                {
                    string deploymentName = AppConfig.OpenAI.DeploymentName;
                    string modelId = AppConfig.OpenAI.ModelId;
                    string endpoint = AppConfig.OpenAI.Endpoint;
                    string apiKey = AppConfig.OpenAI.ApiKey;

                    var builder = Kernel.CreateBuilder()
                        .AddAzureOpenAIChatCompletion(
                            deploymentName: deploymentName,
                            endpoint: endpoint,
                            apiKey: apiKey,
                            serviceId: null,
                            modelId: modelId);

                    builder.Services.AddLogging(logging =>
                    {
                        logging.AddConsole();
                        logging.SetMinimumLevel(LogLevel.Warning);
                    });

                    Kernel kernel = builder.Build();

                    kernel.Plugins.AddFromType<Step5_Cosmos_Agent>("CosmosDB");

                    var history = new ChatHistory();
                    history.AddSystemMessage("You are an intelligent assistant that can control smart lights and analyze Cosmos DB logs. Use the tools 'SmartLighting' and 'CosmosDB' as needed.");

                    Console.WriteLine("Enter your message (type 'exit' to quit):");
                    string? userInput;
                    do
                    {
                        Console.Write("User > ");
                        userInput = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(userInput) || userInput.Equals("exit", StringComparison.OrdinalIgnoreCase))
                            break;

                        history.AddUserMessage(userInput);

                        var chatCompletionService = kernel.Services.GetRequiredService<IChatCompletionService>();

                        var result = await chatCompletionService.GetChatMessageContentAsync(history, new OpenAIPromptExecutionSettings
                        {
                            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
                        }, kernel);

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Assistant > " + result);
                        Console.ResetColor();

                        history.AddMessage(result.Role, result.Content ?? string.Empty);

                    } while (true);

                    Console.WriteLine("Exiting agent. Press any key to close...");
                    Console.ReadKey();

                }
                else if (selection == "6")
                {
                    string deploymentName = AppConfig.OpenAI.DeploymentName;
                    string modelId = AppConfig.OpenAI.ModelId;
                    string endpoint = AppConfig.OpenAI.Endpoint;
                    string apiKey = AppConfig.OpenAI.ApiKey;

                    var builder = Kernel.CreateBuilder()
                        .AddAzureOpenAIChatCompletion(
                            deploymentName: deploymentName,
                            endpoint: endpoint,
                            apiKey: apiKey,
                            serviceId: null,
                            modelId: modelId);

                    builder.Services.AddLogging(logging =>
                    {
                        logging.AddConsole();
                        logging.SetMinimumLevel(LogLevel.Warning);
                    });

                    Kernel kernel = builder.Build();

                    // Add the Step6_Kusto_Agent functions as a plugin named "Kusto"
                    kernel.Plugins.AddFromType<Step6_Kusto_Agent>("Kusto");

                    var history = new ChatHistory();
                    history.AddSystemMessage("You are an intelligent assistant that can analyze Kusto logs. Use the tool 'Kusto' as needed. If chat history already contains logs, reuse them. If you need perform any custom actionss work on this JSON.");

                    Console.WriteLine("Enter your message (type 'exit' to quit):");
                    string? userInput;
                    do
                    {
                        Console.Write("User > ");
                        userInput = Console.ReadLine();
                        if (string.IsNullOrWhiteSpace(userInput) || userInput.Equals("exit", StringComparison.OrdinalIgnoreCase))
                            break;

                        history.AddUserMessage(userInput);

                        var chatCompletionService = kernel.Services.GetRequiredService<IChatCompletionService>();

                        var result = await chatCompletionService.GetChatMessageContentAsync(history, new OpenAIPromptExecutionSettings
                        {
                            FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
                        }, kernel);

                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Assistant > " + result);
                        Console.ResetColor();

                        history.AddMessage(result.Role, result.Content ?? string.Empty);

                    } while (true);

                    Console.WriteLine("Exiting agent. Press any key to close...");
                    Console.ReadKey();
                }
                else if (selection == "q")
                {
                    Console.WriteLine("Exiting...");
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid selection. Exiting.");
                    break;
                }
            }
        }
    }
}
