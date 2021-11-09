using System;
using Newtonsoft.Json;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Syncfusion.Blazor.Internal
{
    internal static class SyncfusionInterop
    {
#pragma warning disable CA1031
        internal static async ValueTask<T> Init<T>(IJSRuntime jsRuntime, string elementId, object model, string[] events, string nameSpace, DotNetObjectReference<object> helper, string bindableProps, Dictionary<string, object> htmlAttributes = null, Dictionary<string, object> templateRefs = null, DotNetObjectReference<object> adaptor = null, string localeText = null)
        {
            try
            {
                return await jsRuntime.InvokeAsync<T>("sfBlazor.initialize", elementId, model, events, nameSpace, helper, bindableProps, htmlAttributes, templateRefs, adaptor, localeText);
            }
            catch (Exception e)
            {
                string issue = nameSpace + " - #" + elementId + " - Init had internal server error \n";
                return await SyncfusionInterop.LogError<T>(jsRuntime, e, issue);
            }
        }

        internal static async ValueTask<T> Update<T>(IJSRuntime jsRuntime, string elementId, string model, string nameSpace)
        {
            try
            {
                return await jsRuntime.InvokeAsync<T>("sfBlazor.setModel", elementId, model, nameSpace);
            }
            catch (Exception e)
            {
                string issue = nameSpace + " - #" + elementId + " - Update model had internal server error \n";
                return await SyncfusionInterop.LogError<T>(jsRuntime, e, issue);
            }
        }

        internal static async ValueTask<T> InvokeMethod<T>(IJSRuntime jsRuntime, string elementId, string methodName, string moduleName, object[] args, string nameSpace, ElementReference? element = null)
        {
            try
            {
                // enum type conversion issue in System.Text.Json serialization - https://github.com/dotnet/corefx/issues/38568
                JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
                {
                    StringEscapeHandling = StringEscapeHandling.EscapeNonAscii
                };
                return await jsRuntime.InvokeAsync<T>("sfBlazor.invokeMethod", elementId, methodName, moduleName, JsonConvert.SerializeObject(args, jsonSerializerSettings), element);
            }
            catch (Exception e)
            {
                string issue = nameSpace + " - #" + elementId + " - invokeMethod had internal server error \n";
                return await SyncfusionInterop.LogError<T>(jsRuntime, e, issue);
            }
        }

        internal static ValueTask<T> InvokeGet<T>(IJSRuntime jsRuntime, string id, string moduleName, string methodName, string nameSpace)
        {
            try
            {
                return SfBaseUtils.InvokeMethod<T>(jsRuntime, "sfBlazor.getMethodCall", id, moduleName, methodName);
            }
            catch (Exception e)
            {
                string issue = nameSpace + " - #" + id + " - getMethodCall had internal server error \n";
                return SyncfusionInterop.LogError<T>(jsRuntime, e, issue);
            }
        }

        internal static ValueTask<T> InvokeSet<T>(IJSRuntime jsRuntime, string id, string moduleName, string methodName, object[] args, string nameSpace)
        {
            try
            {
                return SfBaseUtils.InvokeMethod<T>(jsRuntime, "sfBlazor.setMethodCall", id, moduleName, methodName, args);
            }
            catch (Exception e)
            {
                string issue = nameSpace + " - #" + id + " - setMethodCall had internal server error \n";
                return SyncfusionInterop.LogError<T>(jsRuntime, e, issue);
            }
        }

        internal static async ValueTask<T> SetTemplateInstance<T>(IJSRuntime jsRuntime, string templateName, DotNetObjectReference<object> helper, int guid)
        {
            try
            {
                return await SfBaseUtils.InvokeMethod<T>(jsRuntime, "sfBlazor.setTemplateInstance", templateName, helper, guid);
            }
            catch (Exception e)
            {
                return await SyncfusionInterop.LogError<T>(jsRuntime, e);
            }
        }

        internal static async ValueTask<T> SetTemplates<T>(IJSRuntime jsRuntime, string templateID)
        {
            try
            {
                return await jsRuntime.InvokeAsync<T>("sfBlazor.setTemplate", templateID);
            }
            catch (Exception e)
            {
                return await SyncfusionInterop.LogError<T>(jsRuntime, e);
            }
        }

        internal static ValueTask<T> LogError<T>(IJSRuntime jsRuntime, Exception e, string message = "")
        {
            try
            {
                ErrorMessage error = new ErrorMessage();
                error.Message = message + e.Message;
                error.Stack = e.StackTrace;
                if (e.InnerException != null)
                {
                    error.Message = message + e.InnerException.Message;
                    error.Stack = e.InnerException.StackTrace;
                }

                return jsRuntime.InvokeAsync<T>("sfBlazor.throwError", error);
            }
            catch
            {
                return default;
            }
        }
#pragma warning restore CA1031
    }

    internal class ErrorMessage
    {
        [JsonProperty("message")]
        internal string Message { get; set; }

        [JsonProperty("stack")]
        internal string Stack { get; set; }
    }
}
