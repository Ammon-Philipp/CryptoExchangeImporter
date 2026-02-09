using CryptoExchangeImporter.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Web
builder.Services.AddControllersWithViews();

// Infrastructure
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ExchangeDbContext>(options => options.UseSqlServer(
                                                        connectionString,
                                                        b => b.MigrationsAssembly(
                                                            "CryptoExchangeImporter.Infrastructure"
                                                        )
                                                    )
);
// Application

var app = builder.Build();

// TODO: Also for production?
await using (var scope = app.Services.CreateAsyncScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ExchangeDbContext>();
    await context.Database.MigrateAsync();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

await app.RunAsync();
