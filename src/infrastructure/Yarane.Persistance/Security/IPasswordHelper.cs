
namespace Yarane.Persistance.Security;

public interface IPasswordHelper
{
    string EncodePasswordMd5(string password);
}
