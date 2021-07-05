using System;
using System.Threading.Tasks;
using Univintel.GBN.Core;

namespace UnivIntel.GBN.Core
{
    public interface IAuthentificationService
    {

        void SetDatabase(IDatabaseService databaseService);

        string EncodePassword(string password);

        string RandomPassword();

        Task<string> Signup(string email, string timeZone);

        Task<string> SignupEmployee(string email, string password, Guid companyId, Guid hostAccountId, string position, string firstName, string lastName, Guid? avatarId);

        Task<(string, Guid)> Signin(string email, string code);

        Task<bool> Signout(Guid token);

        Task<bool> VerifyEmail(string email, string code);

        string RandomString(int length);

        Task<string> ResetPassword(Guid accountId, string oldPassword, string newPassword);

    }
}
