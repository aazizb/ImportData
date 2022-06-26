using Import.Interface;
using Import.Services;

using Microsoft.Extensions.DependencyInjection;

using System;

namespace Import
{
    public class Startup
    {
        public static IServiceProvider ConfiguerService()
        {
            //create a provider that returns IServiceProvider
            var provider = new ServiceCollection()
                .AddSingleton<IImportData, ImportData>()
                .BuildServiceProvider();
            return provider;
        }
    }
}
