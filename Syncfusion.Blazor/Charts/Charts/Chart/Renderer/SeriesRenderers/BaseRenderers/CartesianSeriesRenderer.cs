using System.Collections.Generic;

namespace Syncfusion.Blazor.Charts.Internal
{
    public interface IRequireAxis
    {
        ChartAxisRenderer XAxisRenderer { get; set; }

        ChartAxisRenderer YAxisRenderer { get; set; }

        string XAxisName { get; set; }

        string YAxisName { get; set; }

        DoubleRange XRange { get; set; }

        DoubleRange YRange { get; set; }

        bool IsVisible { get; set; }

        double XMin { get; set; }

        double XMax { get; set; }

        double YMin { get; set; }

        double YMax { get; set; }

        List<double> XData { get; set; }

        List<double> YData { get; set; }

        void OnAxisChanged();
    }
}
