using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Youtube_download.Controllers
{
    [Route("UrlList")]
    [ApiController]
    public class UrlListController : ControllerBase
    {
        [HttpPost]
        async public Task<string> ListSaveTotxt()
        {
            using var reader = new StreamReader(Request.Body);
            var url = await reader.ReadToEndAsync();
            url = (url.Split("&")[0].Contains("list") ? url.Split("&")[0] : url.Split("&")[1]);

            Regex regex = new Regex("(?i)(list=.*)");
            var match = regex.Match(url);
            var newurl = $"https://www.youtube.com/playlist?{match.Value[0]}";
            //var headers = "{ 'user-agent': 'Mozilla/5.0 (Windows NT 6.1) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.116 Safari/537.36'}";
            return "";

        }
    }
}