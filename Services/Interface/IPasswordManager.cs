namespace TaskManagement.Services.Interface
{
    public interface IPasswordManager
    {
        public void CreatePasswordHasher(string password, out string passwordHash, out string passwordSalt);
        public bool VerifyPasswordHash(string password, string passwordHash, string passwordSalt);
    }
}
