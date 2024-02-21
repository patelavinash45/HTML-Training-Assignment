using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using HelloDoc.DataContext;
using Microsoft.AspNetCore.Hosting;
using Repositories.Implementation;
using Repositories.Interface;
using Repositories.Interfaces;
using Services.Implementation;
using Services.Implementation.Admin;
using Services.Interface;
using Services.Interfaces;
using Services.Interfaces.Admin;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddNotyf(config => { config.DurationInSeconds = 10; config.IsDismissable = true; config.Position = NotyfPosition.TopRight; });
builder.Services.AddDbContext<HelloDoc.DataContext.ApplicationDbContext>();
builder.Services.AddDbContext<Repositories.DataContext.HalloDocDbContext>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IAspNetUserRepository, AspNetUserRepository>();
builder.Services.AddScoped<IAspNetRoleRepository, AspNetRoleRepository>();
builder.Services.AddScoped<IAspNetUserRoleRepository, AspNetUserRoleRepository>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRequestRepository, RequestRepository>();
builder.Services.AddScoped<IRequestClientRepository, RequestClientRepository>();
builder.Services.AddScoped<IRequestWiseFileRepository, RequestWiseFileRepository>();
builder.Services.AddScoped<IRegionRepository, RegionRepository>();
builder.Services.AddScoped<IAddRequestService, AddRequestService>();
builder.Services.AddScoped<IViewProfileService, ViewProfileService>();
builder.Services.AddScoped<IAdminDashboardService, AdminDashboardService>();


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

app.UseNotyf();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Admin}/{action=Dashboard}/{id?}");

app.Run();
