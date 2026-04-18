using DAL003;
using Microsoft.Extensions.FileProviders;

public partial class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDirectoryBrowser();

        Repository.JSONFileName = "Celebrities.json";
        builder.Services.AddSingleton<IRepository>(sp => Repository.Create("Celebrities"));

        var app = builder.Build();

        var repo = app.Services.GetRequiredService<IRepository>();

        if (!Directory.Exists(repo.BasePath))
        {
            Directory.CreateDirectory(repo.BasePath);
        }

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(repo.BasePath),
            RequestPath = "/Photo"
        });

        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(repo.BasePath),
            RequestPath = "/Celebrities/download",
            OnPrepareResponse = ctx =>
            {
                ctx.Context.Response.Headers.Append("Content-Disposition", "attachment");
            }
        });

        app.UseDirectoryBrowser(new DirectoryBrowserOptions
        {
            FileProvider = new PhysicalFileProvider(repo.BasePath),
            RequestPath = "/Celebrities/download"
        });

        app.MapGet("/Celebrities", (IRepository repository) => repository.getAllCelebrities());
        app.MapGet("/Celebrities/{id:int}", (int id, IRepository repository) => repository.getCelebrityById(id));
        app.MapGet("/Celebrities/BySurname/{surname}", (string surname, IRepository repository) => repository.getCelebritiesBySurname(surname));
        app.MapGet("/Celebrities/PhotoPathById/{id:int}", (int id, IRepository repository) => repository.getPhotoPathById(id));

        app.MapGet("/", () => "Hello World!");

        app.Run();
    }
}