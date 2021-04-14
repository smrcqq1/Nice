using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nice
{
    /// <summary>
    /// 便于针对本封装写单元测试
    /// </summary>
    public abstract class UnitTestBase
    {
        protected abstract IServiceCollection Config(IServiceCollection services);
        protected IORM Orm { 
            get => GetService<IORM>();
        }

        protected IUserinfo Userinfo
        {
            get => GetService<IUserinfo>();
        }
        IServiceCollection _ServiceCollection;
        protected IServiceCollection ServiceCollection
        {
            get
            {
                if (_ServiceCollection == null)
                {
                    _ServiceCollection = new ServiceCollection();
                    _ServiceCollection = Config(_ServiceCollection);
                }
                return _ServiceCollection;
            }
        }
        IServiceProvider _ServiceProvider;
        protected IServiceProvider ServiceProvider
        {
            get
            {
                if (_ServiceProvider == null)
                {
                    _ServiceProvider = ServiceCollection.BuildServiceProvider();
                }
                return _ServiceProvider;
            }
        }
        public TService GetService<TService>()
        {
            return ServiceProvider.GetService<TService>();
        }
    }
}