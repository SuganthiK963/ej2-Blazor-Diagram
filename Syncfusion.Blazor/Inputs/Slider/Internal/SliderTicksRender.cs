using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Internal;
using System.ComponentModel;
using System.Globalization;

namespace Syncfusion.Blazor.Inputs.Slider.Internal
{
    /// <summary>
    /// This class is used to render ticks to slider component.
    /// </summary>
    public partial class SliderTicksRender : SfBaseComponent
    {
        internal const string FIRSTTICK = "e-tick e-large e-first-tick";
        internal const string LASTTICK = "e-tick e-large e-last-tick";
        internal const string LARGETICK = "e-tick e-large";
        internal const string CLASS = "class";
        internal const string TABINDEX = "tabindex";
        internal const string ROLE = "role";
        internal const string TITLE = "title";
        internal const string PRESENTATION = "presentation";
        internal const string ARIAHIDDEN = "aria-hidden";
        internal const string TRUE = "true";
        internal const string TICK = " e-tick-";
        internal const string STYLE = "style";
        internal const string EHSCALE = " e-h-scale";
        internal const string EVSCALE = " e-v-scale";
        internal const string PERCENTAGE = "%";
        internal const int ZERO = 0;
        internal const int ONE = 1;
        internal const int TWO = 2;
        internal const int THREE = 3;
        internal const int FOUR = 4;

        private int totalTicks;
        private double listItemElementWidth;
        private double[] ticksValue;
        private double[] largeTicksValue;
        private int bigNum;
        private int tickCount;

        internal string[] FormattedValue { get; set; }

        internal ElementReference TicksWrapperRef { get; set; }

        internal List<ElementReference> TicksRef { get; set; } = new List<ElementReference>();

        internal Dictionary<string, object> Attributes { get; set; }

        internal string TickValue { get; set; } = "e-tick-value e-tick-";

        internal bool IsCallEvent { get; set; } = true;

        /// <exclude/>
        /// <summary>
        /// Public fiels ChildContent.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [CascadingParameter]
        internal ISlider Parent { get; set; }

        /// <summary>
        /// Class for the Tick.
        /// </summary>
        public string TicksClass { get; set; } = "e-scale";

        /// <summary>
        /// Method invoked after each time the component has been rendered.
        /// </summary>
        /// <param name="firstRender">Set to true for the first time component rendering; otherwise gets false.</param>
        /// <returns>"Task".</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!firstRender && IsCallEvent)
            {
                IsCallEvent = false;
                SliderTickRenderedEventArgs data = await Parent?.TriggeredTicksRendered(TicksWrapperRef, Attributes);
                Attributes = data.HtmlAttributes;
                StateHasChanged();
            }
        }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>"Task".</returns>
        protected override async Task OnInitializedAsync()
        {
            TickValue += Parent.Ticks.Placement.ToString().ToLower(CultureInfo.CurrentCulture);
            await base.OnInitializedAsync();
            TicksClass = Parent.Orientation == SliderOrientation.Horizontal ? TicksClass + EHSCALE : TicksClass + EVSCALE;
            if (Parent.CustomValues == null)
            {
                TicksCalculation();
            }
            else
            {
                CustomTicksCalculation();
            }

            Attributes = new Dictionary<string, object>()
            {
                { ROLE, PRESENTATION },
                { TABINDEX, "-1" },
                { ARIAHIDDEN, TRUE },
                { CLASS, TicksClass + TICK + Parent.Ticks.Placement.ToString().ToLower(CultureInfo.CurrentCulture) },
                { STYLE, "z-index:-1; list-style:none;" }
            };
        }

        // used to get the maximum width of tick element
        private static string FindMaxWidth(string tickValue)
        {
            string ticksWidth = string.Empty;
            if (tickValue.Contains(',', StringComparison.Ordinal))
            {
                ticksWidth = tickValue.Replace(',', '.');
                ticksWidth = ticksWidth + PERCENTAGE;
            }
            else
            {
                ticksWidth = tickValue + PERCENTAGE;
            }

            return ticksWidth;
        }

        private void TicksCalculation()
        {
            string small = Parent.Ticks.SmallStep.ToString(CultureInfo.CurrentCulture);
            string large = Parent.Ticks.LargeStep.ToString(CultureInfo.CurrentCulture);
            string min = Parent.Min.ToString(CultureInfo.CurrentCulture);
            string max = Parent.Max.ToString(CultureInfo.CurrentCulture);
            double difference;

            // calculate largest decimal value
            int afterDecimal = small.Length - small.IndexOf('.', StringComparison.Ordinal);
            if (afterDecimal < (large.Length - large.IndexOf('.', StringComparison.Ordinal)))
            {
                afterDecimal = large.Length - large.IndexOf('.', StringComparison.Ordinal);
            }

            if (afterDecimal < (min.Length - min.IndexOf('.', StringComparison.Ordinal)))
            {
                afterDecimal = min.Length - min.IndexOf('.', StringComparison.Ordinal);
            }

            if (afterDecimal < (max.Length - max.IndexOf('.', StringComparison.Ordinal)))
            {
                afterDecimal = max.Length - max.IndexOf('.', StringComparison.Ordinal);
            }

            // calculate total tick's count
            if (Parent.Ticks.ShowSmallTicks)
            {
                difference = (Parent.Max - Parent.Min) / Parent.Ticks.SmallStep;
            }
            else
            {
                difference = (Parent.Max - Parent.Min) / Parent.Ticks.LargeStep;
            }

            totalTicks = (int)difference;
            if (totalTicks < ((Parent.Max - Parent.Min) / Parent.Ticks.SmallStep))
            {
                if (difference - totalTicks >= 0.5)
                {
                    totalTicks = totalTicks + 1;
                }
            }

            // stored ticks details.
            ticksValue = new double[totalTicks + 1];
            FormattedValue = new string[totalTicks + 1];
            string stringTicksValue;
            double i = Parent.Ticks.ShowSmallTicks ? (double)(Parent.Min / Parent.Ticks.SmallStep) : (double)(Parent.Min / Parent.Ticks.LargeStep);
            int j = 0;
            while (j <= totalTicks)
            {
                if (!Parent.Ticks.ShowSmallTicks)
                {
                    ticksValue[j] = Parent.Ticks.LargeStep * i;
                }
                else
                {
                    ticksValue[j] = Math.Round(Parent.Ticks.SmallStep * i, 5);
                }

                try
                {
                    string tickValue = ticksValue[j].ToString(CultureInfo.CurrentCulture);
                    if (tickValue.Contains('.', StringComparison.Ordinal))
                    {
                        if ((tickValue.IndexOf('.', StringComparison.Ordinal) + afterDecimal) < tickValue.Length)
                        {
                            stringTicksValue = tickValue.Substring(0, tickValue.IndexOf('.', StringComparison.Ordinal) + afterDecimal);
                            _ = double.TryParse(stringTicksValue, out ticksValue[j]);
                            FormattedValue[j] = Intl.GetNumericFormat<double>(ticksValue[j], Parent.Ticks.Format);
                        }
                        else
                        {
                            stringTicksValue = tickValue;
                            FormattedValue[j] = Intl.GetNumericFormat<double>(ticksValue[j], Parent.Ticks.Format);
                        }
                    }
                    else
                    {
                        stringTicksValue = tickValue;
                        FormattedValue[j] = Intl.GetNumericFormat<double>(ticksValue[j], Parent.Ticks.Format);
                    }
                }
                catch (Exception e)
                {
                    throw new InvalidOperationException("Error", e);
                }

                i++;
                j++;
            }

            int largeTicks = (int)((Parent.Max - Parent.Min) / Parent.Ticks.LargeStep);
            if (largeTicks <= ((Parent.Max - Parent.Min) / Parent.Ticks.LargeStep))
            {
                largeTicks = largeTicks + 1;
            }

            // large tick details
            largeTicksValue = new double[largeTicks + 1];
            string stringlargeTickValue;
            double x = (double)(Parent.Min / Parent.Ticks.LargeStep);
            int y = 0;
            while (y <= largeTicks)
            {
                largeTicksValue[y] = Parent.Ticks.LargeStep * Math.Round(x, afterDecimal);
                if ((largeTicksValue[y].ToString(CultureInfo.CurrentCulture).IndexOf('.', StringComparison.Ordinal) + afterDecimal) < largeTicksValue[y].ToString(CultureInfo.CurrentCulture).Length)
                {
                    stringlargeTickValue = largeTicksValue[y].ToString(CultureInfo.CurrentCulture).Substring(0, largeTicksValue[y].ToString(CultureInfo.CurrentCulture).IndexOf('.', StringComparison.Ordinal) + afterDecimal);
                    _ = double.TryParse(stringlargeTickValue, out largeTicksValue[y]);
                }
                else
                {
                    stringlargeTickValue = largeTicksValue[y].ToString(CultureInfo.CurrentCulture);
                }

                x++;
                y++;
            }

            listItemElementWidth = 100F / difference;
        }

        // get the custom ticks count
        private void CustomTicksCalculation()
        {
            if (Parent.CustomValues != null)
            {
                // based on customValues we have provide some fixed small ticks between two large ticks
                bigNum = Parent.CustomValues.Length - 1;
                tickCount = FOUR;
                if (bigNum > 28)
                {
                    tickCount = ZERO;
                }
                else if (bigNum > 14)
                {
                    tickCount = ONE;
                }
                else if (bigNum > 7)
                {
                    tickCount = TWO;
                }
                else if (bigNum > 4)
                {
                    tickCount = THREE;
                }

                totalTicks = (bigNum * tickCount) + bigNum;
            }
        }
    }
}