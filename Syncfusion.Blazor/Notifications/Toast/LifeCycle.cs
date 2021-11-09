using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Syncfusion.Blazor.Internal;
using System.Globalization;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Notifications
{
    /// <summary>
    /// Toast is a small, nonblocking notification pop-up and it is shown to users with readable message content
    /// at the bottom of the screen or at a specific target and disappears automatically after a few seconds (time-out)
    /// with different animation effects.
    /// </summary>
    public partial class SfToast : SfBaseComponent
    {
        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            Target = !string.IsNullOrEmpty(Target) ? Target : BODY;
            toastPosition = Target == BODY ? POSITION_FIXED : POSITION_ABSOLUTE;
            rtlClass = EnableRtl || SyncfusionService.options.EnableRtl ? RTL : string.Empty;
            UpdateLocale();

            UpdatePosition(PositionValue);
            UpdateAnimation(AnimationValue);

            await base.OnInitializedAsync();
            DependentScripts.Add(Syncfusion.Blazor.Internal.ScriptModules.PopupsBase);
            DependentScripts.Add(Syncfusion.Blazor.Internal.ScriptModules.Popup);
            ScriptModules = SfScriptModules.SfToast;
        }

        internal override async Task OnAfterScriptRendered()
        {
            await InvokeMethod(JS_INITIALIZE, ToastElement, GetInstance(), DotnetObjectReference);
        }

        private Dictionary<string, object> GetInstance()
        {
            Dictionary<string, object> toastObj = new Dictionary<string, object>()
            {
                { TARGET, Target },
                { ELEMENT, ToastElement },
                { NEWEST_ON_TOP, NewestOnTop },
                { SHOW_ANIMATION, GetShowAnimation() },
                { HIDE_ANIMATION, GetHideAnimation() }
            };

            return toastObj;
        }

        internal Dictionary<string, object> GetShowAnimation()
        {
            Dictionary<string, object> showAnimation = new Dictionary<string, object>()
            {
                { EFFECT, AnimationValue.ShowSettings != null ? AnimationValue.ShowSettings.Effect.ToString() : FADE_IN },
                { DURATION, AnimationValue.ShowSettings != null && AnimationValue.ShowSettings.Duration != 0 ? AnimationValue.ShowSettings.Duration : DEFAULT_ANIMATION_TIME },
                { EASING, AnimationValue.ShowSettings != null ? Convert.ToString(AnimationValue.ShowSettings.Easing, CultureInfo.InvariantCulture).ToLower(CultureInfo.CurrentCulture) : EASE }
            };

            return showAnimation;
        }

        internal Dictionary<string, object> GetHideAnimation()
        {
            Dictionary<string, object> hideAnimation = new Dictionary<string, object>()
            {
                { EFFECT, AnimationValue.HideSettings != null ? AnimationValue.HideSettings.Effect.ToString() : Animation.HideSettings != null ? AnimationValue.HideSettings.Effect.ToString() : FADE_OUT },
                { DURATION, AnimationValue.HideSettings != null && AnimationValue.HideSettings.Duration != 0 ? Animation.HideSettings.Duration : AnimationValue.HideSettings != null && AnimationValue.HideSettings.Duration != 0 ? AnimationValue.HideSettings.Duration : DEFAULT_ANIMATION_TIME },
                { EASING, AnimationValue.HideSettings != null ? Convert.ToString(AnimationValue.HideSettings.Easing, CultureInfo.InvariantCulture).ToLower(CultureInfo.CurrentCulture) : EASE }
            };
            return hideAnimation;
        }

        internal override async void ComponentDispose()
        {
            if (IsRendered)
            {
                try
                {
                    await InvokeMethod(JS_DESTROY, ToastElement);
                    await SfBaseUtils.InvokeEvent<object>(Delegates?.Destroyed, null);
                    ChildContent = null;
                    if (ShowModels != null)
                    {
                        ShowModels.Clear();
                    }

                    ShowModels = null;
                    if (ProgressObj != null)
                    {
                        ProgressObj.Clear();
                    }

                    ProgressObj = null;
                    if (TimeoutTimer != null)
                    {
                        TimeoutTimer.Clear();
                    }

                    TimeoutTimer = null;
                    if (ProgressBarTimer != null)
                    {
                        ProgressBarTimer.Clear();
                    }

                    ProgressBarTimer = null;
                    if (CloseIconAttributes != null)
                    {
                        CloseIconAttributes.Clear();
                    }

                    CloseIconAttributes = null;
                    titleTemplate = null;
                    contentTemplate = null;
                    ContentTemplate = null;
                    templates = null;
                    if (ArgsCancelList != null)
                    {
                        ArgsCancelList.Clear();
                    }

                    ArgsCancelList = null;
                    if (Toastindex != null)
                    {
                        Toastindex.Clear();
                    }

                    Toastindex = null;
                    ID = null;
                    ToastElement = default(ElementReference);
                    CurrentToast = default(ElementReference);
                    Delegates = null;
                }
                catch (ObjectDisposedException e)
                {
                    await SfBaseUtils.InvokeEvent<object>(Delegates?.Destroyed, e);
                }
                catch (TaskCanceledException e)
                {
                    await SfBaseUtils.InvokeEvent<object>(Delegates?.Destroyed, e);
                }
                catch (InvalidOperationException e)
                {
                    await SfBaseUtils.InvokeEvent<object>(Delegates?.Destroyed, e);
                }
            }
        }
    }
}