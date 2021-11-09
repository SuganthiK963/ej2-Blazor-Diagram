using Syncfusion.Blazor.Internal;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// Accordion is a vertically collapsible panel that displays one or more panels at a time.
    /// </summary>
    public partial class SfAccordion : SfBaseComponent
    {
        private bool enableRtl;
        private ExpandMode expandMode;
        private int[] expandedIndices;
        private string width;
        private string height;

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            if (string.IsNullOrEmpty(ID))
            {
                ID = SfBaseUtils.GenerateID(ACCORDIONPREFIX);
            }

            ScriptModules = SfScriptModules.SfAccordion;
            UpdateLocalProperties();
            await base.OnInitializedAsync();
            UpdateAnimationProperties(AnimationSettings);
            enableRtl = EnableRtl;
            expandMode = ExpandMode;
            expandedIndices = ExpandedIndices;
            width = Width;
            height = Height;
        }

        /// <summary>
        /// Method invoked when the component has received parameters from its parent in the render tree,
        /// and the incoming values have been assigned to properties.
        /// </summary>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            enableRtl = NotifyPropertyChanges(nameof(EnableRtl), EnableRtl, enableRtl);
            expandMode = NotifyPropertyChanges(nameof(ExpandMode), ExpandMode, expandMode);
            expandedIndices = NotifyPropertyChanges(nameof(ExpandedIndices), ExpandedIndices, expandedIndices);
            width = NotifyPropertyChanges(nameof(Width), Width, width);
            height = NotifyPropertyChanges(nameof(Height), Height, height);
            if (PropertyChanges.Count > 0)
            {
                await OnPropertyChangeHandler();
            }
        }

        /// <summary>
        /// Method invoked after each time the component has been rendered.
        /// </summary>
        /// <param name="firstRender">Set to true for the first time component rendering; otherwise gets false.</param>
        /// <returns>A System.Threading.Tasks.Task representing any asynchronous operation.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && EnablePersistence)
            {
                var localStorage = await InvokeMethod<string>("window.localStorage.getItem", false, new object[] { $"accordion{ID}" });
                if (!string.IsNullOrEmpty(localStorage))
                {
                    int[] persistExpandedIndices = new int[Items.Count];
                    persistExpandedIndices = Array.ConvertAll<string, int>(localStorage.Split(','), Convert.ToInt32);
                    ExpandedIndices = expandedIndices = persistExpandedIndices;
                }

                UpdateExpandedIndices();
            }

            if (firstRender || IsItemChanged)
            {
                rootAttributes[ARIA_MULTISELECTABLE] = ExpandMode == ExpandMode.Single ? FALSE : TRUE;

                if (IsItemChanged)
                {
                    StateHasChanged();
                    IsItemChanged = false;
                    await InvokeMethod("sfBlazor.Accordion.itemChanged", new object[] { Element });
                }
            }

            await base.OnAfterRenderAsync(firstRender);
        }

        internal override async Task OnAfterScriptRendered()
        {
            await InvokeMethod("sfBlazor.Accordion.initialize", new object[] { Element, GetInstance(), DotnetObjectReference });
        }

#pragma warning disable CA2008 // Do not create tasks without passing a TaskScheduler
        internal override void ComponentDispose()
        {
            InvokeMethod("sfBlazor.Accordion.destroy", new object[] { Element }).ContinueWith(async t =>
            {
                await SfBaseUtils.InvokeEvent<object>(Delegates?.Destroyed, null);
            });
            Items = null;
            Delegates = null;
            rootAttributes = null;
            ExpandedItem = null;
            AnimationSettings = null;
        }
#pragma warning restore CA2008 // Do not create tasks without passing a TaskScheduler
    }
}
