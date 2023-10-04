using AutoMapper;

namespace Batch_Manager.DataTableObjects
{
    public static class AutoMapperExtensions
    {
        public static ICollection<TDestination> MapList<TSource, TDestination>(this IMapper mapper, ICollection<BatchFileDto> source)
        {
            return mapper.Map<ICollection<TDestination>>(source);
        }
    }
}
