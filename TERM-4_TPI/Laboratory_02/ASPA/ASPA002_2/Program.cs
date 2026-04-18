using Microsoft.Extensions.FileProviders;

public partial class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var app = builder.Build();

        // Middleware 
        app.UseWelcomePage("/aspnetcore");
        app.MapGet("/aspnetcore", () => "Бесполезный код");

        DefaultFilesOptions options = new DefaultFilesOptions();
        options.DefaultFileNames.Clear();
        options.DefaultFileNames.Add("Neumann.html");

        var picturePath = Path.Combine(builder.Environment.ContentRootPath, "Picture");
        StaticFileOptions static_option = new StaticFileOptions();
        static_option.FileProvider = new PhysicalFileProvider(picturePath);
        static_option.RequestPath = "/static";

        // Статические Middleware как запасной вариант
        app.UseDefaultFiles(options);
        app.UseStaticFiles();


        app.UseStaticFiles(static_option);

        //app.MapGet("/", () => "Hello World!");
        app.Run();
    }
}