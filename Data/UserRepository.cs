using INVENTORY.SERVER.Data.Interfaces;
using INVENTORY.SHARED.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace INVENTORY.SERVER.Data
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<User> _signInManager;

        public UserRepository(InventoryDBContext inventoryDBContext, IConfiguration configuration, UserManager<User> userManager, SignInManager<User> signInManager)
            : base(inventoryDBContext)
        {
            _userManager = userManager;
            _configuration = configuration;
            _signInManager = signInManager;
        }

        public async Task<List<User>> GetUserAsync()
        {
            var users = await InventoryDBContext.Users.ToListAsync();

            //var roles = await this.GetRoleAsync();

            //var userRoles = InventoryDBContext.Users.FromSqlRaw("SELECT * FROM AspNetUser").ToList();

            //users.ForEach(u =>
            //{
            //    var rolesByUser = userRoles.Where(ur => ur.UserId == u.Id);

            //    if (u.UsersRoles == null)
            //        u.UsersRoles = new List<UserRole>();

            //    rolesByUser.ToList().ForEach(ru =>
            //    {
            //        u.UsersRoles.Add(
            //            new UserRole
            //            {
            //                UserId = u.Id,
            //                RoleId = ru.RoleId,
            //                Role = roles.FirstOrDefault(r => r.Id == ru.RoleId)
            //            });
            //    });
            //});

            return users;
        }
        public async Task<User> GetUserById(Guid userId)
        {
            return await InventoryDBContext.Users
                    .FirstOrDefaultAsync(t => t.Id == userId.ToString());
        }
        public async Task<string> Register(User user)
        {
            var error = new StringBuilder();

            var result = await _userManager.CreateAsync(user, user.Password);

            if (!result.Succeeded)
            {
                result.Errors.ToList().ForEach(e =>
                {
                    error.Append(e.Description);
                    error.Append(" - ");
                });
            }

            return error.ToString();
        }
        public async Task<IdentityResult> ChangePassword(User user, string password)
        {
            try
            {
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, password);
                var result = await _userManager.UpdateAsync(user);
                return result;
            }
            catch (Exception ex)
            {

                throw new Exception($"{ex.Message}");
            }
        }


        public async Task<bool> LogInAsync(User user)
        {
            var result = await _signInManager.PasswordSignInAsync(user.UserName,
                          user.Password, false, lockoutOnFailure: true);

            return result.Succeeded;

        }
        public async Task<string> GeneratePasswordResetTokenAsync(User user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }
        public User GetUserByEmail(string email)
        {
            return _userManager.Users
                .FirstOrDefault(u => u.Email == email);
        }
    }
}
