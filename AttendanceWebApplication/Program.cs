
using AttendanceWebApplication.Data;
using Microsoft.EntityFrameworkCore;

try
{
    var builder = WebApplication.CreateBuilder(args);

    // ... TODO: Register services
    //services.AddScoped<IAttendanceRepository, AttendanceRepository>();
    //services.AddScoped<IFileComparisonService, FileComparisonService>();

    builder.Services.AddControllersWithViews();

    builder.Services.AddDbContext<ApplicationDbContext>(opt =>
   opt.UseSqlServer(builder.Configuration.GetConnectionString("MSQLConnection")));

    var app = builder.Build();

    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
    }

    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Compare}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine("?? Application failed to start:");
    Console.WriteLine(ex);
    throw;
}
