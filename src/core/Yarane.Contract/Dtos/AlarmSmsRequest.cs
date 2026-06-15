namespace Yarane.Contract.Dtos;

public class AlarmSmsRequest
{
    public List<string>? MobileNumbers { get; set; }
    public string? Task { get; set; }
    public string? Title { get; set; }
    public string? Proccess { get; set; }
    public string? Status { get; set; }
    public string? DateTime { get; set; }
}
