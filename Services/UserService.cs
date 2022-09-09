using AutoMapper;
using INVENTORY.SERVER.Common;
using INVENTORY.SERVER.Data.Interfaces;
using INVENTORY.SERVER.Services.Interfaces;
using INVENTORY.SHARED.Dto;
using INVENTORY.SHARED.Model;

namespace INVENTORY.SERVER.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IMailService _mailService;
        private readonly IConfiguration _configuration;

        public UserService(IMapper mapper, IUserRepository userRepository, IConfiguration configuration,
            ILoggerManager loggerManager, ITokenService tokenService, IMailService mailService)
        : base(mapper, loggerManager)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _tokenService = tokenService;
            _mailService = mailService;
        }
        public async Task<ServiceResponse<List<UserDto>>> GetUserAsync()
        {
            try
            {
                var users = await _userRepository.GetUserAsync();
                if (users == null)
                    users = new List<User>();

                return new ServiceResponse<List<UserDto>>(Mapper.Map<List<UserDto>>(users));
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                return new ServiceResponse<List<UserDto>>($"{ex.Message}");
            }
        }
        public async Task<ServiceResponse<UserDto>> GetUserById(Guid userId)
        {
            try
            {
                var user = await _userRepository.GetUserById(userId);
                if (user == null)
                {
                    throw new ArgumentException($"No hay un obetivo con este identificador");
                }
                return new ServiceResponse<UserDto>(Mapper.Map<UserDto>(user));
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                return new ServiceResponse<UserDto>($"{ex.Message}");
            }
        }
        public async Task<ServiceResponse<UserDto>> RegisterUserAsync(UserRegister userRegister)
        {
            try
            {
                User user = Mapper.Map<User>(userRegister);

                if (user == null || string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.UserName))
                {
                    throw new ArgumentException($"una propiedad es requerida");
                }

                _userRepository.InventoryDBContext.BeginTransaction();
                var registerResult = await _userRepository.Register(user);

                if (registerResult != string.Empty)
                {
                    _userRepository.InventoryDBContext.Rollback();
                    return new ServiceResponse<UserDto>($" {registerResult}");
                }
                else
                {
                    await _userRepository.InventoryDBContext.CommitAsync();
                    return new ServiceResponse<UserDto>(Mapper.Map<UserDto>(user));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                return new ServiceResponse<UserDto>($"{ex.Message}");
            }
        }
        public async Task<ServiceResponse<UserToken>> LoginAsync(UserLogin userLogin)
        {
            try
            {
                User user = Mapper.Map<User>(userLogin);

                if (string.IsNullOrEmpty(userLogin.Email) || string.IsNullOrEmpty(userLogin.Password))
                {
                    _userRepository.InventoryDBContext.Rollback();
                    return new ServiceResponse<UserToken>($"error ein login");
                }
                var userFromDB = _userRepository.GetUserByEmail(userLogin.Email);
                user.UserName = userFromDB.UserName;
                return new ServiceResponse<UserToken>($"error in token");

            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                return new ServiceResponse<UserToken>($"{ex.Message}");
            }
        }
        
        public async Task<ServiceResponse<ForgotPasswordResponse>> ForgotPasswordAsync(ForgotPasswordModel forgotPasswordModel)
        {
            try
            {
                if (string.IsNullOrEmpty(forgotPasswordModel.To))
                {
                    return new ServiceResponse<ForgotPasswordResponse>($"error in password recovery");
                }

                var userFromDB = _userRepository.GetUserByEmail(forgotPasswordModel.To);

                if (userFromDB == null)
                {
                    return new ServiceResponse<ForgotPasswordResponse>($"No hay un usuario con el email registrado");
                }

                var token = _userRepository.GeneratePasswordResetTokenAsync(userFromDB);
                var maiRequest = new MailRequest();
                maiRequest.ToEmail = forgotPasswordModel.To;
                maiRequest.Body = File.ReadAllText(_configuration.GetValue<string>("Templates:ResetPassword"));
                maiRequest.Subject = "GoIn - Recuperación de clave";
                await _mailService.SendEmailAsync(maiRequest);
                var forgotPasswordResponse = new ForgotPasswordResponse();
                forgotPasswordResponse.Message = "Email sent";
                return new ServiceResponse<ForgotPasswordResponse>(forgotPasswordResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                return new ServiceResponse<ForgotPasswordResponse>($"{ex.Message}");
            }
        }
        public async Task<ServiceResponse<UserDto>> ChangePassword(ChangePassword changePassword)
        {
            try
            {
                if (changePassword == null)
                {
                    throw new ArgumentException($"error in change password");
                }
                _userRepository.InventoryDBContext.BeginTransaction();
                var user = _userRepository.GetUserByEmail(changePassword.Email);

                if (user == null)
                {
                    throw new ArgumentException($"una propiedad es requerida");
                }
                var registerResult = await _userRepository.ChangePassword(user, changePassword.Password);
                await _userRepository.InventoryDBContext.CommitAsync();

                var maiRequest = new MailRequest();
                maiRequest.ToEmail = changePassword.Email;
                maiRequest.Body = File.ReadAllText(_configuration.GetValue<string>("Templates:ConfirmPassword"));
                maiRequest.Subject = "Cambio exitoso de contraseña";
                await _mailService.SendEmailAsync(maiRequest);

                return new ServiceResponse<UserDto>(Mapper.Map<UserDto>(user));
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}");
                return new ServiceResponse<UserDto>($"{ex.Message}");
            }
        }
        
    }
}
