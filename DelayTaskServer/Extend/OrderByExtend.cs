using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DelayTaskServer
{
    /// <summary>
    /// 排序
    /// </summary>
    public static class OrderByExtend
    {
        /// <summary>
        /// 排序公共方法
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="orderByKey">排序键排序键(不分大小写)</param>
        /// <param name="orderByMethod">排序方法</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        private static IOrderedQueryable<T> OrderByCommon<T>(IQueryable<T> source, string orderByKey, string orderByMethod) where T : class
        {
            if (string.IsNullOrEmpty(orderByKey))
            {
                throw new ArgumentNullException("orderByKey");
            }

            var sourceTtype = typeof(T);
            var keyProperty = sourceTtype.GetProperties().FirstOrDefault(p => p.Name.ToLower().Equals(orderByKey.Trim().ToLower()));
            if (keyProperty == null)
            {
                throw new ArgumentException("orderByKey不存在...");
            }

            var param = Expression.Parameter(sourceTtype, "item");
            var body = Expression.MakeMemberAccess(param, keyProperty);
            var orderByLambda = Expression.Lambda(body, param);

            var resultExp = Expression.Call(typeof(Queryable), orderByMethod, new Type[] { sourceTtype, keyProperty.PropertyType }, source.Expression, Expression.Quote(orderByLambda));
            var ordereQueryable = source.Provider.CreateQuery<T>(resultExp) as IOrderedQueryable<T>;
            return ordereQueryable;
        }

        /// <summary>
        /// 排序
        /// 选择升序或降序
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="orderByKey">排序键排序键(不分大小写)</param>
        /// <param name="ascending">是否升序</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        private static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string orderByKey, bool ascending) where T : class
        {
            var methodName = ascending ? "OrderBy" : "OrderByDescending";
            return OrderByCommon(source, orderByKey, methodName);
        }

        /// <summary>
        /// 排序次项
        /// 选择升序或降序
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="orderByKey">排序键(不分大小写)</param>
        /// <param name="ascending">是否升序</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <returns></returns>
        private static IOrderedQueryable<T> ThenBy<T>(this IOrderedQueryable<T> source, string orderByKey, bool ascending) where T : class
        {
            var methodName = ascending ? "ThenBy" : "ThenByDescending";
            return OrderByCommon(source, orderByKey, methodName);
        }


        /// <summary>
        /// 多字段混合排序       
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="source">数据源</param>
        /// <param name="orderByString">排序字符串：例如CreateTime desc, ID asc 不区分大小写</param>
        /// <exception cref="ArgumentNullException">orderByString</exception>
        /// <returns></returns>
        public static IOrderedQueryable<T> OrderBy<T>(this IQueryable<T> source, string orderByString) where T : class
        {
            Func<string[], bool> descFun = (item) => item.Length > 1 && item[1].Trim().ToLower().Equals("desc");

            var parameters = orderByString
                .Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                .Select(item => item.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries))
                .Select(item => new { Key = item.FirstOrDefault(), Asc = !descFun(item) })
                .ToList();

            if (parameters.Count == 0)
            {
                throw new ArgumentNullException("orderByString");
            }

            var firstP = parameters.FirstOrDefault();
            var orderQuery = source.OrderBy(firstP.Key, firstP.Asc);
            parameters.Skip(1).ToList().ForEach(p => orderQuery = orderQuery.ThenBy(p.Key, p.Asc));

            return orderQuery;
        }
    }
}
