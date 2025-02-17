# Semantic Kernel Hackathon Guide

## Overview
This repository provides a hands-on guide for exploring Microsoft's Semantic Kernel framework through progressive examples. Each step builds upon the previous one to help you understand the core concepts of AI-powered application development.

## Prerequisites
- .NET 9.0 SDK
- Azure OpenAI Service access with:
  - Deployment name
  - Endpoint URL
  - API key
- **For Visual Studio Code users:** Install the [C# extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp) to enable debugging with F5 or running the code.
- **For Visual Studio users:** You can Debug and Run as regular Console App.

## Getting Started
1. **Clone the Repository:**  
   Run the following command or clone from GitHub:  
   `git clone https://github.com/DimaBir/ImmuneHackathon.git`

2. **Configure Your Keys:**  
   - Open the file `appsettings.template`.
   - Fill in your personal Azure OpenAI credentials (Deployment name, Endpoint URL, API key, etc.).
   - Save your changes and **rename** the file to `appsettings.json`.

3. **Open the Solution:**  
   - **For Visual Studio Code:** Open the folder in VSCode. Ensure the C# extension is installed to run or debug the code with F5.
   - **For Visual Studio:** Open the solution as described in the current README.

4. **Run the Examples:**  
   Navigate to the desired step and run the examples to explore various aspects of the Semantic Kernel framework.

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
