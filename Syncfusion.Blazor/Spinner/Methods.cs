using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Spinner
{
    public partial class SfSpinner : SfBaseComponent
    {
        #region Public Methods
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This Show method is deprecated. Use ShowAsync method with out arguments to achieve the functionality.")]
        public async Task Show()
        {
            await ShowAsync();
        }

        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("This HideSpinner method is deprecated. Use HideAsync method with out arguments to achieve the functionality.")]
        public async Task Hide()
        {
            await HideAsync();
        }

        /// <summary>
        /// Shows the spinner.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        public async Task ShowAsync()
        {
            await ShowInternal();
        }

        /// <summary>
        /// Hides the spinner.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        public async Task HideAsync()
        {
            await HideInternal();
        }
        #endregion
    }
}