using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace YD.Lib
{
    public static class WriteSong
    {
        public static void WriteList(List<Data> dalist)
        {
            using StreamWriter outputFile = new StreamWriter(Path.Combine("WriteSong"));
            outputFile.WriteLineAsync(JsonConvert.SerializeObject(dalist));
        }
    }
}
