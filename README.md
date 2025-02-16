# Semantic Kernel Hackathon Guide

## Overview
This repository provides a hands-on guide for exploring Microsoft's Semantic Kernel framework through progressive examples. Each step builds upon the previous one to help you understand the core concepts of AI-powered application development.

## Prerequisites
- .NET 9.0 SDK
- Azure OpenAI Service access with:
  - Deployment name
  - Endpoint URL
  - API key
- Visual Studio Code or Visual Studio 2022

## Getting Started
1. Clone this repository.
2. Configure Azure OpenAI credentials in `appsettings.json`.
3. Open the solution in Visual Studio Code.
4. Run examples from each step.

## Learning Path

### Step 1: Basic Kernel Setup
[`Step1_Create_Kernel.cs`](Steps/Step1_Create_Kernel.cs)
- Initialize Semantic Kernel with Azure OpenAI.
- Send basic prompts.
- Work with templated prompts.
- Stream AI responses.
- Configure execution settings.
- Get structured JSON responses.

### Step 2: Plugin Development
[`Step2_Add_Plugins.cs`](Steps/Step2_Add_Plugins.cs)
- Create custom semantic plugins.
- Add native code functions.
- Enable automatic function calling.
- Handle structured data with enums.
- Combine AI with C# code.

### Step 3: Chat Interactions
[`Step3_Chat_Prompt.cs`](Steps/Step3_Chat_Prompt.cs)
- Build chat-based prompts.
- Handle system and user messages.
- Format chat responses.
- Manage conversation context.

### Step 4: AI Agents
[`Step4_Agent.cs`](Steps/Step4_Agent.cs)
- Create specialized AI agents.
- Build interactive chat experiences.
- Maintain conversation history.
- Process streaming responses.
- Work with different chat roles.

### Step 5: Cosmos DB Agent
[`Step5_Cosmos_Agent.cs`](Steps/Step5_Cosmos_Agent.cs)
- Connect to Cosmos DB for log storage.
- Retrieve and aggregate log entries.
- Summarize log data by level.
- Filter logs based on severity.
- Execute and process database queries.

### Step 6: Kusto Agent
[`Step6_Kusto_Agent.cs`](Steps/Step6_Kusto_Agent.cs)
- Integrate with Kusto using PowerShell.
- Retrieve logs from the Kusto service.
- Cache logs for optimized performance.
- Provide a menu-driven CLI for log retrieval.
- Enable real-time log viewing and updates.
