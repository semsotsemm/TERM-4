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

            repository.saveChange();
            return Results.Created($"/Celebrities/{id}", celebrity);
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
            int result = repository.updCelebityById(id, celebrity);

            if (result == 0)
                throw new UpdateCelebrityException($"Update error: Id = {id} not found or no changes");

            return Results.Ok(celebrity);
        });

        app.MapFallback((HttpContext ctx) =>
            Results.NotFound(new { error = $"Path {ctx.Request.Path} not supported" }));

        app.Map("/Celebrities/Error", (HttpContext ctx) =>
        {
            var ex = ctx.Features.Get<IExceptionHandlerFeature>()?.Error;
            var env = app.Environment.EnvironmentName;

            if (ex is FoundByIdException)
                return Results.NotFound(ex.Message);

            if (ex is DeleteCelebrityException)
                return Results.NotFound(ex.Message);

            if (ex is UpdateCelebrityException)
                return Results.BadRequest(ex.Message);

            if (ex is SaveException)
                return Results.Problem(title: "ASPA804/saveChange", detail: ex.Message, statusCode: 500);

            if (ex is AddCelebrityException)
                return Results.Problem(title: "ASPA004/addCelebrity", detail: ex.Message, statusCode: 500);

            return Results.Problem(detail: ex?.Message ?? "Panic", title: "ASPA804", statusCode: 500);
        });

        app.Run();
    }

    public class FoundByIdException : Exception { public FoundByIdException(string m) : base(m) { } }
    public class SaveException : Exception { public SaveException(string m) : base(m) { } }
    public class AddCelebrityException : Exception { public AddCelebrityException(string m) : base(m) { } }
    public class DeleteCelebrityException : Exception { public DeleteCelebrityException(string m) : base(m) { } }
    public class UpdateCelebrityException : Exception { public UpdateCelebrityException(string m) : base(m) { } }
}