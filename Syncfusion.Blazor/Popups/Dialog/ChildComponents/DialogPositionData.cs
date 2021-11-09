using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Popups
{
    /// <summary>
    /// Configures to display a dialog in the custom position within the document or target.
    /// </summary>
    public class DialogPositionData : SfBaseComponent
    {
        [CascadingParameter]
        internal SfDialog Parent { get; set; }

        private const string CONST_X = "X";
        private const string CONST_Y = "Y";
        private const string POSITION = "position";
        private const string CHANGE_POSITION = "sfBlazor.Dialog.changePosition";

        private string x;
        private string y;

        /// <summary>
        /// Specifies the offset left value to position the dialog.
        /// </summary>
        [Parameter]
        public string X { get; set; }

        /// <summary>
        /// Specifies the offset top value to position the dialog.
        /// </summary>
        [Parameter]
        public string Y { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.UpdateChildProperties(POSITION, this);
            x = X;
            y = Y;
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            x = NotifyPropertyChanges(CONST_X, X, x);
            y = NotifyPropertyChanges(CONST_Y, Y, y);
            if (PropertyChanges.Count > 0)
            {
                await InvokeMethod(CHANGE_POSITION, Parent.GetInstance());
            }
        }

        internal override void ComponentDispose()
        {
            Parent = null;
        }
    }
}