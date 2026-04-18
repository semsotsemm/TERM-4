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

        app.UseExceptionHandler("/Celebrities/Error");

        RouteGroupBuilder api = app.MapGroup("/Celebrities");

        api.MapGet("/", () => Results.Ok(repository.getAllCelebrities()));

        api.MapGet("/{id:int}", (int id) =>
        {
            var celebrity = repository.getCelebryById(id);
            if (celebrity == null) throw new FoundByIdException($"Celebrity Id = {id}");
            return Results.Ok(celebrity);
        });

        api.MapPost("/", (Celebrity celebrity) =>
        {
            int? id = repository.addCelebrity(celebrity);
            if (id == null) throw new AddCelebrityException("Celebrities error, id == null");
            return Results.Created($"/Celebrities/{id}", celebrity);
        })
        .AddEndpointFilter<SurnameFilter>()    
        .AddEndpointFilter<PhotoExistFilter>(); 

        api.MapPut("/{id:int}", (int id, Celebrity celebrity) =>
        {
            celebrity.celebrity_id = id;
            if (repository.updCelebityById(id, celebrity) == 0)
                throw new UpdateCelebrityException($"Update error: Id = {id} not found");
            return Results.Ok(celebrity);
        }).AddEndpointFilter<IdValidationFilter>();

        api.MapDelete("/{id:int}", (int id) =>
        {
            if (!repository.delCelebityById(id))
                throw new DeleteCelebrityException($"Delete error: Id = {id} not found");
            repository.saveChange();
            return Results.Ok(new { message = $"Celebrity {id} deleted" });
        }).AddEndpointFilter<IdValidationFilter>();

        app.Map("/Celebrities/Error", (HttpContext ctx) =>
        {
            var feature = ctx.Features.Get<IExceptionHandlerFeature>();
            var ex = feature?.Error;
            var env = app.Environment.EnvironmentName;

            if (ex is AddCelebrityException)
                return Results.Problem(title: "ASPA004", detail: ex.Message, instance: env, statusCode: 500);

            if (ex is FoundByIdException || ex is DeleteCelebrityException)
                return Results.NotFound(ex.Message);

            if (ex is UpdateCelebrityException)
                return Results.BadRequest(ex.Message);

            return Results.Problem(detail: ex?.Message ?? "Panic", title: "ASPA804", instance: env, statusCode: 500);
        });

        app.Run();
    }

    public class FoundByIdException : Exception { public FoundByIdException(string m) : base(m) { } }
    public class AddCelebrityException : Exception { public AddCelebrityException(string m) : base(m) { } }
    public class DeleteCelebrityException : Exception { public DeleteCelebrityException(string m) : base(m) { } }
    public class UpdateCelebrityException : Exception { public UpdateCelebrityException(string m) : base(m) { } }
}