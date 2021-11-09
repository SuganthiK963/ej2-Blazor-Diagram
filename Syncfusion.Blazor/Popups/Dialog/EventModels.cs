using System;
using Microsoft.JSInterop;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components.Web;

namespace Syncfusion.Blazor.Popups
{
    /// <summary>
    /// Provides data for overLayClick event.
    /// </summary>
    public class OverlayModalClickEventArgs
    {
        /// <summary>
        /// Defines the mouse event arguments when the event is triggered.
        /// </summary>
        public MouseEventArgs Event { get; set; }

        /// <summary>
        /// Specifies the value whether the default focus on the first focusable element in a dialog can be prevented.
        /// </summary>
        public bool PreventFocus { get; set; }
    }

    /// <summary>
    /// Provides data for the BeforeClose event.
    /// </summary>
    public class BeforeCloseEventArgs
    {
        /// <summary>
        /// Defines whether the current action can be prevented.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Determines whether the dialog is closed by "Close Icon", "Escape", "User Action".
        /// </summary>
        public string ClosedBy { get; set; }

        /// <summary>
        /// Returns the root container element of the dialog.
        /// </summary>
        [Obsolete("This event argument is deprecated and will no longer be used.")]
        public DOM Container { get; set; }

        /// <summary>
        /// Returns the element of the dialog.
        /// </summary>
        [Obsolete("This event argument is deprecated and will no longer be used.")]
        public DOM Element { get; set; }

        /// <summary>
        /// Returns the original event arguments when triggering the event.
        /// </summary>
        public EventArgs Event { get; set; }

        /// <summary>
        /// Determines whether the event is triggered by interaction.
        /// </summary>
        public bool IsInteracted { get; set; }
    }

    /// <summary>
    /// Provides data for the BeforeOpen event.
    /// </summary>
    public class BeforeOpenEventArgs
    {
        /// <summary>
        /// Defines whether the current action can be prevented.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Returns the root container element of the dialog.
        /// </summary>
        public DOM Container { get; set; }

        /// <summary>
        /// Returns the element of the dialog.
        /// </summary>
        public DOM Element { get; set; }

        /// <summary>
        /// Specifies the value to override the max height of dialog.
        /// </summary>
        public string MaxHeight { get; set; }
    }

    /// <summary>
    /// Provides data for the Close event.
    /// </summary>
    public class CloseEventArgs
    {
        /// <summary>
        /// Defines whether the current action can be prevented.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Determines whether the dialog is closed by "Close Icon", "Escape", "User Action".
        /// </summary>
        public string ClosedBy { get; set; }

        /// <summary>
        /// Returns the root container element of the dialog.
        /// </summary>
        public DOM Container { get; set; }

        /// <summary>
        /// Returns the element of the dialog.
        /// </summary>
        public DOM Element { get; set; }

        /// <summary>
        /// Returns the original event arguments when triggering the event.
        /// </summary>
        public EventArgs Event { get; set; }

        /// <summary>
        /// Determines whether the event is triggered by interaction.
        /// </summary>
        public bool IsInteracted { get; set; }

        /// <summary>
        /// Specifies the name of the event.
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// Provides data for the Dragging event.
    /// </summary>
    public class DragEventArgs
    {
        /// <summary>
        /// Returns the element of the dialog.
        /// </summary>
        [JsonPropertyName("element")]
        public DOM Element { get; set; }

        /// <summary>
        /// Returns the original event arguments when triggering the event.
        /// </summary>
        [DefaultValue(null)]
        [JsonPropertyName("event")]
        public MouseEventArgs Event { get; set; } = null;

        /// <summary>
        /// Specifies the name of the event.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Returns the target element of the dialog.
        /// </summary>
        [JsonPropertyName("target")]
        public DOM Target { get; set; }
    }

    /// <summary>
    /// Provides data for the DragStart event.
    /// </summary>
    public class DragStartEventArgs
    {
        /// <summary>
        /// Returns the element of the dialog.
        /// </summary>
        [JsonPropertyName("element")]
        public DOM Element { get; set; }

        /// <summary>
        /// Returns the original event arguments when triggering the event.
        /// </summary>
        [DefaultValue(null)]
        [JsonPropertyName("event")]
        public MouseEventArgs Event { get; set; } = null;

        /// <summary>
        /// Specifies the name of the event.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Returns the target element of the dialog.
        /// </summary>
        [JsonPropertyName("target")]
        public DOM Target { get; set; }
    }

    /// <summary>
    /// Provides data for the DragStop event.
    /// </summary>
    public class DragStopEventArgs
    {
        /// <summary>
        /// Returns the element of the dialog.
        /// </summary>
        [JsonPropertyName("element")]
        public DOM Element { get; set; }

        /// <summary>
        /// Returns the original event arguments when triggering the event.
        /// </summary>
        [DefaultValue(null)]
        [JsonPropertyName("event")]
        public MouseEventArgs Event { get; set; } = null;

        /// <summary>
        /// Returns the helper element.
        /// </summary>
        [JsonPropertyName("helper")]
        public DOM Helper { get; set; }

        /// <summary>
        /// Specifies the name of the event.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Returns the target element of the dialog.
        /// </summary>
        [JsonPropertyName("target")]
        public DOM Target { get; set; }
    }

    /// <summary>
    /// Provides data for the DialogOpen event.
    /// </summary>
    public class OpenEventArgs
    {
        /// <summary>
        /// Defines whether the current action can be prevented.
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Returns the root container element of the dialog.
        /// </summary>
        public DOM Container { get; set; }

        /// <summary>
        /// Returns the element of the dialog.
        /// </summary>
        public DOM Element { get; set; }

        /// <summary>
        /// Specifies the name of the event.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Specifies the value whether the default focus on the first focusable element in a dialog can be prevented.
        /// </summary>
        public bool PreventFocus { get; set; }
    }
}