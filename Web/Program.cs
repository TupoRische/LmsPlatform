using Core.Contracts;
using Core.Services;
using Infrastructure.Data;
using Infrastructure.Data.Seeding;
using Infrastructure.Identity;
using Infrastructure.Repositories.Contracts;
using Infrastructure.Repositories.Implementations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
<<<<<<< HEAD
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.CommandTimeout(60);
        sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
    }));
=======
    options.UseSqlServer(connectionString));
>>>>>>> b1645c236beb100f9b792702ab7ac3ba0a399b56

builder.Services
    .AddDefaultIdentity<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddScoped<ISchoolService, SchoolService>();
builder.Services.AddScoped<IProfessionService, ProfessionService>();
builder.Services.AddScoped<IMaterialService, MaterialService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped<IAdminDashboardService, AdminDashboardService>();
builder.Services.AddScoped<IAdminTeacherRequestsService, AdminTeacherRequestsService>();
builder.Services.AddScoped<ITeacherDashboardService, TeacherDashboardService>();
builder.Services.AddScoped<ITeacherMaterialsService, TeacherMaterialsService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IHomeService, HomeService>();
<<<<<<< HEAD
builder.Services.AddScoped<ICommentService, CommentService>();
=======
>>>>>>> b1645c236beb100f9b792702ab7ac3ba0a399b56


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
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    await DbInitializer.SeedAsync(scope.ServiceProvider);
}

<<<<<<< HEAD
app.Run();
=======
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    // Create Admin role if not exists
    if (!roleManager.RoleExistsAsync("Admin").Result)
    {
        roleManager.CreateAsync(new IdentityRole("Admin")).Wait();
    }

    var admins = userManager.GetUsersInRoleAsync("Admin").Result;

    if (!admins.Any())
    {
        var admin = new ApplicationUser
        {
            UserName = "admin@lms.com",
            Email = "admin@lms.com",
            EmailConfirmed = true
        };

        var result = userManager.CreateAsync(admin, "Admin123!").Result;

        if (result.Succeeded)
        {
            userManager.AddToRoleAsync(admin, "Admin").Wait();
        }
    }
}


app.Run();
>>>>>>> b1645c236beb100f9b792702ab7ac3ba0a399b56
