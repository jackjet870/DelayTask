using NetworkSocket.Fast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DelayTaskServer
{
    /// <summary>
    /// 异常处理过滤器
    /// </summary>
    public class ExceptionFilter : FilterAttribute, IExceptionFilter
    {
        /// <summary>
        /// 异常时
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
        }
    }
}
