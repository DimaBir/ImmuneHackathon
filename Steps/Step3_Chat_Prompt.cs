// Copyright (c) Microsoft. All rights reserved.

using Microsoft.SemanticKernel;
using SemanticKernelExamples;

namespace GettingStarted;

public sealed class Step3_Chat_Prompt
{
    /// <summary>
    /// Show how to construct a chat prompt and invoke it.
    /// </summary>
    public async Task InvokeChatPromptAsync()
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

        // Invoke the kernel with a chat prompt and display the result
        string chatPrompt = """
            <message role="user">What is Seattle?</message>
            <message role="system">Respond with JSON.</message>
            """;

        Console.WriteLine(await kernel.InvokePromptAsync(chatPrompt));
    }
}