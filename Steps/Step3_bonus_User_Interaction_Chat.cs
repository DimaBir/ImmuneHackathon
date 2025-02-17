// Copyright (c) Microsoft. All rights reserved.

using Microsoft.SemanticKernel;
using SemanticKernelExamples;

namespace GettingStarted;

public sealed class Step3_User_Interaction_Chat_Prompt
{
    /// <summary>
    /// Show how to construct a chat prompt with user interaction and maintain chat history.
    /// </summary>
    public async Task InvokeUserInteractionChatPromptAsync()
    {
        const string EXIT_WORD = "EXIT";
        string deploymentName = AppConfig.OpenAI.DeploymentName;
        string modelId = AppConfig.OpenAI.ModelId;
        string endpoint = AppConfig.OpenAI.Endpoint;
        string apiKey = AppConfig.OpenAI.ApiKey;

        // Create a kernel with OpenAI chat completion
        Kernel kernel = Kernel.CreateBuilder()
                        .AddAzureOpenAIChatCompletion(
                            deploymentName: deploymentName,
                            endpoint: endpoint,
                            apiKey: apiKey,
                            serviceId: null,
                            modelId: modelId)
                        .Build();

        List<string> chatHistory = new List<string>();
        string userInput;

        Console.WriteLine("Type your messages below (type 'EXIT' to quit):");

        while (true)
        {
            Console.Write("You: ");
            userInput = Console.ReadLine()?.Trim() ?? string.Empty;

            if (userInput?.ToUpper() == EXIT_WORD)
            {
                break;
            }

            chatHistory.Add($"<message role=\"user\">{userInput}</message>");

            // Construct the chat prompt with history
            // Example 1: Basic chat prompt
            // string chatPrompt = $"{userInput}\nRespond with JSON.";

            // Example 2: Chat prompt with system instructions
            // string chatPrompt = "You are a helpful assistant. " +
            //                     $"{userInput}\nRespond with JSON.";

            // Example 3: Chat prompt with history and system instructions
            // string chatPrompt = string.Join("\n", chatHistory) +
            //                     $"\n{userInput}\nRespond with JSON.";

            // Example 4: Chat prompt with different types of response
            // string chatPrompt = string.Join("\n", chatHistory) +
            //                     $"\n{userInput}\nRespond in a concise and informative manner.";

            // Example 5: Chat prompt with different types of response and system instructions
            string chatPrompt = "You are a knowledgeable tour guide. " +
                                string.Join("\n", chatHistory) +
                                $"\n{userInput}\nProvide interesting facts and recommendations.";

            Console.Write("Bot: ");
            string aggregatedResponse = string.Empty;
            await foreach (var update in kernel.InvokePromptStreamingAsync(chatPrompt))
            {
                Console.Write(update);
                aggregatedResponse += update;
            }
            Console.WriteLine();

            chatHistory.Add($"<message role=\"assistant\">{aggregatedResponse}</message>");
        }
    }
}
