using EFCoreDemo.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext with SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Minimal API endpoint to test the database
app.MapGet("/products", async (AppDbContext context) =>
    await context.Products.ToListAsync());

app.Run();