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
            var result = JsonConvert.DeserializeObject<List<Data>>(instr);
            return result;
        }
        public static String LoagFirstSong()
        {
            var ListData = LoadList();
            var resultFirst = "";
            try
            {
                resultFirst = ListData[1].name;
                return resultFirst;
            }
            catch //(ArgumentOutOfRangeException e)
            {
                return "";
            }
        }
    }

}
