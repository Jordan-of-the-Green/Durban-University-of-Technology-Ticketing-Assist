using Helpful_Hackers._XBCAD7319._POE.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;

var builder = WebApplication.CreateBuilder(args);

// Configuration setup
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add Microsoft authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = MicrosoftAccountDefaults.AuthenticationScheme;
})
    .AddCookie()
    .AddMicrosoftAccount(options =>
    {
        options.ClientId = configuration["Authentication:Microsoft:ClientId"];
        options.ClientSecret = configuration["Authentication:Microsoft:ClientSecret"];
    });

// Configure DbContext
builder.Services.AddDbContext<TicketDbContext>(options =>
{
    options.UseSqlServer(configuration.GetConnectionString("DevConnection"));
});

// Add sessions
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Adjusted the session timeout for 30 minutes
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


// Configure hosted service
builder.Services.AddHostedService<TicketStatusCheckService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
// Authentication middleware
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
     name: "contact-support",
     pattern: "contact-support",
     defaults: new { controller = "AdditionalSupport", action = "ContactSupport" }
 );

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
