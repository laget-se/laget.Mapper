using System.Collections.Generic;
using System.Linq;

namespace laget.Mapper.Extensions
{
    public static class IEnumerableExtension
    {
        public static IEnumerable<TResult> Map<TResult>(this IEnumerable<object> source) => source.Select(x => Mapper.Map<TResult>(x));
    }
}