var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.ConfigureStarshineWebApp();
builder.Services.AddControllers();
builder.Services.AddUserDbContextService();
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseAuthorization();

app.MapControllers();

app.Run();
