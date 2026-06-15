namespace Yarane.Contract.Dtos.Settings
{
    public class Sftp
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string RemoteDirectory { get; set; }
    }
}
