using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Inputs
{
    /// <summary>
    /// It is used to denote the Slider Change/Changed Event arguments.
    /// </summary>
    /// <typeparam name="T">"T".</typeparam>
    public class SliderChangeEventArgs<T>
    {
        /// <summary>
        /// It is used to get the action applied on the Slider.
        /// </summary>
        [Obsolete("This argument is deprecated and no longer be available. use Name argument as replacement")]
        public string Action { get; set; }

        internal string SfSliderAction { get; set; }

        /// <summary>
        /// It is used to get the action applied on the Slider.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// It is used to check whether the event triggered is via user or programmatic way.
        /// </summary>
        public bool IsInteracted { get; set; }

        /// <summary>
        /// It is used to get the previous value of the Slider.
        /// </summary>
        public T PreviousValue { get; set; }

        /// <summary>
        /// It is used to get the current text or formatted text of the Slider, which is placed in tooltip.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// It is used to get the current value of the Slider.
        /// </summary>
        public T Value { get; set; }
    }

    /// <summary>
    /// It is used to denote the TicksRender event arguments.
    /// </summary>
    public class SliderTickEventArgs
    {
        /// <summary>
        /// It is used to get the label text of the tick.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// It is used to get the current tick element.
        /// </summary>
        [Obsolete("This argument is deprecated and no longer be available. Use TickElementRef as replacement")]
        public DOM TickElement { get; set; }

        /// <summary>
        /// It is used to get the current tick element reference.
        /// </summary>
        public ElementReference TickElementRef { get; set; }

        /// <summary>
        /// It is used to get the value of the tick.
        /// </summary>
        public double Value { get; set; }

        /// <summary>
        /// It is used to customize tick elements.
        /// </summary>
        public Dictionary<string, object> HtmlAttributes { get; set; }
    }

    /// <summary>
    /// It is used to denote the ticks rendered Event arguments.
    /// </summary>
    public class SliderTickRenderedEventArgs
    {
        /// <summary>
        /// It returns the collection of tick elements.
        /// </summary>
        [Obsolete("This argument is deprecated and no longer be available. Use TickElementsRef for replacement")]
        public List<DOM> TickElements { get; set; }

        /// <summary>
        /// It returns the collection of tick elements as a reference.
        /// </summary>
        public List<ElementReference> TickElementsRef { get; set; }

        /// <summary>
        /// It returns the container of the ticks element.
        /// </summary>
        [Obsolete("This property is deprecated and no longer be available, Use TicksContainerRef as replacement")]
        public DOM TicksWrapper { get; set; }

        /// <summary>
        /// It returns the container of the ticks element as a reference.
        /// </summary>
        public ElementReference TicksContainerRef { get; set; }

        /// <summary>
        /// It is used to customize ticks parent element.
        /// </summary>
        public Dictionary<string, object> HtmlAttributes { get; set; }
    }

    /// <summary>
    /// It is used to denote the TooltipChange Event arguments.
    /// </summary>
    /// <typeparam name="T">"T".</typeparam>
    public class SliderTooltipEventArgs<T>
    {
        /// <summary>
        /// It is used to get the text shown in the Slider tooltip.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// It is used to get the value of the Slider.
        /// </summary>
        public T Value { get; set; }
    }

    /// <summary>
    /// class for holding ColorRange properties arguments.
    /// </summary>
    public class ColorRangeDataModel
    {
        /// <summary>
        /// It is used to set the color in the slider bar.
        /// </summary>
        public string Color { get; set; } = string.Empty;

        /// <summary>
        /// It is used to get the end value for applying color.
        /// </summary>
        public double End { get; set; }

        /// <summary>
        /// It is used to get the starting value for applying color.
        /// </summary>
        public double Start { get; set; }
    }

    /// <summary>
    /// Class that holds the persistence property details.
    /// </summary>
    internal class PersistenceValues<TValue>
    {
        public TValue Value { get; set; }
    }

    /// <summary>
    /// Class that holds the tick's position.
    /// </summary>
    internal class TicksValues
    {
        public string FirstTickPosition { get; set; }

        public string OtherTickPosition { get; set; }
    }

    /// <summary>
    /// Class that holds the previous and currentValue of slider component.
    /// </summary>
    /// <typeparam name="TValue">"TValue".</typeparam>
    /// <exclude/>
    public class ChangeEventData<TValue>
    {
        /// <summary>
        /// Specifies the PreviousValue.
        /// </summary>
        public TValue PreviousValue { get; set; }

        /// <summary>
        /// Specifies the Value.
        /// </summary>
        public TValue Value { get; set; }

        /// <summary>
        /// Specifies the isValueChanged.
        /// </summary>
        public bool IsValueChanged { get; set; }
    }
}