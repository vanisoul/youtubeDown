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
            string msg = "";
            try
            {
                var dalist = LoadSong.LoadList();
                if (dalist[1].url != "")
                {
                    var urlold = dalist[1].url;
                    Regex rg = new Regex(@"v=(.*\b)");
                    var inurl = $"https://www.yt-download.org/api/internal/mp3/{rg.Match(urlold).Groups[1].Value}";
                    var web = new HtmlWeb();
                    web.PreRequest += OnPreRequest;
                    var doc = web.Load(inurl);
                    HtmlNodeCollection nameNodes = doc.DocumentNode.SelectNodes(@"//div[@class='text-center']");
                    Regex rgx = new Regex(@"href=.*(https://.*)class");
                    var url = rgx.Match(nameNodes[0].InnerHtml).Groups[1].Value.Replace("\"", "");

                    msg = $"{dalist[1].name} 下載成功";
                    dalist.RemoveAt(1);
                    WriteSong.WriteList(dalist);

                }
                else
                {
                    msg = "url空白";
                }

            }
            catch (Exception e)
            {
                return new { msg = e, success = false };
            }
            return new { msg = msg, success = true };

        }

        protected void downloadSong(string filename, Stream FileStream)
        {
            WebClient wc = new WebClient();
            wc.DownloadFile("http://blog.darkthread.net/images/darkthreadbanner.gif", "b:\\darkthread.gif");
        }
    }
}