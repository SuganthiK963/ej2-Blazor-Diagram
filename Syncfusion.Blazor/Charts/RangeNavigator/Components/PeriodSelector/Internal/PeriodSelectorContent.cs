using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.DataVizCommon;
using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Charts.Internal
{
    /// <summary>
    /// Sets and gets the period selector for range navigator.
    /// </summary>
    public partial class PeriodSelectorContent : SfBaseComponent
    {
        private CultureInfo culture = CultureInfo.InvariantCulture;

        [CascadingParameter]
        internal SfRangeNavigator Parent { get; set; }

        /// <summary>
        /// Sets and gets the content of the UI element.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Method invoked after each time the component has been rendered.
        /// </summary>
        /// <param name="firstRender">Specifies the first render of the component.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                await base.OnAfterRenderAsync(firstRender);
                if (firstRender)
                {
                    ChildContent = RenderPeriodSelectorElements();
                    await InvokeAsync(StateHasChanged);
                }
            }
            catch
            {
                if (!IsDisposed)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Returns a flag to indicate whether the component should render.
        /// </summary>
        /// <returns>Returns boolean value.</returns>
        protected override bool ShouldRender()
        {
            return Parent != null && !Parent.IsValueUpdated;
        }

        private RenderFragment RenderPeriodSelectorElements() => builder =>
        {
            builder.OpenElement(SvgRendering.Seq++, "div");
            builder.AddAttribute(SvgRendering.Seq++, "id", Parent.Id + "_selector");
            string style = "position:absolute; left: " + (Parent.PeriodSelectorModule.PeriodSelectorSize.X + (Parent.ThemeStyle.ThumbWidth / 2)).ToString(culture) + "px; top: " + Parent.PeriodSelectorModule.PeriodSelectorSize.Y.ToString(culture) + "px; height: " +
                  Parent.PeriodSelectorModule.PeriodSelectorSize.Height.ToString(culture) + "px; width: " + (Parent.PeriodSelectorModule.PeriodSelectorSize.Width - Parent.ThemeStyle.ThumbWidth).ToString(culture) + "px";
            builder.AddAttribute(SvgRendering.Seq++, "style", style);
            builder.AddElementReferenceCapture(SvgRendering.Seq++, ins => { Parent.PeriodSelectorModule.Element = ins; });
            Parent.PeriodSelectorModule?.RenderSelector(builder);
            builder.CloseElement();
        };

        internal override void ComponentDispose()
        {
            Parent = null;
            ChildContent = null;
        }
    }
}