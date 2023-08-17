#region using
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
#endregion using

namespace Nice.Tests
{
    /// <summary>
    /// 对一个类进行测试的基类（一般方案）
    /// </summary>
    /// <remarks>
    /// 此方案只有基本封装，测试比较随意
    /// </remarks>
    public abstract class TestBase
    {
        /// <summary>
        /// 配置mock数据，仅当前TestMethod有效
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected Mock<T> MockService<T>() where T : class
        {
            var res = Nice.Tests.MockService.Mock<T>();
            return res;
        }
        static ITestHost host;
        /// <summary>
        /// 测试环境
        /// </summary>
        protected ITestHost Host
        {
            get
            {
                if (host == null)
                {
                    host = Extentions.GetLocalHost();
                    Setup(host.Services);
                }
                return host;
            }
        }
        /// <summary>
        /// 获取测试用的数据提供器
        /// </summary>
        protected IDataProvider DataProvider
        {
            get
            {
                return GetService<IDataProvider>();
            }
        }
        /// <summary>
        /// 获取一个Service
        /// </summary>
        /// <typeparam name="TTestTarget"></typeparam>
        /// <returns></returns>
        /// <remarks>
        /// 请注意：所有service都是Transient生命周期，即使在当前TestMethod内再次获取都是新的对象，所以如果要多次调用同一个对象，请先将它赋值给一个变量
        /// 如果没有配置该service，则自动mock一个
        /// </remarks>
        protected TTestTarget GetService<TTestTarget>() where TTestTarget : class
        {
            var service = Host.GetService<TTestTarget>();
            if (service == null)
            {
                service = MockService<TTestTarget>().MockedClass;
            }
            return service;
        }
        /// <summary>
        /// 配置测试环境
        /// </summary>
        /// <param name="services"></param>
        protected virtual void Setup(IServiceCollection services)
        {
            services.AddTransient<IDataProvider, Nice.EFCore.EFDataProvider<Nice.EFCore.AutoDbContext>>();
            services.AddDbContext<Nice.EFCore.AutoDbContext>(op =>
            {
                op.UseInMemoryDatabase(Guid.NewGuid().ToString());
            });
        }

        /// <summary>
        /// 检查接口是否抛出指定的业务逻辑错误
        /// </summary>
        /// <param name="action"></param>
        /// <param name="message"></param>
        public static void ThrowBuzinessException(string message, Action action)
        {
            ThrowException<Nice.BuzinessException>(message, action);
        }
        /// <summary>
        /// 检查接口是否抛出指定的业务逻辑错误
        /// </summary>
        /// <param name="action"></param>
        /// <param name="message"></param>
        public static void ThrowBuzinessException(string message, Func<Task> action)
        {
            ThrowBuzinessException(message, () =>
            {
                action().Wait();
            });
        }
        /// <summary>
        /// 检查是否抛出指定的参数错误异常
        /// </summary>
        /// <param name="message"></param>
        /// <param name="action"></param>
        public static void ThrowParameterErrorException(string message, Func<Task> action)
        {
            ThrowException<Nice.Exception>(message, () =>
            {
                action().Wait();
            });
        }
        /// <summary>
        /// 检查是否抛出指定的参数错误异常
        /// </summary>
        /// <param name="message"></param>
        /// <param name="action"></param>
        public static void ThrowParameterErrorException(string message, Action action)
        {
            ThrowException<Nice.Exception>(message, () =>
            {
                action();
            });
        }

        /// <summary>
        /// 检查接口是否抛出指定的业务逻辑错误
        /// </summary>
        /// <param name="action"></param>
        /// <param name="message"></param>
        public static void ThrowException(string message, Action action) 
        {
            ThrowException<Nice.Exception>(message,action);
        }
        /// <summary>
        /// 检查接口是否抛出指定的业务逻辑错误
        /// </summary>
        /// <param name="action"></param>
        /// <param name="message"></param>
        public static void ThrowSystemException(string message, Action action)
        {
            ThrowException<System.Exception>(message, action);
        }
        /// <summary>
        /// 检查接口是否抛出指定的业务逻辑错误
        /// </summary>
        /// <param name="action"></param>
        /// <param name="message"></param>
        public static void ThrowException<TExeception>(string message, Action action) where TExeception : System.Exception
        {
            try
            {
                action();
            }
            catch (TExeception ex)
            {
                if (ex.Message == message)
                {
                    return;
                }
                Assert.Fail($"抛出了指定的错误,但是提示信息不对,应为[{message}],实际为[{ex.Message}]");
            }
            catch (AggregateException aggr)
            {
                if (aggr.InnerException != null)
                {
                    if (aggr.InnerException is TExeception berror)
                    {
                        if (berror.Message == message)
                        {
                            return;
                        }
                        Assert.Fail($"抛出了指定的错误,但是提示信息不对,应为[{message}],实际为[{berror.Message}]");
                        return;
                    }
                    if (aggr.InnerException is Nice.BuzinessException bex)
                    {
                        Assert.AreEqual(message, bex.Message, $"抛出了指定的错误,但是提示信息不对,应为[{message}],实际为[{bex.Message}]");
                        return;
                    }
                    if (aggr.InnerException is Nice.Exception ex)
                    {
                        Assert.AreEqual(ex.Message, "发生错误", "未抛出错误消息");
                        Assert.IsNotNull(ex.ErrMsg, "未抛出错误消息");
                        Assert.IsTrue(ex.ErrMsg.Any(), "未抛出错误消息");
                        var m = ex.ErrMsg[0];
                        Assert.AreEqual(message, m, $"抛出了指定的错误,但是提示信息不对,应为[{message}],实际为[{m}]");
                        return;
                    }
                }
            }
            catch (Nice.BuzinessException bex)
            {
                Assert.AreEqual(message, bex.Message, $"抛出了指定的错误,但是提示信息不对,应为[{message}],实际为[{bex.Message}]");
                return;
            }
            catch (Nice.Exception ex)
            {
                Assert.AreEqual(ex.Message, "发生错误", "未抛出错误消息");
                Assert.IsNotNull(ex.ErrMsg, "未抛出错误消息");
                Assert.IsTrue(ex.ErrMsg.Any(), "未抛出错误消息");
                var m = ex.ErrMsg[0];
                Assert.AreEqual(message,m,$"抛出了指定的错误,但是提示信息不对,应为[{message}],实际为[{m}]");
                return;
            }
            Assert.Fail("没有抛出指定的错误：" + message);
        }
    }
}