using Vehicles.Common.Models;

namespace Vehicles.API.Helpers
{
    public interface IMailHelper
    {
        Response SendEmail(string to, string subject, string body);
    }
}
