using Nice.ParameterCheck;

namespace Nice;
public static class ParameterCheckExtentions
{
    public static IntCheck For(this IParameterCheck last, int? newTarget)
    {
        return new IntCheck(newTarget, last.Errors);
    }
    public static IntCheck For(this IParameterCheck last, int newTarget)
    {
        return new IntCheck(newTarget, last.Errors);
    }
    public static DateTimeCheck For(this IParameterCheck last, DateTime? newTarget)
    {
        return new DateTimeCheck(newTarget, last.Errors);
    }
    public static DateTimeCheck For(this IParameterCheck last, DateTime newTarget)
    {
        return new DateTimeCheck(newTarget, last.Errors);
    }
    public static StringCheck For(this IParameterCheck last, string? newTarget)
    {
        return new StringCheck(newTarget, last.Errors);
    }
    public static GuidCheck For(this IParameterCheck last, Guid? newTarget)
    {
        return new GuidCheck(newTarget, last.Errors);
    }
    public static GuidCheck For(this IParameterCheck last, Guid newTarget)
    {
        return new GuidCheck(newTarget, last.Errors);
    }
    public static CheckArray<TNew> For<TNew>(this IParameterCheck last, TNew[] newTarget)
    {
        return new CheckArray<TNew>(newTarget, last.Errors);
    }
    public static CheckArray<TNew> For<TNew>(this IParameterCheck last, List<TNew> newTarget)
    {
        return new CheckArray<TNew>(newTarget, last.Errors);
    }
    public static Check<TNew> For<TNew>(this IParameterCheck last, TNew newTarget)
    {
        return new Check<TNew>(newTarget, last.Errors);
    }
}