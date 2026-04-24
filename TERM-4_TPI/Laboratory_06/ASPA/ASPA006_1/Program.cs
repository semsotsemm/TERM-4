using Microsoft.Extensions.Options;
using static DAL_Celebrity.DAL_Celebrity_MSSQL;

namespace DAL_Celebrity;

public partial class Program
{
    private static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);


        builder.Configuration.AddJsonFile("Celebrities.config.json", optional: false, reloadOnChange: true);
        builder.Services.Configure<CelebritiesConfig>(
            builder.Configuration.GetSection("Celebrities")
        );
        builder.Services.AddScoped<IRepository, Repository>((IServiceProvider p) =>
        {
            var config = p.GetRequiredService<IOptions<CelebritiesConfig>>().Value;

            return new Repository(p.GetRequiredService<IOptions<CelebritiesConfig>>());
        });

        var app = builder.Build();

        app.UseDefaultFiles();
        app.UseStaticFiles();

        app.UseMiddleware<DAL_Celebrity.ExceptionMiddleware>();

        var celebrities = app.MapGroup("/api/Celebrities");
        celebrities.MapGet("/data", (IRepository repo) => Results.Ok(repo.GetAllCelebrities()));
        celebrities.MapGet("/", async (HttpContext context) =>
        {
            context.Response.ContentType = "text/html; charset=utf-8";
            await context.Response.SendFileAsync("wwwroot/index.html");
        });

        celebrities.MapGet("/{id:int:min(1)}", (IRepository repo, int id) =>
            repo.GetCelebrityById(id) is Celebrity c ? Results.Ok(c) : Results.NotFound());

        celebrities.MapGet("/Lifeevents/{id:int:min(1)}", (IRepository repo, int id) =>
            Results.Ok(repo.GetLifeeventsByCelebrityId(id)));

        celebrities.MapDelete("/{id:int:min(1)}", (IRepository repo, int id) =>
            repo.DelCelebrity(id) ? Results.NoContent() : Results.NotFound());

        celebrities.MapPost("/", (IRepository repo, Celebrity celebrity) =>
        {
            if (repo.AddCelebrity(celebrity))
                return Results.Created($"/api/Celebrities/{celebrity.Id}", celebrity);
            return Results.BadRequest();
        });

        celebrities.MapPut("/{id:int:min(1)}", (IRepository repo, int id, Celebrity celebrity) =>
            repo.UpdCelebrity(id, celebrity) ? Results.NoContent() : Results.NotFound());

        celebrities.MapGet("/photo/{fname}", async (IOptions<CelebritiesConfig> iconfig, string fname) =>
        {
            var path = Path.Combine(iconfig.Value.PhotoFolder, fname);
            if (!File.Exists(path)) return Results.NotFound();

            return Results.File(path, "image/jpeg");
        });

        var lifeevents = app.MapGroup("/api/Liveevents");

        lifeevents.MapGet("/", (IRepository repo) => repo.GetAllLiveevents());

        lifeevents.MapGet("/{id:int:min(1)}", (IRepository repo, int id) =>
            repo.GetLiveeventsById(id) is Liveevent e ? Results.Ok(e) : Results.NotFound());

        lifeevents.MapGet("/Celebrities/{id:int:min(1)}", (IRepository repo, int id) =>
            Results.Ok(repo.GetLifeeventsByCelebrityId(id)));

        lifeevents.MapDelete("/{id:int:min(1)}", (IRepository repo, int id) =>
            repo.DelLiveevent(id) ? Results.NoContent() : Results.NotFound());

        lifeevents.MapPost("/", (IRepository repo, Liveevent liveevent) =>
        {
            if (repo.AddLiveevent(liveevent))
                return Results.Created($"/api/Liveevents/{liveevent.Id}", liveevent);
            return Results.BadRequest();
        });

        lifeevents.MapPut("/{id:int:min(1)}", (IRepository repo, int id, Liveevent liveevent) =>
            repo.UpdLiveevent(id, liveevent) ? Results.NoContent() : Results.NotFound());

        app.Run();
    }
}