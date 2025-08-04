using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WarehouseManagement.Domain.Entities;
using WarehouseManagement.Application.DTOs;

namespace WarehouseManagement.Application.Mappings
{
    public class ResourceProfile : Profile
    {
        public ResourceProfile()
        {
            CreateMap<Resource, ResourceDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Value))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.ResourceState));
        }
    }
}
