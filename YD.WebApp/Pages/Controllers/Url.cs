using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YD.Lib;

namespace Youtube_download.Controllers
{
    [Route("Url")]
    [ApiController]
    public class UrlController : ControllerBase
    {
        [HttpPost]
        public object SaveTotxt(List<Data> joinDataList)
        {
            try
            {
                //var newDataList = new List<Data>();
                var oldDataList = LoadSong.LoadList();
                joinDataList.ForEach(forDataList =>
                {
                    var url = forDataList.value;
                    if (url != "")
                    {
                        //得到歌名
                        var web = new HtmlWeb();
                        var doc = web.Load(url);
                        var resp = doc.Text;
                        Regex rg1 = new Regex(@"<title>(.*)</title>");
                        var name = rg1.Match(resp).Groups[1].Value.Replace(" - YouTube", "");
                        Regex rg2 = new Regex(@"v=([\w\-]*)\b");

                        var newData = new Data() { value = rg2.Match(url).Groups[1].Value, name = name };
                        //判斷不重復歌單
                        bool repeat = false;
                        oldDataList.ForEach(oldData =>
                        {
                            if (JsonConvert.SerializeObject(newData) == JsonConvert.SerializeObject(oldData))
                            {
                                repeat = true;
                            }
                        });
                        if (!repeat)
                        {
                            oldDataList.Add(newData);
                        }
                    }
                });

                WriteSong.WriteList(oldDataList);
            }
            catch
            {
                return new { success = false };
            }
            return new { success = true };

        }

    }
}