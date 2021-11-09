using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Buttons
{
    /// <summary>
    /// SfIcon component used to render the predefined syncfusion icons using <see cref="SfIcon.Name"/> property or custom font icons using <see cref="SfIcon.IconCss"/> property.
    /// </summary>
    /// <example>
    /// The below example shows loading syncfusion icon using <see cref="SfIcon.Name"/> property.
    /// <code><![CDATA[
    /// <SfIcon Name="IconName.Italic" Title="Italic"></SfIcon>
    /// ]]></code>
    /// The below example shows loading syncfusion icon using <see cref="SfIcon.IconCss"/> property.
    /// <code><![CDATA[
    /// <SfIcon IconCss="e-icons e-bold" Title="Bold"></SfIcon>
    /// ]]></code>
    /// The below example shows loading third party icon using <see cref="SfIcon"/> component.
    /// Plus icon were loaded from open iconic, by defining the open iconic font CSS.
    /// <code><![CDATA[
    /// <SfIcon IconCss="oi oi-plus" Title="Plus"></SfIcon>
    /// ]]></code>
    /// </example>
    public partial class SfIcon : SfBaseComponent
    {
        private const string ROOT_CLS = "e-icons";
        private const string CLS_PREFIX = "e-";
        private const string SPACE = " ";
        private const string CLASS = "class";
        private string cssClass;
        private Dictionary<string, object> htmlAttributes = new Dictionary<string, object>();
        private List<string> directParamKeys = new List<string>();

        /// <exclude/>
        /// <summary>
        /// Gets or sets the child content for Icon component.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Gets or sets a collection of additional attributes that will be applied to icon element.
        /// </summary>
        /// <remarks>
        /// Additional attributes can be added by specifying as inline attributes or by specifying <c>@attributes</c> directive.
        /// </remarks>
        /// <example>
        /// In the below code example, font size of the icon customized using <c>@attributes</c> directive.
        /// <code><![CDATA[
        /// <SfIcon Name="IconName.Copy" @attributes="customAttribute"></SfIcon>
        /// @code{
        ///    Dictionary<string, object> customAttribute = new Dictionary<string, object>()
        ///    {
        ///        { "style", "font-size: 20px" }
        ///    };
        /// }
        /// ]]></code>
        /// </example>
        [Parameter(CaptureUnmatchedValues = true)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Dictionary<string, object> HtmlAttributes { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets a CSS class string which can be used to provide custom icon or custom style for icons.
        /// </summary>
        /// <value>
        /// Accepts a CSS class string separated by space to provide custom icon or custom style for icons. The default value is <c>String.Empty</c>.
        /// </value>
        /// <remarks>
        /// IconCss property used to append custom class to the predefined icons. This property will also render the customized icons like font-awesome icons.
        /// </remarks>
        /// <example>
        /// In the below example custom icon is loaded using <c>IconCss</c> property.
        /// <code><![CDATA[
        /// <SfIcon IconCss="oi oi-home"></SfIcon>
        /// ]]></code>
        /// In the below example icon color is customized using <c>IconCss</c> property.
        /// <code><![CDATA[
        /// <SfIcon IconCss="oi oi-plus color-red"></SfIcon>
        /// <style>
        ///     .color-red {
        ///        color: red; 
        ///     }
        /// </style>
        /// ]]></code>
        /// </example>
        [Parameter]
        public string IconCss { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of the built-in syncfusion icons to render. 
        /// </summary>
        /// <value>
        /// One of the <see cref="IconName"/> enumeration.
        /// </value>
        /// <remarks>
        /// This property is used to render icons from predefined <see cref="IconName"/> options. You can use <see cref="IconCss"/> to load custom icons.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfIcon Name="IconName.Bold"></SfIcon>
        /// ]]></code>
        /// </example>
        [Parameter]
        public IconName Name { get; set; }

        /// <summary>
        /// Gets or sets the size of the icon.
        /// </summary>
        /// <value>
        /// One of the <see cref="IconSize"/> enumeration that specifies the size of the icon. 
        /// The default value is <see cref="IconSize.Medium"/>
        /// </value>
        /// <remarks>
        /// The Size property used to set font size for icons. This property will considered only when icons are rendered using <see cref="Name"/> property.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfIcon Name="IconName.Paste" Size="IconSize.Large"></SfIcon>
        /// ]]></code>
        /// </example>
        [Parameter]
        public IconSize Size { get; set; } = IconSize.Medium;

        /// <summary>
        /// Gets or sets title attribute for icon.
        /// </summary>
        /// <value>
        /// Accepts a string. The default value is <c>String.Empty</c>.
        /// </value>
        /// <remarks>
        /// Title attribute used to improve accessibility with screen readers and show a tooltip on mouse over the icon.
        /// </remarks>
        /// <example>
        /// <code><![CDATA[
        /// <SfIcon Name="IconName.Cut" Title="Cut"></SfIcon>
        /// ]]></code>
        /// </example>
        [Parameter]
        public string Title { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            Dictionary<string, object> attributes = HtmlAttributes;
            cssClass = string.Empty;
            if (directParamKeys.Contains("Name"))
            {
                FieldInfo field = Name.GetType()?.GetField(Name.ToString());
                string iconClsName = field.GetCustomAttribute<EnumMemberAttribute>()?.Value;
                cssClass = ROOT_CLS + SPACE + CLS_PREFIX + iconClsName + SPACE + CLS_PREFIX + Size.ToString().ToLower(CultureInfo.InvariantCulture);
            }

            if (!string.IsNullOrEmpty(IconCss))
            {
                cssClass += (string.IsNullOrEmpty(cssClass) ? string.Empty : SPACE) + IconCss;
            }

            if (attributes.Any())
            {
                htmlAttributes = attributes;
                if (htmlAttributes.ContainsKey(CLASS))
                {
                    cssClass += SPACE + (htmlAttributes[CLASS] as string);
                    htmlAttributes.Remove(CLASS);
                }
            }
        }

        /// <summary>
        /// Triggers while properties get dynamically changed in the component.
        /// </summary>
        /// <returns>System.Threading.Tasks.</returns>
        /// <param name="parameters"><see cref="ParameterView"/> parameters.</param>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override Task SetParametersAsync(ParameterView parameters)
        {
            if (directParamKeys.Count == 0)
            {
                foreach (var parameter in parameters)
                {
                    if (!parameter.Cascading)
                    {
                        directParamKeys.Add(parameter.Name);
                    }
                }
            }

            return base.SetParametersAsync(parameters);
        }
    }
}