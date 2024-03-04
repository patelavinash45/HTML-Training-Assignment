using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using Repositories.Implementation;
using Repositories.Interface;
using Repositories.Interfaces;
using Services.Implementation;
using Services.Implementation.AdminServices;
using Services.Implementation.AuthServices;
using Services.Implementation.PatientServices;
using Services.Interfaces;
using Services.Interfaces.AdminServices;
using Services.Interfaces.AuthServices;
using Services.Interfaces.PatientServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSession(
    options => {
        options.Cookie.Name = ".MySession";
        options.IdleTimeout = TimeSpan.FromDays(1);
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
    }
);
builder.Services.AddControllersWithViews();
builder.Services.AddNotyf(config => { config.DurationInSeconds = 10; config.IsDismissable = true; config.Position = NotyfPosition.TopRight; });
builder.Services.AddDbContext<Repositories.DataContext.HalloDocDbContext>();
builder.Services.AddScoped<ILoginService, LoginService>();
builder.Services.AddScoped<IAspNetUserRepository, AspNetUserRepository>();
builder.Services.AddScoped<IAspNetRoleRepository, AspNetRoleRepository>();
builder.Services.AddScoped<IAspNetUserRoleRepository, AspNetUserRoleRepository>();
builder.Services.AddScoped<IPatientDashboardService, PatientDashboardService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRequestRepository, RequestRepository>();
builder.Services.AddScoped<IRequestClientRepository, RequestClientRepository>();
builder.Services.AddScoped<IRequestWiseFileRepository, RequestWiseFileRepository>();
builder.Services.AddScoped<IRegionRepository, RegionRepository>();
builder.Services.AddScoped<IAddRequestService, AddRequestService>();
builder.Services.AddScoped<IViewProfileService, ViewProfileService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IAdminDashboardService, AdminDashboardService>();
builder.Services.AddScoped<IResetPasswordService, ResetPasswordService>();
builder.Services.AddScoped<IViewDocumentsServices, ViewDocumentsServices>();
builder.Services.AddScoped<IViewCaseService, ViewCaseService>();
builder.Services.AddScoped<IViewNotesService, ViewNotesService>();
builder.Services.AddScoped<IRequestNotesRepository, RequestNotesRepository>();
builder.Services.AddScoped<IRequestStatusLogRepository, RequestStatusLogRepository>();
builder.Services.AddScoped<IConciergeRepository, ConciergeRepository>();
builder.Services.AddScoped<IRequestConciergeRepository, RequestConciergeRepository>();
builder.Services.AddScoped<ICaseTagRepository, CaseTagRepository>();
builder.Services.AddScoped<IBusinessRepository, BusinessRepository>();
builder.Services.AddScoped<IRequestBusinessRepository, RequestBusinessRepository>();
builder.Services.AddScoped<IPhysicianRepository, PhysicianRepository>();
builder.Services.AddScoped<IBlockRequestsRepository, BlockRequestsRepository>();
builder.Services.AddScoped<IAdminRepository, AdminRepository>();


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
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Patient}/{action=PatientSite}/{id?}");

app.Run();
