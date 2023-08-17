using System.Linq;

namespace Nice.ParameterCheck;
public interface IParameterCheck
{
    List<string> Errors { get; }
}
public abstract class CheckBase : IParameterCheck
{
    public CheckBase(List<string> errors)
    {
        errors ??= new List<string>();
        Errors = errors;
    }
    public List<string> Errors { get; private set; }
    /// <summary>
    /// 如果发现错误则抛出参数错误异常
    /// </summary>
    /// <exception cref="Nice.Exception"></exception>
    public IParameterCheck Throw()
    {
        if (Errors.Any())
        {
            throw new Nice.Exception(400, Errors);
        }
        return this;
    }
}
public class Check<T> : CheckBase
{
    internal readonly T Target;
    public Check(T target, List<string> errors):base(errors)
    {
        Target = target;
    }
    public Check(T target) : this(target,new List<string>())
    {
    }
    public Check<T> NotNull(string errorMessage)
    {
        if (Target == null)
        {
            Errors.Add(errorMessage);
        }
        return this;
    }
    /// <summary>
    /// 如果发现错误则抛出参数错误异常
    /// </summary>
    /// <exception cref="Nice.Exception"></exception>
    public new Check<T> Throw()
    {
        base.Throw();
        return this;
    }
}
public class CheckArray<T> : CheckBase
{
    internal readonly IEnumerable<T> Target;
    public CheckArray(IEnumerable<T> target, List<string> errors) : base(errors)
    {
        Target = target;
    }
    public CheckArray(IEnumerable<T> target) : this(target,new List<string>())
    {
    }
    public CheckArray<T> Foreach(Action<T> action)
    {
        foreach (var item in Target)
        {
            action(item);
        }
        return this;
    }
    public CheckArray<T> Foreach(Action<T,int> action)
    {
        var index = 0;
        foreach (var item in Target)
        {
            index++;
            action(item,index);
        }
        return this;
    }
    public CheckArray<T> NotNull(string errorMessage)
    {
        if (Target == null || !Target.Any())
        {
            Errors.Add(errorMessage);
        }
        return this;
    }
    /// <summary>
    /// 如果发现错误则抛出参数错误异常
    /// </summary>
    /// <exception cref="Nice.Exception"></exception>
    public new CheckArray<T> Throw()
    {
        base.Throw();
        return this;
    }
}

public class GuidCheck : Check<Guid?>
{
    public GuidCheck(Guid? target,List<string> errors):base(target, errors)
    {
    }
    public new GuidCheck NotNull(string message)
    {
        if (Target == null || Target.Equals(Guid.Empty))
        {
            Errors.Add(message);
        }
        return this;
    }
    /// <summary>
    /// 如果发现错误则抛出参数错误异常
    /// </summary>
    /// <exception cref="Nice.Exception"></exception>
    public new GuidCheck Throw()
    {
        base.Throw();
        return this;
    }
}
public class DateTimeCheck : Check<DateTime?>
{
    public DateTimeCheck(DateTime? target, List<string> errors) : base(target, errors)
    {
    }
    public DateTimeCheck(DateTime? target) : this(target, new List<string>())
    {
    }

    public DateTimeCheck Between(string displayname, DateTime min, DateTime max)
    {
        if (Target == null)
        {
            Errors.Add($"{displayname}不能为空");
        }
        else if (Target < min)
        {
            Errors.Add($"{displayname}至少需要{min}个字符");
        }
        else if (Target > max)
        {
            Errors.Add($"{displayname}最多{max}个字符");
        }
        return this;
    }
    public DateTimeCheck Max(string message, DateTime max)
    {
        if (Target == null)
        {
            return this;
        }
        if (Target > max)
        {
            Errors.Add(message);
        }
        return this;
    }
    static readonly DateTime EmptyTime = new DateTime();
    public new DateTimeCheck NotNull(string message)
    {
        if (Target == null || Target == EmptyTime)
        {
            Errors.Add(message);
        }
        return this;
    }
    public DateTimeCheck Min(string message, DateTime min)
    {
        if (Target == null || Target < min)
        {
            Errors.Add(message);
        }
        return this;
    }
    /// <summary>
    /// 如果发现错误则抛出参数错误异常
    /// </summary>
    /// <exception cref="Nice.Exception"></exception>
    public new DateTimeCheck Throw()
    {
        base.Throw();
        return this;
    }
}
public class StringCheck : Check<string?>
{
    public StringCheck(string? target, List<string> errors) : base(target, errors)
    {
    }
    public StringCheck(string? target) : this(target, new List<string>())
    {
    }

    public StringCheck Between(string displayname, int min = 1, int max = 200)
    {
        if (string.IsNullOrEmpty(Target) || Target.Length < min)
        {
            Errors.Add($"{displayname}至少需要{min}个字符");
        }
        else if(Target.Length > max)
        {
            Errors.Add($"{displayname}最多{max}个字符");
        }
        return this;
    }
    public StringCheck Max(string message, int max = 200)
    {
        if (string.IsNullOrEmpty(Target))
        {
            return this;
        }
        if (Target.Length > max)
        {
            Errors.Add(message);
        }
        return this;
    }

    public new StringCheck NotNull(string message)
    {
        if (string.IsNullOrEmpty(Target))
        {
            Errors.Add(message);
        }
        return this;
    }
    public StringCheck Min(string message, int min = 1)
    {
        if (string.IsNullOrEmpty(Target) || Target.Length < min)
        {
            Errors.Add(message);
        }
        return this;
    }
    /// <summary>
    /// 如果发现错误则抛出参数错误异常
    /// </summary>
    /// <exception cref="Nice.Exception"></exception>
    public new StringCheck Throw()
    {
        base.Throw();
        return this;
    }
}
public class IntCheck : Check<int?>
{
    public IntCheck(int? target, List<string> errors) : base(target, errors)
    {
    }
    public IntCheck(int? target) : this(target, new List<string>())
    {
    }

    public IntCheck Between(string displayname, int min = 1, int max = 200)
    {
        if (!Target.HasValue)
        {
            Errors.Add($"{displayname}不能为空");
        }
        else if (Target.Value < min)
        {
            Errors.Add($"{displayname}不能小于{min}");
        }
        else if (Target.Value > max)
        {
            Errors.Add($"{displayname}最多{max}个字符");
        }
        return this;
    }
    public IntCheck Max(string message, int max = 200)
    {
        if (!Target.HasValue)
        {
            return this;
        }
        if (Target.Value > max)
        {
            Errors.Add(message);
        }
        return this;
    }

    public new IntCheck NotNull(string message)
    {
        if (!Target.HasValue)
        {
            Errors.Add(message);
        }
        return this;
    }
    public IntCheck Min(string message, int min = 1)
    {
        if (!Target.HasValue || Target.Value < min)
        {
            Errors.Add(message);
        }
        return this;
    }
    /// <summary>
    /// 如果发现错误则抛出参数错误异常
    /// </summary>
    /// <exception cref="Nice.Exception"></exception>
    public new IntCheck Throw()
    {
        base.Throw();
        return this;
    }
}
public class DecimalCheck : Check<Decimal?>
{
    public DecimalCheck(Decimal? target, List<string> errors) : base(target, errors)
    {
    }
    public DecimalCheck(Decimal? target) : this(target, new List<string>())
    {
    }

    public DecimalCheck Between(string displayname, Decimal min = 1, Decimal max = 200)
    {
        if (!Target.HasValue)
        {
            Errors.Add($"{displayname}不能为空");
        }
        else if (Target.Value < min)
        {
            Errors.Add($"{displayname}不能小于{min}");
        }
        else if (Target.Value > max)
        {
            Errors.Add($"{displayname}最多{max}个字符");
        }
        return this;
    }
    public DecimalCheck Max(string message, Decimal max = 200)
    {
        if (!Target.HasValue)
        {
            return this;
        }
        if (Target.Value > max)
        {
            Errors.Add(message);
        }
        return this;
    }

    public new DecimalCheck NotNull(string message)
    {
        if (!Target.HasValue)
        {
            Errors.Add(message);
        }
        return this;
    }
    public DecimalCheck Min(string message, Decimal min = 1)
    {
        if (!Target.HasValue || Target.Value < min)
        {
            Errors.Add(message);
        }
        return this;
    }
    /// <summary>
    /// 小数点位数
    /// </summary>
    /// <param name="message"></param>
    /// <param name="places"></param>
    /// <returns></returns>
    public DecimalCheck DecimalPlaces(string message, int places = 2)
    {
        if (!Target.HasValue)
        {
            Errors.Add(message);
        }
        //todo 好像无法判断小数点位数
        return this;
    }
    /// <summary>
    /// 如果发现错误则抛出参数错误异常
    /// </summary>
    /// <exception cref="Nice.Exception"></exception>
    public new DecimalCheck Throw()
    {
        base.Throw();
        return this;
    }
}