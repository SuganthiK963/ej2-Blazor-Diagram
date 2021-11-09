using Microsoft.AspNetCore.Components;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System;

namespace Syncfusion.Blazor.Popups
{
    /// <summary>
    /// The SfTooltip component displays a pop-up containing information or a message when you hover, click, focus, or touch an element.
    /// </summary>
    public partial class SfTooltip : SfBaseComponent
    {
        /// <exclude/>
        /// <summary>
        /// Defines the content which has to be passed.
        /// </summary>
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <exclude/>
        /// <summary>
        /// Defines the Id of the Tooltip component.
        /// </summary>
        [Parameter]
        public string ID { get; set; }

        /// <exclude/>
        /// <summary>
        /// Defines the content template.
        /// </summary>
        public RenderFragment ContentTemplate { get; set; }

        /// <summary>
        /// It is used to customize the animation of the Tooltip while opening and closing.
        /// The animation property also allows you to set delay, duration, and various other effects of your choice.
        /// You can set the same or different animation options to the Tooltip when it is in the open or close state.
        /// </summary>
        [Parameter]
        public AnimationModel Animation { get; set; } = new AnimationModel
        {
            Open = new TooltipAnimationSettings { Delay = 0, Duration = 150, Effect = Effect.FadeIn },
            Close = new TooltipAnimationSettings { Delay = 0, Duration = 150, Effect = Effect.FadeOut },
        };

        /// <summary>
        /// To close the Tooltip after a specified delay in millisecond.
        /// </summary>
        [Parameter]
        [JsonPropertyName("closeDelay")]
        public double CloseDelay { get; set; } = 0;

        /// <summary>
        /// To display the content of the Tooltip which can be a string element.
        /// </summary>
        [Parameter]
        [JsonPropertyName("content")]
        public string Content { get; set; }

        /// <summary>
        /// To customize the Tooltip which accepts the custom CSS class names that define specific
        /// user-defined styles and themes to be applied to the Tooltip element.
        /// </summary>
        [Parameter]
        [JsonPropertyName("cssClass")]
        public string CssClass { get; set; }

        /// <summary>
        /// This property has been deprecated as we achieve this behavior by using ChildContent of render fragment type and and Content as a string value.
        /// </summary>
        [Obsolete("This property is deprecated and will no longer be used.")]
        [Parameter]
        [JsonPropertyName("enableHtmlSanitizer")]
        public bool EnableHtmlSanitizer { get; set; }

        /// <summary>
        /// As there are no properties required to persist in tooltip component, this property is deprecated.
        /// </summary>
        [Obsolete("This property is deprecated and will no longer be used.")]
        [Parameter]
        [JsonPropertyName("enablePersistence")]
        public bool EnablePersistence { get; set; }

        /// <summary>
        /// To enable or disable the rendering component in the right to left direction.
        /// </summary>
        [Parameter]
        [JsonPropertyName("enableRtl")]
        public bool EnableRtl { get; set; }

        /// <summary>
        /// It is used to set the collision target element as page viewport (window) or Tooltip element, when using the target.
        /// If this property is enabled, tooltip will perform the collision calculation between the target elements.
        /// and viewport(window) instead of Tooltip element.
        /// </summary>
        [Parameter]
        [JsonPropertyName("WindowCollision")]
        public bool WindowCollision { get; set; } = false;

        /// <summary>
        /// To set the height of the Tooltip component which accepts the string values.
        /// When the Tooltip content gets overflowed due to the height value, then the scroll mode will be enabled.
        /// </summary>
        [Parameter]
        [JsonPropertyName("height")]
        public string Height { get; set; } = "auto";

        /// <summary>
        /// To display the Tooltip in an open state until it is closed manually.
        /// </summary>
        [Parameter]
        [JsonPropertyName("isSticky")]
        public bool IsSticky { get; set; }

        /// <summary>
        /// It allows the Tooltip to follow the mouse pointer moves over the specified target element.
        /// </summary>
        [Parameter]
        [JsonPropertyName("mouseTrail")]
        public bool MouseTrail { get; set; }

        /// <summary>
        /// It sets the space between the target and Tooltip element in X-axis.
        /// </summary>
        [Parameter]
        [JsonPropertyName("offsetX")]
        public double OffsetX { get; set; }

        /// <summary>
        /// It sets the space between the target and Tooltip element in Y-axis.
        /// </summary>
        [Parameter]
        [JsonPropertyName("offsetY")]
        public double OffsetY { get; set; }

        /// <summary>
        /// To open the Tooltip after a specified delay in millisecond.
        /// </summary>
        [Parameter]
        [JsonPropertyName("openDelay")]
        public double OpenDelay { get; set; }

        /// <summary>
        /// To determine the type of open mode to display the Tooltip content.
        /// The available open modes are Auto, Hover, Click, Focus, and Custom.
        /// </summary>
        [Parameter]
        [JsonPropertyName("opensOn")]
        public string OpensOn { get; set; } = "Auto";

        /// <summary>
        /// To set the position of the Tooltip element with respect to the Target element.
        /// </summary>
        [Parameter]
        [JsonPropertyName("position")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Position Position { get; set; } = Position.TopCenter;

        /// <summary>
        /// To show or hide the tip pointer of the Tooltip.
        /// </summary>
        [Parameter]
        [JsonPropertyName("showTipPointer")]
        public bool ShowTipPointer { get; set; } = true;

        /// <summary>
        /// To denote the target selector where the Tooltip needs to be displayed.
        /// The target element is considered as the parent container.
        /// </summary>
        [Parameter]
        [JsonPropertyName("target")]
        public string Target { get; set; }

        /// <summary>
        /// It is used to customize the position of the tip pointer on the tooltip. The available options are Auto, Start, Middle, and End.
        /// When set to auto, the tip pointer gets auto adjusted within the space of the target's length and does not point outside.
        /// </summary>
        [Parameter]
        [JsonPropertyName("tipPointerPosition")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TipPointerPosition TipPointerPosition { get; set; } = TipPointerPosition.Auto;

        private TipPointerPosition TooltipTipPointerPosition { get; set; }

        /// <summary>
        /// To set the width of the Tooltip component which accepts a string value.
        /// When set to auto, the Tooltip width gets auto adjusted to display its content within the viewable screen.
        /// </summary>
        [Parameter]
        [JsonPropertyName("width")]
        public string Width { get; set; } = "auto";

        /// <summary>
        /// Adds the additional html attributes to the Tooltip element.
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        [JsonPropertyName("htmlAttributes")]
        [Obsolete("This property is deprecated. Use @attributes to set additional attributes for tooltip element.")]
        public Dictionary<string, object> HtmlAttributes { get { return TooltipHtmlAttributes; } set { TooltipHtmlAttributes = value; } }

        internal Dictionary<string, object> TooltipHtmlAttributes { get; set; }
    }
}