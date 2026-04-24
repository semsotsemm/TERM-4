using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASPA008_1.Helpers
{
    public static class CelebrityHelpers
    {
        public static HtmlString CelebrityPhoto(
            this IHtmlHelper html,
            int id,
            string title,
            string src)
        {
            string onclick = $"location.href='/{id}'";

            string onload =
                "let h=this.height;let w=0;" +
                "let k=this.naturalWidth/this.naturalHeight;" +
                "if(h!=0 && w==0){this.height=h;this.width=k*h;}" +
                "if(h==0 && w!=0){this.height=w/k;this.width=k*h;}";

            string result =
                $"<img id=\"{id}\" " +
                $"class=\"celebrity-photo\" " +
                $"title=\"{title}\" " +
                $"src=\"{src}\" " +
                $"onclick=\"{onclick}\" " +
                $"onload=\"{onload}\" " +
                $"/>";

            return new HtmlString(result);
        }
    }
}