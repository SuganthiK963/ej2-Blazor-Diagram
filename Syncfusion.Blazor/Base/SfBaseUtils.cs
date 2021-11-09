using System;
using System.Linq;
using Newtonsoft.Json;
using System.Text.Json;
using System.Reflection;
using System.Collections;
using Microsoft.JSInterop;
using System.Globalization;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Syncfusion.Blazor.Internal
{
    /// <summary>
    /// Common utility methods which can be used in all the Syncfusion Blazor components.
    /// </summary>
    internal class SfBaseUtils
    {
#if !NETSTANDARD
        public const string IMPORT = "import";
#else
        public const string IMPORT = "sfBlazor.import";
#endif

        /// <summary>
        /// Update the given dictionary value based on the key value check.
        /// </summary>
        /// <param name="key">Key needs to be updated in the dictionary.</param>
        /// <param name="data">Value needs to be updated for the specific key.</param>
        /// <param name="dictionary">Dictionary needs to be add or updated.</param>
        /// <returns>Returns updated Dictionary.</returns>
        internal static Dictionary<string, object> UpdateDictionary(string key, object data, Dictionary<string, object> dictionary)
        {
            dictionary = dictionary != null ? dictionary : new Dictionary<string, object>();
            if (!dictionary.TryAdd(key, data))
            {
                dictionary[key] = data;
            }

            return dictionary;
        }

        /// <summary>
        /// Update the dictionary based on the @attributes key value check.
        /// <param name="classList">class list to be added in the string format.</param>
        /// <param name="dictionary">@attribute property value for updating class list.</param>
        /// <returns>Returns Dictionary.</returns>
        /// </summary>
        internal static Dictionary<string, object> GetAttribtues(string classList, Dictionary<string, object> dictionary)
        {
            if (!dictionary.TryAdd("class", classList))
            {
                dictionary["class"] = SfBaseUtils.AddClass(classList, dictionary["class"].ToString());
            }

            return dictionary;
        }

        /// <summary>
        /// Returns the bool value based on comparing given values with the EqualityComparer.
        /// </summary>
        /// <param name="oldValue">Old value of the property.</param>
        /// <param name="newValue">New value of the property.</param>
        /// <returns>Returns bool value.</returns>
        internal static bool Equals<T>(T oldValue, T newValue)
        {
            var valueType = oldValue?.GetType();
            var typeValue = new HashSet<string>(new[] { typeof(Int32[,]).Name, typeof(Double[,]).Name, typeof(Int32?[,]).Name, typeof(Decimal[,]).Name });
            var isValueCollection = valueType != null && ((valueType.Namespace != null && valueType.Namespace.Contains("Collections", StringComparison.Ordinal)) || valueType.IsArray);

            // Compare collection values using Json Serializer
            if (isValueCollection && !typeValue.Contains(valueType.Name))
            {
#if !NETSTANDARD
                JsonSerializerOptions options = new JsonSerializerOptions()
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    IgnoreReadOnlyProperties = true
                };

                var oldString = System.Text.Json.JsonSerializer.Serialize(oldValue, options);
                var newString = System.Text.Json.JsonSerializer.Serialize(newValue, options);
#else
                var jsonOptions = new JsonSerializerOptions()
                {
                    IgnoreReadOnlyProperties = true
                };
                var oldString = System.Text.Json.JsonSerializer.Serialize(oldValue, jsonOptions);
                var newString = System.Text.Json.JsonSerializer.Serialize(newValue, jsonOptions);
#endif
                return string.Equals(oldString, newString, StringComparison.Ordinal);
            }
            else
            {
                return EqualityComparer<T>.Default.Equals(oldValue, newValue);
            }
        }

        /// <summary>
        /// Invoking events for two-way bindings property changes.
        /// </summary>
        /// <param name="publicValue">Public value needs to be updated in the two-way binding event.</param>
        /// <param name="privateValue">Private value to compare with public value for invoking two-way bindings.</param>
        /// <param name="eventCallback">EventCallback for invoking two-way binding event handler function.</param>
        /// <param name="editContext">EditContext instance for invoking NotifyFieldChanged method.</param>
        /// <param name="expression">Expression needs to be passed in the NotifyFieldChanged method.</param>
        /// <returns>Returns public property value.</returns>
        internal static async Task<T> UpdateProperty<T>(T publicValue, T privateValue, EventCallback<T> eventCallback, EditContext editContext = null, Expression<Func<T>> expression = null)
        {
            var finalValue = publicValue;

            // Checking eventcallback for two-way notification
            if (eventCallback.HasDelegate && !SfBaseUtils.Equals(publicValue, privateValue))
            {
                await eventCallback.InvokeAsync(finalValue);
                if (editContext != null)
                {
                    SfBaseUtils.ValidateExpression(editContext, expression);
                }
            }

            return finalValue;
        }

        /// <summary>
        /// Convert given arguments into double array.
        /// </summary>
        /// <param name="args">arguments for convert into double array.</param>
        internal static double[] ToDoubleArray(object args)
        {
            List<double> result = new List<double>();
            IEnumerable arr = args as IEnumerable;
            foreach (var item in arr)
            {
                result.Add(Convert.ToDouble(item, CultureInfo.CurrentCulture));
            }

            return result.ToArray();
        }

        /// <summary>
        /// Notify the field changes of the EditContext with specified expression.
        /// </summary>
        /// <param name="editContext">EditContext instance for invoking the NotifyFieldChanged method.</param>
        /// <param name="expression">Expression need to be passed in the NotifyFieldChanged method.</param>
        internal static void ValidateExpression<T>(EditContext editContext, Expression<Func<T>> expression)
        {
            // Notify form fields about the value changes
            if (expression != null)
            {
                editContext?.NotifyFieldChanged(FieldIdentifier.Create<T>(expression));
            }
        }

        /// <summary>
        /// Import component specific interop script modules in the application.
        /// </summary>
        /// <param name="jsRuntime">JSRuntime service to invoke import method.</param>
        /// <param name="scriptModule">Component specific interop script module.</param>
        /// <param name="hashKey">Component specific interop script module Key.</param>
        /// <returns>JSObjectReference.</returns>
        internal static async Task ImportModule(IJSRuntime jsRuntime, SfScriptModules scriptModule, string hashKey = "")
        {
            var hyphen = string.IsNullOrEmpty(hashKey) ? hashKey : "-";
            var moduleName = SfBaseUtils.GetEnumValue(scriptModule) + hyphen + hashKey;
#if SyncfusionBlazorCore
            await jsRuntime.InvokeVoidAsync(SfBaseUtils.IMPORT, GetScriptPath(scriptModule) + moduleName + ".min.js");
#else
            await jsRuntime.InvokeVoidAsync(SfBaseUtils.IMPORT, GetScriptPath() + moduleName + ".min.js");
#endif
        }

        /// <summary>
        /// Import component dependent interop script modules in the application.
        /// </summary>
        /// <param name="jsRuntime">JSRuntime service to invoke import method.</param>
        /// <param name="scriptModules">Component specific interop script module.</param>
        /// <param name="hashKey">Component specific interop script module Key.</param>
        internal static async Task ImportModules(IJSRuntime jsRuntime, List<ScriptModules> scriptModules, string hashKey = "")
        {
            if (scriptModules.Count > 0)
            {
                List<Task> listOfTasks = new List<Task>();
                foreach (var scriptModule in scriptModules)
                {
                    var moduleName = SfBaseUtils.GetEnumValue(scriptModule) + "-" + hashKey;
#if SyncfusionBlazorCore
                    listOfTasks.Add(SfBaseUtils.ImportScripts(jsRuntime, GetScriptPath(scriptModule) + moduleName + ".min.js"));
#else
                    listOfTasks.Add(SfBaseUtils.ImportScripts(jsRuntime, GetScriptPath() + moduleName + ".min.js"));
#endif
                }

                await Task.WhenAll(listOfTasks);
            }
        }

        internal static string GetScriptPath()
        {
            string scriptPath = "./_content/Syncfusion.Blazor/scripts/";
            return scriptPath;
        }

        internal static string GetScriptPath<T>(T enumProperty)
        {
            string scriptPath = string.Empty;
            var name = Enum.GetName(typeof(T), enumProperty);
            var packName = typeof(T).GetField(name).GetCustomAttributes(false).OfType<PackageNameAttribute>().SingleOrDefault().PackageName;
            scriptPath = "./_content/Syncfusion.Blazor." + packName + "/scripts/";
            return scriptPath;
        }

        /// <summary>
        /// Async method to execute script module import.
        /// </summary>
        /// <param name="jsRuntime">JSRuntime service to invoke import method.</param>
        /// <param name="modulePath">Module path to be load the script modules.</param>
        /// <returns>Async Task.</returns>
        internal static async Task ImportScripts(IJSRuntime jsRuntime, string modulePath)
        {
            await jsRuntime.InvokeVoidAsync(SfBaseUtils.IMPORT, modulePath);
        }

        internal static async Task ImportScripts(IJSRuntime jsRuntime, ScriptModules scriptModule, string hashKey)
        {
            var moduleName = SfBaseUtils.GetEnumValue(scriptModule) + "-" + hashKey;
#if SyncfusionBlazorCore
            await jsRuntime.InvokeVoidAsync(SfBaseUtils.IMPORT, GetScriptPath(scriptModule) + moduleName + ".min.js");
#else
            await jsRuntime.InvokeVoidAsync(SfBaseUtils.IMPORT, GetScriptPath() + moduleName + ".min.js");
#endif
        }

        internal static async Task ImportDependencies(IJSRuntime jsRuntime, List<ScriptModules> dependentScripts, SfScriptModules scriptModules, string hashKey)
        {
            // Render component's dependency scripts.
            await SfBaseUtils.ImportModules(jsRuntime, dependentScripts, hashKey);

            // Render the component's interop scripts.
            if (scriptModules != SfScriptModules.None)
            {
                // load component specific interop scripts.
                await SfBaseUtils.ImportModule(jsRuntime, scriptModules, hashKey);
            }
        }

        /// <summary>
        /// Invokes event handler function of the corresponding event name with parameters.
        /// </summary>
        /// <param name="eventFn">EventCallback to invoke the event handler method.</param>
        /// <param name="eventArgs">Arguments of the event handler method.</param>
        internal static async Task InvokeEvent<T>(object eventFn, T eventArgs)
        {
            if (eventFn != null)
            {
                var eventHandler = (EventCallback<T>)eventFn;
                if (eventHandler.HasDelegate)
                {
                    await eventHandler.InvokeAsync(eventArgs);
                }
            }
        }

        internal static bool IsObservableCollection(object data)
        {
            return (data != null && data.GetType().GetGenericTypeDefinition() == typeof(System.Collections.ObjectModel.ObservableCollection<>)) ? true : false;
        }

        /// <summary>
        /// Convert an object to the specified type.
        /// </summary>
        /// <param name="dataValue">Value needs to be converted with specific type.</param>
        /// <param name="conversionType">Type that needs to be converted to the given value.</param>
        /// <param name="isClientChange">Validate the client changes for the components.</param>
        /// <param name="isParseValue">Validate the Parse Value for the components.</param>
        /// <returns>Returns converted object.</returns>
#pragma warning disable CA1502
        internal static object ChangeType(object dataValue, Type conversionType, bool isClientChange = false, bool isParseValue = false)
#pragma warning disable CA1502
        {
            var valueType = dataValue?.GetType();
            var isValueCollection = valueType != null && valueType.Namespace != null && (valueType.Namespace.Contains("Collections", StringComparison.Ordinal) || valueType.IsArray ||
                (valueType.BaseType != null && valueType.BaseType.Namespace != null && (valueType.BaseType.Namespace.Contains("Collections", StringComparison.Ordinal) || valueType.BaseType.IsArray)));

            // Returns null value
            if (dataValue == null)
            {
                return null;
            }

            // Returns the nullable type values
            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                conversionType = Nullable.GetUnderlyingType(conversionType);
            }

            // Returns the enum type values
            if (conversionType.IsEnum)
            {
                if (!Enum.IsDefined(conversionType, dataValue.ToString()))
                {
                    var enumValues = Enum.GetNames(conversionType);
                    var result = enumValues.Where(enumValue => enumValue.ToLower(CultureInfo.CurrentCulture) == dataValue.ToString());

                    // Get the Enum Value from Enum Display attribute Value
                    var enumDisplayValue = conversionType.GetFields().Where(x => x.GetCustomAttribute<DisplayAttribute>()?.Name == dataValue.ToString());
                    dataValue = enumDisplayValue.Any() ? enumDisplayValue.FirstOrDefault().Name : result.FirstOrDefault();
                }

                return dataValue == null ? null : Enum.Parse(conversionType, dataValue.ToString());
            }

            // Returns the basic Convert.ChangeType value when both source and destination has the same Type name.
            if (dataValue.GetType().Name == conversionType.Name)
            {
                return Convert.ChangeType(dataValue, conversionType, CultureInfo.CurrentCulture);
            }

            // Converts the value to string for array and collection values
            if ((conversionType.IsPrimitive && !conversionType.IsArray && !conversionType.Namespace.Contains("Collections", StringComparison.Ordinal)) || conversionType == typeof(decimal) || conversionType == typeof(string) || (conversionType == typeof(object) && !isValueCollection) || conversionType == typeof(DateTime))
            {
                dataValue = Convert.ToString(dataValue, CultureInfo.InvariantCulture);
            }
            else
            {
                if (conversionType == typeof(Guid))
                {
                    dataValue = new Guid(dataValue.ToString());
                }

                // Returns the value for collection and interface types
                else if (isValueCollection || (conversionType.IsInterface && !isClientChange))
                {
                    return dataValue;
                }
                // Returns DateTime value for DateTimeOffset type values.
                else if (conversionType.Name == "DateTimeOffset")
                {
                    dataValue = DateTimeOffset.Parse(dataValue.ToString(), CultureInfo.InvariantCulture);
                }

                // Returns deserialized object for other then DateTimeOffset type values.
                else
                {
                    string tempValue = conversionType.Name != "TimeSpan" ? dataValue.ToString() : JsonConvert.SerializeObject(dataValue);
                    tempValue = valueType == typeof(System.Text.Json.JsonElement) ? ConvertJsonString(dataValue) : tempValue;
                    dataValue = JsonConvert.DeserializeObject(tempValue, conversionType, new JsonSerializerSettings
                    {
                        DateTimeZoneHandling = DateTimeZoneHandling.Local
                    });
                }
            }
            // return Convert.ChangeType(dataValue, conversionType, CultureInfo.InvariantCulture); -> this line not working with grid filtering in 'de' culture.
            var currentCulture = isParseValue ? CultureInfo.InvariantCulture : CultureInfo.CurrentCulture;
            return Convert.ChangeType(dataValue, conversionType, currentCulture);
        }

        internal static string ConvertJsonString(object jsonElement)
        {
            string jsonString = jsonElement.ToString();
            if (!jsonString.Contains(",", StringComparison.Ordinal))
            {
                jsonString = jsonString.Replace("\\", string.Empty, StringComparison.Ordinal);
            }

            if ((jsonString.IndexOf("\"[", StringComparison.Ordinal) != jsonString.IndexOf("\"[\\", StringComparison.Ordinal)) || (jsonString.IndexOf("\"{", StringComparison.Ordinal) != jsonString.IndexOf("\"{\\", StringComparison.Ordinal)))
            {
                if ((jsonString.LastIndexOf("\"{", StringComparison.Ordinal) > 0) && (!jsonString.Contains(",", StringComparison.Ordinal)))
                {
                    jsonString = jsonString.Replace("\"[", "[", StringComparison.Ordinal).Replace("]\"", "]", StringComparison.Ordinal)
                        .Replace("\"{", "{", StringComparison.Ordinal).Replace("}\"", "}", StringComparison.Ordinal);
                }
            }

            return jsonString;
        }

        /// <summary>
        /// Generate the unique Guid.
        /// </summary>
        /// <param name="name">add addtional name into ID.</param>
        /// <returns>Returns ID string.</returns>
        internal static string GenerateID(string name = null)
        {
            var idName = !string.IsNullOrEmpty(name) ? name + "-" : string.Empty;
            return idName + Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Invoke void return type methods.
        /// </summary>
        internal static async Task InvokeMethod(IJSRuntime jsRuntime, string methodName, params object[] methodParams)
        {
            try
            {
                var runtime = jsRuntime as IJSInProcessRuntime;
                if (runtime != null)
                {
                    runtime.InvokeVoid(methodName, methodParams);
                }
                else
                {
                    await jsRuntime.InvokeVoidAsync(methodName, methodParams);
                }
            }
            #pragma warning disable CA1031
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Invoke object return type methods.
        /// </summary>
        internal static async ValueTask<T> InvokeMethod<T>(IJSRuntime jsRuntime, string methodName, params object[] methodParams)
        {
            try
            {
                var runtime = jsRuntime as IJSInProcessRuntime;
                if (runtime != null)
                {
                    return runtime.Invoke<T>(methodName, methodParams);
                }
                else
                {
                    return await jsRuntime.InvokeAsync<T>(methodName, methodParams);
                }
            }
            catch (Exception e)
            {
                return default(T);
                throw new InvalidOperationException(e.Message);
            }
        }

        /// <summary>
        /// compare the two values and returns a value indicating whether one value is less than, equal to, or greater than the second value.
        /// </summary>
        /// <returns>Less than Zero - value1 is less than value 2.</returns>
        /// <returns>Zero - Both are equals.</returns>
        /// <returns>Greater than Zero - value1 is greater than value 2.</returns>
        internal static int CompareValues<T>(T value1, T value2)
        {
            return Comparer<T>.Default.Compare(value1, value2);
        }

        /// <summary>
        /// Add a class to the existing string content.
        /// </summary>
        /// <param name="prevClass">Previous class list in string format.</param>
        /// <param name="className">Class name needs to be added in the string content.</param>
        /// <returns>Returns class string.</returns>
        internal static string AddClass(string prevClass, string className)
        {
            var finalClass = string.IsNullOrEmpty(prevClass) ? string.Empty : prevClass.Trim();
            finalClass = finalClass.Split(' ').Contains(className) ? finalClass : finalClass + " " + className;
            return finalClass;
        }

        /// <summary>
        /// Remove a class from the existing string content.
        /// </summary>
        /// <param name="prevClass">Previous class list in string format.</param>
        /// <param name="className">Class name needs to be removed in the string content.</param>
        /// <returns>Returns class string.</returns>
        internal static string RemoveClass(string prevClass, string className)
        {
            var finalClass = string.IsNullOrEmpty(prevClass) ? string.Empty : prevClass.Trim();
            if (!string.IsNullOrEmpty(className))
            {
                finalClass = finalClass.Contains(className, StringComparison.Ordinal) ? prevClass.Replace(className, string.Empty, StringComparison.Ordinal) : finalClass;
            }

            return finalClass;
        }

        /// <summary>
        /// Adds a value to a array.
        /// <param name="arrayValue">array to which value should be added.</param>
        /// <param name="addValue">value to be added to the array</param>
        /// <returns>Returns Dictionary.</returns>
        /// </summary>
        internal static T[] AddArrayValue<T>(T[] arrayValue, T addValue)
        {
            return arrayValue.Concat(new T[] { addValue }).ToArray();
        }

        /// <summary>
        /// Removes a value from a array.
        /// <param name="arrayValue">array to which value should be removed.</param>
        /// <param name="removeValue">value to be removed from the array</param>
        /// <returns>Returns Dictionary.</returns>
        /// </summary>
        internal static T[] RemoveArrayValue<T>(T[] arrayValue, T removeValue)
        {
            return arrayValue.Where(val => !SfBaseUtils.Equals<T>(val, removeValue)).ToArray();
        }

        /// <summary>
        /// Returns true if the list value is not null or empty.
        /// <param name="list">list value to be checked for having null or empty values.</param>
        /// <returns>Returns boolean value.</returns>
        /// </summary>
        internal static bool IsNotNullOrEmpty(IList list)
        {
            return list != null && list.Count > 0;
        }

        /// <summary>
        /// Function to normalize the units applied to the element.
        /// </summary>
        /// <param name="propertyValue">Value.</param>
        /// <returns>Retuns normalized unit value.</returns>
        internal static string FormatUnit(string propertyValue)
        {
            if (propertyValue == "auto" || propertyValue.EndsWith("%", StringComparison.Ordinal) || propertyValue.EndsWith("px", StringComparison.Ordinal) || propertyValue.EndsWith("vh", StringComparison.Ordinal) || propertyValue.EndsWith("vm", StringComparison.Ordinal) || propertyValue.EndsWith("vmax", StringComparison.Ordinal) || propertyValue.EndsWith("vmin", StringComparison.Ordinal) || propertyValue.EndsWith("em", StringComparison.Ordinal))
            {
                return propertyValue;
            }

            return propertyValue + "px";
        }

        /// <summary>
        /// Returns enumeration member value.
        /// </summary>
        /// <param name="enumValue">Actual enumeration value to be processed for its member value.</param>
        /// <returns>Returns actual enumeration member value.</returns>
        internal static string GetEnumValue<T>(T enumValue)
            where T : struct, IConvertible
        {
            return typeof(T).GetTypeInfo().DeclaredMembers.SingleOrDefault(x => x.Name == enumValue.ToString())?.GetCustomAttribute<EnumMemberAttribute>(false)?.Value;
        }

        internal static string GetEnumValue<T>(T? enumValue)
            where T : struct, IConvertible
        {
            return typeof(T).GetTypeInfo().DeclaredMembers.SingleOrDefault(x => x.Name == enumValue.ToString())?.GetCustomAttribute<EnumMemberAttribute>(false)?.Value;
        }

        /// <summary>
        /// Function to perform the animation.
        /// </summary>
        /// <param name="jsRuntime">JSRuntime service to invoke import method.</param>
        /// <param name="reference">Represents a reference to a rendered element.</param>
        /// <param name="animationObject">Animation object for performing animation transition.</param>
        /// <returns>Async Task.</returns>
        internal static async Task Animate(IJSRuntime jsRuntime, ElementReference reference, AnimationSettings animationObject)
        {
            await SfBaseUtils.InvokeMethod(jsRuntime, "sfBlazor.animate", reference, animationObject);
        }

        /// <summary>
        /// Function to perform the ripple effect.
        /// </summary>
        /// <param name="jsRuntime">JSRuntime service to invoke import method.</param>
        /// <param name="reference">Represents a reference to a rendered element.</param>
        /// <param name="rippleObject">Ripple object for performing the ripple effect.</param>
        /// <returns>Async Task.</returns>
        internal static async Task RippleEffect(IJSRuntime jsRuntime, ElementReference reference, RippleSettings rippleObject)
        {
            await SfBaseUtils.InvokeMethod(jsRuntime, "sfBlazor.callRipple", reference, rippleObject);
        }
    }
}
