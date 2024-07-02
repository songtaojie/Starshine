using Microsoft.EntityFrameworkCore;
using Starshine.TestProject;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureStarshineWebApp();
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddStarshineEfCore()
    .AddStarshineDbContextPool<TestDbContext>(options =>
    {
        options.Provider = Starshine.EntityFrameworkCore.EfCoreDatabaseProvider.MySql;
        options.Configure(s =>
        {
            s.UseMySql(ServerVersion.AutoDetect(options.ConnectionString));
        });
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
