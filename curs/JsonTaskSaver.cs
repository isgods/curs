using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;

namespace curs
{
    public class JsonTaskSaver : ISaveList<List<TaskItem>>
    {
        private readonly JsonSerializerOptions options = new()
        {
            WriteIndented = true
        };

        public void Save(List<TaskItem> data, string path)
        {
            File.WriteAllText(path, JsonSerializer.Serialize(data, options));
        }

        public List<TaskItem> Load(string path)
        {
            if (!File.Exists(path))
                return new List<TaskItem>();

            return JsonSerializer.Deserialize<List<TaskItem>>(File.ReadAllText(path), options)
                   ?? new List<TaskItem>();
        }
    }
}
