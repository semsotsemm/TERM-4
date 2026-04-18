public partial class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        // Middleware 
        app.UseWelcomePage("/aspnetcore");
        app.MapGet("/aspnetcore", () => "Бесполезный код");

        // Статические Middleware как запасной вариант
        app.UseDefaultFiles();
        app.UseStaticFiles();

        //app.MapGet("/", () => "Hello World!");
        app.Run();
    }
}