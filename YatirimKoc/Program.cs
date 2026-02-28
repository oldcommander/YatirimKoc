using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using YatirimKoc.Application;
using YatirimKoc.Application.Abstractions.Persistence;
using YatirimKoc.Application.Interfaces;
using YatirimKoc.Application.Interfaces.Settings;
using YatirimKoc.Domain.Entities.Identity;
using YatirimKoc.Infrastructure.Persistence;
using YatirimKoc.Infrastructure.Persistence.Interceptors;
using YatirimKoc.Infrastructure.Persistence.Seed;
using YatirimKoc.Infrastructure.Repositories;
using YatirimKoc.Infrastructure.Services;
using YatirimKoc.Infrastructure.Services.Settings;
using YatirimKoc.Web.Middlewares;
using YatirimKoc.Application.Common.Models;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<AuditSaveChangesInterceptor>();

// --------------------
// DB CONTEXT
// --------------------
builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
{
    var connectionString =
        builder.Configuration.GetConnectionString("DefaultConnection");

    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
    );

    options.AddInterceptors(
        serviceProvider.GetRequiredService<AuditSaveChangesInterceptor>()
    );
});

builder.Services.AddScoped<IApplicationDbContext>(
    provider => provider.GetRequiredService<ApplicationDbContext>()
);

// --------------------
// IDENTITY
// --------------------
builder.Services
    .AddIdentity<User, Role>(options =>
    {
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 6;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));
builder.Services.AddTransient<IMailService, MailService>();

// --------------------
// MVC
// --------------------
builder.Services.AddMemoryCache();

builder.Services.AddApplication();

builder.Services.AddScoped<IFileUploadService, FileUploadService>();

builder.Services.AddScoped<ISiteSettingService, SiteSettingService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IPropertyTypeRepository, PropertyTypeRepository>();
builder.Services.AddScoped<ITransactionTypeRepository, TransactionTypeRepository>();




builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SuperAdminOnly",
        policy => policy.RequireRole(RoleConstants.SuperAdmin));

    options.AddPolicy("AdminOrSuperAdmin",
        policy => policy.RequireRole(
            RoleConstants.Admin,
            RoleConstants.SuperAdmin));
});


// URL’leri küçük harfe zorla
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = true;
});

var app = builder.Build();

// --------------------
// PIPELINE
// --------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseAuthentication(); // 🔥 ÇOK ÖNEMLİ
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Auth}/{action=Login}/{id?}"
);

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);


await IdentitySeeder.SeedAsync(app.Services);

// --- SEED DATA VE OTOMATİK MİGRATİON İŞLEMİ ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<YatirimKoc.Infrastructure.Persistence.ApplicationDbContext>();

        // 1. EKSİK TABLOLARI VE MİGRATİONLARI OTOMATİK OLUŞTUR (Hatanın çözümü)
        await context.Database.MigrateAsync();

        // 2. KİMLİK (KULLANICI VE ROL) TOHUMLAMASI
        // IdentitySeeder büyük ihtimalle doğrudan IServiceProvider bekliyor.
        await YatirimKoc.Infrastructure.Persistence.Seed.IdentitySeeder.SeedAsync(services);

        // 3. İLAN ÖZELLİKLERİ TOHUMLAMASI
        await YatirimKoc.Infrastructure.Persistence.Seed.ListingSeeder.SeedAsync(context);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Veritabanı oluşturma veya Seed işlemi sırasında hata oluştu: " + ex.Message);
    }
}
// --------------------------------------------

app.Run();
