using Identity.KeyGen.Service;
using Identity.KeyGen.Service.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServices();

var app = builder.Build();



Console.WriteLine("DEBUG: Starting KeyUpdateExecutor");
using (var scope = app.Services.CreateScope())
{
    var executor = scope.ServiceProvider.GetRequiredService<KeyUpdateExecutor>();

    try
    {
        // Execute the key update logic
        await executor.ExecuteAsync();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error during execution: {ex.Message}");
    }
}

// Exit the program after execution
Environment.Exit(0);

