using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Syncfusion.Blazor.CircularGauge
{
    /// <summary>
    /// Gets or sets the cap of pointer in an axis.
    /// </summary>
    public partial class CircularGaugeCap : SfBaseComponent
    {
        private double radius;
        private string color;

        /// <summary>
        /// Gets or sets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Gets or sets the color for the pointer cap in the circular gauge component.
        /// </summary>
        [Parameter]
        public string Color { get; set; }

        /// <summary>
        /// Gets or sets the radius of pointer cap in the circular gauge component.
        /// </summary>
        [Parameter]
        public double Radius { get; set; } = 8;

        /// <summary>
        ///  Gets or sets the properties of the pointer.
        /// </summary>
        [CascadingParameter]
        internal CircularGaugePointer Parent { get; set; }

        /// <summary>
        ///  Gets or sets the properties of the circular gauge.
        /// </summary>
        [CascadingParameter]
        internal SfCircularGauge BaseParent { get; set; }

        /// <summary>
        /// Gets or sets the border of the cap.
        /// </summary>
        internal CircularGaugeCapBorder Border { get; set; }

        /// <summary>
        /// Gets or sets the linear gradient for the cap.
        /// </summary>
        internal LinearGradient LinearGradient { get; set; }

        /// <summary>
        /// Gets or sets the radial gradient for the cap.
        /// </summary>
        internal RadialGradient RadialGradient { get; set; }

        /// <summary>
        /// UpdateChildProperties is used to update the child properties.
        /// </summary>
        /// <param name="key">represents the keys.</param>
        /// <param name="keyValue">represents the key values.</param>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void UpdateChildProperties(string key, object keyValue)
        {
            if (key == "Border")
            {
                Border = (CircularGaugeCapBorder)keyValue;
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
            LinearGradient = null;
            RadialGradient = null;
            Border = null;
        }

        /// <summary>
        /// OnInitializedAsync method is called when the component has received its initial parameters.
        /// </summary>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.UpdateChildProperties("Cap", this);
        }

        /// <summary>
        /// OnParametersSetAsync is a lifecycle method that is invoked when the component has received parameters, and the incoming values have been assigned to the properties.
        /// </summary>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            color = NotifyPropertyChanges(nameof(Color), Color, color);
            radius = NotifyPropertyChanges(nameof(Radius), Radius, radius);

            if (PropertyChanges.Count > 0)
            {
                BaseParent.PropertyChangeHandler();
                PropertyChanges.Clear();
            }
        }
    }
}
