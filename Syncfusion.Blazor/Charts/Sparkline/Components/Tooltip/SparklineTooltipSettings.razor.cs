using Microsoft.AspNetCore.Components;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Sets and gets the options for customizing the sparkline tooltip.
    /// </summary>
    /// <typeparam name="TValue">Represents the generic data type of tooltip in sparkline component.</typeparam>
    public partial class SparklineTooltipSettings<TValue>
    {
        private string fill;
        private string format;
        private bool visible;

        [CascadingParameter]
        internal SfSparkline<TValue> Parent { get; set; }

        /// <summary>
        /// Sets and gets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Sets and gets the tooltip fill color.
        /// </summary>
        [Parameter]
        public string Fill { get; set; }

        /// <summary>
        /// Sets and gets the tooltip text format.
        /// </summary>
        [Parameter]
        public string Format { get; set; }

        /// <summary>
        /// Sets and gets the tooltip template.
        /// </summary>
        [Parameter]
        public RenderFragment<TValue> Template { get; set; }

        /// <summary>
        /// Sets and gets the tooltip visibility.
        /// </summary>
        [Parameter]
        public bool Visible { get; set; }

        internal SparklineTooltipTextStyle TextStyle { get; set; }

        internal SparklineTrackLineSettings TrackLineSettings { get; set; }

        internal SparklineTooltipBorder Border { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.TooltipSettings = this;
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            fill = NotifyPropertyChanges(nameof(Fill), Fill, fill);
            format = NotifyPropertyChanges(nameof(Format), Format, format);
            visible = NotifyPropertyChanges(nameof(Visible), Visible, visible);
        }

        /// <summary>
        /// Specifies to update the dependent class value.
        /// </summary>
        /// <param name="key">Represents the class name.</param>
        /// <param name="keyValue">Represents the value of class.</param>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void UpdateChildProperties(string key, object keyValue)
        {
            if (!string.IsNullOrEmpty(key) && key.Equals(nameof(SparklineTrackLineSettings), StringComparison.Ordinal))
            {
                TrackLineSettings = (SparklineTrackLineSettings)keyValue;
            }
            else if (key.Equals(nameof(SparklineTooltipBorder), StringComparison.Ordinal))
            {
                Border = (SparklineTooltipBorder)keyValue;
            }
            else if (key.Equals(nameof(SparklineTooltipTextStyle), StringComparison.Ordinal))
            {
                TextStyle = (SparklineTooltipTextStyle)keyValue;
            }
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            ChildContent = null;
            Template = null;
            TextStyle = null;
            TrackLineSettings = null;
            Border = null;
        }
    }
}