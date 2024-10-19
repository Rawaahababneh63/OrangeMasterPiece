using System.Text;

namespace Masterpiece.DTO
{
    public class PasswordHasherNew
    {

        public static void createPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {

            using (var h = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = h.Key;  // key come from method sha512
                passwordHash = h.ComputeHash(Encoding.UTF8.GetBytes(password));  // password ==> hashing 
            }

        }


        public static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(storedHash);
            }
        }

    }
}
