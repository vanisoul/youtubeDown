using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
            var success = true;
            var msg = "歌單新增成功\n";
            var oldDataList = LoadSong.LoadList();
            Parallel.ForEach(joinDataList, async forDataList =>
            {
                var url = forDataList.value;
                if (url != "")
                {
                    try
                    {
                        //得到v
                        Regex rg2 = new Regex(@"v=([\w\-]*)\b");
                        if (rg2.IsMatch(url))
                        {
                            var val = rg2.Match(url).Groups[1].Value;
                            //得到歌名
                            var web = new HtmlWeb();
                            var doc = await web.LoadFromWebAsync($"https://www.youtube.com/watch?v={val}");
                            var resp = doc.Text;
                            Regex rg1 = new Regex(@"<title>(.*)</title>");
                            var name = rg1.Match(resp).Groups[1].Value.Replace(" - YouTube", "");


                            var newData = new Data() { value = val, name = name };
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
                            WriteSong.WriteList(oldDataList);
                        }
                        else
                        {
                            throw new Exception($"{url} 建立錯誤\n");
                        }
                    }
                    catch (Exception e)
                    {
                        success = false;
                        msg += e.Message;
                    }
                }
            });
            return new { success = success, msg = msg };

        }

    }
}