
using Identity.KeyGen.Service.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddServices();

var app = builder.Build();

app.UseHttpsRedirection();


app.Run();

