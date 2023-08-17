#region using
using Microsoft.EntityFrameworkCore;

#endregion using
namespace Nice.EFCore
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public interface IEFCoreDataProvider<TDbContext> : IDataProvider where TDbContext : DbContext
    {
    }
}
