using System.Linq.Expressions;

namespace Nice.Tests
{
    /// <summary>
    /// 对Moq进行封装
    /// </summary>
    public class MockService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TMock"></typeparam>
        /// <returns></returns>
        public static Mock<TMock> Mock<TMock>() where TMock : class
        {
            var target = new Moq.Mock<TMock>();
            return new Mock<TMock>(target);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TMock"></typeparam>
    public class Mock<TMock> where TMock : class
    {
        internal Moq.Mock<TMock> Target;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        public Mock(Moq.Mock<TMock> target)
        {
            this.Target = target;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public Setup<TMock, TResult> Setup<TResult>(Expression<Func<TMock, TResult>> expression)
        {
            var exp = Target.Setup(expression);
            return new Setup<TMock, TResult>(this, exp);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public SetupAsync<TMock, TResult> Setup<TResult>(Expression<Func<TMock, Task<TResult>>> expression)
        {
            var exp = Target.Setup(expression);
            return new SetupAsync<TMock, TResult>(this, exp);
        }
        /// <summary>
        /// 获取被mock出来的实例
        /// </summary>
        public TMock MockedClass
        {
            get
            {
                return Target.Object;
            }
        }
    }

    /// <summary>
    /// 单个Setup
    /// </summary>
    /// <typeparam name="TMock"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public class Setup<TMock,TResult> where TMock :class
    {
        Mock<TMock> service;
        Moq.Language.Flow.ISetup<TMock, TResult> setupExpression;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="setupExpression"></param>
        public Setup(Mock<TMock> service,Moq.Language.Flow.ISetup<TMock, TResult> setupExpression)
        {
            this.service = service;
            this.setupExpression = setupExpression;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Mock<TMock> SetReturns(TResult value)
        {
            setupExpression.Returns(value);
            return service;
        }
    }
    /// <summary>
    /// 单个Setup
    /// </summary>
    /// <typeparam name="TMock"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public class SetupAsync<TMock, TResult> where TMock : class
    {
        Mock<TMock> service;
        Moq.Language.Flow.ISetup<TMock, Task<TResult>> setupExpression;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="service"></param>
        /// <param name="setupExpression"></param>
        public SetupAsync(Mock<TMock> service, Moq.Language.Flow.ISetup<TMock, Task<TResult>> setupExpression)
        {
            this.service = service;
            this.setupExpression = setupExpression;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Mock<TMock> SetReturns(TResult value)
        {
            setupExpression.Returns(Task.FromResult(value));
            return service;
        }
    }
}