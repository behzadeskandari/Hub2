namespace Yarane.Contract.Dtos.Settings;

public class ConnectigConfig
{
    public string? ApiKey { get; set; }
    public string? ServiceUrl { get; set; }
    public string? ConnectionString { get; set; }
    public string? RedisIp { get; set; }
    public string? DatabaseNumber { get; set; }
    public BiSettings? BiSettings { get; set; }
}
