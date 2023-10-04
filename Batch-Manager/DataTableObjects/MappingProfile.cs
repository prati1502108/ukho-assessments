using AutoMapper;
using Batch_Manager.Models;

namespace Batch_Manager.DataTableObjects
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BatchFileDto, BatchFile>().ReverseMap();
        }
    }
}
