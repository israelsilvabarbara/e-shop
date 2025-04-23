var builder = WebApplication.CreateBuilder(args);

builder.AddServices();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapLoggerEndpoints();

app.Run();
