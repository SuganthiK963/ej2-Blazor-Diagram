namespace Syncfusion.Blazor.Charts
{
    public partial class ChartPrimaryXAxis : ChartAxis
    {
        public override string GetName()
        {
            Name = string.IsNullOrEmpty(Name) ? Constants.PRIMARYXAXIS : Name;
            return Name;
        }
    }
}
