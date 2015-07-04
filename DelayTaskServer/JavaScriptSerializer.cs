using NetworkSocket.Fast;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Script = System.Web.Script.Serialization;

namespace DelayTaskServer
{
    /// <summary>
    /// 序列化工具
    /// </summary>
    internal class JavaScriptSerializer : ISerializer
    {
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public object Deserialize(byte[] bytes, Type type)
        {
            if (bytes == null || bytes.Length == 0)
            {
                return null;
            }

            var serializer = new Script.JavaScriptSerializer();
            var json = Encoding.UTF8.GetString(bytes);
            return serializer.Deserialize(json, type);
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public byte[] Serialize(object model)
        {
            if (model == null)
            {
                return null;
            }

            var serializer = new Script.JavaScriptSerializer();
            var json = serializer.Serialize(model);

            var jsonFixed = Regex.Replace(json, @"\\/Date\((\d+)\)\\/", match => new DateTime(1970, 1, 1)
                .AddMilliseconds(long.Parse(match.Groups[1].Value))
                .ToLocalTime()
                .ToString("yyyy/MM/dd HH:mm:ss.fff")
                );
            return Encoding.UTF8.GetBytes(jsonFixed);
        }
    }
}
