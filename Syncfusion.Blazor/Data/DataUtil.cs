using System;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;
using System.Globalization;
using System.Linq;
using System.Dynamic;
using System.Text.RegularExpressions;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.Data
{
    /// <summary>
    /// Provides utility method used by data manager.
    /// </summary>
    public static class DataUtil
    {
        /// <summary>
        /// Resolves the given base url and relative url to generate absolute url. And merge query string if any.
        /// </summary>
        /// <param name="baseUrl">Base address url.</param>
        /// <param name="relativeUrl">Relative url.</param>
        /// <param name="queryParams">Query string.</param>
        /// <returns>string - absolute url.</returns>
        public static string GetUrl(string baseUrl, string relativeUrl, string queryParams = null)
        {
            var bHasSlash = !string.IsNullOrEmpty(baseUrl) && baseUrl[baseUrl.Length - 1] == '/';
            string url = baseUrl;
            var queryString = string.Empty;

            if (!string.IsNullOrEmpty(relativeUrl))
            {
                var rHasSlash = !string.IsNullOrEmpty(relativeUrl) && relativeUrl[0] == '/';
                if (bHasSlash ^ rHasSlash)
                {
                    url = $"{baseUrl}{relativeUrl}";
                }
                else if (!bHasSlash && !rHasSlash)
                {
                    url = $"{baseUrl}/{relativeUrl}";
                }
                else if (bHasSlash && rHasSlash)
                {
                    url = $"{baseUrl}{relativeUrl.Substring(1, relativeUrl.Length - 1)}";
                }
                else
                {
                    url = $"{baseUrl}{relativeUrl}";
                }
            }

            if (string.IsNullOrEmpty(queryParams))
            {
                return url;
            }

            // Query parameters process
            if (url[url.Length - 1] != '?' && url.IndexOf("?", StringComparison.Ordinal) > -1)
            {
                queryString = $"&{queryParams}";
            }
            else if (url.IndexOf("?", StringComparison.Ordinal) < 0 && !string.IsNullOrEmpty(queryParams))
            {
                queryString = $"?{queryParams}";
            }

            return url + queryString;
        }

        /// <summary>
        /// Gets the property value with the given key.
        /// </summary>
        /// <param name="key">Property name.</param>
        /// <param name="value">Source object.</param>
        /// <returns>string.</returns>
        public static string GetKeyValue(string key, object value)
        {
            PropertyInfo propInfo = value?.GetType().GetProperty(key);
            Type propType = propInfo.PropertyType;

            // Check nullable types.
            if (propType.IsGenericType && propType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                propType = NullableHelperInternal.GetUnderlyingType(propType);
            }

            object propVal = propInfo.GetValue(value);
            object returnVal = propVal;

            if (propType.Name == "DateTime")
            {
                returnVal = ((DateTime)propVal).ToString("s", CultureInfo.InvariantCulture);
            }

            return returnVal?.ToString();
        }

        /// <summary>
        /// Converts dictionary of key/value pair to query string.
        /// </summary>
        /// <param name="Params">Input dictionary value.</param>
        /// <returns>string - Query string.</returns>
        public static string ToQueryParams(IDictionary<string, object> Params)
        {
            string[] sb = new string[Params != null ? Params.Count : 0];
            var i = 0;
            foreach (var param in Params ?? new Dictionary<string, object>())
            {
                if (param.Value != null)
                {
                    sb[i++] = $"{param.Key}={param.Value.ToString()}";
                }
            }

            return string.Join("&", sb);
        }

        /// <summary>
        /// Converts dictionary of key/value pair to query string.
        /// </summary>
        /// <param name="dataSource">Collection of Data source.</param>
        /// <param name="propertyName">property name which is need to distincts </param>.
        /// <returns>IEnumerable Distinct collections</returns>
        internal static IEnumerable<T> GetDistinct<T>(IEnumerable<T> dataSource, string propertyName)
        {
            List<T> DistinctCollections = new List<T>();
            IDictionary<string, object> DistinctData = new Dictionary<string, object>();

            foreach (var value in dataSource)
            {
                if (value is ExpandoObject)
                {
                    var dictionaryValue = value as IDictionary<string, object>;
                    bool isComplex = propertyName.Split(".").Length > 1;
                    var complexNameSpace = isComplex ? propertyName.Split(".")[0] : propertyName;
                    if (dictionaryValue != null && !dictionaryValue.ContainsKey(complexNameSpace))
                    {
                        continue;
                    }
                }

                var propertyValue = GetObject(propertyName, value);
                string key = propertyValue == null ? "null" : propertyValue.ToString();
                if (!DistinctData.ContainsKey(key))
                {
                    DistinctData.Add(key, value);
                    DistinctCollections.Add(value);
                }
            }

            return DistinctCollections.AsEnumerable<T>();
        }

        /// <exclude/>
        public static int GetValue(int value, object inst)
        {
            _ = inst;
            return value;
        }

        internal static IDictionary<string, string> odUniOperator = new Dictionary<string, string>()
        {
            { "$=", "endswith" },
            { "^=", "startswith" },
            { "*=", "substringof" },
            { "endswith", "endswith" },
            { "startswith", "startswith" },
            { "contains", "substringof" }
        };

        internal static IDictionary<string, string> odBiOperator = new Dictionary<string, string>()
        {
            { "<", " lt " },
            { ">", " gt " },
            { "<=", " le " },
            { ">=", " ge " },
            { "==", " eq " },
            { "!=", " ne " },
            { "lessthan", " lt " },
            { "lessthanorequal", " le " },
            { "greaterthan", " gt " },
            { "greaterthanorequal", " ge " },
            { "equal", " eq " },
            { "notequal", " ne " }
        };

        internal static IDictionary<string, string> Odv4UniOperator = new Dictionary<string, string>()
        {
            { "$=", "endswith" },
            { "^=", "startswith" },
            { "*=", "contains" },
            { "endswith", "endswith" },
            { "startswith", "startswith" },
            { "contains", "contains" }
        };

        internal static IDictionary<string, string> consts = new Dictionary<string, string>()
            {
                { "GroupGuid", "{271bbba0-1ee7}" }
            };

        /// <summary>
        /// Groups the given data source with the field name.
        /// </summary>
        /// <typeparam name="T">Type of the data source elements.</typeparam>
        /// <param name="jsonArray">Input data source.</param>
        /// <param name="field">Specifies the group by field name.</param>
        /// <param name="aggregates">Aggregate details to aggregate grouped records.</param>
        /// <param name="level">Level of the group. For parent group it is 0.</param>
        /// <param name="format">Specifies the format and handler method to perform group by format.</param>
        /// <param name="isLazyLoad">Specifies the isLazyLoad property as true to handle lazy load grouping.</param>
        /// <param name="isLazyGroupExpandAll">Specifies the isLazyGroupExpandAll as true to perform expand all for lazy load grouping.</param>
        /// <returns>IEnumerable - Grouped record.</returns>
        public static IEnumerable Group<T>(IEnumerable jsonArray, string field, List<Aggregate> aggregates, int level, IDictionary<string, string> format, bool isLazyLoad = false, bool isLazyGroupExpandAll = false)
        {
            if (level == 0)
            {
                level = 1;
            }

            string guid = "GroupGuid";
            if (jsonArray?.GetType().GetProperty(guid) != null && (jsonArray as Group<T>).GroupGuid == consts[guid])
            {
                Group<T> json = (Group<T>)jsonArray;
                for (int j = 0; j < json.Count; j++)
                {
                    json[j].Items = (IEnumerable)Group<T>(json[j].Items, field, aggregates, level + 1,
                        format, isLazyLoad, isLazyGroupExpandAll);
                    json[j].CountItems = json[j].Items.Cast<object>().ToList().Count;
                }

                json.ChildLevels += 1;
                return json;
            }

            object[] jsonData = jsonArray.Cast<object>().ToArray();
            IDictionary<object, Group<T>> grouped = new Dictionary<object, Group<T>>();
            Group<T> groupedArray = new Group<T>() { GroupGuid = consts["GroupGuid"], Level = level, ChildLevels = 0, Records = jsonData };
            for (int i = 0; i < jsonData.Length; i++)
            {
                var val = GetGroupValue(field, jsonData[i]);
                if (format != null && format.ContainsKey(field) && format[field] != null && val != null)
                {
                    val = DataUtil.GetFormattedValue(val, format[field]);
                }

                if (val == null)
                {
                    val = "null";
                }

                if (!grouped.ContainsKey(val))
                {
                    grouped.Add(val, new Group<T>() { Key = val, CountItems = 0, Level = level, Items = new List<T>(), Aggregates = new object(), Field = field, GroupedData = new List<T>() });
                    groupedArray.Add(grouped[val]);
                }

                grouped[val].CountItems = grouped[val].CountItems += 1;
                if (!isLazyLoad || (isLazyLoad && aggregates != null))
                {
                    (grouped[val].Items as List<T>).Add((T)jsonData[i]);
                }
                if (isLazyLoad || isLazyGroupExpandAll)
                {
                    (grouped[val].GroupedData as List<T>).Add((T)jsonData[i]);
                }
            }

            if (aggregates != null && aggregates.Count > 0)
            {
                for (int i = 0; i < groupedArray.Count; i++)
                {
                    IDictionary<string, object> res = new Dictionary<string, object>();
                    Func<IEnumerable, string, string, object> fn;
                    var type = groupedArray[i].Items.Cast<object>().ToArray()[0].GetType();
                    groupedArray[i].Items = groupedArray[i].Items as List<T>;
                    for (int j = 0; j < aggregates.Count; j++)
                    {
                        fn = CalculateAggregateFunc();
                        if (fn != null)
                        {
                            res[(aggregates[j] as Aggregate).Field + " - " + (aggregates[j] as Aggregate).Type] = fn(groupedArray[i].Items, (aggregates[j] as Aggregate).Field, (aggregates[j] as Aggregate).Type);
                        }
                    }

                    groupedArray[i].Aggregates = res;
                }
            }

            return Result<T>(jsonData, isLazyLoad, aggregates, groupedArray);
        }

        public static IEnumerable Result<T>(object[] jsonData, bool isLazyLoad, List<Aggregate> aggregates, Group<T> groupedArray)
        {
            if (jsonData == null) { throw new ArgumentNullException(nameof(jsonData)); }
            if (groupedArray == null) { throw new ArgumentNullException(nameof(groupedArray)); }
            if (jsonData.Length > 0)
            {
                if (isLazyLoad && aggregates != null)
                {
                    for (int i = 0; i < groupedArray.Count; i++)
                    {
                        groupedArray[i].Items = new List<T>();
                    }
                }
                return groupedArray;
            }
            else
            {
                return jsonData;
            }
        }

        /// <summary>
        /// Performs aggregation on the given data source.
        /// </summary>
        /// <param name="jsonData">Input data source.</param>
        /// <param name="aggregates">List of aggregate to be calculated.</param>
        /// <returns>Dictionary of aggregate results.</returns>
        public static IDictionary<string, object> PerformAggregation(IEnumerable jsonData, List<Aggregate> aggregates)
        {
            IDictionary<string, object> res = new Dictionary<string, object>();
            Func<IEnumerable, string, string, object> fn;
            if (jsonData == null || !jsonData.Cast<object>().Any())
            {
                return res;
            }

            var type = jsonData.Cast<object>().ToArray()[0].GetType();
            IEnumerable ConvData = CastList(type, jsonData.Cast<object>().ToList());
            for (int j = 0; j < (aggregates?.Count ?? 0); j++)
            {
                fn = CalculateAggregateFunc();
                if (fn != null)
                {
                    res[(aggregates[j] as Aggregate).Field + " - " + (aggregates[j] as Aggregate).Type.ToLowerInvariant()] = fn(ConvData, (aggregates[j] as Aggregate).Field, (aggregates[j] as Aggregate).Type);
                }
            }

            return res;
        }

        internal static IEnumerable CastList(Type type, List<object> items)
        {
            var containedType = type;
            var enumerableType = typeof(System.Linq.Enumerable);
            var castMethod = enumerableType.GetMethod(nameof(System.Linq.Enumerable.Cast)).MakeGenericMethod(containedType);
            var toListMethod = enumerableType.GetMethod(nameof(System.Linq.Enumerable.ToList)).MakeGenericMethod(containedType);
            IEnumerable<object> itemsToCast = items as IEnumerable<object>;
            var castedItems = castMethod.Invoke(null, new[] { itemsToCast });
            return toListMethod.Invoke(null, new[] { castedItems }) as IEnumerable;
        }

        /// <summary>
        /// Gets the property value from list of object.
        /// </summary>
        /// <param name="jsonData">List of object.</param>
        /// <param name="index">Index of the item to be processed.</param>
        /// <param name="field">Property name to get value.</param>
        /// <returns>object.</returns>
        public static object GetVal(IEnumerable jsonData, int index, string field)
        {
            if(jsonData.Cast<object>().Any())
            {
                if (field != null)
                {
                    return GetObject(field, jsonData.Cast<object>().ToArray()[index]);
                }
                else
                {
                    return jsonData.Cast<object>().ToArray()[index];
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the property value from object.
        /// </summary>
        /// <param name="nameSpace">Property name to be accessed.</param>
        /// <param name="from">Source object.</param>
        /// <returns>object - property value.</returns>
        public static object GetGroupValue(string nameSpace, Object from)
        {
            if (nameSpace != null)
            {
                return GetObject(nameSpace, from);
            }
            else
            {
                return from;
            }
        }

        /// <summary>
        /// Gets the property value from object.
        /// </summary>
        /// <param name="nameSpace">Property name to be accessed.</param>
        /// <param name="from">Source object.</param>
        /// <returns>object - property value.</returns>
        /// <remarks>For accessing complex/nested property value, given the nameSpace with field names delimited by dot(.).</remarks>
        public static object GetObject(string nameSpace, object from)
        {
            if (string.IsNullOrEmpty(nameSpace))
            {
                return from;
            }

            if (from == null)
            {
                return null;
            }

            object value = from;
            string[] splits = nameSpace.Split('.');
            if (splits.Length > 0)
            {
                for (int i = 0; i < splits.Length; i++)
                {
                    if (value == null)
                    {
                        break;
                    }

                    if (from.GetType().GetField(nameSpace) != null)
                    {
                        return value.GetType().GetField(splits[i])?.GetValue(value);
                    }
                    else if (value is ExpandoObject)
                    {
                        value = GetExpandoValue(value as ExpandoObject, splits[i]);
                    }
                    else if (value is DynamicObject)
                    {
                        value = GetDynamicValue(value as DynamicObject, splits[i]);
                    }
                    else
                    {
                        value = value.GetType().GetProperty(splits[i])?.GetValue(value);
                    }
                }

                return value;
            }

            if (from is ExpandoObject)
            {
                return GetExpandoValue(from as ExpandoObject, nameSpace);
            }
            else if (from is DynamicObject)
            {
                return GetDynamicValue(from as DynamicObject, nameSpace);
            }

            if (!nameSpace.Contains(".", StringComparison.Ordinal))
            {
                if (from.GetType().GetField(nameSpace) != null)
                {
                    return from.GetType().GetField(nameSpace)?.GetValue(from);
                }
                else
                {
                    return from.GetType().GetProperty(nameSpace)?.GetValue(from);
                }
            }

            return value;
        }

        /// <summary>
        /// Returns enum column type.
        /// </summary>
        /// <exclude/>
        internal static Type GetEnumType(string fieldName, Type type)
        {
            string[] Fields = fieldName.Split(".");
            int Count = Fields.Length;
            bool IsComplex = Fields.Length > 1;
            Type EnumPropType = null;
            Type ComplexType = null;
            if (IsComplex)
            {
                for (int v = 0; v < (Count - 1); v++)
                {
                    if (ComplexType == null)
                    {
                        ComplexType = type.GetProperty(Fields[v]).PropertyType;
                    }
                    else
                    {
                        ComplexType = ComplexType.GetProperty(Fields[v]).PropertyType;
                    }
                }

                EnumPropType = ComplexType?.GetProperty(Fields[Count - 1]).PropertyType;
            }
            else
            {
                EnumPropType = type.GetProperty(fieldName).PropertyType;
            }

            return EnumPropType;
        }

        internal static Func<IEnumerable, string, string, object> CalculateAggregateFunc()
        {
            return (items, property, pd) =>
            {
                var aggregateType = pd;
                IQueryable queryable = items.AsQueryable();
                var isDynamicObjectType = items.Cast<object>().ToList()[0].GetType().BaseType == typeof(DynamicObject);
                var isExpandoObjectType = items.Cast<object>().ToList()[0].GetType() == typeof(ExpandoObject);
                if (isDynamicObjectType || isExpandoObjectType)
                {
                    IQueryable<IDynamicMetaObjectProvider> dt = items.Cast<IDynamicMetaObjectProvider>().AsQueryable();
                    switch (aggregateType)
                    {
                        case "Count":
                            return dt.Count();
                        case "Max":
                            if (isDynamicObjectType)
                            {
                                return dt.Max(item => DataUtil.GetDynamicValue(item as DynamicObject, property));
                            }
                            else
                            {
                                return dt.Max(item => DataUtil.GetExpandoValue(item as ExpandoObject, property));
                            }

                        case "Min":
                            if (isDynamicObjectType)
                            {
                                return dt.Min(item => DataUtil.GetDynamicValue(item as DynamicObject, property));
                            }
                            else
                            {
                                return dt.Min(item => DataUtil.GetExpandoValue(item as ExpandoObject, property));
                            }

                        case "Average":
                            if (isDynamicObjectType)
                            {
                                return dt.Select(item => DataUtil.GetDynamicValue(item as DynamicObject, property)).ToList().Average(value => Convert.ToDouble(value, CultureInfo.InvariantCulture));
                            }
                            else
                            {
                                return dt.Select(item => DataUtil.GetExpandoValue(item as ExpandoObject, property)).ToList().Average(value => Convert.ToDouble(value, CultureInfo.InvariantCulture));
                            }

                        case "Sum":
                            if (isDynamicObjectType)
                            {
                                return dt.Select(item => DataUtil.GetDynamicValue(item as DynamicObject, property)).ToList().Sum(value => Convert.ToDouble(value, CultureInfo.InvariantCulture));
                            }
                            else
                            {
                                return dt.Select(item => DataUtil.GetExpandoValue(item as ExpandoObject, property)).ToList().Sum(value => Convert.ToDouble(value, CultureInfo.InvariantCulture));
                            }

                        case "TrueCount":
                            List<WhereFilter> trueWhereFilter = new List<WhereFilter>() { new WhereFilter { Field = property, Operator = "equal", value = true } };
                            return DynamicObjectOperation.PerformFiltering(items, trueWhereFilter, null, null).Count();
                        case "FalseCount":
                            List<WhereFilter> falseWhereFilter = new List<WhereFilter>() { new WhereFilter { Field = property, Operator = "equal", value = false } };
                            return DynamicObjectOperation.PerformFiltering(items, falseWhereFilter, null, null).Count();
                        default:
                            return null;
                    }
                }
                else
                {
                    switch (aggregateType)
                    {
                        case "Count":
                            return queryable.Count();
                        case "Max":
                            return queryable.Max(property);
                        case "Min":
                            return queryable.Min(property);
                        case "Average":
                            return queryable.Average(property);
                        case "Sum":
                            return queryable.Sum(property);
                        case "TrueCount":
                            return queryable.Where(property, true, FilterType.Equals, false).Count();
                        case "FalseCount":
                            return queryable.Where(property, false, FilterType.Equals, false).Count();
                        default:
                            return null;
                    }
                }
            };
        }

        internal static object CompareAndRemove(object data, object original, string key = "")
        {
            if (original == null)
            {
                return data;
            }

            Type myType = data.GetType();
            IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());

            foreach (PropertyInfo prop in props)
            {
                PropertyInfo orgProp = original.GetType().GetProperty(prop.Name);
                var propertyValue = prop.GetValue(data);
                if (!(propertyValue == null || propertyValue is string || propertyValue.GetType().GetTypeInfo().IsPrimitive || propertyValue is TimeSpan ||
                        propertyValue is decimal || propertyValue is DateTime || propertyValue is IEnumerable || propertyValue is DateTimeOffset ||
                        propertyValue is ICollection || propertyValue is Guid || propertyValue.GetType().GetTypeInfo().IsEnum))
                {
                    CompareAndRemove(propertyValue, orgProp.GetValue(original));
                    Type propType = prop.GetType();
                    IList<PropertyInfo> propsOfComplex = new List<PropertyInfo>(myType.GetProperties());
                    IEnumerable<PropertyInfo> final = propsOfComplex.Where((data) => data.Name != "@odata.etag");
                    if (!final.Any())
                    {
                        prop.SetValue(data, null);
                    }
                }
                else if (prop.Name != key && prop.Name != "@odata.etag" && propertyValue != null && propertyValue.Equals(orgProp.GetValue(original)))
                {
                    if (propertyValue is bool)
                    {
                        prop.SetValue(data, propertyValue);
                    }
                    else
                    {
                        prop.SetValue(data, null);
                    }
                }
            }

            return data;
        }

        /// <summary>
        /// Formats the given value.
        /// </summary>
        /// <param name="value">Value to be formatted.</param>
        /// <param name="format">Format string.</param>
        /// <returns>string.</returns>
        public static string GetFormattedValue(object value, string format)
        {
            List<string> Type = new List<string>() { "Double", "Int64", "Int32", "Int16", "Decimal", "Single" };
            if (value?.GetType().Name == "DateTime" || value?.GetType().Name == "DateTimeOffset")
            {
                return Intl.GetDateFormat<object>(value, format);
            }
            else if (value != null && Type.Any(t => value.GetType().Name.Contains(t, StringComparison.Ordinal)))
            {
                return Intl.GetNumericFormat<object>((object)value, format);
            }
            else
            {
                return (string)value;
            }
        }

        internal static IDictionary<string, Type> GetColumnType(IEnumerable dataSource, bool nullable = true, string columnName = null)
        {
            _ = columnName;
            IDictionary<string, Type> columnTypes = new Dictionary<string, Type>();
            List<IDynamicMetaObjectProvider> dynamics = dataSource.Cast<IDynamicMetaObjectProvider>().ToList();
            Type rowType = null;
            if (dynamics.Count > 0)
            {
                rowType = dynamics[0].GetType();
            }

            if (rowType == null || rowType.IsSubclassOf(typeof(DynamicObject)))
            {
                return null;
            }

            foreach (var item in dataSource.Cast<ExpandoObject>().ToList())
            {
                IDictionary<string, object> propertyValues = item;
                foreach (var fields in propertyValues.Keys)
                {
                    var value = propertyValues[fields];
                    if (value != null)
                    {
                        Type type = value.GetType();
                        if (type.IsValueType && nullable)
                        {
                            type = typeof(Nullable<>).MakeGenericType(type);
                        }

                        columnTypes.Add(fields, type);
                    }
                    else
                    {
                        columnTypes.Add(fields, typeof(object));
                    }
                }

                if (columnTypes.Count == propertyValues.Keys.Count)
                {
                    break;
                }
                else
                {
                    continue;
                }
            }

            return columnTypes;
        }

        internal static string GetODataUrlKey(object rowData, string keyField, object value = null, Type ModelType = null)
        {
            var keyVal = value ?? GetObject(keyField, rowData);
            if (keyVal?.GetType() == typeof(string))
            {
                if ((ModelType != typeof(string)) && (Guid.TryParse((string)keyVal, out var newGuid) || int.TryParse((string)keyVal, out var newint) || decimal.TryParse((string)keyVal, out var newdecimal) || (keyVal?.GetType() == null)
                 || double.TryParse((string)keyVal, out var newdouble)))
                {
                    return $"({keyVal})";
                }
                else if (DateTime.TryParse((string)keyVal, out var newdatetime))
                {
                    if (Regex.IsMatch(keyVal.ToString(), @"(Z|[+-]\d{2}:\d{2})$"))
                    {
                        keyVal = DateTimeOffset.Parse((string)keyVal, CultureInfo.InvariantCulture);
                        return $"({((DateTimeOffset)keyVal).ToString("yyyy-MM-dd'T'HH:mm:ss.fff'Z'", CultureInfo.InvariantCulture)})";
                    }
                    else
                    {
                        keyVal = Convert.ToDateTime(keyVal, CultureInfo.InvariantCulture);
                        return $"({((DateTime)keyVal).ToString("yyyy-MM-dd'T'HH:mm:ss.fff'Z'", CultureInfo.InvariantCulture)})";
                    }
                }
                else
                {
                    return $"('{keyVal}')";
                }
            }
            else if (keyVal?.GetType() == typeof(DateTime))
            {
                return $"({((DateTime)keyVal).ToString("yyyy-MM-dd'T'HH:mm:ss.fff'Z'", CultureInfo.InvariantCulture)})";
            }
            else if (keyVal?.GetType() == typeof(DateTimeOffset))
            {
                return $"({((DateTimeOffset)keyVal).ToString("yyyy-MM-dd'T'HH:mm:ss.fff'Z'", CultureInfo.InvariantCulture)})";
            }
            else
            {
                return $"({keyVal})";
            }
        }

        /// <summary>
        /// Gets the property value from the DynamicObject.
        /// </summary>
        /// <param name="obj">Input dynamic object.</param>
        /// <param name="name">Property name to get.</param>
        /// <returns>object.</returns>
        public static object GetDynamicValue(DynamicObject obj, string name)
        {
            object value = null;
            obj?.TryGetMember(new DataMemberBinder(name, false), out value);
            return value;
        }

        /// <summary>
        /// Gets the property value from the ExpandoObject.
        /// </summary>
        /// <param name="obj">Input Expando object.</param>
        /// <param name="name">Property name to get.</param>
        /// <returns>object.</returns>
        public static object GetExpandoValue(IDictionary<string, object> obj, string name)
        {
            object value = null;
            obj?.TryGetValue(name, out value);

            return value;
        }

        internal static object UpdateDictionary(IEnumerable<object> ExpandData, string[] columns)
        {
            List<IDictionary<string, object>> DicData = new List<IDictionary<string, object>>();

            // this.DataHashTable.Clear();
            IDictionary<string, object> DicValue;
            if (!ExpandData.Any())
            {
                return null;
            }

            PropertyInfo[] props = ExpandData?.First()?.GetType().GetProperties();
            foreach (var obj in ExpandData)
            {
                string guid = System.Guid.NewGuid().ToString();
                if (obj is DynamicObject)
                {
                    ((DynamicObject)obj).TrySetMember(new DataSetMemberBinder("BlazId", false), "BlazTempId_" + guid);
                    var rowDataHolder = new Dictionary<string, object>();
                    foreach (var col in columns)
                    {
                        (obj as DynamicObject).TryGetMember(new DataMemberBinder(col, false), out var value);
                        rowDataHolder.Add(col, value);
                    }

                    DicData.Add(rowDataHolder);
                }
                else if (obj is ExpandoObject)
                {
                    DicValue = (IDictionary<string, object>)obj;
                    DicValue.AddOrUpdateDataItem("BlazId", "BlazTempId_" + guid);
                    DicData.Add(DicValue);
                }
                else
                {
                    DicValue = ObjectToDictionary(obj, props);
                    DicValue.AddOrUpdateDataItem("BlazId", "BlazTempId_" + guid);
                    DicData.Add(DicValue);
                }

                // this.DataHashTable.Add("BlazTempId_" + guid, obj);
            }

            return DicData.Any() ? DicData : ExpandData;
        }

        internal static IDictionary<string, object> ObjectToDictionary(object o, PropertyInfo[] props)
        {
            IDictionary<string, object> res = new Dictionary<string, object>();
            for (int i = 0; i < props.Length; i++)
            {
                if (props[i].CanRead &&
                    (!Attribute.IsDefined(props[i], typeof(Newtonsoft.Json.JsonIgnoreAttribute)) && !Attribute.IsDefined(props[i], typeof(System.Text.Json.Serialization.JsonIgnoreAttribute))))
                {
                    res.AddOrUpdateDataItem(props[i].Name, props[i].GetValue(o, null));
                }
            }

            return res;
        }
    }

    internal static class DataUtilExtension
    {
        internal static void AddOrUpdateDataItem(this IDictionary<string, object> dict, string key, object value)
        {
            if (dict.ContainsKey(key))
            {
                dict[key] = value;
            }
            else
            {
                dict.Add(key, value);
            }
        }
    }

    /// <summary>
    /// Defines the data member binder for setting dynamic object property.
    /// </summary>
    public class DataMemberBinder : GetMemberBinder
    {
        public DataMemberBinder(string name, bool ignoreCase)
            : base(name, ignoreCase)
        {
        }

        public override DynamicMetaObject FallbackGetMember(DynamicMetaObject target, DynamicMetaObject errorSuggestion)
        {
            throw new NotImplementedException();
        }
    }
}
