using BusinessLogic.Interfaces;
using BusinessLogic.Services;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using BusinessLogic;
using NewsManagementSystem.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.UseAuthentication(); // Enables authentication
app.UseAuthorization();  // Enables authorization

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
