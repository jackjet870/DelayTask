using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace DelayTask.Model
{
    /// <summary>
    /// 错误信息
    /// 线程安全类型
    /// </summary>
    public static class LastErrors
    {
        /// <summary>
        /// 保存错误信息的字典 
        /// </summary>
        private static ConcurrentDictionary<Guid, string> dic = new ConcurrentDictionary<Guid, string>();

        /// <summary>
        /// 设置错误信息
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="exception">错误信息</param>
        public static void SetLastError(Guid key, Exception exception)
        {
            var error = exception.ToString();
            if (dic.ContainsKey(key))
            {
                dic[key] = error;
            }
            dic.TryAdd(key, error);
        }

        /// <summary>
        /// 获取最近错误错误信息
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static string GetLastError(Guid key)
        {
            string value;
            dic.TryGetValue(key, out value);
            return value;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Remove(Guid key)
        {
            string value;
            return dic.TryRemove(key, out value);
        }
    }
}
