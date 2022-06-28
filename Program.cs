using Import.Context;
using Import.Interface;
using Import.Models;
using Import.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Serilog;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Import
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var builder = new ConfigurationBuilder();

                BuildConfig(builder);

                //add logging with serilog

                Log.Logger=new LoggerConfiguration()
                    .ReadFrom.Configuration(builder.Build())
                    .Enrich.FromLogContext()
                    .WriteTo.Console()
                    .CreateLogger();
                Log.Logger.Information("Application has started...");

                if (args.Length == 0)
                {
                    Log.Logger.Information("There is no file to process...");
                    return;
                }

                //add application settings and dependency injection

                var host = Host.CreateDefaultBuilder()
                    .ConfigureServices((context, services) =>
                    {
                        services.AddTransient<IImportData, ImportData>();
                        services.AddDbContext<ImportDbContext>(options => options.UseSqlServer());
                    })
                    .UseSerilog()
                    .Build();

                var service = ActivatorUtilities.CreateInstance<ImportData>(host.Services);

                string fullPath = args[0].ToString();

                Extension fileExtension = GetFileExtension(fullPath);

                if (fileExtension == Extension.Unknown)
                {
                    Log.Logger.Information("File could not be found or its format is not supported");
                    return;
                }
                Log.Logger.Information("Reading file content....");

                var fileContent = service.ReadFileContent(fullPath);

                switch (fileExtension)
                {

                    case Extension.Json:
                        JsonProduct jsonProduct = service.DeserializeJson<JsonProduct>(fileContent);
                        if (jsonProduct is null)
                        {
                            Log.Logger.Information("File could not be deserialized...");
                            return;
                        }
                        //flatten nested object
                        IList<Product> newJson = jsonProduct.products.ToList();
                        var flatten = newJson.SelectMany(o => o.Categories
                                .Select(p => new
                                {
                                    Category = p,
                                    o.Title,
                                    o.Twitter
                                }));

                        foreach (var item in flatten)
                        {
                            Console.WriteLine($"importing: name: {item.Title}; Categories: {item.Category}; Twitter: {item.Twitter}");
                        }

                        break;
                    case Extension.Yaml:
                        IList<YamlProduct> yaml = service.DeserializeYaml<YamlProduct>(fileContent);
                        if (yaml is null)
                        {
                            Log.Logger.Information($"File {fullPath} could not be deserialized.");
                            return;
                        }

                        foreach (YamlProduct item in yaml)
                        {
                            Console.WriteLine($"importing: name: {item.Name}; Categories: {item.Tags}; Twitter: {item.Twitter}");
                        }
                        ////in real world we would use EF Core for example to persist the data as shown bellow
                        //using (ImportDbContext context = new ImportDbContext())
                        //{
                        //    context.YamlProducts.AddRange(yaml);
                        //    context.SaveChanges();
                        //}
                        break;
                    default:
                        Log.Logger.Information($"File {fullPath} format is not supported");
                        break;
                }

            }
            catch (Exception exception)
            {
                Log.Logger.Fatal(exception, "Error while importing products");
                throw;
            }

        }
        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true);
                
        }
        static Extension GetFileExtension(string fileName)
        {
            string ex= Path.GetExtension(fileName).ToLower();
            return ex switch
            {
                ".json" => Extension.Json,
                ".yaml" => Extension.Yaml,
                _ => Extension.Unknown,
            };
        }
        enum Extension
        {
            Json,
            Yaml,
            Unknown
        }
    }
}
