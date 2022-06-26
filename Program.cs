using Import.Context;
using Import.Interface;
using Import.Models;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Import
{
    class Program
    {
        static void Main(string[] args)
        {

            #region configure provider and resolve dependencies with GetService<IImportData>
            var container = Startup.ConfiguerService();
            var service = container.GetService<IImportData>(); 
            #endregion

            #region yaml data import
            const string yamlFile = @"C:\Users\aaziz\Downloads\software engineer coding assessment\software engineer\coding\feed-products\capterra.yaml";
            var yamlContent = service.ReadFileContent(yamlFile);
            IList<YamlProduct> yaml = service.DeserializeYaml<YamlProduct>(yamlContent);
            if (yaml is null)
            {
                return;
            }

            foreach (YamlProduct item in yaml)
            {
                Console.WriteLine($"importing: name: {item.Name}; Categories: {item.Tags}; Twitter: {item.Twitter}");
            }
            //in real world we would use EF Core for example to persist the data as shown bellow
            //using(var context = new ImportDbContext())
            //{
            //    context.YamlProducts.AddRange(yaml);
            //    context.SaveChanges();
            //}
            #endregion

            #region json data import
            const string jsonFile = @"C:\Users\aaziz\Downloads\software engineer coding assessment\software engineer\coding\feed-products\softwareadvice.json";
            var jsonContent = service.ReadFileContent(jsonFile);
            JsonProduct jsonProduct = service.DeserializeJson<JsonProduct>(jsonContent);
            if (jsonProduct is null)
            {
                return;
            }
            //flatten nested object
            IList<Product> newJson = jsonProduct.products.ToList();
            var flatten = newJson.SelectMany(o => o.Categories
                    .Select(p => new
                    {
                        Category = p,
                        Title = o.Title,
                        Twitter = o.Twitter
                    }));

            foreach (var item in flatten)
            {
                Console.WriteLine($"importing: name: {item.Title}; Categories: {item.Category}; Twitter: {item.Twitter}");
            }

            #endregion
        }
    }
}
