using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagement.Application.Mappings
{
    public class UnitOfMeasureProfile : Profile
    {
        public UnitOfMeasureProfile()
        {
            CreateMap<UnitOfMeasure, UnitOfMeasureDto>()
                .ForMember(dest => dest.Currency, opt => opt.MapFrom(src => src.Currency.Value))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.UnitOfMeasureState));
        }
    }
}
