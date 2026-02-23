using eDoctor.Data;
using eDoctor.Interfaces;
using eDoctor.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IScheduleService, ScheduleService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();


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

app.Run();
