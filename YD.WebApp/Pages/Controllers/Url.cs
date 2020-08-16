using System;
using System.Collections.Generic;
using System.IO;
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
                        Data newData = new Data() { url = url, name = url };
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
                using StreamWriter outputFile = new StreamWriter(Path.Combine("WriteSong"));
                outputFile.WriteLine(JsonConvert.SerializeObject(dalist));
            }
            catch
            {
                return new { success = false };
            }
            return new { success = true };

        }
    }
}