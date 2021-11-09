using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Inputs.Slider.Internal
{
    /// <summary>
    /// Interface for holding slider properties.
    /// </summary>
    public interface ISlider
    {
        /// <summary>
        /// Get and set the Slider.
        /// </summary>
        public ElementReference Slider { get; set; }

        /// <summary>
        /// Get and set the ColorRange.
        /// </summary>
        public List<ColorRangeDataModel> ColorRange { get; set; }

        /// <summary>
        /// Get and set the CssClass.
        /// </summary>
        public string CssClass { get; set; }

        /// <summary>
        /// Get and set the CustomValues.
        /// </summary>
        public string[] CustomValues { get; set; }

        /// <summary>
        /// Get and set the EnableAnimation.
        /// </summary>
        public bool EnableAnimation { get; set; }

        /// <summary>
        /// Get and set the EnableHtmlSanitizer.
        /// </summary>
        public bool EnableHtmlSanitizer { get; set; }

        /// <summary>
        /// Get and set the EnablePersistence.
        /// </summary>
        public bool EnablePersistence { get; set; }

        /// <summary>
        /// Get and set the EnableRtl.
        /// </summary>
        public bool EnableRtl { get; set; }

        /// <summary>
        /// Get and set the Enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Get and set the Limits.
        /// </summary>
        public SliderLimits Limits { get; set; }

        /// <summary>
        /// Get and set the Locale.
        /// </summary>
        public string Locale { get; set; }

        /// <summary>
        /// Get and set the Max Value.
        /// </summary>
        public double Max { get; set; }

        /// <summary>
        /// Get and set the Min Value.
        /// </summary>
        public double Min { get; set; }

        /// <summary>
        /// Get and set the Orientation.
        /// </summary>
        public SliderOrientation Orientation { get; set; }

        /// <summary>
        /// Get and set the ReadOnly.
        /// </summary>
        public bool ReadOnly { get; set; }

        /// <summary>
        /// Get and set the ShowButtons.
        /// </summary>
        public bool ShowButtons { get; set; }

        /// <summary>
        /// Get and set the Step.
        /// </summary>
        public double Step { get; set; }

        /// <summary>
        /// Get and set the Slider.
        /// </summary>
        public SliderTicks Ticks { get; set; }

        /// <summary>
        /// Get and set the Slider.
        /// </summary>
        public SliderTooltip Tooltip { get; set; }

        /// <summary>
        /// Get and set the Slider.
        /// </summary>
        public SliderType Type { get; set; }

        /// <summary>
        /// Get and set the Slider.
        /// </summary>
        public string Width { get; set; }

        /// <summary>
        /// Get and set the Slider.
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// Updates the Child Properties.
        /// </summary>
        /// <param name="key">key.</param>
        /// <param name="value">value.</param>
        public void UpdateChildProperties(string key, object value);

        /// <summary>
        /// Get the Limit Data.
        /// </summary>
        /// <returns>Limit.</returns>
        public SliderLimits GetLimitData();

        /// <summary>
        /// Triggers after the ticks rendered.
        /// </summary>
        /// <param name="ticksWrapperRef">ticksWrapperRef.</param>
        /// <param name="attributes">attributes.</param>
        /// <returns>="Task".</returns>
        public Task<SliderTickRenderedEventArgs> TriggeredTicksRendered(ElementReference ticksWrapperRef, Dictionary<string, object> attributes);

        /// <summary>
        /// Triggers while the ticks rendered.
        /// </summary>
        /// <param name="ticksRef">ticksRef.</param>
        /// <param name="text">text.</param>
        /// <param name="value">value.</param>
        /// <param name="attributes">attributes.</param>
        /// <returns>="Task".</returns>
        public Task<SliderTickEventArgs> TriggeredTicksRendering(ElementReference ticksRef, string text, double value, Dictionary<string, object> attributes);
    }
}
