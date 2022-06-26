using Import.Interface;

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
        public T DeserializeCsv<T>(string csvContent)
        {
            //no implemented
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

        public string ReadFileContent(string filePath)
        {
            using StreamReader stream = new StreamReader ($@"{filePath}");
            return stream.ReadToEnd();
        }
    }
}
