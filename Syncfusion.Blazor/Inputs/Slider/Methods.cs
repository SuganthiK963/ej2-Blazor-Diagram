using System.Threading.Tasks;
using System.ComponentModel;

namespace Syncfusion.Blazor.Inputs
{
    /// <summary>
    /// Specifies the partial class SfSlider.
    /// </summary>
    public partial class SfSlider<TValue> : SfBaseComponent
    {
        /// <summary>
        /// This method is used to reposition slider.
        /// </summary>
        /// <returns>"Task".</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task Reposition()
        {
            await InvokeMethod("sfBlazor.Slider.initialize", Slider, DotnetObjectReference, GetProperties());
        }

        /// <summary>
        /// This method is used to reposition slider.
        /// </summary>
        /// <returns>"Task".</returns>
        public async Task RepositionAsync()
        {
            await Reposition();
        }
    }
}
