using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = DistributedApplication.CreateBuilder(args);

// Add Azure Storage with Azurite emulator for local development
var storage = builder.AddAzureStorage("Storage")
    .RunAsEmulator();

// Add individual storage services
var blobs = storage.AddBlobs("blobs");
var queues = storage.AddQueues("queues");
var tables = storage.AddTables("tables");

// Add Azure Cosmos DB with emulator for local development
var cosmosDb = builder.AddAzureCosmosDB("cosmosdb")
    .RunAsEmulator();

var cosmosDatabase = cosmosDb.AddCosmosDatabase("SocialManagerStorage");

var apiProject = builder.AddProject<Projects.SocialManager_API>("SocialManagerApi")
    .WithReference(blobs)
    .WithReference(queues)
    .WithReference(tables)
    .WithReference(cosmosDatabase);

builder.AddProject<Projects.SocialManager>("SocialManager")
    .WithReference(apiProject)
    .WaitFor(apiProject);

builder.Build().Run();
