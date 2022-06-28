using Import.Interface;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Import.Services
{
    public class ImportData : IImportData
    {
        public ImportData(ILogger<ImportData> log, IConfiguration config)
        {
            Log = log;
            Config = config;
        }
  
        public ILogger<ImportData> Log { get; }
        public IConfiguration Config { get; }

        public T DeserializeCsv<T>(string csvContent)
        {
            //not implemented
            throw new NotImplementedException();
        }

        public T DeserializeJson<T>(string jsonContent)
        {
            //do not distinguish lowercase and uppercase
            return (T)JsonSerializer.Deserialize(jsonContent, typeof(T),
                           new JsonSerializerOptions()
                           {
                               PropertyNameCaseInsensitive = true
                           });
        }

        public IList<T> DeserializeYaml<T>(string yamlContent)
        {
            //do not distinguish lowercase and uppercase
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            IList<T> results = deserializer.Deserialize<IList<T>>(yamlContent);
            return results;
        }

        public string ReadFileContent(string fileFullPath)
        {
            using StreamReader stream = new StreamReader ($"{fileFullPath}");
            return stream.ReadToEnd();
        }
    }
}
