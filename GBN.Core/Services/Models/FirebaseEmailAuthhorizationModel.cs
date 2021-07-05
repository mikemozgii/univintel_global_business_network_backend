namespace UnivIntel.GBN.Core.Services.Models
{

    /// <summary>
    /// Model for authorization by email/password pair.
    /// </summary>
    public class FirebaseEmailAuthhorizationModel
    {

        public string Email { get; set; }

        public string Password { get; set; }

        public bool ReturnSecureToken { get; set; } = true; // From documentation: Whether or not to return an ID and refresh token. Should always be true.

    }

}
