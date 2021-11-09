namespace Syncfusion.Blazor.Internal
{
    /// <summary>
    /// Animation properties for performing animation transition.
    /// </summary>
    internal class AnimationSettings
    {
        /// <summary>
        /// Gets or sets the animation duration.
        /// </summary>
        public int Duration { get; set; } = 400;

        /// <summary>
        /// Gets or sets the animation name.
        /// </summary>
        public string Name { get; set; } = "FadeIn";

        /// <summary>
        /// Gets or sets the animation timing function.
        /// </summary>
        public string TimingFunction { get; set; } = "ease";

        /// <summary>
        /// Gets or sets the animation delay.
        /// </summary>
        public int Delay { get; set; }
    }

    /// <summary>
    /// Ripple settings for performing the ripple effect.
    /// </summary>
    internal class RippleSettings
    {
        /// <summary>
        /// Gets or sets the ripple selector.
        /// </summary>
        public string Selector { get; set; }

        /// <summary>
        /// Gets or sets the ripple ignore.
        /// </summary>
        public string Ignore { get; set; }

        /// <summary>
        /// Gets or sets the ripple flag.
        /// </summary>
        public bool RippleFlag { get; set; } = true;

        /// <summary>
        /// Gets or sets the boolean value whether ripple center enabled or not.
        /// </summary>
        public bool IsCenterRipple { get; set; }

        /// <summary>
        /// Gets or sets the ripple duration.
        /// </summary>
        public int Duration { get; set; } = 350;
    }
}