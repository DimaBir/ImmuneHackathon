// Copyright (c) Microsoft. All rights reserved.

using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using SemanticKernelExamples;

namespace GettingStarted;

/// <summary>
/// This example shows how to load <see cref="KernelPlugin"/> instances.
/// </summary>
public sealed class Step2_Add_Plugins
{
    /// <summary>
    /// Shows different ways to load and invoke plugins.
    /// </summary>
    public async Task AddPluginsAsync()
    {
        // Create a kernel with OpenAI chat completion using AppConfig
        string deploymentName = AppConfig.OpenAI.DeploymentName;
        string modelId = AppConfig.OpenAI.ModelId;
        string endpoint = AppConfig.OpenAI.Endpoint;
        string apiKey = AppConfig.OpenAI.ApiKey;
        
        // Create a kernel with OpenAI chat completion
        IKernelBuilder kernelBuilder = Kernel.CreateBuilder()
                        .AddAzureOpenAIChatCompletion(
                            deploymentName: deploymentName,
                            endpoint: endpoint,
                            apiKey: apiKey,
                            serviceId: null,
                            modelId: modelId);

        kernelBuilder.Plugins.AddFromType<TimeInformation>();
        kernelBuilder.Plugins.AddFromType<WidgetFactory>();
        Kernel kernel = kernelBuilder.Build();

        // Example 1. Invoke the kernel with a prompt that asks the AI for information it cannot provide directly
        Console.WriteLine(await kernel.InvokePromptAsync("How many days until Christmas?"));

        // Example 2. Invoke the kernel with a templated prompt that calls a plugin function
        Console.WriteLine(await kernel.InvokePromptAsync("The current time is {{TimeInformation.GetCurrentUtcTime}}. How many days until Christmas?"));

        // Example 3. Invoke the kernel with a prompt that allows automatic function invocation
        var settings = new OpenAIPromptExecutionSettings { FunctionChoiceBehavior = FunctionChoiceBehavior.Auto() };
        Console.WriteLine(await kernel.InvokePromptAsync("How many days until Christmas? Explain your thinking.", new(settings)));

        // Example 4. Invoke the kernel with a prompt that calls a plugin function using enumerations
        Console.WriteLine(await kernel.InvokePromptAsync("Create a handy lime colored widget for me.", new(settings)));
        Console.WriteLine(await kernel.InvokePromptAsync("Create a beautiful scarlet colored widget for me.", new(settings)));
        Console.WriteLine(await kernel.InvokePromptAsync("Create an attractive maroon and navy colored widget for me.", new(settings)));
    }

    /// <summary>
    /// A plugin that returns the current time.
    /// </summary>
    public class TimeInformation
    {
        [KernelFunction]
        [System.ComponentModel.Description("Retrieves the current time in UTC.")]
        public string GetCurrentUtcTime() => DateTime.UtcNow.ToString("R");
    }

    /// <summary>
    /// A plugin that creates widgets.
    /// </summary>
    public class WidgetFactory
    {
        [KernelFunction]
        [System.ComponentModel.Description("Creates a new widget of the specified type and colors")]
        public WidgetDetails CreateWidget(
            [System.ComponentModel.Description("The type of widget to be created")] WidgetType widgetType,
            [System.ComponentModel.Description("The colors of the widget to be created")] WidgetColor[] widgetColors)
        {
            var colors = string.Join('-', widgetColors.Select(c => (WidgetType)c).ToArray());
            return new WidgetDetails
            {
                SerialNumber = $"{widgetType}-{colors}-{Guid.NewGuid()}",
                Type = widgetType,
                Colors = widgetColors
            };
        }
    }

    [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
    public enum WidgetType
    {
        [System.ComponentModel.Description("A widget that is useful.")]
        Useful,

        [System.ComponentModel.Description("A widget that is decorative.")]
        Decorative
    }

    [System.Text.Json.Serialization.JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
    public enum WidgetColor
    {
        [System.ComponentModel.Description("Use when creating a red item.")]
        Red,

        [System.ComponentModel.Description("Use when creating a green item.")]
        Green,

        [System.ComponentModel.Description("Use when creating a blue item.")]
        Blue
    }

    public class WidgetDetails
    {
        public required string SerialNumber { get; init; }
        public WidgetType Type { get; init; }
        public required WidgetColor[] Colors { get; init; }
    }
}
