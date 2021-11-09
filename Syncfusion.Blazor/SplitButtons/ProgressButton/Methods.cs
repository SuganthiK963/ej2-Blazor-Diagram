using System.Threading.Tasks;
using System;
using System.ComponentModel;

namespace Syncfusion.Blazor.SplitButtons
{
    /// <summary>
    /// ProgressButton visualizes the progression of an operation to indicate the user that a process is happening in the background with visual representation.
    /// </summary>
    public partial class SfProgressButton : SfBaseComponent
    {
        /// <summary>
        /// Starts the button progress at the specified percent.
        /// </summary>
        /// <param name="percent">Specifies the Progress percent.</param>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task Start(double percent = 0)
        {
            isPaused = false;
            await FocusAsync();
            await StartSpinner();
            InitProgress(DateTimeOffset.Now.ToUnixTimeMilliseconds(), DateTimeOffset.Now.ToUnixTimeMilliseconds(), percent);
        }

        /// <summary>
        /// Starts the button progress at the specified percent.
        /// </summary>
        /// <param name="percent">Specifies the Progress percent.</param>
        public async Task StartAsync(double percent = 0)
        {
            await Start(percent);
        }

        /// <summary>
        /// Stops the button progress.
        /// </summary>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task Stop()
        {
            isPaused = true;
            await CancelFrame();
        }

        /// <summary>
        /// Stops the button progress.
        /// </summary>
        public async Task StopAsync()
        {
            await Stop();
        }

        /// <summary>
        /// Sets the focus to ProgressButton.
        /// </summary>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task FocusIn()
        {
            await buttonObj.FocusAsync();
        }

        /// <summary>
        /// Sets the focus to ProgressButton.
        /// </summary>
        public async Task FocusAsync()
        {
            await FocusIn();
        }

        /// <summary>
        /// Complete the button progress.
        /// </summary>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task ProgressComplete()
        {
            isPaused = true;
            percent = 100;
            await EndProgress();
        }

        /// <summary>
        /// Complete the button progress.
        /// </summary>
        public async Task EndProgressAsync()
        {
            await ProgressComplete();
        }

        /// <summary>
        /// Click the button element.
        /// </summary>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task Click()
        {
            await ClickHandler();
            await FocusAsync();
            StateHasChanged();
        }

        /// <summary>
        /// Click the button element.
        /// </summary>
        public async Task ClickAsync()
        {
            await Click();
        }
    }
}
