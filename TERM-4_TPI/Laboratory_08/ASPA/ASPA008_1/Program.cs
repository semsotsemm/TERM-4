using ANC25_WEBAPI_DLL;
using Microsoft.Extensions.Options;


namespace DAL_Celebrity;

public partial class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.AddCelebritiesConfiguration();  
        builder.AddCelebritiesServices();         

        builder.Services.AddControllersWithViews();

        var app = builder.Build();

        {
            var cfg = app.Services.GetRequiredService<IOptions<CelebritiesConfig>>().Value;
            new Init(cfg.ConnectionString);
            Init.Execute(delete: false, create: true);
        }

        app.UseHttpsRedirection();

        if (!app.Environment.IsDevelopment())
            app.UseExceptionHandler("/Home/Error");

        app.UseStaticFiles();
        app.UseRouting();

        app.UseCelebritiesErrorHandler("ExceptionMiddleware");
        app.MapCelebrities();                     
        app.MapLifeevents();                      
        app.MapPhotoCelebrities();             

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "celebrity-new",
            pattern: "/0",
            defaults: new { Controller = "Celebrities", Action = "NewHumanForm" });

        app.MapControllerRoute(
            name: "celebrity-one",
            pattern: "/{id:int:min(1)}",
            defaults: new { Controller = "Celebrities", Action = "Human" });

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Celebrities}/{action=Index}/{id?}");

        app.Run();
    }
}