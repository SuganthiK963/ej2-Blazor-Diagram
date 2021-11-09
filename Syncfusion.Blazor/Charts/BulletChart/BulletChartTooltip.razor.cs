using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Sets and gets the options for customizing the tooltip of the bullet chart component.
    /// </summary>
    /// <typeparam name="TValue">Represents the generic data type of the bullet chart control.</typeparam>
    public partial class BulletChartTooltip<TValue>
    {
        [CascadingParameter]
        internal SfBulletChart<TValue> Parent { get; set; }

        /// <summary>
        /// Sets and gets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Enables / Disables the visibility of the tooltip.
        /// </summary>
        [Parameter]
        public bool Enable { get; set; }

        /// <summary>
        /// Sets and gets the fill color of the tooltip that accepts value in hex and rgba as a valid CSS color string.
        /// </summary>
        [Parameter]
        public string Fill { get; set; }

        /// <summary>
        /// Sets and gets value of the tooltip template.
        /// </summary>
        [Parameter]
        public RenderFragment<TValue> Template { get; set; }

        internal BulletChartTooltipTextStyle TextStyle { get; set; }

        internal BulletChartTooltipBorder Border { get; set; }

        /// <summary>
        /// Specifies to update the dependent class value.
        /// </summary>
        /// <param name="key">Represents the class name.</param>
        /// <param name="keyValue">Represents the value of class.</param>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void UpdateChildProperties(string key, object keyValue)
        {
            if (!string.IsNullOrEmpty(key) && key.Equals("TextStyle", System.StringComparison.Ordinal))
            {
                TextStyle = (BulletChartTooltipTextStyle)keyValue;
            }
            else if (key.Equals("Border", System.StringComparison.Ordinal))
            {
                Border = (BulletChartTooltipBorder)keyValue;
            }
        }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.Tooltip = this;
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            ChildContent = null;
            TextStyle = null;
            Border = null;
            Template = null;
        }
    }
}