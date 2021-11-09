using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Inputs.Slider.Internal;

namespace Syncfusion.Blazor.Inputs
{
    /// <summary>
    /// This class is used to set limit bar to slider component.
    /// </summary>
    public partial class SliderLimits : SfBaseComponent
    {
        internal const string LIMITBAR = " e-limit-bar";
        internal const string LIMITFIRST = " e-limit-first";
        internal const string LIMITS = "limits";

        internal string LimitsClass { get; set; } = "e-limits";

        /// <exclude/>
        [CascadingParameter]
        internal ISlider Parent { get; set; }

        /// <summary>
        /// Specifies the ChildContent.
        /// </summary>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// It is used to enable the limit in the slider.
        /// </summary>
        [Parameter]
        public bool Enabled { get; set; }

        private bool SliderEnabled { get; set; }

        /// <summary>
        /// It is used to lock the second handle.
        /// </summary>
        [Parameter]
        public bool EndHandleFixed { get; set; }

        private bool SliderEndHandleFixed { get; set; }

        /// <summary>
        /// It is used to set the maximum end limit value.
        /// </summary>
        [Parameter]
        public double? MaxEnd { get; set; }

        private double? SliderMaxEnd { get; set; }

        /// <summary>
        /// It is used to set the maximum start limit value.
        /// </summary>
        [Parameter]
        public double? MaxStart { get; set; }

        private double? SliderMaxStart { get; set; }

        /// <summary>
        /// It is used to set the minimum end limit value.
        /// </summary>
        [Parameter]
        public double? MinEnd { get; set; }

        private double? SliderMinEnd { get; set; }

        /// <summary>
        /// It is used to set the minimum start limit value.
        /// </summary>
        [Parameter]
        public double? MinStart { get; set; }

        private double? SliderMinStart { get; set; }

        /// <summary>
        /// It is used to lock the first handle.
        /// </summary>
        [Parameter]
        public bool StartHandleFixed { get; set; }

        private bool SliderStartHandleFixed { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>"Task".</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            SliderEnabled = Enabled;
            SliderEndHandleFixed = EndHandleFixed;
            SliderMaxEnd = MaxEnd;
            SliderMaxStart = MaxStart;
            SliderMinEnd = MinEnd;
            SliderMinStart = MinStart;
            SliderStartHandleFixed = StartHandleFixed;
            LimitsClass += (Parent?.Type != SliderType.Range) ? LIMITBAR : LIMITFIRST;
            Parent?.UpdateChildProperties(LIMITS, this);
        }

        /// <summary>
        /// Method invoked when any changes in component state occurs.
        /// </summary>
        /// <returns>"Task".</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            SliderEnabled = NotifyPropertyChanges(nameof(Enabled), Enabled, SliderEnabled);
            SliderStartHandleFixed = NotifyPropertyChanges(nameof(StartHandleFixed), StartHandleFixed, SliderStartHandleFixed);
            SliderEndHandleFixed = NotifyPropertyChanges(nameof(EndHandleFixed), EndHandleFixed, SliderEndHandleFixed);
            SliderMinStart = NotifyPropertyChanges(nameof(MinStart), MinStart, SliderMinStart);
            SliderMinEnd = NotifyPropertyChanges(nameof(MinEnd), MinEnd, SliderMinEnd);
            SliderMaxStart = NotifyPropertyChanges(nameof(MaxStart), MaxStart, SliderMaxStart);
            SliderMaxEnd = NotifyPropertyChanges(nameof(MaxEnd), MaxEnd, SliderMaxEnd);
            await DynamicPropertyChange();
        }

        /// <summary>
        /// Method invoked after each time the component has been rendered.
        /// </summary>
        /// <param name="firstRender">Set to true for the first time component rendering; otherwise gets false.</param>
        /// <returns>"Task".</returns>
        protected override Task OnAfterRenderAsync(bool firstRender)
        {
            Parent?.UpdateChildProperties(LIMITS, this);
            return base.OnAfterRenderAsync(firstRender);
        }

        internal async Task DynamicPropertyChange()
        {
            if (PropertyChanges.Count > 0)
            {
                Parent?.UpdateChildProperties(LIMITS, this);
                await InvokeMethod("sfBlazor.Slider.updateLimitData", Parent.Slider, Parent.GetLimitData());
            }
        }
    }
}