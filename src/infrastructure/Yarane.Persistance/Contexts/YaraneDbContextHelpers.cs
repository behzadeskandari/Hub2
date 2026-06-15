using Microsoft.EntityFrameworkCore;
using Yarane.Domain.Entities;
using Yarane.Persistance.EntityConfigs;

internal static class YaraneDbContextHelpers
{

    public static void EntityConfigurations(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfigs());
    }
}