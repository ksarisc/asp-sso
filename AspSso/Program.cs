using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http;

var builder = WebApplication.CreateBuilder(args);

var conf = builder.Configuration;
var authConf = AspSso.Models.Config.AuthConfig.Parse(conf);

builder.Services.AddHttpClient();

//builder.Services.Configure<AspSso.Models.Config.AuthConfig>(c => { return authConf; });
builder.Services.AddSingleton(authConf);
builder.Services.AddSingleton<AspSso.Services.SigninManager>();

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
    })
    .AddCookie()
    .AddGoogle(g =>
    {
        g.ClientId = authConf.Google.ClientId;
        g.ClientSecret = authConf.Google.ClientSecret;
        g.SaveTokens = true;
    });
//.AddMicrosoftAccount(m => { ... })
//.AddGoogle(g => { ... })
//.AddTwitter(t => { ... })
//.AddFacebook(f => { ... });

// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
