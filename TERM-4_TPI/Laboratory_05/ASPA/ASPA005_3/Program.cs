using DAL004;
using Microsoft.AspNetCore.Diagnostics;
using Validation;

public partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        string dataPath = AppContext.BaseDirectory;
        JsonRepository.json_filename = "Celebrities.json";

        var app = builder.Build();

        var repository = new JsonRepository(dataPath);
        Validation.SurnameFilter.repository = Validation.PhotoExistFilter.repository = repository;
        Validation.PhotoExistFilter.dataPath = dataPath;

        app.UseExceptionHandler("/Error");

        app.MapGet("/A/{x:int:max(100)}", (int x) => Results.Ok(new { path = "/A/x", x }));
        app.MapPost("/A/{x:int:range(0,100)}", (int x) => Results.Ok(new { path = "/A/x", x }));
        app.MapPut("/A/{x:int:min(1)}/{y:int:min(1)}", (int x, int y) => Results.Ok(new { path = "/A/x/y", x, y }));
        app.MapDelete("/A/{x:int:min(1)}-{y:int:range(1,100)}", (int x, int y) => Results.Ok(new { path = "/A/x-y", x, y }));

        app.MapGet("/B/{x:float}", (float x) => Results.Ok(new { path = "/B/x", x }));
        app.MapPost("/B/{x:float}/{y:float}", (float x, float y) => Results.Ok(new { path = "/B/x/y", x, y }));
        app.MapDelete("/B/{x:float}-{y:float}", (float x, float y) => Results.Ok(new { path = "/B/x-y", x, y }));

        app.MapGet("/C/{x:bool}", (bool x) => Results.Ok(new { path = "/C/x", x }));
        app.MapPost("/C/{x:bool},{y:bool}", (bool x, bool y) => Results.Ok(new { path = "/C/x,y", x, y }));

        app.MapGet("/D/{x:datetime}", (DateTime x) => Results.Ok(new { path = "/D/x", x }));
        app.MapPost("/D/{x:datetime}|{y:datetime}", (DateTime x, DateTime y) => Results.Ok(new { path = "/D/x|y", x, y }));

        app.MapGet("/E/12-{x:required}", (string x) => Results.Ok(new { path = "/E/12-x", x }));
        app.MapPut("/E/{x:alpha:length(2,12)}", (string x) => Results.Ok(new { path = "/E/x", x }));

        app.MapPut("/F/{x:regex(^[[a-zA-Z0-9._%+-]]+@[[a-zA-Z0-9.-]]+\\.by$)}", (string x) => Results.Ok(new { path = "/F/x", x }));

        var api = app.MapGroup("/Celebrities");
        api.MapGet("/", () => Results.Ok(repository.getAllCelebrities()));
        api.MapPost("/", (Celebrity celebrity) => {
            int? id = repository.addCelebrity(celebrity);
            return Results.Created($"/Celebrities/{id}", celebrity);
        }).AddEndpointFilter<SurnameFilter>().AddEndpointFilter<PhotoExistFilter>();

        app.MapFallback((HttpContext ctx) =>
            Results.NotFound(new { message = $"path {ctx.Request.Path} not supported" }));

        app.Map("/Error", (HttpContext ctx) =>
        {
            var feature = ctx.Features.Get<IExceptionHandlerFeature>();
            return Results.Ok(new { message = feature?.Error.Message });
        });

        app.Run();
    }

    // Исключения (из предыдущего задания)
    public class FoundByIdException : Exception { public FoundByIdException(string m) : base(m) { } }
    public class AddCelebrityException : Exception { public AddCelebrityException(string m) : base(m) { } }
}