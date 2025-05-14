using Logger.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.AddConfiguration();
builder.AddServices();

var app = builder.Build();

var swaggerEnabled = builder.Configuration["swagger:enabled"]!.Equals("true", StringComparison.CurrentCultureIgnoreCase);

if (swaggerEnabled)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapLoggerEndpoints();
app.UseHttpsRedirection();

app.Run();
