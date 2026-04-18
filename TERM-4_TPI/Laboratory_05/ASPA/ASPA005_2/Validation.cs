using DAL004;

namespace Validation
{
    public class SurnameFilter : IEndpointFilter
    {
        public static JsonRepository repository;
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var celebrity = context.GetArgument<Celebrity>(0);
            if (celebrity == null) throw new Exception("Celebrity is null");

            if (string.IsNullOrEmpty(celebrity.surname) || celebrity.surname.Length < 2)
                return Results.Json(new { error = "Surname must be at least 2 characters" }, statusCode: 409);

            var existing = repository.getCelebritiesBySurname(celebrity.surname);
            if (existing != null && existing.Length > 0)
                return Results.Json(new { error = $"Surname {celebrity.surname} already exists" }, statusCode: 409);

            return await next(context);
        }
    }

    public class PhotoExistFilter : IEndpointFilter
    {
        public static JsonRepository repository;
        public static string dataPath; 

        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var celebrity = context.GetArgument<Celebrity>(0);
            string fileName = Path.GetFileName(celebrity.photo_path);
            string fullPath = Path.Combine(dataPath, fileName);

            if (!File.Exists(fullPath))
            {
                context.HttpContext.Response.Headers.Append("X-Celebrity", $"NotFound = {fileName}");
                return Results.BadRequest(new { error = $"Photo file {fileName} not found" });
            }

            return await next(context);
        }
    }

    public class IdValidationFilter : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var id = context.GetArgument<int>(0);
            if (id <= 0) return Results.BadRequest("Invalid ID");
            return await next(context);
        }
    }
}