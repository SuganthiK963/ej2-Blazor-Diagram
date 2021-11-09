namespace Syncfusion.Blazor.Popups
{
    // Tooltip Interop Models
    internal class InternalTooltipAnimationSettings
    {
        public double? Delay { get; set; }

        public double? Duration { get; set; }

        public string Effect { get; set; }
    }

    internal class InternalAnimation
    {
        public InternalTooltipAnimationSettings Close { get; set; }

        public InternalTooltipAnimationSettings Open { get; set; }
    }
}