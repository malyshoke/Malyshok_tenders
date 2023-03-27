using System.Security.Cryptography;
using System.Text;

namespace LR2_Malyshok.Models
{
    public class User
    {
        public string Login { get; set; }
        private byte[] password;
        public string Password
        {
            get
            {
                var sb = new StringBuilder();
                foreach (var b in new MD5CryptoServiceProvider().ComputeHash(password))
                    sb.Append(b.ToString("x2"));
                return sb.ToString();
            }
            set { password = Encoding.UTF8.GetBytes(value); }
        }
        public bool IsAdmin => Login == "admin";

        public bool CheckPassword(string password) => password == Password;
    }
}
