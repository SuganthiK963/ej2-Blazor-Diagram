using System.Text.Json.Serialization;
using System.ComponentModel;
using Microsoft.JSInterop;
using System;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.Popups
{
    /// <summary>
    /// Animation options that are common for both open and close actions of the Tooltip.
    /// </summary>
    public class TooltipAnimationSettings
    {
        /// <summary>
        /// It is used to denote the delay value in milliseconds and indicating the waiting time before the animation begins.
        /// </summary>
        [JsonPropertyName("delay")]
        public double? Delay { get; set; }

        /// <summary>
        /// It is used to denote the duration of the animation that is completed per the animation cycle.
        /// </summary>
        [JsonPropertyName("duration")]
        public double? Duration { get; set; }

        /// <summary>
        /// It is used to apply the Animation effect on the Tooltip, during open and close actions.
        /// </summary>
        [JsonPropertyName("effect")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Effect Effect { get; set; }
    }

    /// <summary>
    /// Interface for Tooltip event arguments.
    /// </summary>
    public class TooltipEventArgs
    {
        /// <summary>
        /// It determines whether the current action needs to be prevented or not.
        /// </summary>
        [JsonPropertyName("cancel")]
        public bool Cancel { get; set; }

        /// <summary>
        /// It is used to denote the Collided Tooltip position.
        /// </summary>
        [JsonPropertyName("collidedPosition")]
        public string CollidedPosition { get; set; }

        /// <summary>
        /// It is used to denote the Tooltip element.
        /// </summary>
        [JsonPropertyName("element")]
        public DOM Element { get; set; }

        /// <summary>
        /// It is used to specify the current event object.
        /// </summary>
        [JsonPropertyName("event")]
        public EventArgs Event { get; set; }

        /// <summary>
        /// It determines whether the tooltip content contains text character or not.
        /// </summary>
        [JsonPropertyName("hasText")]
        public bool HasText { get; set; }

        /// <summary>
        /// Specifies the clientY position of the target element.
        /// </summary>
        [JsonPropertyName("top")]
        public double? Top { get; set; }

        /// <summary>
        /// Specifies the clientX position of the target element.
        /// </summary>
        [JsonPropertyName("left")]
        public double? Left { get; set; }

        /// <summary>
        /// If the event is triggered by interaction, it returns true. Otherwise, it returns false.
        /// </summary>
        [JsonPropertyName("isInteracted")]
        public bool IsInteracted { get; set; }

        /// <summary>
        /// Specifies the name of the event.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// It is used to denote the current target element where the Tooltip is to be displayed.
        /// </summary>
        [JsonPropertyName("target")]
        public DOM Target { get; set; }

        /// <summary>
        /// It is used to denote the type of triggered event.
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonIgnore]
        internal IJSRuntime JsRuntime
        {
            get
            {
                return TooltipJsRuntime;
            }

            set
            {
                if (Element != null)
                {
                    Element.JsRuntime = value;
                }

                if (Target != null)
                {
                    Target.JsRuntime = value;
                }

                TooltipJsRuntime = value;
            }
        }

        internal IJSRuntime TooltipJsRuntime { get; set; }

        /// <exclude/>
        /// <summary>
        /// Compares the obj.
        /// </summary>
        /// <param name="obj">obj.</param>
        /// <returns>="obj".</returns>
        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        /// <summary>
        /// Gets the HashCode.
        /// </summary>
        /// <exclude/>
        /// <returns>int.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    /// <summary>
    /// Interface for a class Animation.
    /// </summary>
    public class AnimationModel
    {
        /// <summary>
        /// Animation settings to be applied to the Tooltip when it is closed.
        /// </summary>
        [JsonPropertyName("close")]
        public TooltipAnimationSettings Close { get; set; }

        /// <summary>
        /// Animation settings to be applied on the Tooltip, while it is being shown over the target.
        /// </summary>
        [JsonPropertyName("open")]
        public TooltipAnimationSettings Open { get; set; }
    }
}