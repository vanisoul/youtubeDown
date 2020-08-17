using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using YD.Lib;

namespace Youtube_download.Controllers
{
    [Route("Download")]
    [ApiController]
    public class DownloadController : ControllerBase
    {
        bool OnPreRequest(HttpWebRequest req)
        {
            req.Referer = "https://www.yt-download.org"; //how to know the referrer address here?
            return true;
        }
        [HttpPost]
        public object Download()
        {
            var dalist = LoadSong.LoadList();
            string msg = "";
            try
            {
                if (dalist.Count == 1)
                {
                    return new { msg = "待載清單以抓完", success = false };
                }
                else if (dalist[1].url != "")
                {
                    var urlold = dalist[1].url.Split("&");
                    Regex rg = new Regex(@"v=(.*\b)");
                    var inurl = $"https://www.yt-download.org/api/internal/mp3/{rg.Match(urlold[0]).Groups[1].Value}";
                    var web = new HtmlWeb();
                    web.PreRequest += OnPreRequest;
                    var doc = web.Load(inurl);
                    HtmlNodeCollection nameNodes = doc.DocumentNode.SelectNodes(@"//div[@class='text-center']");
                    Regex rgx = new Regex(@"href=.*(https://.*)class");
                    var url = rgx.Match(nameNodes[0].InnerHtml).Groups[1].Value.Replace("\"", "");
                    downloadSong(url, dalist[1].name);
                    msg = $"{dalist[1].name} 下載成功";
                    dalist.RemoveAt(1);
                    WriteSong.WriteList(dalist);

                }
                else
                {
                    dalist.RemoveAt(1);
                    WriteSong.WriteList(dalist);
                }

            }
            catch (Exception e)
            {
                dalist.RemoveAt(1);
                WriteSong.WriteList(dalist);
                return new { msg = $"{dalist[1].name} 抓取錯誤 跳過", success = true };
            }
            return new { msg = msg, success = true };

        }

        protected void downloadSong(string url, string name)
        {
            WebClient wc = new WebClient();
            wc.DownloadFile(new Uri(url), $".\\..\\..\\Downloadmp3\\{name}.mp3");
        }
    }
}