
using AutoMapper;
using AutoMapper.QueryableExtensions;
using IConfigurationProvider = AutoMapper.IConfigurationProvider;

namespace NGOAPP;
public static class IQueryableExtensions
{
    private static IMapper _mapper;
        private static IConfigurationProvider _configurationProvider;
        public static void Configure(IMapper mapper, IConfigurationProvider configurationProvider)
        {
            _mapper = mapper;
            _configurationProvider = configurationProvider;
        }

        public static void InitIQueryableExtensions(IMapper mapper, IConfigurationProvider configurationProvider)
        {
            _mapper = mapper;
            _configurationProvider = configurationProvider;
        }

        public static PagedCollection<T> ToPagedCollection<U, T>(this IQueryable<U> source, PagingOptions options, Link link)
        {
            
            var pagedQuery = source.Skip(options.Offset.Value).Take(options.Limit.Value);
            var mappedView = pagedQuery.ProjectTo<T>(_configurationProvider);
            var pagedCollection = PagedCollection<T>.Create(link, mappedView.ToArray(), source.Count(), options);
            return pagedCollection;
        }

        public static IQueryable<T> ApplyPagination<T>(this IQueryable<T> source, PagingOptions options)
        {
            return source.Skip(options.Offset.Value).Take(options.Limit.Value);
        }
}
