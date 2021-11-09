using System;
using System.Threading;
using System.ComponentModel;
using System.Threading.Tasks;
using Syncfusion.Blazor.Internal;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Spinner.Internal
{
    /// <summary>
    /// Represents the Spinner Material class.
    /// </summary>
    /// <exclude/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public partial class Material : SpinnerBase
    {
        private bool show;
        private bool typeUpdate;

        #region Path Variables

        private string attributeD;
        private string strokeWidth;
        private string strokeDashArray;
        private string strokeDashOffset;
        private string pathTransform;
        #endregion

        #region Animation Variables
        private int circleId;
        private string circleUniqueId;
        private int circleChange;
        private double startTime;
        private float circleDiameter;
        private decimal circleStrokeSize;
        private int circleStart;
        private int circleRotate;
        private int circleDuration;
        private int circleMax;
        private Timer timer;
        #endregion

        private const string SVG_CLASS = "e-spin-material";
        private const string ROTATE = "rotate";
        private const int MINUS_NINETY_ANGLE = -90;
        private const int MINUS_FIFTEEN_EASE = -15;
        private const double POINT_FIVE_SECONDS = 0.5;
        private const double POINT_SEVEN_FIVE_STROKE = 0.75;
        private const int THREE_OFFSET = 3;
        private const int FOUR_CIRCLE_VALUE = 4;
        private const int SIX_EASE = 6;
        private const int SEVENTY_FIVE_MAX = 75;
        private const int ANIMATE_END = 149;
        private const int ANIMATE_DURATION = 1333;
        private const int YEAR = 1970;
        #region Other local variables

        private int radius;
        private IDictionary<string, GlobalTimeOut> globalTimeOutObj = new Dictionary<string, GlobalTimeOut>();
        private IDictionary<string, GlobalVariables> globalObject = new Dictionary<string, GlobalVariables>();
        private IDictionary<string, SpinnerInfo> spinnerInfoObj = new Dictionary<string, SpinnerInfo>();
        #endregion

        [CascadingParameter]
        private SfSpinner BaseParent { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether visible status of the spinner.
        /// </summary>
        [Parameter]
        public bool Show { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether type of the spinner.
        /// </summary>
        [Parameter]
        public bool TypeUpdate { get; set; }

        #region Render Methods

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            radius = CalculateRadius(BaseParent.Type, BaseParent.Size);
            SpinnerSvgClass = SVG_CLASS;
            SvgId = SfBaseUtils.GenerateID(SVG_ID);
            globalTimeOutObj.Add(SvgId, new GlobalTimeOut() { TimeOut = ZERO, Radius = radius, Type = BaseParent.Type });
            decimal diameter = radius * TWO_DIVISION;
            decimal strokeSize = GetStrokeSize(diameter);
            string transformOrigin = (diameter / TWO_DIVISION) + PX;
            string tranformStyle = TRANSFORM_ORIGIN + COLON_GAP + transformOrigin + SPACE + transformOrigin + SPACE + transformOrigin + SEMICOLON;
            ViewBox = VIEWBOX_CONST + diameter + SPACE + diameter;
            SvgStyle = WIDTH + COLON_GAP + diameter + PX_GAP + HEIGHT + COLON_GAP + diameter + PX_GAP + tranformStyle;
            attributeD = DrawArc(diameter, strokeSize);
            strokeWidth = Convert.ToString(strokeSize, System.Globalization.CultureInfo.InvariantCulture);
            strokeDashArray = Convert.ToString((double)(diameter - strokeSize) * Math.PI * POINT_SEVEN_FIVE_STROKE, System.Globalization.CultureInfo.InvariantCulture);
            strokeDashOffset = Convert.ToString(GetDashOffset(diameter, strokeSize, ONE, SEVENTY_FIVE_MAX), System.Globalization.CultureInfo.InvariantCulture);
            show = Show;
            await base.OnInitializedAsync();
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            show = NotifyPropertyChanges(SHOW, Show, show);
            typeUpdate = NotifyPropertyChanges(TYPEUPDATE, TypeUpdate, typeUpdate);
            if (PropertyChanges.Count > ZERO)
            {
                if (BaseParent.Visible && (show || typeUpdate))
                {
                    await AnimateShow(false);
                }
                else
                {
                    timer?.Dispose();
                    globalTimeOutObj[SvgId].IsAnimate = false;
                }
            }

            await base.OnParametersSetAsync();
        }

        private static double GetDashOffset(decimal diameter, decimal strokeSize, int offset, int max)
        {
            int tempA = THREE_OFFSET * max;
            decimal tempB = (decimal)tempA / HUNDRED_DIVISION;
            decimal tempC = (decimal)offset / HUNDRED_DIVISION;
            return (double)(diameter - strokeSize) * Math.PI * (double)(tempB - tempC);
        }

        #endregion

        #region Animation Methods
        private async Task AnimateShow(bool isHide)
        {
            globalTimeOutObj[SvgId].IsAnimate = !isHide;
            if (globalTimeOutObj[SvgId].Type == SpinnerType.Material)
            {
                globalTimeOutObj[SvgId].TimeOut = ZERO;
                globalObject.Clear();
                globalObject.Add(SvgId, new GlobalVariables() { Radius = globalTimeOutObj[SvgId].Radius, Count = ZERO, PreviousId = ZERO });
                spinnerInfoObj.Clear();
                spinnerInfoObj.Add(SvgId, new SpinnerInfo() { UniqueID = SvgId, TimeOutVar = ZERO, GlobalInfo = new GlobalVariables() { Radius = globalTimeOutObj[SvgId].Radius, Count = ZERO, PreviousId = ZERO } });
                await Animate(spinnerInfoObj, SvgId);
            }
        }

        private async Task Animate(IDictionary<string, SpinnerInfo> spinnerInfoObj, string uniqueId)
        {
            int start = ONE;
            int end = ANIMATE_END;
            int duration = ANIMATE_DURATION;
            int max = SEVENTY_FIVE_MAX;
            await CreateCircle(start, end, duration, max, spinnerInfoObj, uniqueId);
            if (spinnerInfoObj.ContainsKey(uniqueId))
            {
                spinnerInfoObj[uniqueId].GlobalInfo.Count = spinnerInfoObj[uniqueId].GlobalInfo.Count + ONE;
                spinnerInfoObj[uniqueId].GlobalInfo.Count = spinnerInfoObj[uniqueId].GlobalInfo.Count % FOUR_CIRCLE_VALUE;
            }
            else
            {
                timer?.Dispose();
                timer = null;
            }
        }

        private void TickTimer(object state)
        {
            InvokeAsync(() => Animation());
        }

        private async Task CreateCircle(int start, int end, int duration, int max, IDictionary<string, SpinnerInfo> spinnerInfoObj, string uniqueId)
        {
            circleStart = start;
            circleUniqueId = uniqueId;
            circleDuration = duration;
            circleMax = max;
            circleId = ++spinnerInfoObj[uniqueId].GlobalInfo.PreviousId;
            TimeSpan time = DateTime.Now.ToUniversalTime() - new DateTime(YEAR, ONE, ONE);
            startTime = time.TotalMilliseconds + POINT_FIVE_SECONDS;
            circleChange = end - circleStart;
            circleDiameter = float.Parse((spinnerInfoObj[uniqueId].GlobalInfo.Radius * TWO_DIVISION) + string.Empty, null);
            circleStrokeSize = (decimal)TEN_STROKE_SIZE / HUNDRED_DIVISION * (decimal)circleDiameter;
            circleRotate = MINUS_NINETY_ANGLE * ((spinnerInfoObj[uniqueId].GlobalInfo.Count == ZERO) ? ZERO : spinnerInfoObj[uniqueId].GlobalInfo.Count);
            await Animation();
        }

        private async Task Animation()
        {
            if (globalTimeOutObj.Count > 0 && globalTimeOutObj[SvgId].IsAnimate)
            {
                double currentTime = Math.Max(ZERO, Math.Min((DateTime.Now.ToUniversalTime() - new DateTime(YEAR, ONE, ONE)).TotalMilliseconds - startTime, circleDuration));
                UpdatePath(EaseAnimation(currentTime));
                if (circleId == spinnerInfoObj[circleUniqueId].GlobalInfo.PreviousId && currentTime < circleDuration)
                {
                    await Task.Delay(10);
                    timer = new Timer(new TimerCallback(TickTimer), null, ZERO, ZERO);
                }
                else
                {
                    await Animate (spinnerInfoObj, circleUniqueId);
                }

                StateHasChanged();
            }
        }

        private double EaseAnimation(double current)
        {
            double timeStamp = (current /= circleDuration) * current;
            double timeCount = timeStamp * current;
            return circleStart + (circleChange * ((SIX_EASE * timeCount * timeStamp) + (MINUS_FIFTEEN_EASE * timeStamp * timeStamp) + (TEN_STROKE_SIZE * timeCount)));
        }

        private double GetDashOffsetCircle(double offset)
        {
            int tempA = THREE_OFFSET * circleMax;
            decimal tempB = (decimal)tempA / HUNDRED_DIVISION;
            decimal tempC = (decimal)offset / HUNDRED_DIVISION;
            return (circleDiameter - Convert.ToInt32(circleStrokeSize)) * Math.PI * (double)(tempB - tempC);
        }

        private void UpdatePath(double offset)
        {
            strokeDashOffset = Convert.ToString(GetDashOffsetCircle(offset), System.Globalization.CultureInfo.InvariantCulture);
            pathTransform = ROTATE + OPEN_ROUND_BRACKET + circleRotate + SPACE + (circleDiameter / TWO_DIVISION) + SPACE + (circleDiameter / TWO_DIVISION) + CLOSE_ROUND_BRACKET;
        }

        #endregion
        internal override void ComponentDispose()
        {
            if (globalTimeOutObj[SvgId].IsAnimate)
            {
                timer?.Dispose();
                globalTimeOutObj[SvgId].IsAnimate = false;
            }

            globalTimeOutObj.Clear();
            globalObject.Clear();
            spinnerInfoObj.Clear();
            BaseParent = null;
        }
    }

    internal class GlobalVariables
    {
        internal int Radius { get; set; }

        internal int Count { get; set; }

        internal int PreviousId { get; set; }
    }

    internal class SpinnerInfo
    {
        internal string UniqueID { get; set; }

        internal int TimeOutVar { get; set; }

        internal GlobalVariables GlobalInfo { get; set; }
    }
}