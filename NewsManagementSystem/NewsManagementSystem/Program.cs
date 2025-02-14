using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using BusinessLogic;
using NewsManagementSystem.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add logging
builder.Services.AddLogging(config =>
{
    config.AddConsole(); 
    config.AddDebug();   
});

// Get connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("MyCnn");

// Register DbContext with DI
builder.Services.AddDbContext<NewsManagementDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddApplication(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseMiddleware<CustomExceptionHandlerMiddleware>();

app.UseRouting();

// Add logging middleware
app.Use(async (context, next) =>
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();

    // Log incoming request details (method, path, headers, etc.)
    logger.LogInformation($"Request Method: {context.Request.Method}");
    logger.LogInformation($"Request Path: {context.Request.Path}");
    logger.LogInformation($"Request Headers: {string.Join(", ", context.Request.Headers.Select(h => $"{h.Key}: {h.Value}"))}");

    // Log JWT token if present in the header
    var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
    if (!string.IsNullOrWhiteSpace(token))
    {
        logger.LogInformation($"JWT Token: {token}");
    }
    
    await next.Invoke();
});


app.UseAuthentication(); // Enables authentication
app.UseAuthorization();  // Enables authorization

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
