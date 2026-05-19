using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace Hoshiko.Utils
{
    public static class JsonDictionaryReader
    {
        public static string FilePath = "settings.json";

        public static Dictionary<string, string> ReadAsDictionary(string filePath)
        {
            if (!File.Exists(filePath))
                return new Dictionary<string, string>();

            var json = File.ReadAllText(filePath);

            return JsonSerializer.Deserialize<Dictionary<string, string>>(json)
                   ?? new Dictionary<string, string>();
        }

        private sealed class Item
        {
            public string Key { get; set; } = "";
            public string Value { get; set; } = "";
        }
    }
}
