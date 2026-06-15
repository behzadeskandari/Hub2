namespace Yarane.Contract.Dtos;

public class ForgetPasswordApplyRequest
{
    public long? Phone { get; set; }
    public string? UserName { get; set; }

    public string? Otp { get; set; }
    public string? PassWord { get; set; }
    public string? ConfirmPassword { get; set; }
    public string? Tracker { get; set; }


}