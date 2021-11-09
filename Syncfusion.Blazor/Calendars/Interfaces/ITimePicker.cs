using Syncfusion.Blazor.Internal;
using System;

namespace Syncfusion.Blazor.Calendars
{
    /// <summary>
    /// Defines the <see cref="ITimePicker" />.
    /// </summary>
    public interface ITimePicker : IBaseComponent
    {
        /// <summary>
        /// Gets or sets a value indicating whether editing is enabled.
        /// </summary>
        public bool AllowEdit { get; set; }

        /// <summary>
        /// Gets or sets the css class.
        /// </summary>
        public string CssClass { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether persistence is enabled.
        /// </summary>
        public bool EnablePersistence { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether rtl mode is enabled.
        /// </summary>
        public bool EnableRtl { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the component is disabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the floatLabel type.
        /// </summary>
        public Syncfusion.Blazor.Inputs.FloatLabelType FloatLabelType { get; set; }

        /// <summary>
        /// Gets or sets the format.
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// Gets or sets the html attributes.
        /// </summary>
        public object HtmlAttributes { get; set; }

        /// <summary>
        /// Gets or sets the key configurations.
        /// </summary>
        public object KeyConfigs { get; set; }

        /// <summary>
        /// Gets or sets the locale.
        /// </summary>
        public string Locale { get; set; }

        /// <summary>
        /// Gets or sets the maximum date and time value.
        /// </summary>
        public DateTime Max { get; set; }

        /// <summary>
        /// Gets or sets the minimum date and time value.
        /// </summary>
        public DateTime Min { get; set; }

        /// <summary>
        /// Gets or sets the placeholder.
        /// </summary>
        public string Placeholder { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Readonly.
        /// </summary>
        public bool Readonly { get; set; }

        /// <summary>
        /// Gets or sets the ScrollTo property.
        /// </summary>
        public DateTime? ScrollTo { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether clear button is enabled.
        /// </summary>
        public bool ShowClearButton { get; set; }

        /// <summary>
        /// Gets or sets the step.
        /// </summary>
        public int Step { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether strict mode is enabled.
        /// </summary>
        public bool StrictMode { get; set; }

        /// <summary>
        /// Gets or sets the width.
        /// </summary>
        public string Width { get; set; }

        /// <summary>
        /// Gets or sets the Z-index.
        /// </summary>
        public int ZIndex { get; set; }
    }
}
