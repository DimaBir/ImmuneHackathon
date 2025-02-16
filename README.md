# ImmuneGettingStartedSemanticKernel# Semantic Kernel Hackathon Guide

## Overview
This repository provides a hands-on guide for exploring Microsoft's Semantic Kernel framework through progressive examples. Each step builds upon the previous one to help you understand core concepts of AI-powered application development.

## Prerequisites
- .NET 9.0 SDK
- Azure OpenAI Service access with:
  - Deployment name
  - Endpoint URL
  - API key
- Visual Studio Code or Visual Studio 2022

## Getting Started
1. Clone this repository
2. Configure Azure OpenAI credentials in `appsettings.json`
3. Open solution in Visual Studio Code
4. Run examples from each step

## Learning Path

### Step 1: Basic Kernel Setup
[`Step1_Create_Kernel.cs`](Steps/Step1_Create_Kernel.cs)
- Initialize Semantic Kernel with Azure OpenAI
- Send basic prompts
- Work with templated prompts
- Stream AI responses
- Configure execution settings
- Get structured JSON responses

### Step 2: Plugin Development
[`Step2_Add_Plugins.cs`](Steps/Step2_Add_Plugins.cs)
- Create custom semantic plugins
- Add native code functions
- Enable automatic function calling
- Handle structured data with enums
- Combine AI with C# code

### Step 3: Chat Interactions
[`Step3_Chat_Prompt.cs`](Steps/Step3_Chat_Prompt.cs)
- Build chat-based prompts
- Handle system and user messages
- Format chat responses
- Manage conversation context

### Step 4: AI Agents
[`Step4_Agent.cs`](Steps/Step4_Agent.cs)
- Create specialized AI agents
- Build interactive chat experiences
- Maintain conversation history
- Process streaming responses
- Work with different chat roles

## Example Usage

Basic kernel initialization:
```csharp
var kernel = Kernel.CreateBuilder()
    .AddAzureOpenAIChatCompletion(
        deploymentName: deploymentName,
        endpoint: endpoint,
        apiKey: apiKey)
    .Build();