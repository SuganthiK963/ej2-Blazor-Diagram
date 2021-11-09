using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel;
using Microsoft.JSInterop;
using System.Text.Json;
using Syncfusion.Blazor.Internal;
using System;

namespace Syncfusion.Blazor.Popups
{
    /// <summary>
    /// The SfTooltip component displays a pop-up containing information or a message when you hover, click, focus, or touch an element.
    /// </summary>
    public partial class SfTooltip : SfBaseComponent
    {
        // region class name constants.
        private string classList = "e-blazor-hidden e-control e-tooltip e-lib";

        private ElementReference tooltipElement;

        private bool renderWrapper;

        private bool beforeCollisionTriggered;

        private bool isDestroyed;

        private bool isScriptRendered;

        private IDictionary<string, object> attributes = new Dictionary<string, object>();

        private JsonSerializerOptions options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        internal string TooltipContent { get; set; }

        internal string TooltipHeight { get; set; }

        internal bool TooltipIsSticky { get; set; }

        internal double TooltipOffsetX { get; set; }

        internal string TooltipCssClass { get; set; }

        internal bool TooltipEnableRtl { get; set; }

        internal bool TooltipWindowCollision { get; set; }

        internal double TooltipOffsetY { get; set; }

        internal string TooltipOpensOn { get; set; }

        internal string TooltipTarget { get; set; }

        internal Position TooltipPosition { get; set; }

        internal string TooltipWidth { get; set; }

        internal TooltipTemplates Template { get; set; }

        /// <summary>
        /// The method to get events list.
        /// </summary>
        /// <exclude/>
        /// <returns>eventList.</returns>
        protected IDictionary<string, object> GetEventsList()
        {
            Dictionary<string, object> eventList = new Dictionary<string, object>();
            SfBaseUtils.UpdateDictionary("beforeRender", OnRender.HasDelegate, eventList);
            SfBaseUtils.UpdateDictionary("beforeCollision", OnCollision.HasDelegate, eventList);
            SfBaseUtils.UpdateDictionary("beforeOpen", OnOpen.HasDelegate, eventList);
            SfBaseUtils.UpdateDictionary("opened", Opened.HasDelegate, eventList);
            SfBaseUtils.UpdateDictionary("beforeClose", OnClose.HasDelegate, eventList);
            SfBaseUtils.UpdateDictionary("closed", Closed.HasDelegate, eventList);
            return eventList;
        }

        /// <summary>
        /// The method to get Properties.
        /// </summary>
        /// <exclude/>
        /// <returns>properties.</returns>
        protected IDictionary<string, object> GetProperties()
        {
            Dictionary<string, object> properties = new Dictionary<string, object>();
            SfBaseUtils.UpdateDictionary("target", Target, properties);
            SfBaseUtils.UpdateDictionary("opensOn", OpensOn, properties);
            SfBaseUtils.UpdateDictionary("animation", GetAnimationValue(), properties);
            SfBaseUtils.UpdateDictionary("isSticky", IsSticky, properties);
            SfBaseUtils.UpdateDictionary("mouseTrail", MouseTrail, properties);
            SfBaseUtils.UpdateDictionary("position", SfBaseUtils.ChangeType(Position, typeof(string)), properties);
            SfBaseUtils.UpdateDictionary("showTipPointer", ShowTipPointer, properties);
            SfBaseUtils.UpdateDictionary("offsetX", OffsetX, properties);
            SfBaseUtils.UpdateDictionary("offsetY", OffsetY, properties);
            SfBaseUtils.UpdateDictionary("tipPointerPosition", SfBaseUtils.ChangeType(TipPointerPosition, typeof(string)), properties);
            SfBaseUtils.UpdateDictionary("closeDelay", CloseDelay, properties);
            SfBaseUtils.UpdateDictionary("openDelay", OpenDelay, properties);
            SfBaseUtils.UpdateDictionary("width", Width, properties);
            SfBaseUtils.UpdateDictionary("height", Height, properties);
            SfBaseUtils.UpdateDictionary("enableRtl", EnableRtl || (SyncfusionService != null && SyncfusionService.options.EnableRtl), properties);
            SfBaseUtils.UpdateDictionary("windowCollision", WindowCollision, properties);
            SfBaseUtils.UpdateDictionary("content", !string.IsNullOrEmpty(Content) || ContentTemplate != null, properties);
            return properties;
        }

        internal InternalAnimation GetAnimationValue()
        {
            return new InternalAnimation()
            {
                Open = new InternalTooltipAnimationSettings
                {
                    Delay = Animation.Open != null ? Animation.Open.Delay : 0,
                    Duration = Animation.Open != null ? Animation.Open.Duration : 150,
                    Effect = Animation.Open != null ? Animation.Open.Effect.ToString() : Effect.FadeIn.ToString()
                },
                Close = new InternalTooltipAnimationSettings
                {
                    Delay = Animation.Close != null ? Animation.Close.Delay : 0,
                    Duration = Animation.Close != null ? Animation.Close.Duration : 150,
                    Effect = Animation.Close != null ? Animation.Close.Effect.ToString() : Effect.FadeOut.ToString()
                }
            };
        }

        /// <summary>
        /// The method to get Property changes.
        /// </summary>
        /// <exclude/>
        /// <returns>properties.</returns>
        protected IDictionary<string, object> GetPorpertyChanges()
        {
            Dictionary<string, object> properties = new Dictionary<string, object>();
            if (PropertyChanges.ContainsKey("Target"))
            {
                properties.Add("target", Target);
            }

            if (PropertyChanges.ContainsKey("OpensOn"))
            {
                properties.Add("opensOn", OpensOn);
            }

            if (PropertyChanges.ContainsKey("Animation"))
            {
                properties.Add("animation", GetAnimationValue());
            }

            if (PropertyChanges.ContainsKey("IsSticky"))
            {
                properties.Add("isSticky", IsSticky);
            }

            if (PropertyChanges.ContainsKey("MouseTrail"))
            {
                properties.Add("mouseTrail", MouseTrail);
            }

            if (PropertyChanges.ContainsKey("Position"))
            {
                properties.Add("position", Position.ToString());
            }

            if (PropertyChanges.ContainsKey("ShowTipPointer"))
            {
                properties.Add("showTipPointer", ShowTipPointer);
            }

            if (PropertyChanges.ContainsKey("OffsetX"))
            {
                properties.Add("offsetX", OffsetX);
            }

            if (PropertyChanges.ContainsKey("OffsetY"))
            {
                properties.Add("offsetY", OffsetY);
            }

            if (PropertyChanges.ContainsKey("TipPointerPosition"))
            {
                properties.Add("tipPointerPosition", TipPointerPosition.ToString());
            }

            if (PropertyChanges.ContainsKey("CloseDelay"))
            {
                properties.Add("closeDelay", CloseDelay);
            }

            if (PropertyChanges.ContainsKey("OpenDelay"))
            {
                properties.Add("openDelay", OpenDelay);
            }

            if (PropertyChanges.ContainsKey("Width"))
            {
                properties.Add("width", Width);
            }

            if (PropertyChanges.ContainsKey("Height"))
            {
                properties.Add("height", Height);
            }

            if (PropertyChanges.ContainsKey("EnableRtl"))
            {
                properties.Add("enableRtl", EnableRtl);
            }

            if (PropertyChanges.ContainsKey("windowCollision"))
            {
                properties.Add("windowCollision", WindowCollision);
            }

            SfBaseUtils.UpdateDictionary("content", !string.IsNullOrEmpty(Content) || ContentTemplate != null, properties);
            return properties;
        }

        // Updating the content local property with user given template content
        internal void UpdateTemplate(string name, RenderFragment template)
        {
            if (name == nameof(Content))
            {
                ContentTemplate = template;
            }

            StateHasChanged();
        }

        /// <exclude/>
        /// <summary>
        /// Add/Removes the Tooltip Element.
        /// </summary>
        /// <param name="args">args.</param>
        /// <returns>="Task".</returns>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task CreateTooltip(bool args)
        {
            renderWrapper = args;
            await InvokeAsync(StateHasChanged);
        }

        /// <exclude/>
        /// <summary>
        /// Closes the Tooltip Element.
        /// </summary>
        /// <returns>="Task".</returns>
        protected async Task StickyClose()
        {
            await Close();
        }

        /// <exclude/>
        /// <summary>
        /// Triggers before render event.
        /// </summary>
        /// <returns>="Task".</returns>
        /// <param name="args">args.</param>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task TriggerBeforeRenderEvent(string args)
        {
            TooltipEventArgs beforeRenderArgs = JsonSerializer.Deserialize<TooltipEventArgs>(args, options);
            beforeRenderArgs.JsRuntime = JSRuntime;
            await SfBaseUtils.InvokeEvent<TooltipEventArgs>(OnRender, beforeRenderArgs);
            await JSRuntime.InvokeAsync<string>("sfBlazor.Tooltip.beforeRenderCallBack", new object[] { tooltipElement, beforeRenderArgs.Cancel });
        }

        /// <exclude/>
        /// <summary>
        /// Triggers before collision event.
        /// </summary>
        /// <param name="args">args.</param>
        /// <returns>="Task".</returns>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task TriggerBeforeCollisionEvent(string args)
        {
            if (!beforeCollisionTriggered)
            {
                beforeCollisionTriggered = true;
                TooltipEventArgs beforeCollisionArgs = JsonSerializer.Deserialize<TooltipEventArgs>(args, options);
                beforeCollisionArgs.JsRuntime = JSRuntime;
                await SfBaseUtils.InvokeEvent<TooltipEventArgs>(OnCollision, beforeCollisionArgs);
            }
        }

        /// <exclude/>
        /// <summary>
        /// Triggers before open event.
        /// </summary>
        /// <param name="args">args.</param>
        /// <returns>"Task".</returns>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task TriggerBeforeOpenEvent(string args)
        {
            TooltipEventArgs beforeOpenEventArgs = JsonSerializer.Deserialize<TooltipEventArgs>(args, options);
            beforeOpenEventArgs.JsRuntime = JSRuntime;
            await SfBaseUtils.InvokeEvent<TooltipEventArgs>(OnOpen, beforeOpenEventArgs);
            await JSRuntime.InvokeAsync<string>("sfBlazor.Tooltip.beforeOpenCallBack", new object[] { tooltipElement, beforeOpenEventArgs.Cancel });
        }

        /// <exclude/>
        /// <summary>
        /// Triggers before opened event.
        /// </summary>
        /// <param name="args">args.</param>
        /// <returns>="Task".</returns>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task TriggerOpenedEvent(string args)
        {
            TooltipEventArgs openedEventArgs = JsonSerializer.Deserialize<TooltipEventArgs>(args, options);
            openedEventArgs.JsRuntime = JSRuntime;
            await SfBaseUtils.InvokeEvent<TooltipEventArgs>(Opened, openedEventArgs);
        }

        /// <exclude/>
        /// <summary>
        /// Triggers before close event.
        /// </summary>
        /// <returns>"Task".</returns>
        /// <param name="args">args.</param>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task TriggerBeforeCloseEvent(string args)
        {
            TooltipEventArgs beforeCloseEventArgs = JsonSerializer.Deserialize<TooltipEventArgs>(args, options);
            beforeCloseEventArgs.JsRuntime = JSRuntime;
            await SfBaseUtils.InvokeEvent<TooltipEventArgs>(OnClose, beforeCloseEventArgs);
            await JSRuntime.InvokeAsync<string>("sfBlazor.Tooltip.beforeCloseCallBack", new object[] { tooltipElement, beforeCloseEventArgs.Cancel });
        }

        /// <exclude/>
        /// <summary>
        /// Triggers before closed event.
        /// </summary>
        /// <param name="args">args.</param>
        /// <returns>="Task".</returns>
        [JSInvokable]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task TriggerClosedEvent(string args)
        {
            TooltipEventArgs closedEventArgs = JsonSerializer.Deserialize<TooltipEventArgs>(args, options);
            closedEventArgs.JsRuntime = JSRuntime;
            await SfBaseUtils.InvokeEvent<TooltipEventArgs>(Closed, closedEventArgs);
            beforeCollisionTriggered = false;
        }

        internal async override void ComponentDispose()
        {
            if (IsRendered && !isDestroyed)
            {
                try
                {
                    attributes = null;
                    classList = null;
                    isDestroyed = true;
                    await JSRuntime.InvokeAsync<string>("sfBlazor.Tooltip.destroy", tooltipElement);
                    await SfBaseUtils.InvokeEvent<object>(Destroyed, null);
                    Dispose(true);
                }
#pragma warning disable CA1031 // Do not catch general exception types
                catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
                {
                    await SfBaseUtils.InvokeEvent<object>(Destroyed, e);
                }
            }
        }
    }
}