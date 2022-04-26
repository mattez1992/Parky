global using ParkyWeb.Models;
global using ParkyWeb.Repository;
global using ParkyWeb.Models.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
// auth config
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opt =>
    {
        opt.Cookie.HttpOnly = true;
        opt.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        opt.LoginPath = "/Home/Login";
        opt.AccessDeniedPath = "/Home/AccessDenied";
        opt.SlidingExpiration = true;
    });
builder.Services.AddSession(opt =>
{
    // Set a short timeout for easy testing.
    opt.IdleTimeout = TimeSpan.FromMinutes(10);
    opt.Cookie.HttpOnly = true;
    // Make the session cookie essential
    opt.Cookie.IsEssential = true;
});


// repos
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<INationalParkRepo, NationalParkRepo>();
builder.Services.AddScoped<ITrailRepo, TrailRepo>();
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
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

app.UseRouting();
app.UseCors(x => x
 .AllowAnyOrigin()
 .AllowAnyMethod()
 .AllowAnyHeader());

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
