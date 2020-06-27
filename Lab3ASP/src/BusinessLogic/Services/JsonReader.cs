using BusinessLogic.Interfaces;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class JsonReader : IReader
    {
        public async Task<IEnumerable<T>> Read<T>(string path)
        {
            using var fileStream = new FileStream(path, FileMode.OpenOrCreate);
            return await JsonSerializer.DeserializeAsync<IEnumerable<T>>(fileStream);
        }
    }
}