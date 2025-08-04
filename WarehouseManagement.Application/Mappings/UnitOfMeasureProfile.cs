using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WarehouseManagement.Application.DTOs;
using WarehouseManagement.Domain.Entities;

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
