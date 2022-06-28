using Import.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using System.Collections.Generic;

namespace Import.Interface
{
    public interface IImportData
    {
        T DeserializeCsv<T>(string csvContent);
        T DeserializeJson<T>(string jsonContent);
        IList<T> DeserializeYaml<T>(string yamlContent);
        string ReadFileContent(string filePath);
        IConfiguration Config { get; }
        ILogger<ImportData> Log { get; }
    }
}
