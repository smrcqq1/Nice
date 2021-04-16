namespace Nice
{
    public class TypeConveter
    {
        public static T As<T>(object val)
        {
            return default(T);
        }
    }
}