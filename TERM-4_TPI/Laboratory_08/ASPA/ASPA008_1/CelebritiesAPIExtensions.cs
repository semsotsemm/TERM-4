using DAL_Celebrity;
using Microsoft.Extensions.Options;

namespace ANC25_WEBAPI_DLL
{
    public class CelebritiesConfig
    {
        public string PhotosRequestPath { get; set; } = "/Photos";
        public string PhotoFolder { get; set; } = string.Empty;
        public string ConnectionString { get; set; } = string.Empty;
        public string ISO3166alpha2Path { get; set; } = string.Empty;
    }

    public class CelerbrityTitles
    {
        public string Head { get; set; } = "Celebrities Dictionary Internet Service";
        public string Title { get; set; } = "Celebrities";
        public string Copyright { get; set; } = "© Copyright BSTU Development";
    }

    public class CountryItem
    {
        public string code { get; set; } = string.Empty;
        public string countryLabel { get; set; } = string.Empty;
    }

    public class CountryCodes
    {
        private readonly List<CountryItem> _items;

        public CountryCodes(IOptions<CelebritiesConfig> options)
        {
            var path = options.Value.ISO3166alpha2Path;
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                _items = System.Text.Json.JsonSerializer.Deserialize<List<CountryItem>>(json)
                         ?? new List<CountryItem>();
            }
            else
            {
                _items = new List<CountryItem>
                {
                    new() { code = "US", countryLabel = "United States" },
                    new() { code = "UK", countryLabel = "United Kingdom" },
                    new() { code = "NL", countryLabel = "Netherlands" },
                    new() { code = "RU", countryLabel = "Russia" },
                    new() { code = "JP", countryLabel = "Japan" },
                    new() { code = "DE", countryLabel = "Germany" },
                    new() { code = "FR", countryLabel = "France" },
                };
            }
        }

        public IEnumerable<CountryItem> GetAll() => _items;

        public static implicit operator List<CountryItem>(CountryCodes cc) => cc._items;
    }

    public static class CelebritiesAPIExtensions
    {
        public static void AddCelebritiesConfiguration(this WebApplicationBuilder builder)
        {
            builder.Configuration.AddJsonFile(
                "Celebrities.config.json", optional: false, reloadOnChange: true);

            builder.Services.Configure<CelebritiesConfig>(
                builder.Configuration.GetSection("Celebrities"));
            
            builder.Services.Configure<DAL_Celebrity.CelebritiesConfig>(
                builder.Configuration.GetSection("Celebrities"));

            builder.Services.PostConfigure<CelebritiesConfig>(cfg =>
            {
                if (!Path.IsPathRooted(cfg.PhotoFolder))
                    cfg.PhotoFolder = Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, cfg.PhotoFolder));
                if (!Path.IsPathRooted(cfg.ISO3166alpha2Path))
                    cfg.ISO3166alpha2Path = Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, cfg.ISO3166alpha2Path));
            });
            builder.Services.PostConfigure<DAL_Celebrity.CelebritiesConfig>(cfg =>
            {
                if (!Path.IsPathRooted(cfg.PhotoFolder))
                    cfg.PhotoFolder = Path.GetFullPath(Path.Combine(builder.Environment.ContentRootPath, cfg.PhotoFolder));
            });
        }

        public static void AddCelebritiesServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<DAL_Celebrity.DAL_Celebrity_MSSQL.IRepository,
                                       DAL_Celebrity.DAL_Celebrity_MSSQL.Repository>();

            builder.Services.AddSingleton<CelerbrityTitles>();
            builder.Services.AddSingleton<CountryCodes>();
        }

        public static void MapCelebrities(this WebApplication app)
        {
            var g = app.MapGroup("/api/Celebrities");

            g.MapGet("/data", (DAL_Celebrity.DAL_Celebrity_MSSQL.IRepository repo)
                => Results.Ok(repo.GetAllCelebrities()));

            g.MapGet("/{id:int:min(1)}", (DAL_Celebrity.DAL_Celebrity_MSSQL.IRepository repo, int id)
                => repo.GetCelebrityById(id) is { } c ? Results.Ok(c) : Results.NotFound());

            g.MapDelete("/{id:int:min(1)}", (DAL_Celebrity.DAL_Celebrity_MSSQL.IRepository repo, int id)
                => repo.DelCelebrity(id) ? Results.NoContent() : Results.NotFound());

            g.MapPost("/", (DAL_Celebrity.DAL_Celebrity_MSSQL.IRepository repo,
                            DAL_Celebrity.DAL_Celebrity_MSSQL.Celebrity celebrity) =>
            {
                if (repo.AddCelebrity(celebrity))
                    return Results.Created($"/api/Celebrities/{celebrity.Id}", celebrity);
                return Results.BadRequest();
            });

            g.MapPut("/{id:int:min(1)}", (DAL_Celebrity.DAL_Celebrity_MSSQL.IRepository repo,
                                          int id,
                                          DAL_Celebrity.DAL_Celebrity_MSSQL.Celebrity celebrity)
                => repo.UpdCelebrity(id, celebrity) ? Results.NoContent() : Results.NotFound());
        }

        public static void MapLifeevents(this WebApplication app)
        {
            var g = app.MapGroup("/api/Liveevents");

            g.MapGet("/", (DAL_Celebrity.DAL_Celebrity_MSSQL.IRepository repo)
                => Results.Ok(repo.GetAllLiveevents()));

            g.MapGet("/Celebrities/{id:int:min(1)}", (DAL_Celebrity.DAL_Celebrity_MSSQL.IRepository repo, int id)
                => Results.Ok(repo.GetLifeeventsByCelebrityId(id)));
        }

        public static void MapPhotoCelebrities(this WebApplication app)
        {
            app.MapGet("/Photos/{fname}", async (IOptions<CelebritiesConfig> cfg, string fname) =>
            {
                var path = Path.Combine(cfg.Value.PhotoFolder, fname);
                if (!File.Exists(path)) return Results.NotFound();
                var mime = Path.GetExtension(fname).ToLower() == ".png" ? "image/png" : "image/jpeg";
                return Results.File(path, mime);
            });
        }

        public static void UseCelebritiesErrorHandler(this WebApplication app, string appName)
        {
            app.Use(async (ctx, next) =>
            {
                try { await next(); }
                catch (Exception ex)
                {
                    ctx.Response.ContentType = "text/plain; charset=utf-8";
                    ctx.Response.StatusCode = 500;
                    await ctx.Response.WriteAsync(
                        $"[{appName}] Ошибка: {ex.Message}");
                }
            });
        }
    }
}