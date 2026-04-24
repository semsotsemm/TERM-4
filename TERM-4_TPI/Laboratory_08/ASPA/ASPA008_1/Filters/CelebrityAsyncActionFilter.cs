using Microsoft.AspNetCore.Mvc.Filters;

namespace ASPA008_1.Filters
{
    public class WikiInfoCelebrity
    {
        private readonly HttpClient _client;
        private readonly string _wikiURI;

        public Dictionary<string, string> WikiReferens { get; set; } = new();

        private WikiInfoCelebrity(string fullname)
        {
            _client = new HttpClient();
            _wikiURI = string.Format(
                "https://en.wikipedia.org/w/api.php?action=opensearch&search=\"{0}\"&prop=info&format=json",
                fullname);
        }

        public static async Task<Dictionary<string, string>> GetRefereces(string fullname)
        {
            var info = new WikiInfoCelebrity(fullname);
            try
            {
                var message = await info._client.GetAsync(info._wikiURI);
                if (message.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var result = await message.Content
                        .ReadFromJsonAsync<List<System.Text.Json.JsonElement>>();

                    if (result != null && result.Count >= 4)
                    {
                        var ls1 = System.Text.Json.JsonSerializer
                            .Deserialize<List<string>>(result[1].GetRawText())
                            ?? new();
                        var ls3 = System.Text.Json.JsonSerializer
                            .Deserialize<List<string>>(result[3].GetRawText())
                            ?? new();

                        for (int i = 0; i < ls1.Count; i++)
                            info.WikiReferens[ls1[i]] = ls3[i];
                    }
                }
            }
            catch {
                /* Wikipedia недоступна — возвращаем пустой словарь */ 
            }

            return info.WikiReferens;
        }
    }

    public class CelebrityAsyncActionFilter : Attribute, IAsyncActionFilter
    {
        public static readonly string Wikipedia = "WIKI";
        public static readonly string Facebook = "FACE";

        private readonly string _infotype;

        public CelebrityAsyncActionFilter(string infotype = "")
        {
            _infotype = infotype.ToUpper();
        }

        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            var repo = context.HttpContext.RequestServices
                .GetService<DAL_Celebrity.DAL_Celebrity_MSSQL.IRepository>();

            int id = (int)(context.ActionArguments["id"] ?? -1);
            var celebrity = repo?.GetCelebrityById(id);

            if (_infotype.Contains(Wikipedia) && celebrity != null)
            {
                var refs = await WikiInfoCelebrity.GetRefereces(celebrity.FullName);
                context.HttpContext.Items[Wikipedia] = refs;
            }

            if (_infotype.Contains(Facebook) && celebrity != null)
            {
                context.HttpContext.Items[Facebook] = $"Info from Face: {celebrity.FullName}";
            }

            await next();
        }
    }
}