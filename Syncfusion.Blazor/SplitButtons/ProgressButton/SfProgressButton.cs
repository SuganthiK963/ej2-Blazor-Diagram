using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Syncfusion.Blazor.Internal;
using Syncfusion.Blazor.Buttons;
using Syncfusion.Blazor.Spinner;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Syncfusion.Blazor.SplitButtons
{
    /// <summary>
    /// ProgressButton visualizes the progression of an operation to indicate the user that a process is happening in the background with visual representation.
    /// </summary>
    public partial class SfProgressButton : SfBaseComponent
    {
        private const int MIN = 0;
        private const int MAX = 100;
        private const string SPACE = " ";
        private const string ONEND = "OnEnd";
        private const string SPIN = "e-spin-";
        private const string ONBEGIN = "OnBegin";
        private const string LEFT = "e-spin-left";
        private const string BTNICON = "e-btn-icon";
        private const string SPINNER = "e-spinner";
        private const string HIDE = "e-hide-spinner";
        private const string VERTICAL = "e-vertical";
        private const string PROGRESSING = "Progressing";
        private const string ACTIVE = "e-progress-active";
        private const string PROGRESSBTN = "e-progress-btn";
        private const string ANIMATION = "sfBlazor.setAnimation";
        private const string PROGRESS = "sfBlazor.setProgress";
        private const string CANCELANIMATION = "sfBlazor.cancelAnimation";
        private double percent;
        private SfButton buttonObj;
        private SfSpinner spinnerObj;
        private double stepTime = 1;
        private double currDur = MIN;
        private bool isHide;
        private bool isVertical;
        private bool isPaused;
        private string spinWidth = "16";
        private string iconCss = string.Empty;
        private string buttonCss = string.Empty;
        private string contentCss = "e-btn-content";
        private string spinPos = string.Empty;
        private int timerId = MIN;
        private Dictionary<string, object> htmlAttributes;
        private ElementReference progressElem;
        private ElementReference contElem;
        private ElementReference spinnerElem;
        private string id = SfBaseUtils.GenerateID("sfprogressbutton");
        private ProgressButtonSpinSettings spinSettings;
        private ProgressButtonAnimationSettings animationSettings;

        internal ProgressButtonEvents Delegates { get; set; }

        private async Task ClickHandler(MouseEventArgs e = null)
        {
            if (buttonCss.Contains(ACTIVE, StringComparison.Ordinal))
            {
                return;
            }
            isPaused = false;
            InitProgress(DateTimeOffset.Now.ToUnixTimeMilliseconds(), DateTimeOffset.Now.ToUnixTimeMilliseconds());
            if (e != null)
            {
                await OnClick.InvokeAsync(e);
            }
        }

        private async void InitProgress(long timeStamp, long prevTime, double perc = MIN)
        {
            try
            {
                percent = perc == MIN ? percent : perc;
                long timeDiff = timeStamp - prevTime;
                var stepTime = Duration * this.stepTime / MAX;
                var buffer = (timeDiff != MIN) ? (timeDiff < stepTime ? timeDiff - stepTime : timeDiff % stepTime) : MIN;
                currDur = currDur + timeDiff - buffer;
                prevTime = timeStamp - (long)buffer;
                percent += (timeDiff - buffer) / Duration * MAX;
                if (percent == MIN || !buttonCss.Contains(ACTIVE, StringComparison.InvariantCulture))
                {
                    SuccessCallBack(buffer, prevTime);
                    await BeginProgress();
                }
                else if (percent == MAX || percent > MAX)
                {
                    percent = MIN;
                    await EndProgress();
                }
                else
                {
                    SuccessCallBack(buffer, prevTime);
                    await Progress();
                }
            }
            catch (Exception e)
            {
                await CancelFrame();
                await SfBaseUtils.InvokeEvent<Exception>(Delegates?.OnFailure, e);
                throw;
            }
        }

        private async Task CancelFrame() => await InvokeMethod(CANCELANIMATION, timerId);

        private async void SuccessCallBack(double buffer, long prevTime)
        {
            // delays before callback
            await Task.Delay(TimeSpan.FromMilliseconds((Duration / MAX) - buffer));
            if (!isPaused)
            {
                InitProgress(DateTimeOffset.Now.ToUnixTimeMilliseconds(), prevTime);
            }
        }

        private async Task StartSpinner()
        {
            if (!isHide)
            {
                await spinnerObj.ShowAsync();
            }

            if (animationSettings != null)
            {
                bool isCenter = spinSettings.Position == SpinPosition.Center;
                string effect = animationSettings.Effect.ToString();
                await InvokeMethod(ANIMATION, contElem, spinnerElem, effect, animationSettings.Duration, animationSettings.Easing, isCenter);
            }
        }

        private async Task BeginProgress()
        {
            buttonCss = buttonCss.Contains(ACTIVE, StringComparison.Ordinal) ? buttonCss : buttonCss + SPACE + ACTIVE;
            await StartSpinner();
            var eventArgs = new ProgressEventArgs() { Percent = percent, Name = ONBEGIN, Step = 1, CurrentDuration = MIN };
            await SfBaseUtils.InvokeEvent<ProgressEventArgs>(Delegates?.OnBegin, eventArgs);
            stepTime = eventArgs.Step;
            percent = eventArgs.Percent;
            await ResetProgress();
        }

        private async Task Progress()
        {
            var eventArgs = new ProgressEventArgs() { Percent = percent, Name = PROGRESSING, Step = stepTime, CurrentDuration = currDur };
            await SfBaseUtils.InvokeEvent<ProgressEventArgs>(Delegates?.Progressing, eventArgs);
            percent = eventArgs.Percent;
            stepTime = eventArgs.Step;
            if (percent < MAX && (percent) % (stepTime) == MIN)
            {
                timerId = await InvokeMethod<int>(PROGRESS, false, progressElem, contElem, spinnerElem, percent, EnableProgress, isVertical, false);
            }
        }

        private async Task EndProgress()
        {
            var eventArgs = new ProgressEventArgs() { Percent = MAX, Name = ONEND, Step = stepTime, CurrentDuration = Duration };
            await SfBaseUtils.InvokeEvent<ProgressEventArgs>(Delegates?.OnEnd, eventArgs);
            timerId = await InvokeMethod<int>(PROGRESS, false, progressElem, contElem, spinnerElem, MAX, EnableProgress, isVertical, false);
            if (spinnerObj != null)
            {
                await spinnerObj.HideAsync();
                buttonCss = SfBaseUtils.RemoveClass(buttonCss, ACTIVE);
                StateHasChanged();
            }

            // delays the percent before resetting to 0.
            await Task.Delay(100);
            await ResetProgress(false);
        }

        private async Task ResetProgress(bool isFirst = true)
        {
            percent = MIN;
            currDur = MIN;
            timerId = await InvokeMethod<int>(PROGRESS, false, progressElem, contElem, spinnerElem, MIN, EnableProgress, isVertical, isFirst);
        }

        internal override void ComponentDispose()
        {
            spinnerObj = null;
            buttonObj = null;
        }
    }
}