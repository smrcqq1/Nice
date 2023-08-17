#region using
using Microsoft.EntityFrameworkCore;
using System.Reflection;
#endregion using
namespace Nice.EFCore
{
    /// <summary>
    /// 自动映射数据库的DbContext
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public class AutoDbContext: DbContext
    {
        /// <summary>
        /// 设置自动生成数据库表
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public static void SetTables(params Type[] types)
        {
            Tables.AddRange(types.Where(o => !o.IsAbstract && typeof(IDataTable).IsAssignableFrom(o)));
        }
        /// <summary>
        /// 设置自动生成数据库表
        /// </summary>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static void SetTables(params Assembly[] assemblies)
        {
            var types = assemblies.SelectMany(x => x.GetTypes()).ToArray();
            SetTables(types);
        }
        public AutoDbContext(DbContextOptions options) : base(options)
        {
            if (EnsureDeleted)
            {
                Database.EnsureDeleted();
            }
            if (EnsureCreated)
            {
                Database.EnsureCreated();
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var table in Tables)
            {
                modelBuilder.Entity(table);
            }
            base.OnModelCreating(modelBuilder);
        }
        public static List<Type> Tables = new List<Type>();

        public static bool EnsureDeleted { get; set; }
        public static bool EnsureCreated { get; set; }
    }
}