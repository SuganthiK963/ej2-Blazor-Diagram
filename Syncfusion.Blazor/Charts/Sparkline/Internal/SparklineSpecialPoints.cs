using Syncfusion.Blazor.DataVizCommon;
using Syncfusion.Blazor.Sparkline.Internal;

namespace Syncfusion.Blazor.Charts
{
    public partial class SfSparkline<TValue>
    {
        private double maxPoint;
        private double minPoint;
        private double minXPOint;
        private double maxXPoint;
        private double highPointIndex;
        private double lowPointIndex;
        private double startPointIndex;
        private double endPointIndex;

        private string GetPieSpecialPoint(SparklineValues data, int i, double high, double low, int length, string[] colors)
        {
            string fill = colors[i % colors.Length];
            if (data.YVal < 0 && !string.IsNullOrEmpty(NegativePointColor))
            {
                fill = NegativePointColor;
            }

            if (i == 0 && !string.IsNullOrEmpty(StartPointColor))
            {
                fill = StartPointColor;
                startPointIndex = i;
            }
            else if ((i == length - 1) && !string.IsNullOrEmpty(EndPointColor))
            {
                fill = EndPointColor;
                endPointIndex = i;
            }

            if (data.YVal == high && !string.IsNullOrEmpty(HighPointColor))
            {
                fill = HighPointColor;
                highPointIndex = i;
            }
            else if (data.YVal == low && !string.IsNullOrEmpty(LowPointColor))
            {
                fill = LowPointColor;
                lowPointIndex = i;
            }

            return fill;
        }

        private bool GetSpecialPoint(bool render, SparklineValues data, CircleOptions option, int i, double highPos, double lowPos, int length, string visible)
        {
            if (data.MarkerPosition > AxisHeight)
            {
                option.Fill = !string.IsNullOrEmpty(NegativePointColor) ? NegativePointColor : option.Fill;
                render = render ? render : visible.Contains(VisibleType.Negative.ToString(), comparison);
            }

            if (i == 0)
            {
                option.Fill = !string.IsNullOrEmpty(StartPointColor) ? StartPointColor : option.Fill;
                startPointIndex = i;
                render = render ? render : visible.Contains(VisibleType.Start.ToString(), comparison);
            }
            else if (i == length - 1)
            {
                option.Fill = !string.IsNullOrEmpty(EndPointColor) ? EndPointColor : option.Fill;
                endPointIndex = i;
                render = render ? render : visible.Contains(VisibleType.End.ToString(), comparison);
            }

            if (data.MarkerPosition == highPos)
            {
                option.Fill = !string.IsNullOrEmpty(HighPointColor) ? HighPointColor : option.Fill;
                highPointIndex = i;
                render = render ? render : visible.Contains(VisibleType.High.ToString(), comparison);
            }
            else if (data.MarkerPosition == lowPos)
            {
                option.Fill = !string.IsNullOrEmpty(LowPointColor) ? LowPointColor : option.Fill;
                lowPointIndex = i;
                render = render ? render : visible.Contains(VisibleType.Low.ToString(), comparison);
            }

            if (visible.Contains(VisibleType.None.ToString(), comparison))
            {
                render = false;
            }

            return render;
        }

        private bool GetSpecialPoint(bool render, SparklineValues data, RectOptions option, int i, double highPos, double lowPos, int length, string visible = "")
        {
            if (data.MarkerPosition > AxisHeight)
            {
                option.Fill = !string.IsNullOrEmpty(NegativePointColor) ? NegativePointColor : option.Fill;
                render = render ? render : visible.Contains(VisibleType.Negative.ToString(), comparison);
            }

            if (i == 0)
            {
                option.Fill = !string.IsNullOrEmpty(StartPointColor) ? StartPointColor : option.Fill;
                startPointIndex = i;
                render = render ? render : visible.Contains(VisibleType.Start.ToString(), comparison);
            }
            else if (i == length - 1)
            {
                option.Fill = !string.IsNullOrEmpty(EndPointColor) ? EndPointColor : option.Fill;
                endPointIndex = i;
                render = render ? render : visible.Contains(VisibleType.End.ToString(), comparison);
            }

            if (data.MarkerPosition == highPos)
            {
                option.Fill = !string.IsNullOrEmpty(HighPointColor) ? HighPointColor : option.Fill;
                highPointIndex = i;
                render = render ? render : visible.Contains(VisibleType.High.ToString(), comparison);
            }
            else if (data.MarkerPosition == lowPos)
            {
                option.Fill = !string.IsNullOrEmpty(LowPointColor) ? LowPointColor : option.Fill;
                lowPointIndex = i;
                render = render ? render : visible.Contains(VisibleType.Low.ToString(), comparison);
            }

            if (visible.Contains(VisibleType.None.ToString(), comparison))
            {
                render = false;
            }

            return render;
        }
    }
}