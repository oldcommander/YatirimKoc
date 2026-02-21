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

// --- SEED DATA (Örnek Veri Yükleme) İŞLEMİ ---
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<YatirimKoc.Infrastructure.Persistence.ApplicationDbContext>();
        // Eğer identity seeder'ınız da varsa onu da burada çağırabilirsiniz.
        await YatirimKoc.Infrastructure.Persistence.Seed.ListingSeeder.SeedAsync(context);
    }
    catch (Exception ex)
    {
        // Gerekirse loglama yapılabilir
        Console.WriteLine("Seed işlemi sırasında hata oluştu: " + ex.Message);
    }
}
// --------------------------------------------

app.Run();

app.Run();
