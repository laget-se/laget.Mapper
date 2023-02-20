namespace laget.Mapper.Extensions
{
    public static class ObjectExtension
    {
        public static TResult Map<TResult>(this object source) => Mapper.Map<TResult>(source);
    }
}