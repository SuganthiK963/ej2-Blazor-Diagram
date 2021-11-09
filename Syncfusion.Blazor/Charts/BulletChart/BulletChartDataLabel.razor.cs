using Microsoft.AspNetCore.Components;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Sets and gets the data label of the bullet chart component.
    /// </summary>
    public partial class BulletChartDataLabel : SfBaseComponent
    {
        [CascadingParameter]
        internal IBulletChart Parent { get; set; }

        internal BulletChartDataLabelStyle LabelStyle { get; set; }

        /// <summary>
        /// Sets and gets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Sets and gets to enabled the data label.
        /// </summary>
        /// <exclude />
        [Parameter]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This property is deprecated and will no longer be used.")]
        public bool Enabled { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.DataLabel = this;
        }

        internal override void ComponentDispose()
        {
            ChildContent = null;
            LabelStyle = null;
            Parent = null;
        }
    }
}