namespace Syncfusion.Blazor.Charts
{
    public partial class ChartPrimaryYAxis : ChartAxis
    {
        public override string GetName()
        {
            Name = string.IsNullOrEmpty(Name) ? Constants.PRIMARYYAXIS : Name;
            return Name;
        }
    }
}
