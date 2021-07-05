using System.Threading.Tasks;

namespace UnivIntel.GBN.Core
{
    public interface IEmailService
    {

        Task SendSimpleEmail(string to, string subject, string content);

    }
}
