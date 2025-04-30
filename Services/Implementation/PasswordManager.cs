using System.Security.Cryptography;
using System.Text;
using TaskManagement.Services.Interface;
namespace TaskManagement.Services.Implementation
{

    public class PasswordManager : IPasswordManager
    {

        public void CreatePasswordHasher(string password, out string passwordHash, out string passwordSalt)
        {
            var hmac = new HMACSHA256();
            passwordSalt = Convert.ToBase64String(hmac.Key);
            passwordHash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }

        public bool VerifyPasswordHash(string password, string passwordHash, string passwordSalt)
        {
            var hmac = new HMACSHA256(Convert.FromBase64String(passwordSalt));
            var hash = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(password)));
            if (hash == passwordHash)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
