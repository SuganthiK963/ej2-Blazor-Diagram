using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.Spinner.Internal
{
    /// <summary>
    /// Represents the Spinner's Bootstrap class.
    /// </summary>
    /// <exclude/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public partial class Bootstrap : SpinnerBase
    {
        private const string CIRCLE_CLASS = "e-path-circle";
        private const string SVG_CLASS = "e-spin-bootstrap";
        private const string TRANSLATE = "translate";
        private const int VIEWBOX_VALUE = 64;
        private const int TRANSLATE_VALUE = 32;
        private const int DEFAULT_RADIUS = 2;
        private const int THREE_SIXTY_ANGLE = 360;
        private const int INITIAL_RADIUS = 24;
        private const int FOURTY_FIVE_ANGLE = 45;
        private const int TIME_OUT_HUNDRED = 110;
        private const double TIME_DIFFERENCE = 0.2;
        private const int EIGHT_DOT = 8;
        private const int SEVEN_DOT = 7;
        private const string TIME_FORMAT = "{0:0.00}";

        private Timer timer;
        private Timer loopTimer;
        private IDictionary<string, GlobalTimeOut> globalTimeOutObj = new Dictionary<string, GlobalTimeOut>();
        private bool show;
        private bool typeUpdate;
        private string circlePathClass = CIRCLE_CLASS;
        private List<string> circleRadius = new List<string>();
        private string circleTransform;
        private List<int> circleIndex = new List<int>();
        private List<double> circleCx = new List<double>();
        private List<double> circleCy = new List<double>();
        private List<float> circleSeries = new List<float>();
        private List<List<float>> radiusSeries = new List<List<float>>();
        private int start;
        private int end;
        private bool increment;
        private int count;
        private int rotationCount;
        private float currentSeries;
        private int elementIndex;
        private int radius;

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

        [CascadingParameter]
        private SfSpinner BaseParent { get; set; }

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
            int viewBoxValue = VIEWBOX_VALUE;
            int trans = TRANSLATE_VALUE;
            int defaultRadius = DEFAULT_RADIUS;
            ViewBox = VIEWBOX_CONST + viewBoxValue + SPACE + viewBoxValue;
            circleTransform = TRANSLATE + OPEN_ROUND_BRACKET + trans + COMMA + trans + CLOSE_ROUND_BRACKET;
            SvgStyle = WIDTH + COLON_GAP + radius + PX_GAP + HEIGHT + COLON_GAP + radius + PX + SEMICOLON;
            int varX = ZERO;
            int varY = ZERO;
            int initialRadius = INITIAL_RADIUS;
            int startArc = NINETY_ANGLE;
            for (int item = 0; item <= SEVEN_DOT; item++)
            {
                circleRadius.Add(defaultRadius + string.Empty);
                ArcPoints startObj = DefineArcPoints(varX, varY, initialRadius, startArc);
                circleCx.Add(startObj.VarX);
                circleCy.Add(startObj.VarY);
                startArc = startArc >= THREE_SIXTY_ANGLE ? ZERO : startArc;
                startArc = startArc + FOURTY_FIVE_ANGLE;
            }

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
                    BootstrapAnimateShow(false);
                }
                else
                {
                    timer?.Dispose();
                    loopTimer?.Dispose();
                    globalTimeOutObj[SvgId].IsAnimate = false;
                }
            }

            await base.OnParametersSetAsync();
        }

        private static float FloatParse(double difference)
        {
            return float.Parse(string.Format(null, TIME_FORMAT, difference), null);
        }

        private void BootstrapAnimateShow(bool isHide)
        {
            globalTimeOutObj[SvgId].IsAnimate = !isHide;
            radiusSeries.Add(new List<float>());
            for (int i = 1; i <= EIGHT_DOT; i++)
            {
                elementIndex = i == EIGHT_DOT ? ZERO : i;
                radiusSeries.Add(new List<float>());
                radiusSeries[elementIndex] = GenerateSeries(i, i);
                Rotation(i);
            }
        }

        private void Rotation(float radius)
        {
            rotationCount = ZERO;
            BootAnimate(radius);
        }

        private void TickTimer(object state)
        {
            InvokeAsync(() => BootAnimateLoop(currentSeries));
        }

        private void BootAnimate(float radius)
        {
            if (globalTimeOutObj.Count > 0 && globalTimeOutObj[SvgId].IsAnimate)
            {
                UpdateCircleRadius(radius);
                currentSeries = circleSeries[rotationCount];
                StateHasChanged();
                timer = new Timer(new TimerCallback(TickTimer), currentSeries, TIME_OUT_HUNDRED, ZERO);
            }
        }

        private void UpdateCircleRadius(float radius)
        {
            if (elementIndex == 1 && rotationCount == 1)
            {
                radius = radiusSeries[1][rotationCount];
            }

            ++rotationCount;
            circleRadius[elementIndex] = Convert.ToString(radius, System.Globalization.CultureInfo.InvariantCulture);
            elementIndex = elementIndex == SEVEN_DOT ? ZERO : elementIndex + 1;
            if (rotationCount >= circleSeries.Count)
            {
                rotationCount = ZERO;
            }
        }

        private void BootAnimateLoop(float radius)
        {
            if (globalTimeOutObj.Count > 0 && globalTimeOutObj[SvgId].IsAnimate)
            {
                UpdateCircleRadius(radius);
                currentSeries = radiusSeries[elementIndex][rotationCount];
                StateHasChanged();
                loopTimer = new Timer(new TimerCallback(TickTimer), null, TIME_OUT_HUNDRED, ZERO);
            }
        }

        private List<float> GenerateSeries(int begin, int stop)
        {
            start = begin;
            end = stop;
            increment = false;
            count = 1;
            circleSeries = new List<float>();
            FormSeries(start);
            return circleSeries;
        }

        private void FormSeries(float circle)
        {
            circleSeries.Add(circle);
            if (circle != end || count == 1)
            {
                if (circle <= start && circle > 1 && !increment)
                {
                    circle = FloatParse(circle - TIME_DIFFERENCE);
                }
                else if (circle == 1)
                {
                    circle = SEVEN_DOT;
                    circle = FloatParse(circle + TIME_DIFFERENCE);
                    increment = true;
                }
                else if (circle < EIGHT_DOT && increment)
                {
                    circle = FloatParse(circle + TIME_DIFFERENCE);
                    if (circle == EIGHT_DOT)
                    {
                        increment = false;
                    }
                }
                else if (circle <= EIGHT_DOT && !increment)
                {
                    circle = FloatParse(circle - TIME_DIFFERENCE);
                }

                ++count;
                FormSeries(circle);
            }
        }

        internal override void ComponentDispose()
        {
            if (globalTimeOutObj[SvgId].IsAnimate)
            {
                timer?.Dispose();
                loopTimer?.Dispose();
                globalTimeOutObj[SvgId].IsAnimate = false;
            }

            circleRadius.Clear();
            circleIndex.Clear();
            circleCx.Clear();
            circleCy.Clear();
            circleSeries.Clear();
            radiusSeries.Clear();
            globalTimeOutObj.Clear();
            BaseParent = null;
        }
    }
}