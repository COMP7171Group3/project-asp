using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using _7071Group.Data;

var builder = WebApplication.CreateBuilder(args);

// Local helper to get connection string or throw
string GetConnection(string name) =>
    builder.Configuration.GetConnectionString(name) ??
    throw new InvalidOperationException($"Connection string '{name}' not found.");

// Register DbContexts with reduced repetition
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlite(GetConnection("AuthDb")));
builder.Services.AddDbContext<CareDbContext>(options =>
    options.UseSqlite(GetConnection("CareDb")));
builder.Services.AddDbContext<HrDbContext>(options =>
    options.UseSqlite(GetConnection("HrDb")));
builder.Services.AddDbContext<HousingDbContext>(options =>
    options.UseSqlite(GetConnection("HousingDb")));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<AuthDbContext>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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
app.MapRazorPages();

app.Run();