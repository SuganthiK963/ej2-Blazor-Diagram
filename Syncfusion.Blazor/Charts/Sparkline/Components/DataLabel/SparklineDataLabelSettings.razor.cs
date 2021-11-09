using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using System.Linq;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Sets and gets the options for customizing the data label settings of the sparkline component.
    /// </summary>
    public partial class SparklineDataLabelSettings
    {
        private EdgeLabelMode edgeLabelMode;
        private string fill;
        private string format;
        private double opacity;
        private List<VisibleType> visible;

        [CascadingParameter]
        internal ISparkline Parent { get; set; }

        /// <summary>
        /// Sets and gets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Sets and gets the edge placement of the data label.
        /// </summary>
        [Parameter]
        public EdgeLabelMode EdgeLabelMode { get; set; } = EdgeLabelMode.None;

        /// <summary>
        /// Sets and gets the dataLabel fill color.
        /// </summary>
        [Parameter]
        public string Fill { get; set; } = "transparent";

        /// <summary>
        /// Sets and gets to configure the data label format.
        /// </summary>
        [Parameter]
        public string Format { get; set; } = string.Empty;

        /// <summary>
        /// Sets and gets the data label opacity.
        /// </summary>
        [Parameter]
        public double Opacity { get; set; } = 1;

        /// <summary>
        /// Sets and gets the data label visibility type.
        /// </summary>
        [Parameter]
        public List<VisibleType> Visible { get; set; } = new List<VisibleType>();

        internal SparklineFont TextStyle { get; set; }

        internal SparklineDataLabelBorder Border { get; set; } = new SparklineDataLabelBorder();

        internal SparklineDataLabelOffset Offset { get; set; } = new SparklineDataLabelOffset();

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.DataLabelSettings = this;
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            edgeLabelMode = NotifyPropertyChanges(nameof(EdgeLabelMode), EdgeLabelMode, edgeLabelMode);
            fill = NotifyPropertyChanges(nameof(Fill), Fill, fill);
            format = NotifyPropertyChanges(nameof(Format), Format, format);
            opacity = NotifyPropertyChanges(nameof(Opacity), Opacity, opacity);
            visible = NotifyPropertyChanges(nameof(Visible), Visible, visible);
            if (PropertyChanges.Any())
            {
                await Parent.OnPropertyChanged(PropertyChanges, nameof(SparklineDataLabelSettings));
                PropertyChanges.Clear();
            }
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            ChildContent = null;
            Visible = visible = null;
            TextStyle = null;
            Border = null;
            Offset = null;
        }
    }
}