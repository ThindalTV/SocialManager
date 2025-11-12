var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.SocialManager>("SocialManager");

builder.Build().Run();
