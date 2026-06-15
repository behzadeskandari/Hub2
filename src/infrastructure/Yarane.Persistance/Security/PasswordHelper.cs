using System.Security.Cryptography;
using System.Text;

namespace Yarane.Persistance.Security;
public class PasswordHelper : IPasswordHelper
{
    public string EncodePasswordMd5(string password) //Encrypt using MD5    
    {
        byte[] originalBytes;
        byte[] encodedBytes;
        MD5 md5;
        //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)    
        md5 = new MD5CryptoServiceProvider();
        originalBytes = Encoding.Default.GetBytes(password);
        encodedBytes = md5.ComputeHash(originalBytes);
        //Convert encoded bytes back to a 'readable' string    
        return BitConverter.ToString(encodedBytes);
    }
}

