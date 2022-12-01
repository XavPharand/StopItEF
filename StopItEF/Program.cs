using Microsoft.EntityFrameworkCore;
using StopItEF.Models;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.ConfigureKestrel(serverOptions => {
    serverOptions.Listen(System.Net.IPAddress.Any, 5083);
    });

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<cloakinglebg_webserverContext>(
    options => options.UseMySql(
        builder.Configuration.GetConnectionString("Data"),
        Microsoft.EntityFrameworkCore.ServerVersion.Parse("10.6.7-mariadb")
        ).UseLazyLoadingProxies()
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=SignIn}");

app.Run();
