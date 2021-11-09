using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Diagram.Internal;
using Syncfusion.Blazor.Internal;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Diagram
{
    /// <summary>
    /// Specifies the extra space around the outer boundaries of an element.
    /// </summary>
    public class DiagramMargin : SfBaseComponent
    {
        private double left;
        private double right;
        private double top;
        private double bottom;

        /// <summary>
        /// Specifies the callback to trigger when the left values changes.
        /// </summary>
        [JsonIgnore]
        [Parameter]
        public EventCallback<double> LeftChanged { get; set; }
        /// <summary>
        /// Specifies the callback to trigger when the right values changes.
        /// </summary>
        [JsonIgnore]
        [Parameter]
        public EventCallback<double> RightChanged { get; set; }
        /// <summary>
        /// Specifies the callback to trigger when the top values changes.
        /// </summary>
        [JsonIgnore]
        [Parameter]
        public EventCallback<double> TopChanged { get; set; }
        /// <summary>
        /// Specifies the callback to trigger when the bottom values changes.
        /// </summary>
        [JsonIgnore]
        [Parameter]
        public EventCallback<double> BottomChanged { get; set; }

        /// <summary>
        /// Gets or sets the extra space at the left side of an element.
        /// </summary>
        [Parameter]
        [JsonPropertyName("left")]
        public double Left { get; set; }

        /// <summary>
        /// Gets or sets the extra space at the right side of an element.
        /// </summary>
        [Parameter]
        [JsonPropertyName("right")]
        public double Right { get; set; }

        /// <summary>
        /// Gets or sets the extra space at the top side of an element.
        /// </summary>
        [Parameter]
        [JsonPropertyName("top")]
        public double Top { get; set; }

        /// <summary>
        /// Gets or sets the extra space at the bottom of an element.
        /// </summary>
        [Parameter]
        [JsonPropertyName("bottom")]
        public double Bottom { get; set; }
        /// <summary>
        /// Creates a new instance of the <see cref="DiagramMargin"/> from the given <see cref="DiagramMargin"/>.
        /// </summary>
        /// <param name="src">DiagramMargin</param>
        public DiagramMargin(DiagramMargin src)
        {
            if (src != null)
            {
                LeftChanged = src.LeftChanged;
                RightChanged = src.RightChanged;
                TopChanged = src.TopChanged;
                BottomChanged = src.BottomChanged;
                left = src.left;
                Left = src.Left;
                right = src.right;
                Right = src.Right;
                top = src.top;
                Top = src.Top;
                bottom = src.bottom;
                Bottom = src.Bottom;
            }
        }
        /// <summary>
        /// Initializes a new instance of the DiagramMargin.
        /// </summary>
        public DiagramMargin() : base()
        {

        }
        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync().ConfigureAwait(true);
            left = Left;
            right = Right;
            top = Top;
            bottom = Bottom;
        }
        /// <summary>
        /// Method invoked when any changes in component state occurs.
        /// </summary>
        /// <returns>="Task".</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync().ConfigureAwait(true);
            left = BaseUtil.UpdateDictionary(nameof(Left), left, Left, PropertyChanges);
            right = BaseUtil.UpdateDictionary(nameof(Right), right, Right, PropertyChanges);
            top = BaseUtil.UpdateDictionary(nameof(Top), top, Top, PropertyChanges);
            bottom = BaseUtil.UpdateDictionary(nameof(Bottom), bottom, Bottom, PropertyChanges);
        }
        internal async Task<DiagramMargin> PropertyUpdate(DiagramMargin margin)
        {
            Left = await SfBaseUtils.UpdateProperty(margin.Left, Left, LeftChanged, null, null);
            Right = await SfBaseUtils.UpdateProperty(margin.Right, Right, RightChanged, null, null);
            Top = await SfBaseUtils.UpdateProperty(margin.Top, Top, TopChanged, null, null);
            Bottom = await SfBaseUtils.UpdateProperty(margin.Bottom, Bottom, BottomChanged, null, null);
            return this;
        }
        /// <summary>
        /// Creates a new margin that is a copy of the current margin.
        /// </summary>
        /// <returns>DiagramMargin</returns>
        public object Clone()
        {
            return new DiagramMargin(this);
        }
        /// <summary>
        /// This method releasing all unmanaged resources.
        /// </summary>
        public new void Dispose()
        {
            left = Left = 0;
            right = Right = 0;
            top = Top = 0;
            bottom = Bottom = 0;
        }
    }
}
