using System;
using Newtonsoft.Json;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor
{
    /// <summary>
    /// The HTML DOM equivalent C# class object to define basic properties and perform the basic DOM operations.
    /// </summary>
    public class DOM
    {
        /// <summary>
        /// A unique ID of the HTML DOM element object.
        /// </summary>
        [JsonProperty("id")]
        public string ID { get; set; }

        /// <summary>
        /// Gets or sets xPath.
        /// </summary>
        [JsonProperty("xPath")]
        internal string XPath { get; set; }

        /// <summary>
        /// Gets or sets DOM UID.
        /// </summary>
        [JsonProperty("domUUID")]
        internal string DomUUID { get; set; }

        /// <summary>
        /// Gets or sets element ID.
        /// </summary>
        [JsonProperty("elementID")]
        internal string ElementID { get; set; }

        /// <summary>
        /// Gets or sets JsRuntime.
        /// </summary>
        internal IJSRuntime JsRuntime { get; set; }

        /// <summary>
        /// Add a list of CSS classes to the HTML DOM element.
        /// </summary>
        /// <param name="classList">An array of string CSS class to be added in the HTML DOM.</param>
        /// <returns>Task.</returns>
        public async Task AddClass(string[] classList)
        {
            await SfBaseUtils.InvokeMethod(JsRuntime, "sfBlazor.addClass", GetElementID(), DomUUID, XPath, classList);
        }

        /// <summary>
        /// Remove a list of CSS classes in the HTML DOM element.
        /// </summary>
        /// <param name="classList">An array of string CSS class to be removed in the HTML DOM.</param>
        /// <returns>Task.</returns>
        public async Task RemoveClass(string[] classList)
        {
            await SfBaseUtils.InvokeMethod(JsRuntime, "sfBlazor.removeClass", GetElementID(), DomUUID, XPath, classList);
        }

        /// <summary>
        /// Get the CSS class names from the HTML DOM element.
        /// </summary>
        /// <returns>CSS class names in string of Array.</returns>
        public ValueTask<string[]> GetClassList()
        {
            return SfBaseUtils.InvokeMethod<string[]>(JsRuntime, "sfBlazor.getClassList", GetElementID(), DomUUID, XPath);
        }

        /// <summary>
        /// Set the attribute to the HTML DOM element.
        /// </summary>
        /// <typeparam name="T">The attribute value type.</typeparam>
        /// <param name="attributeName">The attribute name needs to be updated in the HTML element.</param>
        /// <param name="attributeValue">The attribute value needs to be updated to the corresponding attribute in the HTML element.</param>
        /// <returns>Task.</returns>
        public async Task SetAttribute<T>(string attributeName, T attributeValue)
        {
            await SfBaseUtils.InvokeMethod(JsRuntime, "sfBlazor.setAttribute", GetElementID(), DomUUID, XPath, attributeName, attributeValue);
        }

        /// <summary>
        /// Get the attribute value from the HTML DOM element.
        /// </summary>
        /// <typeparam name="T">The result.</typeparam>
        /// <param name="attributeName">The attribute name to get the attribute value from the HTML element.</param>
        /// <returns>Returns the HTML attribute value with specific type.</returns>
        public ValueTask<T> GetAttribute<T>(string attributeName)
        {
            return SfBaseUtils.InvokeMethod<T>(JsRuntime, "sfBlazor.getAttribute", GetElementID(), DomUUID, XPath, attributeName);
        }

        private string GetElementID()
        {
            return ElementID != null ? ElementID : ID;
        }
    }
}