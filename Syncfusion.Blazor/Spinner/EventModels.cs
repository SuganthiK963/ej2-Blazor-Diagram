namespace Syncfusion.Blazor.Spinner
{
    /// <summary>
    /// Provides data for the OnBeforeOpen and OnBeforeClose events.
    /// </summary>
    public class SpinnerEventArgs
    {
        /// <summary>
        /// Set cancel as true to prevent showing or hiding of the spinner.
        /// </summary>
        public bool Cancel { get; set; }
    }
}