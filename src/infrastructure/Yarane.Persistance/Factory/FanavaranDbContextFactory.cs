using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Yarane.Contract.Dtos.Settings;
using Yarane.Persistance.Contexts;

namespace Yarane.Persistance.Factory
{
    public class FanavaranDbContextFactory : IDesignTimeDbContextFactory<YaraneDbContext>
    {

        public YaraneDbContext CreateDbContext(string[] args)
        {
            Console.WriteLine($"Creating CheckContextFactory...{Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "FanavaranDbContextFactory"))}");
            // بارگذاری کانفیگ از appsettings.json یا appsettings.Development.json
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "Yarane.Api")))
                .AddJsonFile("appsettings.Development.json", optional: true)
                 .AddJsonFile("appsettings.json", optional: true)
                .Build();



            var publishConfig = configuration.GetSection("PublishConfig").Get<PublishConfig>();
            string connectionConfig = Convert.ToBoolean(publishConfig.IsProduction)
                ? "ConnectigConfigProduction"
                : "ConnectigConfigDevelopment";
            var connectigConfig = configuration.GetSection(connectionConfig).Get<ConnectigConfig>();

            //services.AddDbContext<YaraneDbContext>(options => options.UseSqlServer(connectigConfig.ConnectionString));


            var optionsBuilder =
                new DbContextOptionsBuilder<YaraneDbContext>();

            Console.WriteLine(Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "endpoint", "Yarane.Api")));

            optionsBuilder.UseSqlServer(connectigConfig.ConnectionString);


            return new YaraneDbContext(optionsBuilder.Options);
        }
    }
}
