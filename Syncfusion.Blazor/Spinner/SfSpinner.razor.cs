using System.Threading.Tasks;
using Syncfusion.Blazor.Internal;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Syncfusion.Blazor.PivotView")]
namespace Syncfusion.Blazor.Spinner
{
    /// <summary>
    /// Represents the spinner component that displays when spinner is shown.
    /// </summary>
    /// <exclude/> 
    public partial class SfSpinner : SfBaseComponent
    {
        #region Element/Module reference
        private ElementReference spinnerElement;
        private string statusClass = CLASS_HIDE;
        private string labelClass = CLASS_LABEL;
        private string spinnerClass = SPINNER_CLASS;
        private string innerClass = SPINNER_INNER_CLASS;
        private string style = string.Empty;
        private RenderFragment spinnerTemplate;
        private Dictionary<string, object> attributes = new Dictionary<string, object>();

        #endregion

        #region Internal variables
        private bool previousVisible;
        private SpinnerType initialType;
        private bool isShow;
        private bool typeUpdated;

        internal SpinnerEvents Delegates { get; set; }

        #endregion

        #region Internal methods
        private async Task ShowInternal()
        {
            SpinnerEventArgs beforeOpenEventArgs = new SpinnerEventArgs()
            {
                Cancel = false
            };
            await SfBaseUtils.InvokeEvent<SpinnerEventArgs>(Delegates?.OnBeforeOpen, beforeOpenEventArgs);
            if (!beforeOpenEventArgs.Cancel)
            {
                await UpdateVisible(true, CLASS_SHOW);
            }
        }

        private async Task UpdateVisible(bool visible, string status)
        {
            statusClass = status;
            previousVisible = Visible;
            await SfBaseUtils.UpdateProperty(visible, visible, VisibleChanged);
            Visible = visible;
            isShow = visible;
            await InvokeAsync(StateHasChanged);
        }

        private async Task HideInternal()
        {
            SpinnerEventArgs beforeCloseEventArgs = new SpinnerEventArgs()
            {
                Cancel = false
            };
            await SfBaseUtils.InvokeEvent<SpinnerEventArgs>(Delegates?.OnBeforeClose, beforeCloseEventArgs);
            if (!beforeCloseEventArgs.Cancel)
            {
                await UpdateVisible(false, CLASS_HIDE);
            }
        }

        /// <summary>
        /// Method invoked when property has been changed.
        /// </summary>
        /// <param name="changedKeys"> Specifies the updated properties.</param>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected void OnPropertyChange(List<string> changedKeys)
        {
            if (changedKeys != null)
            {
                foreach (string key in changedKeys)
                {
                    switch (key)
                    {
                        case TYPE:
                            if (Visible)
                            {
                                typeUpdated = true;
                                StateHasChanged();
                            }
                            break;
                        case ZINDEX:
                            style = $"{Z_INDEX}: {ZIndex};";
                            attributes.Clear();
                            attributes.Add(STYLE, style);
                            break;
                    }
                }
            }
        }

        internal void UpdateTemplate(RenderFragment template)
        {
            spinnerTemplate = template;
        }
        #endregion
    }
}