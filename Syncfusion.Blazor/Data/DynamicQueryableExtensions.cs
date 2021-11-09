namespace Syncfusion.Blazor.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Globalization;
    using System.Dynamic;
    using Syncfusion.Blazor.Data;
    using Syncfusion.Blazor.Internal;
#if EJ2_DNX
    using System.Data;
#endif

    /// <summary>
    /// Provides extension methods for Queryable source.
    /// <para></para>
    /// <para></para>
    /// <para>var fonts = FontFamily.Families.AsQueryable();. </para>
    /// <para></para>
    /// <para></para>
    /// <para>We would normally write Expressions as,. </para>
    /// <para></para>
    /// <code lang="C#">var names = new string[] {&quot;Tony&quot;, &quot;Al&quot;,
    /// &quot;Sean&quot;, &quot;Elia&quot;}.AsQueryable();
    /// names.OrderBy(n=&gt;n);</code>
    /// <para></para>
    /// <para></para>
    /// <para>This would sort the names based on alphabetical order. Like so, the
    /// Queryable extensions are a set of extension methods that define functions which
    /// will generate expressions based on the supplied values to the functions.</para>
    /// </summary>
    /// <exclude/>
    public static class DynamicQueryableExtensions
    {
        /// <summary>
        /// Predicate is a Binary expression that needs to be built for a single or a series
        /// of values that needs to be passed on to the WHERE expression.
        /// <para></para>
        /// <para></para>
        /// <code lang="C#">var binaryExp = queryable.Predicate(parameter,
        /// &quot;EmployeeID&quot;, &quot;4&quot;, true);</code>
        /// </summary>
        /// <remarks>
        /// First create a ParameterExpression using the Parameter extension function, then
        /// use the same ParameterExpression to generate the predicates.
        /// </remarks>
        /// <param name="source">Data source.</param>
        /// <param name="paramExpression">Parameter expression to merge.</param>
        /// <param name="propertyName">Property name to be filtered.</param>
        /// <param name="constValue">Const value.</param>
        /// <param name="filterType">Filter operator type.</param>
        /// <param name="filterBehaviour">Specifies the filter behavior.</param>
        /// <param name="isCaseSensitive">Performs the case sensitive if true.</param>
        /// <param name="sourceType">Specifies the data source element type.</param>
        /// <param name="columnType">Specifies the current field type.</param>
        public static Expression Predicate(this IQueryable source, ParameterExpression paramExpression,
                                           string propertyName, object constValue, FilterType filterType,
                                           FilterBehavior filterBehaviour, bool isCaseSensitive, Type sourceType, Type columnType = null) // Predicate1
        {
            if (sourceType == null) { throw new ArgumentNullException(nameof(sourceType)); }
            if (propertyName == null) { throw new ArgumentNullException(nameof(propertyName)); }
            return Predicate(source, constValue, filterType, filterBehaviour, isCaseSensitive, sourceType, null, null, paramExpression, propertyName, columnType);
        }

        private static ValueTuple<Expression, Expression> GetExpression(FilterType filterType,
            Type memberType, object value, Expression memExp,
            bool isCaseSensitive)
        {
            var nullablememberType = NullableHelperInternal.GetNullableType(memberType);
            Expression bExp = null;
            switch (filterType)
            {
                case FilterType.Equals:
                    if (isCaseSensitive || memberType != typeof(string))
                    {
                        if (value != null)
                        {
                            var exp = Expression.Constant(value, memberType);
#if !EJ2_DNX
                            if ((nullablememberType == memberType && memberType != typeof(object)) || memberType.GetTypeInfo().IsEnum)
#else
                                 if ((nullablememberType == memberType && memberType != typeof(object)) || memberType.IsEnum)
#endif
                            {
                                memExp = Expression.Convert(memExp, nullablememberType);
                                bExp = Expression.Equal(memExp, Expression.Constant(value, nullablememberType));
                            }
                            else
                            {
                                bExp = Expression.Call(exp, exp.Type.GetMethod("Equals", new[] { memExp.Type }), memExp);
                            }
                        }
                        else
                        {
                            memExp = Expression.Convert(memExp, nullablememberType);
                            bExp = Expression.Equal(memExp, Expression.Constant(value, nullablememberType));
                            // bExp = Expression.Call(exp, nullablememberType.GetMethod("Equals", new[] { nullablememberType }), Expression.Constant(memExp));
                        }
                    }
                    else
                    {
                        memExp = Expression.Coalesce(memExp, Expression.Constant(value == null ? "blanks" : string.Empty));
                        var toLowerMethodCall = memExp.ToLowerMethodCallExpression();
                        bExp = Expression.Equal(toLowerMethodCall,
                                                Expression.Constant(
                                                    value == null ? "blanks" : value.ToString().ToLowerInvariant(),
                                                    typeof(string)));
                    }

                    break;
                case FilterType.NotEquals:
                    if (isCaseSensitive || memberType != typeof(string))
                    {
                        if (value != null)
                        {
                            memExp = Expression.Convert(memExp, nullablememberType);
                            bExp = Expression.NotEqual(memExp, Expression.Constant(value, nullablememberType));
                        }
                        else
                        {
                            memExp = Expression.Convert(memExp, nullablememberType);
                            bExp = Expression.NotEqual(memExp, Expression.Constant(value, nullablememberType));
                        }
                    }
                    else
                    {
                        memExp = Expression.Coalesce(memExp, Expression.Constant(value == null ? "blanks" : string.Empty));
                        var toLowerMethodCall = memExp.ToLowerMethodCallExpression();
                        bExp = Expression.NotEqual(toLowerMethodCall,
                                                   Expression.Constant(
                                                       value == null ? "blanks" : value.ToString().ToLowerInvariant(),
                                                       memberType));
                    }

                    break;
                case FilterType.LessThan:

                    memExp = Expression.Convert(memExp, nullablememberType);
                    bExp = Expression.LessThan(memExp, Expression.Constant(value, nullablememberType));

                    break;
                case FilterType.LessThanOrEqual:

                    memExp = Expression.Convert(memExp, nullablememberType);
                    bExp = Expression.LessThanOrEqual(memExp, Expression.Constant(value, nullablememberType));

                    break;
                case FilterType.GreaterThan:

                    memExp = Expression.Convert(memExp, nullablememberType);
                    bExp = Expression.GreaterThan(memExp, Expression.Constant(value, nullablememberType));

                    break;
                case FilterType.GreaterThanOrEqual:

                    memExp = Expression.Convert(memExp, nullablememberType);
                    bExp = Expression.GreaterThanOrEqual(memExp, Expression.Constant(value, nullablememberType));

                    break;
            }
            return (memExp, bExp);
        }

        private static Expression Predicate(this IQueryable source, object constValue, FilterType filterType,
                                           FilterBehavior filterBehaviour, bool isCaseSensitive, Type sourceType, Type memberType, Expression memExp, ParameterExpression paramExpression, string propertyName, Type columnType = null)
        {
            string[] propertyNameList = null; _ = filterBehaviour;
            int propCount = 1;
            if (memExp == null)
            {
                if (sourceType.IsSubclassOf(typeof(DynamicObject)))
                {
                    Expression param = Expression.Convert(paramExpression, typeof(DynamicObject));
                    MethodInfo methodName = typeof(DataUtil).GetMethod("GetDynamicValue");
                    memExp = Expression.Call(methodName, param, Expression.Constant(propertyName));
                    memberType = columnType;
                    propertyNameList = propertyName.Split('.');
                    propCount = propertyNameList.Length;
                    if (propCount > 1)
                    {
                        memExp = null;
                        for (int i = 0; i < propCount; i++)
                        {
                            if (memExp == null)
                            {
                                memExp = Expression.Call(methodName, param, Expression.Constant(propertyNameList[i]));
                            }
                            else
                            {
                                param = Expression.Convert(memExp, typeof(DynamicObject));
                                memExp = Expression.Call(methodName, param, Expression.Constant(propertyNameList[i]));
                            }
                        }
                    }
                }
                else
                {
                    Expression param = Expression.Convert(paramExpression, typeof(IDictionary<string, object>));
                    memExp = Expression.Property(param, "Item", new Expression[] { Expression.Constant(propertyName) });
                    memberType = columnType == null ? memExp.Type : columnType;
                    propertyNameList = propertyName.Split('.');
                    propCount = propertyNameList.Length;
                     if (propCount > 1) {
                        memExp = null;
                        for (int i = 0; i < propCount; i++)
                        {
                            if (memExp == null)
                            {
                                memExp = Expression.Property(param, "Item", new Expression[] { Expression.Constant(propertyNameList[i]) });
                            }
                            else
                            {
                                param = Expression.Convert(memExp, typeof(IDictionary<string, object>));
                                memExp = Expression.Property(param, "Item", new Expression[] { Expression.Constant(propertyNameList[i]) });
                            }
                        }
                    }
                }
            }

            var value = constValue;
            Expression bExp = null;
            //Func<FilterType, bool> hasFilterType = (FilterType filterType)
            //    => (filterType == FilterType.Equals || filterType == FilterType.NotEquals ||
            //     filterType == FilterType.LessThan || filterType == FilterType.LessThanOrEqual ||
            //     filterType == FilterType.GreaterThan || filterType == FilterType.GreaterThanOrEqual);
            if ((filterType == FilterType.Equals || filterType == FilterType.NotEquals ||
                 filterType == FilterType.LessThan || filterType == FilterType.LessThanOrEqual ||
                 filterType == FilterType.GreaterThan || filterType == FilterType.GreaterThanOrEqual))
            {
                var underlyingType = memberType;
                if (NullableHelperInternal.IsNullableType(memberType))
                {
                    underlyingType = NullableHelperInternal.GetUnderlyingType(memberType);
                }

                if (value != null)
                {
                    bool isJsonElement = value.GetType().Name == "JsonElement";
                    value = isJsonElement ? SfBaseUtils.ChangeType(value, underlyingType) : ValueConvert.ChangeType(value, underlyingType, CultureInfo.CurrentCulture);
                }

                ValueTuple<Expression, Expression> v = GetExpression(filterType, memberType, value, memExp, isCaseSensitive);
                memExp = v.Item1; bExp = v.Item2;
            }
            else
            {
                if (!isCaseSensitive && (filterType == FilterType.Equals || filterType == FilterType.NotEquals))
                {
                    value = NullableHelperInternal.FixDbNUllasNull(constValue, memberType);
                }

                ValueTuple<Expression, Expression> v = GetPExpression(filterType, isCaseSensitive, value, memExp, memberType);
                memExp = v.Item1; 
                bExp = v.Item2 ?? bExp;
            }

            return bExp;
        }

        private static ValueTuple<Expression, Expression> GetPExpression(FilterType filterType, 
            bool isCaseSensitive, object value, Expression memExp, Type memberType)
        {
            Expression bExp = null;
            var toString = memExp.Type.GetMethods().FirstOrDefault(d => d.Name == "ToString");
            // if (NullableHelperInternal.IsNullableType(memberType) || memberType == typeof(string))
            string stringValue = string.Empty;
            if (NullableHelperInternal.IsNullableType(memberType) || memberType == typeof(string))
            {
                memExp = Expression.Coalesce(memExp, Expression.Constant("Blanks"));
                stringValue = value == null ? "Blanks" : value.ToString();
            }
            else
            {
                stringValue = value == null ? "" : value.ToString();
            }

            memExp = Expression.Call(memExp, toString);
            switch (filterType)
            {
                case FilterType.NotEquals:
                    if (!isCaseSensitive)
                    {
                        memExp = ToLowerMethodCallExpression(memExp);
                        bExp = Expression.NotEqual(memExp,
                                                    Expression.Constant(stringValue.ToLowerInvariant(), typeof(string)));
                    }
                    else
                    {
                        bExp = Expression.NotEqual(memExp, Expression.Constant(stringValue, typeof(string)));
                    }

                    break;
                case FilterType.Equals:
                case FilterType.StartsWith:
                case FilterType.Contains:
                case FilterType.EndsWith:
                    var stringMethod = typeof(string).GetMethod(filterType.ToString(), new[] { memExp.Type });
                    if (isCaseSensitive)
                    {
                        bExp = Expression.Call(memExp, stringMethod, new Expression[] { Expression.Constant(stringValue, typeof(string)) });
                    }
                    else
                    {
                        var toLowerMethod = ToLowerMethodCallExpression(memExp);
                        bExp = Expression.Call(toLowerMethod, stringMethod,
                                    new Expression[]
                                    {
                                            Expression.Constant(stringValue.ToLowerInvariant(), typeof (string))
                                    });
                    }

                    break;
            }

            return (memExp, bExp);
        } 

        private static MethodCallExpression ToLowerMethodCallExpression(this Expression memExp)
        {
            var tolowerMethod = typeof(string).GetMethods().FirstOrDefault(m => m.Name == "ToLower");
            if (memExp.Type == typeof(object))
            {
                memExp = Expression.Convert(memExp, typeof(string));
            }

            var toLowerMethodCall = Expression.Call(memExp, tolowerMethod, Array.Empty<Expression>());
            return toLowerMethodCall;
        }
    }
}