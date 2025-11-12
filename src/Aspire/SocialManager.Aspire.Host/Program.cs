var builder = DistributedApplication.CreateBuilder(args);

// Add Azure Storage with Azurite emulator for local development
var storage = builder.AddAzureStorage("storage")
    .RunAsEmulator();

// Add individual storage services
var blobs = storage.AddBlobs("blobs");
var queues = storage.AddQueues("queues");
var tables = storage.AddTables("tables");

// Add Azure Cosmos DB with emulator for local development
var cosmosDb = builder.AddAzureCosmosDB("cosmosdb")
    .RunAsEmulator()
    .AddCosmosDatabase("socialmanager");

builder.AddProject<Projects.SocialManager>("SocialManager")
    .WithReference(blobs)
    .WithReference(queues)
    .WithReference(tables)
    .WithReference(cosmosDb);

builder.Build().Run();
