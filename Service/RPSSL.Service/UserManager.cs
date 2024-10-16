using System.Text;
using System.Security.Cryptography;

using Microsoft.Extensions.Logging;

using Mmicovic.RPSSL.Service.Models;

namespace Mmicovic.RPSSL.Service
{
    public class UserAlreadyExistsException : Exception { };

    public interface IUserManager
    {
        Task CreateUser(string username, string passphrase, CancellationToken ct);
        Task<bool> AuthenticateUser(string username, string passphrase, CancellationToken ct);
    }

    /* Creates and authenticates users.
     * Passphrases are saved as salted hashes. */
    public class UserManager(UserContext userContext, ILogger<UserManager> logger) : IUserManager
    {
        private const int SALT_LENGTH = 5;

        private readonly ILogger<UserManager> logger = logger;
        private readonly UserContext userContext = userContext;

        public async Task<bool> AuthenticateUser(string username, string passphrase, CancellationToken ct)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(passphrase))
                return false;

            // Get the user from the database, check if it exists
            var user = await userContext.Users.FindAsync(username, ct);
            if (user is null)
                return false;

            // Generate a passphrase hash using the saved salt value
            // and compare it with the saved passphrase hash.
            var passphraseHash = GenerateSaltedHash(passphrase, user.Salt);
            return user.PassphraseHash.SequenceEqual(passphraseHash);
        }

        public async Task CreateUser(string username, string passphrase, CancellationToken ct)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(passphrase))
                throw new ArgumentException("Username and Passphrase are mandatory fields");

            // Check if a user already exists
            var user = await userContext.Users.FindAsync(username, ct);
            if (user is not null)
            {
                logger.LogInformation($"User {username} already exists!");
                throw new UserAlreadyExistsException();
            }

            // Generate a random salt and the salted passphrase hash
            var salt = GenerateSalt();
            var passphraseHash = GenerateSaltedHash(passphrase, salt);

            // Create and save the user
            user = new User
            {
                UserName = username,
                Salt = salt,
                PassphraseHash = passphraseHash
            };
            userContext.Users.Add(user);
            await userContext.SaveChangesAsync(ct);
        }

        private static byte[] GenerateSaltedHash(string passphrase, byte[] salt)
        {
            // Add the generated salt to the passphrase
            var passphraseBytes = Encoding.UTF8.GetBytes(passphrase);
            var saltedPassphrase = salt.Concat(passphraseBytes).ToArray();

            // Hash the salted passphrase
            return SHA256.HashData(saltedPassphrase);
        }

        private static byte[] GenerateSalt()
        {
            // Generate salt as a cryptographically strong random random byte array
            return RandomNumberGenerator.GetBytes(SALT_LENGTH);
        }
    }
}
