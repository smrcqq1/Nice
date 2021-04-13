using System;
using System.Collections.Generic;
using System.Text;

namespace Nice.AutoInjects
{
    /// <summary>
    /// 自动注入标记,生命周期Singleton
    /// </summary>
    public interface ISingleton
    {
    }
    /// <summary>
    /// 自动注入标记,生命周期Scoped
    /// </summary>
    public interface IScoped
    {
    }
    /// <summary>
    /// 自动注入标记,生命周期Transient
    /// </summary>
    public interface ITransient
    {
    }
    
}
