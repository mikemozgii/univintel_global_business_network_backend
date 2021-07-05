using BCryptRoot = BCrypt.Net.BCrypt;
using SaltRevision = BCrypt.Net.SaltRevision;

namespace UnivIntel.GBN.Core.Services
{
    public class BCryptProvider : IBCryptProvider
    {

        public string Hash(string input, string salt) => BCryptRoot.HashPassword(input + salt, BCryptRoot.GenerateSalt(SaltRevision.Revision2));

        public bool IsValid(string input, string validInput, string salt) => BCryptRoot.Verify(input + salt, validInput);

    }

}
