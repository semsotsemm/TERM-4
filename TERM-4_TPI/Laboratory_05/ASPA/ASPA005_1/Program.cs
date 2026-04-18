using DAL004;
using Microsoft.AspNetCore.Diagnostics;

public partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        string dataPath = AppContext.BaseDirectory;
        JsonRepository.json_filename = "Celebrities.json";

        var app = builder.Build();

        app.UseExceptionHandler("/Celebrities/Error");

        app.MapGet("/Celebrities", () =>
        {
            using var repository = new JsonRepository(dataPath);
            return Results.Ok(repository.getAllCelebrities());
        });

        app.MapGet("/Celebrities/{id:int}", (int id) =>
        {
            using var repository = new JsonRepository(dataPath);
            var celebrity = repository.getCelebryById(id);
            if (celebrity == null)
                throw new FoundByIdException($"Celebrity Id = {id}");
            return Results.Ok(celebrity);
        });


        app.MapPost("/Celebrities", (Celebrity celebrity) =>
        {
            using var repository = new JsonRepository(dataPath);
            int? id = repository.addCelebrity(celebrity);
            if (id == null)
                throw new AddCelebrityException("Celebrities error, id == null");

            return Results.Created($"/Celebrities/{id}", celebrity);
        })
 .AddEndpointFilter(async (context, next) =>
 {
     var celebrity = context.GetArgument<Celebrity>(0);

     if (celebrity == null)
         throw new AddCelebrityException("POST /Celebrities: celebrity is null");

     if (string.IsNullOrEmpty(celebrity.surname) || celebrity.surname.Length < 2)
         return Results.Json(new { error = "Surname must be at least 2 characters" }, statusCode: 409);

     return await next(context);
 })
 .AddEndpointFilter(async (context, next) =>
 {
     var celebrity = context.GetArgument<Celebrity>(0);

     if (celebrity == null)
         throw new AddCelebrityException("POST /Celebrities: celebrity is null");

     using var repository = new JsonRepository(dataPath);
     var existing = repository.getCelebritiesBySurname(celebrity.surname);

     if (existing != null && existing.Length > 0)
         return Results.Json(new { error = $"Surname {celebrity.surname} already exists" }, statusCode: 409);

     return await next(context);
 })
 .AddEndpointFilter(async (context, next) =>
 {
     var celebrity = context.GetArgument<Celebrity>(0);

     if (celebrity == null)
         throw new AddCelebrityException("POST /Celebrities: celebrity is null");

     string fileName = Path.GetFileName(celebrity.photo_path);
     string fullPath = Path.Combine(dataPath, fileName);

     if (!File.Exists(fullPath))
     {
         context.HttpContext.Response.Headers.Append("X-Celebrity", $"NotFound = {fileName}");
         return Results.BadRequest(new { error = $"Photo file {fileName} not found" });
     }

     return await next(context);
 });

        app.MapDelete("/Celebrities/{id:int}", (int id) =>
        {
            using var repository = new JsonRepository(dataPath);
            if (!repository.delCelebityById(id))
                throw new DeleteCelebrityException($"Delete error: Id = {id} not found");

            repository.saveChange();
            return Results.Ok(new { message = $"Celebrity {id} deleted" });
        });

        app.MapPut("/Celebrities/{id:int}", (int id, Celebrity celebrity) =>
        {
            using var repository = new JsonRepository(dataPath);

            celebrity.celebrity_id = id;

            int result = repository.updCelebityById(id, celebrity);

            if (result == 0)
                throw new UpdateCelebrityException($"Update error: Id = {id} not found");

            return Results.Ok(celebrity);
        });

        app.MapFallback((HttpContext ctx) =>
            Results.NotFound(new { error = $"Path {ctx.Request.Path} not supported" }));

        app.Map("/Celebrities/Error", (HttpContext ctx) =>
        {
            var feature = ctx.Features.Get<IExceptionHandlerFeature>();
            var ex = feature?.Error;
            var env = app.Environment.EnvironmentName; 

            if (ex is AddCelebrityException)
            {
                return Results.Problem(
                    title: "ASPA004", 
                    detail: string.IsNullOrEmpty(ex.Message) ? "Panic" : ex.Message,
                    instance: env,
                    statusCode: 500
                );
            }

            if (ex is FoundByIdException || ex is DeleteCelebrityException)
                return Results.NotFound(ex.Message);

            if (ex is UpdateCelebrityException)
                return Results.BadRequest(ex.Message);

            if (ex is SaveException)
                return Results.Problem(title: "ASPA804/saveChange", detail: ex.Message, instance: env, statusCode: 500);

            return Results.Problem(
                detail: ex?.Message ?? "Panic",
                title: "ASPA804",
                instance: env,
                statusCode: 500
            );
        });

        app.Run();
    }

    public class FoundByIdException : Exception { public FoundByIdException(string m) : base(m) { } }
    public class SaveException : Exception { public SaveException(string m) : base(m) { } }
    public class AddCelebrityException : Exception { public AddCelebrityException(string m) : base(m) { } }
    public class DeleteCelebrityException : Exception { public DeleteCelebrityException(string m) : base(m) { } }
    public class UpdateCelebrityException : Exception { public UpdateCelebrityException(string m) : base(m) { } }
}