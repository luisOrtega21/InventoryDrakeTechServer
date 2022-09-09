using INVENTORY.SHARED.Model;
using Microsoft.AspNetCore.Identity;

namespace INVENTORY.SERVER.Data.Interfaces
{
    public interface IUserRepository : IBaseRepository<User>
    {
        Task<List<User>> GetUserAsync();
        Task<User> GetUserById(Guid userId);
        Task<string> Register(User use);
        Task<IdentityResult> ChangePassword(User user, string newPassword);
        Task<bool> LogInAsync(User user);
        Task<string> GeneratePasswordResetTokenAsync(User user);
        User GetUserByEmail(string email);

    }
}
