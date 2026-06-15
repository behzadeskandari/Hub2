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

    public class ReportContextFactory
         : IDesignTimeDbContextFactory<ReportDbContext>
    {
        public ReportDbContext CreateDbContext(string[] args)
        {

            IConfigurationRoot configuration = new ConfigurationBuilder()
                 .SetBasePath(
                     Path.GetFullPath(
                         Path.Combine(
                             Directory.GetCurrentDirectory(),
                             "..",
                             "Yarane.Api")))
                 .AddJsonFile("appsettings.Development.json", optional: true)
                 .AddJsonFile("appsettings.json", optional: true)
                 .Build();

            var publishConfig =
                configuration
                    .GetSection("PublishConfig")
                    .Get<PublishConfig>();

            string connectionKey =
                Convert.ToBoolean(publishConfig.IsProduction)
                    ? "ReportConnection"
                    : "ReportConnection";

            string connectionString =
                configuration.GetConnectionString(connectionKey);

            var optionsBuilder =
                new DbContextOptionsBuilder<ReportDbContext>();

            optionsBuilder.UseSqlServer(connectionString);

            return new ReportDbContext(optionsBuilder.Options);
        }
    }
}