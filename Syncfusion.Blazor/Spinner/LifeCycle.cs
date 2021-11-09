using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.Spinner
{
    public partial class SfSpinner : SfBaseComponent
    {
        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            style = $"{Z_INDEX}: {ZIndex};";
            attributes.Add(STYLE, style);
            await base.OnInitializedAsync();
            ScriptModules = SfScriptModules.SfSpinner;
            initialType = Type;
            indexZ = ZIndex;
            type = Type;
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            visible = await SfBaseUtils.UpdateProperty(Visible, visible, VisibleChanged);
            indexZ = NotifyPropertyChanges(ZINDEX, ZIndex, indexZ);
            type = NotifyPropertyChanges(TYPE, Type, type);
            if (PropertyChanges.Count > 0)
            {
                List<string> changedKeys = new List<string>(PropertyChanges.Keys);
                OnPropertyChange(changedKeys);
            }
        }

        /// <summary>
        /// Method invoked after each time the component has been rendered.
        /// </summary>
        /// <param name="firstRender"> Set to true if this is the first time OnAfterRender(Boolean) has been invoked.</param>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await SfBaseUtils.InvokeEvent<object>(Delegates?.Created, new { Name = CREATED });
            }

            if (previousVisible != Visible)
            {
                previousVisible = Visible;
                if (Visible)
                {
                    await ShowInternal();
                }
                else
                {
                    await HideInternal();
                }
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        internal override async Task OnAfterScriptRendered()
        {
            if (spinnerElement.Id != null)
            {
                string theme = await InvokeMethod<string>(JS_INITIALIZE, false, spinnerElement, DotnetObjectReference);
                if (Type == SpinnerType.None)
                {
                    switch (theme)
                    {
                        case FABRIC:
                            Type = SpinnerType.Fabric;
                            break;
                        case HIGHCONTRAST:
                            Type = SpinnerType.HighContrast;
                            break;
                        case MATERIAL:
                            Type = SpinnerType.Material;
                            break;
                        case BOOTSTRAP:
                            Type = SpinnerType.Bootstrap;
                            break;
                        case BOOTSTRAP4:
                            Type = SpinnerType.Bootstrap4;
                            break;
                        case BOOTSTRAP5:
                            Type = SpinnerType.Bootstrap5;
                            break;
                        case TAILWIND:
                            Type = SpinnerType.Tailwind;
                            break;
                    }

                    typeUpdated = true;
                    await InvokeAsync(() => StateHasChanged());
                }
            }
        }

        internal override async void ComponentDispose()
        {
            if (IsRendered)
            {
                try
                {
                    ChildContent = null;
                    attributes.Clear();
                    await SfBaseUtils.InvokeEvent<object>(Delegates?.Destroyed, new { Name = DESTROYED });
                }
                catch (ObjectDisposedException e)
                {
                    await SfBaseUtils.InvokeEvent<object>(Delegates?.Destroyed, e);
                }
                catch (InvalidOperationException e)
                {
                    await SfBaseUtils.InvokeEvent<object>(Delegates?.Destroyed, e);
                }
            }
        }
    }
}