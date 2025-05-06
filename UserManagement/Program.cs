using UserManagement.Models;
using UserManagement.Repositories;

var builder = WebApplication.CreateBuilder(args); // Create a new instance of the WebApplicationBuilder class, which is used to configure the application and its services.


// Add services to the container.
builder.Services.AddControllersWithViews(); // is used to add MVC services to the container

builder.Services.AddScoped<IItemsRepo, ItemsRepo>(); // Register the IItemsRepo interface and its implementation ItemsRepo with a scoped lifetime, meaning a new instance will be created for each request.
builder.Services.AddScoped<IEmail, Email>(); // Register the IEmail interface and its implementation Email with a scoped lifetime.
builder.Services.Configure<ManagerSettings>(builder.Configuration.GetSection("ManagerSettings")); // Bind the ManagerSettings section of the configuration to the ManagerSettings class, allowing access to the settings in the application.
builder.Services.AddSingleton<Manager>();

var app = builder.Build(); // Build the application pipeline and create an instance of the WebApplication class, which represents the application itself.

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();
