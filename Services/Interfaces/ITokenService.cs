using INVENTORY.SHARED.Model;

namespace INVENTORY.SERVER.Services.Interfaces
{
    public interface ITokenService
    {
        string BuildToken(string key, string issuer, User user);
        bool ValidateToken(string key, string issuer, string token);
    }
}
