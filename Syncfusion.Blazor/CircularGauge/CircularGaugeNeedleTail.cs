using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Syncfusion.Blazor.CircularGauge
{
    /// <summary>
    /// Gets or sets the tail of needle pointer in an axis.
    /// </summary>
    public partial class CircularGaugeNeedleTail
    {
        private string color;
        private string length;

        /// <summary>
        /// Gets or sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Gets or sets the color for the needle pointer in the circular gauge component.
        /// </summary>
        [Parameter]
        public string Color { get; set; }

        /// <summary>
        /// Gets or sets the length of the needle in pixels or in percentage in circular gauge component.
        /// </summary>
        [Parameter]
        public string Length { get; set; } = "0%";

        /// <summary>
        /// Gets or sets the properties of pointers.
        /// </summary>
        [CascadingParameter]
        internal CircularGaugePointer Parent { get; set; }

        /// <summary>
        /// Gets or sets the properties of circular gauges.
        /// </summary>
        [CascadingParameter]
        internal SfCircularGauge BaseParent { get; set; }

        /// <summary>
        /// Gets or sets the border of the needle tail.
        /// </summary>
        internal CircularGaugeNeedleTailBorder Border { get; set; }

        /// <summary>
        /// Gets or sets the linear gradient to the needle tail.
        /// </summary>
        internal LinearGradient LinearGradient { get; set; }

        /// <summary>
        /// Gets or sets the radial gradient to the needle tail.
        /// </summary>
        internal RadialGradient RadialGradient { get; set; }

        /// <summary>
        /// UpdateChildProperties is used to update the child properties.
        /// </summary>
        /// <param name="key">represents the key.</param>
        /// <param name="keyValue">represents the key value.</param>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void UpdateChildProperties(string key, object keyValue)
        {
            if (key == "Border")
            {
                Border = (CircularGaugeNeedleTailBorder)keyValue;
            }
            else if (key == "LinearGradient")
            {
                LinearGradient = (LinearGradient)keyValue;
            }
            else if (key == "RadialGradient")
            {
                RadialGradient = (RadialGradient)keyValue;
            }
        }

        /// <summary>
        /// Disposes the property values during the destroy of the component that is hold up for the execution of the component.
        /// </summary>
        internal override void ComponentDispose()
        {
            Parent = null;
            BaseParent = null;
            ChildContent = null;
            Border = null;
            LinearGradient = null;
            RadialGradient = null;
            Color = color = null;
            Length = length = null;
        }

        /// <summary>
        /// OnInitializedAsync method is called when the component has received its initial parameters.
        /// </summary>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.UpdateChildProperties("NeedleTail", this);
        }

        /// <summary>
        /// OnParametersSetAsync is a lifecycle method that is invoked when the component has received parameters, and the incoming values have been assigned to the properties.
        /// </summary>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            color = NotifyPropertyChanges(nameof(Color), Color, color);
            length = NotifyPropertyChanges(nameof(Length), Length, length);

            if (PropertyChanges.Count > 0)
            {
                BaseParent.PropertyChangeHandler();
                PropertyChanges.Clear();
            }
        }
    }
}
