using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// A class used for configuring the Tab header properties.
    /// </summary>
    public partial class TabHeader : SfBaseComponent
    {
        private const string ICONCSS = "IconCss";
        private const string ICONPOSITION = "IconPosition";
        private const string TEXT = "Text";
        private string iconCss;
        private string iconPosition;
        private string text;

        [CascadingParameter]
        internal TabItem Parent { get; set; }

        [CascadingParameter]
        internal SfTab BaseParent { get; set; }

        /// <summary>
        /// Specifies the icon class that is used to render an icon in the Tab header.
        /// </summary>
        [Parameter]
        public string IconCss { get; set; } = string.Empty;

        /// <summary>
        /// Options for positioning the icon in the Tab item header. This property depends on the `IconCss` property.
        /// The possible values are:
        /// - left: Places the icon to the `left` of the item.
        /// - top: Places the icon on the `top` of the item.
        /// - right: Places the icon to the `right` end of the item.
        /// - bottom: Places the icon at the `bottom` of the item.
        /// </summary>
        [Parameter]
        public string IconPosition { get; set; } = "left";

        /// <summary>
        /// Specifies the display text of the Tab header.
        /// </summary>
        [Parameter]
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.UpdateHeaderProperties(this);
            iconCss = IconCss;
            iconPosition = IconPosition;
            text = Text;
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree,
        /// and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            iconCss = NotifyPropertyChanges(ICONCSS, IconCss, iconCss);
            iconPosition = NotifyPropertyChanges(ICONPOSITION, IconPosition, iconPosition);
            text = NotifyPropertyChanges(TEXT, Text, text);
            if (PropertyChanges.Count > 0)
            {
                BaseParent.IsTabItemChanged = true;
            }
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            BaseParent = null;
        }
    }
}