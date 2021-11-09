using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// An item object that is used to configure the Tab.
    /// </summary>
    public partial class TabItem : SfBaseComponent
    {
        private const string CONTENT = "Content";
        private const string CSSCLASS = "CssClass";
        private const string DISABLED = "Disabled";
        private const string HEADER = "Header";
        private const string VISIBLE = "Visible";
        private const string ITEMS = "Items";
        private string content;
        private string cssClass;
        private bool disabled;
        private TabHeader header;
        private bool visible;

        [CascadingParameter]
        internal TabItems Parent { get; set; }

        [CascadingParameter]
        internal SfTab BaseParent { get; set; }

        /// <summary>
        /// Child Content for the Tab item.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Specifies the content template of the Tab item.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public RenderFragment ContentTemplate { get; set; }

        /// <summary>
        /// Specifies the content of Tab item.
        /// </summary>
        [Parameter]
        public string Content { get; set; } = string.Empty;

        /// <summary>
        /// Sets the CSS classes to the Tab item to customize its styles.
        /// </summary>
        [Parameter]
        public string CssClass { get; set; } = string.Empty;

        /// <summary>
        /// Specifies a value that indicates whether the control is disabled or not.
        /// </summary>
        [Parameter]
        public bool Disabled { get; set; }

        /// <summary>
        /// The object used for configuring the Tab item header properties.
        /// </summary>
        [Parameter]
        public TabHeader Header { get; set; }

        /// <summary>
        /// Specifies the header content of the Tab item.
        /// </summary>
        [Parameter]
        [JsonIgnore]
        public RenderFragment HeaderTemplate { get; set; }

        /// <summary>
        /// Specifies a value that indicates whether the tab is visible or not.
        /// </summary>
        [Parameter]
        public bool Visible { get; set; } = true;

        [EditorBrowsable(EditorBrowsableState.Never)]
        internal void UpdateHeaderProperties(TabHeader tabHeader)
        {
            var headerCnt = tabHeader == null ? new TabHeader() : tabHeader;
            Header = header = headerCnt;
        }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.UpdateChildProperty(this);
            BaseParent.IsTabItemChanged = true;
            content = Content;
            cssClass = CssClass;
            disabled = Disabled;
            visible = Visible;
            UpdateHeaderProperties(Header);
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree,
        /// and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            content = NotifyPropertyChanges(CONTENT, Content, content);
            cssClass = NotifyPropertyChanges(CSSCLASS, CssClass, cssClass);
            disabled = NotifyPropertyChanges(DISABLED, Disabled, disabled);
            header = NotifyPropertyChanges(HEADER, Header, header);
            visible = NotifyPropertyChanges(VISIBLE, Visible, visible);
            if (PropertyChanges.Count > 0)
            {
                BaseParent.IsTabItemChanged = true;
            }
        }

        internal void EnableItem(bool isDisabled)
        {
            if (PropertyChanges == null)
            {
                /*Intialize this field for dynamically added Tab items*/
                PropertyChanges = new Dictionary<string, object>();
            }

            Disabled = disabled = NotifyPropertyChanges(DISABLED, isDisabled, disabled);
        }

        internal override void ComponentDispose()
        {
            if (Parent.Items != null && Parent.Items.Contains(this))
            {
                Parent.Items.Remove(this);
                SfBaseUtils.UpdateDictionary(ITEMS, Parent.Items, BaseParent.PropertyChanges);
            }

            Parent = null;
            BaseParent = null;
            ChildContent = null;
        }
    }
}