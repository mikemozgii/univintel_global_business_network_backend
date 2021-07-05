namespace UnivIntel.GBN.Core
{
    public interface IBCryptProvider
    {

        string Hash(string input, string salt);

        bool IsValid(string input, string validInput, string salt);

    }
}
