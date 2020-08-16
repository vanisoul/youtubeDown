using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace YD.Lib
{
    public static class LoadSong
    {
        public static List<Data> LoadList()
        {
            List<Data> dalist = new List<Data>();

            using var inputFile = new StreamReader(Path.Combine("WriteSong"));
            var instr = inputFile.ReadToEndAsync().Result;
            var result = (instr != "") ? JsonConvert.DeserializeObject<List<Data>>(instr) : new List<Data>() { new Data { url = "", name = "沒有待下載歌單" } };
            // dalist.Add(new Data() { url = "Craig1", name = "Playstead1" });
            // dalist.Add(new Data() { url = "Craig2", name = "Playstead2" });
            // dalist.Add(new Data() { url = "Craig3", name = "Playstead3" });

            // var o = new Data() { url = "Craig4", name = "Playstead4" };
            // using StreamWriter outputFile = new StreamWriter(Path.Combine("WriteLines.txt"));
            // outputFile.WriteLine(JsonConvert.SerializeObject(dalist));
            return result;
        }
    }

    public class Data
    {
        public string url { get; set; }
        public string name { get; set; }
    }
}
