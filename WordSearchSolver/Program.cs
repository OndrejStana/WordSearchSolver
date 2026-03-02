using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WordSearchSolver;
using WordSearchSolver.Resolver;
using WordSearchSolver.Serializer;

// Setup Dependency Injection
var serviceCollection = new ServiceCollection();
serviceCollection.AddLogging(configure => configure.AddConsole());
serviceCollection.AddSingleton<IJsonReaderService, JsonReaderService>();
serviceCollection.AddSingleton<IJsonValidatorService, JsonValidatorService>();
serviceCollection.AddSingleton<IResolverService, ResolverService>();
serviceCollection.AddTransient<IWordSearchApplication, WordSearchApplication>();
var serviceProvider = serviceCollection.BuildServiceProvider();

// Resolve and run the application
var app = serviceProvider.GetRequiredService<IWordSearchApplication>();
await app.RunAsync(args);