using AutoMapper;
using Challenge.Bravo.Api.Models;

namespace Challenge.Bravo.Api.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CurrencyViewModel, Currency>();
            CreateMap<Currency, CurrencyViewModel>();
        }
    }
}
