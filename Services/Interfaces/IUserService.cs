using INVENTORY.SERVER.Common;
using INVENTORY.SHARED.Dto;

namespace INVENTORY.SERVER.Services.Interfaces
{
    public interface IUserService
    {
        Task<ServiceResponse<List<UserDto>>> GetUserAsync();
        Task<ServiceResponse<UserDto>> GetUserById(Guid userId);
        Task<ServiceResponse<UserDto>> RegisterUserAsync(UserRegister userRegister);
        Task<ServiceResponse<UserToken>> LoginAsync(UserLogin userLogin);
        Task<ServiceResponse<UserDto>> ChangePassword(ChangePassword changePassword);
        Task<ServiceResponse<ForgotPasswordResponse>> ForgotPasswordAsync(ForgotPasswordModel forgotPasswordModel);
    }
}
