using laget.Mapper.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace laget.Mapper.Extensions
{
    public static class IApplicationBuilderExtensions
    {
        public static void UseMappers(this IApplicationBuilder builder)
        {
            Mapper.RegisterMappers(builder.ApplicationServices.GetServices<IMapper>());
        }
    }
}