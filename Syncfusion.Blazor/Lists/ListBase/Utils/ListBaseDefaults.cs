namespace Syncfusion.Blazor.Lists.Internal
{
    // update default listbase option

    /// <exclude/>
    /// <summary>
    /// List base default component model classes.
    /// </summary>
    /// <typeparam name="T">The generic type parameter.</typeparam>
    public class DefaultListBaseOptions<T>
    {
        /// <summary>
        ///  maps and returns default listbase fields values to the listbase fields
        /// </summary>
#pragma warning disable CA1024 // Use properties where appropriate
        public ListBaseFields<T> GetDefaultFieldsMapping()
#pragma warning restore CA1024 // Use properties where appropriate
        {
            return new ListBaseFields<T>
            {
#pragma warning disable BL0005 // Component parameter should not be set outside of its component.
                Id = "id",
                Text = "text",
                Value = "value",
                IsChecked = "isChecked",
                Enabled = "enabled",
                IconCss = "iconCss",
                Child = "child",
                IsVisible = "isVisible",
                Tooltip = "tooltip",
                HtmlAttributes = "htmlAttributes",
                GroupBy = null
#pragma warning restore BL0005 // Component parameter should not be set outside of its component.
            };
        }

        /// <summary>
        /// sets and returns default classlist for listbase.
        /// </summary>
        /// <param name="moduleName">Specifies the module name.</param>
        /// <returns>returns the module class list.</returns>
        public ClassList GetModuleClassList(string moduleName)
        {
            var module = moduleName;
            return new ClassList()
            {
                Li = $"e-{module}-item",
                Ul = $"e-{module}-parent e-ul",
                Group = $"e-{module}-group-item",
                Icon = $"e-{module}-icon",
                Text = $"e-{module}-text",
                Check = $"e-{module}-check",
                Checked = "e-checked",
                TextContent = "e-text-content",
                HasChild = "e-has-child",
                Level = "e-level",
                Disabled = "e-disabled",
                IconWrapper = "e-icon-wrapper"
            };
        }
    }
}
