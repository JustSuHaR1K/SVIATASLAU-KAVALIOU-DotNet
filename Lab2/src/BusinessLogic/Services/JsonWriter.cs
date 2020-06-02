using BusinessLogic.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class JsonWriter : IWriter
    {
        public async Task Write<T>(IEnumerable<T> list, string path)
        {
            using FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate);
            await JsonSerializer.SerializeAsync(fileStream, list);
        }
    }
}