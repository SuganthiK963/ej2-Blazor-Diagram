using System.Threading.Tasks;
using System.ComponentModel;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// Accordion is a vertically collapsible panel that displays one or more panels at a time.
    /// </summary>
    public partial class SfAccordion : SfBaseComponent
    {
        /// <summary>
        /// Sets focus to the specified index item header in Accordion.
        /// </summary>
        /// <param name="index">Number value that determines which item should be focused.</param>
        /// <returns><see cref="System.Threading.Tasks.Task"/>.</returns>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task Select(int index)
        {
            await SelectAsync(index);
        }

        /// <summary>
        /// Sets focus to the specified index item header in Accordion.
        /// </summary>
        /// <param name="index">Number value that determines which item should be focused.</param>
        /// <returns><see cref="System.Threading.Tasks.Task"/>.</returns>
        public async Task SelectAsync(int index)
        {
            await InvokeMethod("sfBlazor.Accordion.select", new object[] { Element, index });
        }
    }
}
