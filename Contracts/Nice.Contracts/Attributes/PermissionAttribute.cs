//#region using
//using System;
//using System.Collections.Generic;
//using System.Text;

//#endregion using
//namespace Nice.Attributes
//{
//    /// <summary>
//    /// 标记需要指定的权限才能访问的接口
//    /// </summary>
//    /// <remarks>
//    /// 
//    /// </remarks>
//    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Method)]
//    public class PermissionAttribute<TEnum> : AuthorizeAttribute
//    {
//        public PermissionAttribute(params TEnum[] values)
//        {
//            Values = values;
//        }
//        /// <summary>
//        /// 权限集合
//        /// </summary>
//        public TEnum[] Values { get; private set; }
//    }
//}