namespace Yarane.Contract.Dtos;

public class LoginUser
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Key { get; set; }
    public string Captcha { get; set; }


}

public class GetUserByEmail
{
    public string Email { get; set; }
}

public class GetUserByUserName
{
    public string UserName { get; set; }
}

public class GetUserByUserId
{
    public Guid UserId { get; set; }
}

public class GetUserByPhone
{
    public long Phone { get; set; }
}