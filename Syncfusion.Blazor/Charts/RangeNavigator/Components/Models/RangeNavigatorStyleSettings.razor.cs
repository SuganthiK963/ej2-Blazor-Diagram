using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Sets and gets the options to customize the navigator style settings for range navigator.
    /// </summary>
    public partial class RangeNavigatorStyleSettings
    {
        private string selectedRegionColor;
        private string unselectedRegionColor;

        [CascadingParameter]
        internal SfRangeNavigator Parent { get; set; }

        /// <summary>
        /// Sets and gets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Defines the selected region color for selected area.
        /// </summary>
        [Parameter]
        public string SelectedRegionColor { get; set; }

        internal RangeNavigatorThumbSettings Thumb { get; set; } = new RangeNavigatorThumbSettings();

        /// <summary>
        /// Un Selected region color for un selected area.
        /// </summary>
        [Parameter]
        public string UnselectedRegionColor { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.UpdateChildProperties("NavigatorStyleSettings", this);
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            selectedRegionColor = NotifyPropertyChanges(nameof(SelectedRegionColor), SelectedRegionColor, selectedRegionColor);
            unselectedRegionColor = NotifyPropertyChanges(nameof(UnselectedRegionColor), UnselectedRegionColor, unselectedRegionColor);
            if (PropertyChanges.Any() && IsRendered)
            {
                SfBaseUtils.UpdateDictionary("NavigatorStyleSettings", this, Parent.PropertyChanges);
                PropertyChanges.Clear();
                await Parent.OnRangeNaivgatorParametersSet();
            }
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            ChildContent = null;
            Thumb = null;
        }
    }
}