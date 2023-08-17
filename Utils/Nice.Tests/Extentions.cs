#region using
using Nice.Tests.Host;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nice.Tests;
#endregion using
namespace Nice
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    public static class Extentions
    {
        /// <summary>
        /// 获取单机测试HOST,用于日常单机开发
        /// </summary>
        /// <remarks>
        /// 本机环境的host，每次测试都是一个全新的环境
        /// </remarks>
        public static ITestHost GetLocalHost()
        {
            return new LocalHost();
        }
        /// <summary>
        /// 获取测试环境Host，基本相当于真实环境
        /// </summary>
        public static ITestHost GetRemoteHost(string remoteServer)
        {
            return new RemoteHost();
        }
    }
}