using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarehouseManagement.Application.Mappings
{
    public class ReceiptProfile : Profile
    {
        public ReceiptProfile()
        {
            CreateMap<ReceiptResource, ReceiptResourceItemDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ResourceId, opt => opt.MapFrom(src => src.ResourceId))
                .ForMember(dest => dest.ReceiptDocumentId, opt => opt.MapFrom(src => src.ReceiptDocumentId))
                .ForMember(dest => dest.ResourceName, opt => opt.MapFrom(src => src.Resource.Name.Value))
                .ForMember(dest => dest.UnitId, opt => opt.MapFrom(src => src.UnitOfMeasureId))
                .ForMember(dest => dest.UnitName, opt => opt.MapFrom(src => src.UnitOfMeasure.Currency.Value))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount.Value));
                

            CreateMap<ReceiptDocument, ReceiptDocumentDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.Number, opt => opt.MapFrom(src => src.Number.Value))
                .ForMember(dest => dest.Resources, opt => opt.MapFrom(src => src.ReceiptResources));

            CreateMap<ReceiptDocument, ReceiptRecordDto>()
                .ForMember(dest => dest.DocumentId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.DocumentNumber, opt => opt.MapFrom(src => src.Number.Value))
                .ForMember(dest => dest.DocumentDate, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.ReceiptResourceItemDtos, opt => opt.MapFrom(src => src.ReceiptResources));
        }
    }
}
