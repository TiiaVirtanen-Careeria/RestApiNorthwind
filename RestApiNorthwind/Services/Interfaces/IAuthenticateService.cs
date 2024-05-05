using RestApiNorthwind.Models;

namespace RestApiNorthwind.Services.Interfaces
{
    public interface IAuthenticateService
    {
        LoggedUser Authenticate(string username, string password);
    }
}
