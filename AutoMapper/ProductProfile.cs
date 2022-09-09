namespace INVENTORY.SERVER
{
    public class ProductProfile : AutoMapper.Profile
    {
        public ProductProfile()
        {
            CreateMap<INVENTORY.SHARED.Dto.ProductDto, INVENTORY.SHARED.Model.Product>().ReverseMap();
            CreateMap<INVENTORY.SHARED.Dto.ProductCreateDto, INVENTORY.SHARED.Model.Product>().ReverseMap();
        }
    }
}
