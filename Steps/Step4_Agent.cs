// Copyright (c) Microsoft. All rights reserved.

using System;
using System.Threading.Tasks;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Agents;
using Microsoft.SemanticKernel.ChatCompletion;
using SemanticKernelExamples;

namespace GettingStarted
{
    public class Step4_Chat_Agent
    {
        public async Task InvokeYodaTranslatorAsync()
        {
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

            // Define the ChatCompletionAgent with instructions to translate to Yoda dialect.
            ChatCompletionAgent agent = new ChatCompletionAgent
            {
                Name = "YodaTranslator",
                Instructions = "Translate the user's message to Yoda dialect.",
                Kernel = kernel
            };

            while (true)
            {
                Console.Write("Enter a message (or press Enter to exit): ");
                string input = Console.ReadLine() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(input))
                {
                    break;
                }

                // Create the chat history for the current input.
                ChatHistory chat = new ChatHistory();

                // Add the user's message.
                ChatMessageContent userMessage = new ChatMessageContent(AuthorRole.User, input);
                chat.Add(userMessage);
                Console.WriteLine($"User: {input}");

                // Invoke the agent and output its response.
                await foreach (ChatMessageContent response in agent.InvokeAsync(chat))
                {
                    chat.Add(response);
                    Console.WriteLine($"{response.Role}: {response.Content}");
                }
            }
        }
    }
}
