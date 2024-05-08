using BlazorWebAppAuthentication.Client.Components;
using BlazorWebAppAuthentication.Client.Configurations;
using BlazorWebAppAuthentication.Client.FraudPrevention;
using BlazorWebAppAuthentication.Client.Payment;
using BlazorWebAppAuthentication.Client.Services;
using BlazorWebAppAuthentication.Client.Services.Interfaces;
using BlazorWebAppAuthentication.Database;
using BlazorWebAppAuthentication.Database.Interfaces;
using IgniteUI.Blazor.Controls;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Config = BlazorWebAppAuthentication.Client.Configurations.Config;
using ICustomersSanctionStatusService = BlazorWebAppAuthentication.Client.Services.Interfaces.ICustomersSanctionStatusService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "auth_token";
        options.LoginPath = "/login";
        // options.Cookie.MaxAge = TimeSpan.FromMinutes(3000);
        options.AccessDeniedPath = "/access-denied";
    });

builder.Configuration.Bind("Project", new Config());
builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseMySQL(Config.ConnectionString));

builder.Services.AddAntiforgery(options =>
{
    options.Cookie.Expiration = TimeSpan.Zero;
});

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ICountryRepository, CountryRepository>();
builder.Services.AddScoped<IUserAccountRepository, UserAccountRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
builder.Services.AddScoped<UserAccountRepository, UserAccountRepository>();
builder.Services.AddScoped<IFradulentNamesRepository, FraudulentNamesRepository>();
builder.Services.AddScoped<ICustomersSanctionStatusRepository, CustomersSanctionStatusRepository>();


builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<IFraudulentNamesService, FraudulentNamesService>();
builder.Services.AddScoped<ICustomersSanctionStatusService, CustomersSanctionStatusService>();
builder.Services.AddScoped<PaymentService>();
builder.Services.AddScoped<FraudPreventionService>();


builder.Services.AddIgniteUIBlazor();

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("wwwroot/MT103Config.json", optional: false, reloadOnChange: true)
    .Build();
builder.Services.Configure<MT103Settings>(configuration.GetSection("MT103Settings"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorComponents<App>()
    .DisableAntiforgery()
    .AddInteractiveServerRenderMode();

//DbInitializer.Seed(app);

app.Run();