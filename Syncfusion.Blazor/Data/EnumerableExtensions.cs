using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Syncfusion.Blazor.Data;

namespace Syncfusion.Blazor.Data
{
    /// <summary>
    /// Provides enumerable extension.
    /// </summary>
    /// <exclude/>
    public static class EnumerableExtensions
    {
        internal static IEnumerable InvokeParallel(this IEnumerable source, Predicate<object> func, Type sourceType)
        {
            if (sourceType == null)
            {
                sourceType = source.GetElementType();
            }

            var genericWrapper = typeof(EnumerableExtensions).GetMethods().FirstOrDefault(m => m.Name == "InvokeParallelExecution" && m.IsStatic && m.IsGenericMethod);
            genericWrapper = genericWrapper.MakeGenericMethod(sourceType);
            return genericWrapper.Invoke(null, new object[] { source, func }) as ParallelQuery;
        }

        internal static IEnumerable InvokeParallel(this IEnumerable source, Predicate<object> func)
        {
            var sourceType = source.GetElementType();
            return source.InvokeParallel(func, sourceType);
        }

        public static ParallelQuery InvokeParallelExecution<T>(IEnumerable source, Predicate<object> func)
        {
            var enumerable = source as IEnumerable<T>;
            return enumerable.AsParallel().AsOrdered().Where(rec =>
            {
                return func(rec);
            });
        }

        #region Average

        public static double Average<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, short>> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }
#if EJ2_DNX
            return source.Provider.Execute<Int16>(Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(TSource) }), new Expression[] { source.Expression, Expression.Quote(selector) }));
#else
            return source.Provider.Execute<short>(Expression.Call(null, ((MethodInfo)MethodBase.GetMethodFromHandle(new RuntimeMethodHandle())).MakeGenericMethod(new Type[] { typeof(TSource) }), new Expression[] { source.Expression, Expression.Quote(selector) }));
#endif
        }

        public static double Average<TSource>(this IEnumerable<TSource> source, Func<TSource, short> selector)
        {
            return source.Select<TSource, short>(selector).Average();
        }

        public static double Average(this IEnumerable<short> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            long num = 0L;
            long num2 = 0L;
            foreach (int num3 in source)
            {
                num += num3;
                num2 += 1L;
            }

            if (num2 <= 0L)
            {
                throw new InvalidOperationException("Not enough elements");
            }

            return ((double)num) / ((double)num2);
        }

        public static double? Average<TSource>(
            this IQueryable<TSource> source,
            Expression<Func<TSource, short?>> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }
#if EJ2_DNX
            return source.Provider.Execute<Int16?>(Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(TSource) }), new Expression[] { source.Expression, Expression.Quote(selector) }));
#else
            return source.Provider.Execute<short?>(Expression.Call(null, ((MethodInfo)MethodBase.GetMethodFromHandle(new RuntimeMethodHandle())).MakeGenericMethod(new Type[] { typeof(TSource) }), new Expression[] { source.Expression, Expression.Quote(selector) }));
#endif
        }

        public static double? Average<TSource>(this IEnumerable<TSource> source, Func<TSource, short?> selector)
        {
            return source.Select<TSource, short?>(selector).Average();
        }

        public static double? Average(this IEnumerable<short?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            long num = 0L;
            long num2 = 0L;
            foreach (short? nullable in source)
            {
                if (nullable.HasValue)
                {
                    num += (long)nullable.GetValueOrDefault();
                    num2 += 1L;
                }
            }

            if (num2 > 0L)
            {
                return new double?(((double)num) / ((double)num2));
            }

            return null;
        }

        #endregion

        #region Sum

        public static short Sum<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, short>> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }
#if EJ2_DNX
            return source.Provider.Execute<Int16>(Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(TSource) }), new Expression[] { source.Expression, Expression.Quote(selector) }));
#else
            return source.Provider.Execute<short>(Expression.Call(null, ((MethodInfo)MethodBase.GetMethodFromHandle(new RuntimeMethodHandle())).MakeGenericMethod(new Type[] { typeof(TSource) }), new Expression[] { source.Expression, Expression.Quote(selector) }));
#endif
        }

        public static short Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, short> selector)
        {
            return source.Select<TSource, short>(selector).Sum();
        }

        public static short Sum(this IEnumerable<short> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));;
            }

            short num = 0;
            foreach (var num2 in source)
            {
                num += num2;
            }

            return num;
        }

        public static short? Sum<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, short?>> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));;
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }
#if EJ2_DNX
            return source.Provider.Execute<Int16?>(Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(TSource) }), new Expression[] { source.Expression, Expression.Quote(selector) }));
#else
            return source.Provider.Execute<short?>(Expression.Call(null, ((MethodInfo)MethodBase.GetMethodFromHandle(new RuntimeMethodHandle())).MakeGenericMethod(new Type[] { typeof(TSource) }), new Expression[] { source.Expression, Expression.Quote(selector) }));
#endif
        }

        public static short? Sum<TSource>(this IEnumerable<TSource> source, Func<TSource, short?> selector)
        {
            return source.Select<TSource, short?>(selector).Sum();
        }

        public static short? Sum(this IEnumerable<short?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            short num = 0;
            foreach (var nullable in source)
            {
                if (nullable.HasValue)
                {
                    num += nullable.GetValueOrDefault();
                }
            }

            return new short?(num);
        }

        #endregion

        #region Max

        public static short Max<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, short>> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }
#if EJ2_DNX
            return source.Provider.Execute<Int16>(Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(TSource) }), new Expression[] { source.Expression, Expression.Quote(selector) }));
#else
            return source.Provider.Execute<short>(Expression.Call(null, ((MethodInfo)MethodBase.GetMethodFromHandle(new RuntimeMethodHandle())).MakeGenericMethod(new Type[] { typeof(TSource) }), new Expression[] { source.Expression, Expression.Quote(selector) }));
#endif
        }

        public static short Max<TSource>(this IEnumerable<TSource> source, Func<TSource, short> selector)
        {
            return source.Select<TSource, short>(selector).Max();
        }

        public static short Max(this IEnumerable<short> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            short num = 0;
            bool flag = false;
            foreach (var num2 in source)
            {
                if (flag)
                {
                    if (num2 > num)
                    {
                        num = num2;
                    }
                }
                else
                {
                    num = num2;
                    flag = true;
                }
            }

            if (!flag)
            {
                throw new InvalidOperationException("Not enough elements");
            }

            return num;
        }

        public static short? Max<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, short?>> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }
#if EJ2_DNX
            return source.Provider.Execute<Int16?>(Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(TSource) }), new Expression[] { source.Expression, Expression.Quote(selector) }));
#else
            return source.Provider.Execute<short?>(Expression.Call(null, ((MethodInfo)MethodBase.GetMethodFromHandle(new RuntimeMethodHandle())).MakeGenericMethod(new Type[] { typeof(TSource) }), new Expression[] { source.Expression, Expression.Quote(selector) }));
#endif
        }

        public static short? Max<TSource>(this IEnumerable<TSource> source, Func<TSource, short?> selector)
        {
            return source.Select<TSource, short?>(selector).Max();
        }

        public static short? Max(this IEnumerable<short?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            short? nullable = null;
            foreach (var nullable2 in source)
            {
                if (nullable.HasValue)
                {
                    var nullable3 = nullable2;
                    var nullable4 = nullable;
                    if ((nullable3.GetValueOrDefault() <= nullable4.GetValueOrDefault()) ||
                        !(nullable3.HasValue & nullable4.HasValue))
                    {
                        continue;
                    }
                }

                nullable = nullable2;
            }

            return nullable;
        }

        #endregion

        #region Min

        public static short Min<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, short>> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }
#if EJ2_DNX
            return source.Provider.Execute<Int16>(Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(TSource) }), new Expression[] { source.Expression, Expression.Quote(selector) }));
#else
            return source.Provider.Execute<short>(Expression.Call(null, ((MethodInfo)MethodBase.GetMethodFromHandle(new RuntimeMethodHandle())).MakeGenericMethod(new Type[] { typeof(TSource) }), new Expression[] { source.Expression, Expression.Quote(selector) }));
#endif
        }

        public static short Min<TSource>(this IEnumerable<TSource> source, Func<TSource, short> selector)
        {
            return source.Select<TSource, short>(selector).Min();
        }

        public static short Min(this IEnumerable<short> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            short num = 0;
            bool flag = false;
            foreach (var num2 in source)
            {
                if (flag)
                {
                    if (num2 < num)
                    {
                        num = num2;
                    }
                }
                else
                {
                    num = num2;
                    flag = true;
                }
            }

            if (!flag)
            {
                throw new InvalidOperationException("Not enough elements");
            }

            return num;
        }

        public static short? Min<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, short?>> selector)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (selector == null)
            {
                throw new ArgumentNullException(nameof(selector));
            }
#if EJ2_DNX
            return source.Provider.Execute<Int16?>(Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod(new Type[] { typeof(TSource) }), new Expression[] { source.Expression, Expression.Quote(selector) }));
#else
            return source.Provider.Execute<short?>(Expression.Call(null, ((MethodInfo)MethodBase.GetMethodFromHandle(new RuntimeMethodHandle())).MakeGenericMethod(new Type[] { typeof(TSource) }), new Expression[] { source.Expression, Expression.Quote(selector) }));
#endif
        }

        public static short? Min<TSource>(this IEnumerable<TSource> source, Func<TSource, short?> selector)
        {
            return source.Select<TSource, short?>(selector).Min();
        }

        public static short? Min(this IEnumerable<short?> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            short? nullable = null;
            foreach (var nullable2 in source)
            {
                if (nullable.HasValue)
                {
                    long? nullable3 = nullable2;
                    long? nullable4 = nullable;
                    if ((nullable3.GetValueOrDefault() >= nullable4.GetValueOrDefault()) ||
                        !(nullable3.HasValue & nullable4.HasValue))
                    {
                        continue;
                    }
                }

                nullable = nullable2;
            }

            return nullable;
        }

        #endregion

        #region Sorting

        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> entities, string propertyName, Func<string, object, object> GetFunc)
        {
            if (!entities.Any() || string.IsNullOrEmpty(propertyName))
            {
                return entities;
            }

            if (GetFunc != null)
            {
                return entities.OrderBy(e => GetFunc(propertyName, e));
            }
            else
            {
                var propertyInfo = entities.First().GetType().GetProperty(propertyName);
                return entities.OrderBy(e => propertyInfo.GetValue(e, null));
            }
        }

        public static IEnumerable<T> OrderByDescending<T>(this IEnumerable<T> entities, string propertyName, Func<string, object, object> GetFunc)
        {
            if (!entities.Any() || string.IsNullOrEmpty(propertyName))
            {
                return entities;
            }

            if (GetFunc != null)
            {
                return entities.OrderByDescending(e => GetFunc(propertyName, e));
            }
            else
            {
                var propertyInfo = entities.First().GetType().GetProperty(propertyName);
                return entities.OrderByDescending(e => propertyInfo.GetValue(e, null));
            }
        }

        public static IEnumerable<T> ThenBy<T>(this IOrderedEnumerable<T> entities, string propertyName, Func<string, object, object> GetFunc)
        {
            if (!entities.Any() || string.IsNullOrEmpty(propertyName))
            {
                return entities;
            }

            if (GetFunc != null)
            {
                return entities.ThenBy(e => GetFunc(propertyName, e));
            }
            else
            {
                var propertyInfo = entities.First().GetType().GetProperty(propertyName);
                return entities.ThenBy(e => propertyInfo.GetValue(e, null));
            }
        }

        public static IEnumerable<T> ThenByDescending<T>(this IOrderedEnumerable<T> entities, string propertyName, Func<string, object, object> GetFunc)
        {
            if (!entities.Any() || string.IsNullOrEmpty(propertyName))
            {
                return entities;
            }

            if (GetFunc != null)
            {
                return entities.ThenByDescending(e => GetFunc(propertyName, e));
            }
            else
            {
                var propertyInfo = entities.First().GetType().GetProperty(propertyName);
                return entities.ThenByDescending(e => propertyInfo.GetValue(e, null));
            }
        }

        public static IEnumerable<T> OrderBy<T>(this IEnumerable<T> entities, string propertyName, Func<string, object, object> GetFunc, IComparer<object> comparer)
        {
            if (!entities.Any() || string.IsNullOrEmpty(propertyName))
            {
                return entities;
            }

            if (GetFunc != null)
            {
                return entities.OrderBy(e => GetFunc(propertyName, e), comparer);
            }
            else
            {
                var propertyInfo = entities.First().GetType().GetProperty(propertyName);
                return entities.OrderBy(e => propertyInfo.GetValue(e, null), comparer);
            }
        }

        public static IEnumerable<T> OrderByDescending<T>(this IEnumerable<T> entities, string propertyName, Func<string, object, object> GetFunc, IComparer<object> comparer)
        {
            if (!entities.Any() || string.IsNullOrEmpty(propertyName))
            {
                return entities;
            }

            if (GetFunc != null)
            {
                return entities.OrderByDescending(e => GetFunc(propertyName, e), comparer);
            }
            else
            {
                var propertyInfo = entities.First().GetType().GetProperty(propertyName);
                return entities.OrderByDescending(e => propertyInfo.GetValue(e, null), comparer);
            }
        }

        public static IEnumerable<T> ThenBy<T>(this IOrderedEnumerable<T> entities, string propertyName, Func<string, object, object> GetFunc, IComparer<object> comparer)
        {
            if (!entities.Any() || string.IsNullOrEmpty(propertyName))
            {
                return entities;
            }

            if (GetFunc != null)
            {
                return entities.ThenBy(e => GetFunc(propertyName, e), comparer);
            }
            else
            {
                var propertyInfo = entities.First().GetType().GetProperty(propertyName);
                return entities.ThenBy(e => propertyInfo.GetValue(e, null), comparer);
            }
        }

        public static IEnumerable<T> ThenByDescending<T>(this IOrderedEnumerable<T> entities, string propertyName, Func<string, object, object> GetFunc, IComparer<object> comparer)
        {
            if (!entities.Any() || string.IsNullOrEmpty(propertyName))
            {
                return entities;
            }

            if (GetFunc != null)
            {
                return entities.ThenByDescending(e => GetFunc(propertyName, e), comparer);
            }
            else
            {
                var propertyInfo = entities.First().GetType().GetProperty(propertyName);
                return entities.ThenByDescending(e => propertyInfo.GetValue(e, null), comparer);
            }
        }

        #endregion

        public static ParallelQuery<T> GetParallelQueryFor<T>(IEnumerable source)
        {
            var enumerable = source as IEnumerable<T>;
            return enumerable.AsParallel();
        }

        public static ParallelQuery GetParallelQuery(this IEnumerable source, Type sourceType = null)
        {
            if (sourceType == null)
            {
                sourceType = source.GetElementType();
            }

            var genericWrapper = typeof(EnumerableExtensions).GetMethods().FirstOrDefault(m => m.Name == "GetParallelQueryFor" && m.IsStatic && m.IsGenericMethod);
            genericWrapper = genericWrapper.MakeGenericMethod(sourceType);
            var parallelQuery = (ParallelQuery)genericWrapper.Invoke(null, new object[] { source });
            return parallelQuery;
        }

        public static Type GetElementType(this IEnumerable source)
        {
            return GetElementTypeByRepresentativeItem(source, false);
        }

        internal static Type GetElementTypeByRepresentativeItem(this IEnumerable source, bool useRepresentativeItem)
        {
            var list = source;

            // var prop = list.GetType().GetProperty("Item");
            var prop = list.GetItemPropertyInfo();
            return prop != null ? prop.PropertyType : GetItemType(source, useRepresentativeItem);
        }

        public static PropertyInfo GetItemPropertyInfo(this IEnumerable list)
        {
            var prop = list?.GetType().GetProperties().Where(p => p.Name.Equals("Item", StringComparison.Ordinal));
            if (prop.Count() > 1)
            {
                return prop.FirstOrDefault(p =>
                {
                    ParameterInfo[] para = p.GetGetMethod().GetParameters();
                    if (para.Any())
                    {
                        return para[0].ParameterType == typeof(int);
                    }

                    return false;
                });
            }
            else
            {
                return list.GetType().GetProperty("Item");
            }
        }

        internal static Type GetElementType(this IEnumerable source, ref bool isEmpty, object firstItem = null)
        {
            if (firstItem == null)
            {
                firstItem = GetRepresentativeItem(source);
            }

            if (firstItem == null)
            {
                isEmpty = true;
            }
            else
            {
                isEmpty = false;
            }

            if (isEmpty)
            {
                return GetElementTypeByRepresentativeItem(source, true);
            }

            var castType = GetGenericSourceType(source);
            if (castType != null)
            {
                return castType;
            }

            var firstItemType = firstItem.GetType();
            if (firstItemType != null && (!string.IsNullOrEmpty(firstItemType.AssemblyQualifiedName) &&
                firstItemType.AssemblyQualifiedName.Contains("System.Data.Entity.DynamicProxies", StringComparison.Ordinal)))
            {
                return GetElementType(source);
            }

            return firstItemType;
        }

        public static Type GetGenericSourceType(IEnumerable source)
        {
            var type = source?.GetType();
            return GetBaseGenericInterfaceType(type, false);
        }

        private static Type GetBaseGenericInterfaceType(Type type, bool canreturn)
        {
#if EJ2_DNX
            if (type.IsGenericType)
#else
            if (type.GetTypeInfo().IsGenericType)
#endif
            {
                Type[] genericArguments = type.GetGenericArguments();
                if (genericArguments.Length == 1)
                {
#if EJ2_DNX
                    if (genericArguments[0].IsInterface || genericArguments[0].IsAbstract)
#else
                    if (genericArguments[0].GetTypeInfo().IsInterface || genericArguments[0].GetTypeInfo().IsAbstract)
#endif
                    {
                        return genericArguments[0];
                    }

                    if (canreturn)
                    {
                        return genericArguments[0];
                    }
                }
            }
#if EJ2_DNX
            else if (type.BaseType != null)
#else
            else if (type.GetTypeInfo().BaseType != null)
#endif
            {
#if EJ2_DNX
                return GetBaseGenericInterfaceType(type.BaseType, canreturn);
#else
                return GetBaseGenericInterfaceType(type.GetTypeInfo().BaseType, canreturn);
#endif
            }

            return null;
        }

        public static Type GetItemType(this IEnumerable source, bool useRepresentativeItem)
        {
            var type = source?.GetType();
#if EJ2_DNX
            if (type.IsGenericType)
#else
            if (type.GetTypeInfo().IsGenericType)
#endif
            {
                var generictype = GetBaseGenericInterfaceType(type, true);
#if EJ2_DNX
                if (generictype == null || generictype.IsInterface || generictype.IsAbstract)
#else
                if (generictype == null || generictype.GetTypeInfo().IsInterface || generictype.GetTypeInfo().IsAbstract)
#endif
                {
                    if (useRepresentativeItem)
                    {
                        var representativeItem = GetRepresentativeItem(source);
                        if (representativeItem != null)
                        {
                            return representativeItem.GetType();
                        }
                    }
                }

                return generictype;
            }
            else if (useRepresentativeItem)
            {
                var representativeItem = GetRepresentativeItem(source);
                if (representativeItem != null)
                {
                    return representativeItem.GetType();
                }
#if !EJ2_DNX
                else if (type.GetTypeInfo().BaseType != null && type.GetTypeInfo().BaseType.GetTypeInfo().IsGenericType)
                {
                    return type.GetTypeInfo().BaseType.GetGenericArguments()[0];
                }
#else
                else if (type.BaseType != null && type.BaseType.IsGenericType)
                    return type.BaseType.GetGenericArguments()[0];
#endif
            }

            return null;
        }

        private static object GetRepresentativeItem(IEnumerable source)
        {
            var enumerator = source.GetEnumerator();
            if (enumerator.MoveNext())
            {
                return enumerator.Current;
            }

            return null;
        }

        public static int IndexOf<T>(this IEnumerable<T> source, T value)
        {
            if (source == null) { throw new ArgumentNullException(nameof(source)); }
            int index = 0;
            var comparer = EqualityComparer<T>.Default;
            foreach (T item in source)
            {
                if (comparer.Equals(item, value))
                {
                    return index;
                }

                index++;
            }

            return -1;
        }
    }
}
