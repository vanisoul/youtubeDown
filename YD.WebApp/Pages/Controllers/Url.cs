using System;
using System.Collections.Generic;
using System.IO;
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
        public object SaveTotxt(dynamic json)
        {
            try
            {
                var dalist = LoadSong.LoadList();
                var str = $"{json}";
                var strlist = str.Split(",");
                foreach (var url in strlist)
                {
                    if (url != "")
                    {
                        //得到歌名
                        var web = new HtmlWeb();
                        var doc = web.Load(url);
                        var resp = doc.Text;
                        Regex rg = new Regex(@"<title>(.*)</title>");
                        var name = rg.Match(resp).Groups[1].Value.Replace(" - YouTube", "");

                        //判斷不重復歌單
                        Data newData = new Data() { url = url, name = name };
                        bool repeat = false;
                        dalist.ForEach(x =>
                        {
                            if (JsonConvert.SerializeObject(newData) == JsonConvert.SerializeObject(x))
                            {
                                repeat = true;
                            }
                        });
                        if (!repeat)
                        {
                            dalist.Add(newData);
                        }
                    }
                }
                WriteSong.WriteList(dalist);
            }
            catch
            {
                return new { success = false };
            }
            return new { success = true };

        }
    }
}