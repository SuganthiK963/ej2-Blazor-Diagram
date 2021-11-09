using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Internal;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Inputs.Slider.Internal
{
    /// <summary>
    /// Specifies the Partial class TickElement.
    /// </summary>
    public partial class TickElement : SfBaseComponent
    {
        internal const string CLASS = "class";

        internal const string TABINDEX = "tabindex";

        internal const string ROLE = "role";

        internal const string TITLE = "title";

        internal const string PRESENTATION = "presentation";

        internal const string ARIAHIDDEN = "aria-hidden";

        internal const string TRUE = "true";

        internal const string STYLE = "style";

        [CascadingParameter]
        internal SliderTicksRender Parent { get; set; }

        internal ElementReference TickRef { get; set; }

        /// <summary>
        /// Specifies the ClassName.
        /// </summary>
        [Parameter]
        public string ClassName { get; set; }

        /// <summary>
        /// Specifies the Styles.
        /// </summary>
        [Parameter]
        public string Styles { get; set; }

        /// <summary>
        /// Specifies the Value.
        /// </summary>
        [Parameter]
        public double Value { get; set; }

        /// <summary>
        /// Specifies the FormattedValue.
        /// </summary>
        [Parameter]
        public string FormattedValue { get; set; }

        /// <summary>
        /// Specifies the IsSmallTick.
        /// </summary>
        [Parameter]
        public bool IsSmallTick { get; set; }

        internal string TickContent { get; set; }

        internal Dictionary<string, object> Attributes { get; set; } = new Dictionary<string, object>();

        internal Dictionary<string, object> SpanAttributes { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        protected override void OnInitialized()
        {
            SfBaseUtils.UpdateDictionary(ROLE, PRESENTATION, Attributes);
            SfBaseUtils.UpdateDictionary(TABINDEX, "-1", Attributes);
            SfBaseUtils.UpdateDictionary(ARIAHIDDEN, TRUE, Attributes);
            SfBaseUtils.UpdateDictionary(CLASS, ClassName, Attributes);
            SfBaseUtils.UpdateDictionary(TITLE, TickContent, Attributes);
            SfBaseUtils.UpdateDictionary(STYLE, Styles, Attributes);
            SfBaseUtils.UpdateDictionary(ROLE, PRESENTATION, SpanAttributes);
            SfBaseUtils.UpdateDictionary(TABINDEX, "-1", SpanAttributes);
            SfBaseUtils.UpdateDictionary(ARIAHIDDEN, TRUE, SpanAttributes);
            TickContent = FormattedValue;
            base.OnInitialized();
        }

        /// <summary>
        /// Method invoked after each time the component has been rendered.
        /// </summary>
        /// <param name="firstRender">Set to true for the first time component rendering; otherwise gets false.</param>
        /// <returns>""Task".</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Parent?.TicksRef.Add(TickRef);
                SliderTickEventArgs data = await Parent.Parent.TriggeredTicksRendering(TickRef, FormattedValue, Value, Attributes);
                TickContent = data.Text;
                Attributes = data.HtmlAttributes;
                StateHasChanged();
            }
        }
    }
}
