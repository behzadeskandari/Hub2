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
    public class ExceptionContextFactory
      : IDesignTimeDbContextFactory<ExceptionContext>
    {
        public ExceptionContext CreateDbContext(string[] args)
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
                    ? "ProdExceptionConnection"
                    : "ExceptionConnection";

            string connectionString =
                configuration.GetConnectionString(connectionKey);

            var optionsBuilder =
                new DbContextOptionsBuilder<ExceptionContext>();

            optionsBuilder.UseSqlServer(connectionString);

            return new ExceptionContext(optionsBuilder.Options);
        }
    }
}
