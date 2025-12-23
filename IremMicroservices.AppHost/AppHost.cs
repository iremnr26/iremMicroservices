var builder = DistributedApplication.CreateBuilder(args);

// Add API projects
var locationsApi = builder.AddProject<Projects.Locations_API>("locations-api");
var moviesApi = builder.AddProject<Projects.Movies_API>("movies-api");
var usersApi = builder.AddProject<Projects.Users_API>("users-api");

builder.Build().Run();