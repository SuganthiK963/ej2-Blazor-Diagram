using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Sets and gets the options to customize the navigator thumb settings.
    /// </summary>
    public partial class RangeNavigatorThumbSettings
    {
        private string fill;
        private double height;
        private ThumbType type;
        private double width;

        [CascadingParameter]
        internal RangeNavigatorStyleSettings Parent { get; set; }

        [CascadingParameter]
        internal SfRangeNavigator BaseParent { get; set; }

        /// <summary>
        /// Sets and gets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        internal RangeNavigatorThumbBorder Border { get; set; } = new RangeNavigatorThumbBorder();

        /// <summary>
        /// Set and gets the fill color for the thumb.
        /// </summary>
        [Parameter]
        public string Fill { get; set; }

        /// <summary>
        /// Get and sets the height of thumb.
        /// </summary>
        [Parameter]
        public double Height { get; set; } = double.NaN;

        /// <summary>
        /// Get and sets the type of thumb.
        /// </summary>
        [Parameter]
        public ThumbType Type { get; set; } = ThumbType.Circle;

        /// <summary>
        /// Get and sets the width of thumb.
        /// </summary>
        [Parameter]
        public double Width { get; set; } = double.NaN;

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.Thumb = this;
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            height = NotifyPropertyChanges(nameof(Height), Height, height);
            width = NotifyPropertyChanges(nameof(Width), Width, width);
            fill = NotifyPropertyChanges(nameof(Fill), Fill, fill);
            type = NotifyPropertyChanges(nameof(Type), Type, type);
            if (PropertyChanges.Any() && IsRendered)
            {
                SfBaseUtils.UpdateDictionary("Thumb", this, BaseParent.PropertyChanges);
                PropertyChanges.Clear();
                await BaseParent.OnRangeNaivgatorParametersSet();
            }
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            ChildContent = null;
            Border = null;
        }
    }
}