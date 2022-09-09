namespace INVENTORY.SERVER
{
    public class UserProfile : AutoMapper.Profile
    {
        public UserProfile()
        {
            CreateMap<INVENTORY.SHARED.Dto.UserCreateDto, INVENTORY.SHARED.Model.User>().ReverseMap();
            CreateMap<INVENTORY.SHARED.Dto.UserDto, INVENTORY.SHARED.Model.User>().ReverseMap();
            CreateMap<INVENTORY.SHARED.Dto.UserLogin, INVENTORY.SHARED.Model.User>().ReverseMap();
            CreateMap<INVENTORY.SHARED.Dto.UserRegister, INVENTORY.SHARED.Model.User>().ReverseMap();
        }
    }
}
