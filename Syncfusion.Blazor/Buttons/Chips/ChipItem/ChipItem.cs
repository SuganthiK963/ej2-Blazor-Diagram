using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Buttons
{
    /// <summary>
    /// A Chip is a small block of essential information that contains the text, image, or both and mostly used in contacts, mails, or filter tags.
    /// </summary>
    public partial class ChipItem : SfBaseComponent
    {
        /// <summary>
        /// Indicates the ChipItems component.
        /// </summary>
        [CascadingParameter]
        internal ChipItems Parent { get; set; }

        /// <summary>
        /// Indicates the SfChip component.
        /// </summary>
        [CascadingParameter]
        internal SfChip BaseParent { get; set; }
    }
}
