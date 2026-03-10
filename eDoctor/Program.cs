using eDoctor.Data;
using eDoctor.Hubs;
using eDoctor.Interfaces;
using eDoctor.Jobs;
using eDoctor.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaypalServerSdk.Standard;
using PaypalServerSdk.Standard.Authentication;
using Quartz;
using QuestPDF.Infrastructure;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// License
QuestPDF.Settings.License = LicenseType.Community;

// Auth
builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie();

builder.Services.Configure<CookieAuthenticationOptions>(
    CookieAuthenticationDefaults.AuthenticationScheme,
    options =>
    {
        options.Events.OnRedirectToLogin = context =>
        {
            if (context.Request.Path.StartsWithSegments("/Doctor"))
            {
                context.Response.Redirect("/Doctor/Account/Login");
            }
            else
            {
                context.Response.Redirect("/Account/Login");
            }

            return Task.CompletedTask;
        };

        options.AccessDeniedPath = "/Error/Error";
    });

builder.Services.AddAuthorization();

// MVC
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});

// SignalR
builder.Services.AddSignalR();

// Database
string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (connectionString == null)
{
    throw new Exception("Connection string not found");
}

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

// Services
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IPdfService, PdfService>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IScheduleService, ScheduleService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IMeetingService, MeetingService>();
builder.Services.AddScoped<IMedicalRecordService, MedicalRecordService>();
builder.Services.AddScoped<INotificationService, NotificationService>();

// PayPal
string? paypalClientId = builder.Configuration["PayPal:OAuthClientId"];
string? paypalClientSecret = builder.Configuration["PayPal:OAuthClientSecret"];

if (paypalClientId == null || paypalClientSecret == null)
{
    throw new Exception("PayPal setting not found");
}

builder.Services.AddSingleton(service =>
    new PaypalServerSdkClient.Builder()
        .ClientCredentialsAuth(
            new ClientCredentialsAuthModel.Builder(
                paypalClientId,
                paypalClientSecret
            )
            .Build())
        .Environment(PaypalServerSdk.Standard.Environment.Sandbox)
        .Build()
);

// Quartz
builder.Services.AddQuartz(q =>
{
    JobKey jobKey = new JobKey("ScheduleStatusJob");
    q.AddJob<ScheduleStatusJob>(opts => opts.WithIdentity(jobKey));

    q.AddTrigger(opts => opts
        .ForJob(jobKey)
        .WithIdentity("ScheduleStatusJob-Trigger")
        .WithCronSchedule("0 */5 * * * ?"));
});

builder.Services.AddQuartzHostedService(options =>
{
    options.WaitForJobsToComplete = true;
});


WebApplication app = builder.Build();

// Production
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapHub<MeetingHub>("/MeetingHub");
app.MapHub<NotificationHub>("/NotificationHub");

app.Run();
