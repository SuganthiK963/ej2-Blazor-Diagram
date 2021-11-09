using System.Threading.Tasks;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.Navigations
{
    /// <summary>
    /// Partial Class SfSidebar.
    /// </summary>
    public partial class SfSidebar : SfBaseComponent
    {
       

        /// <summary>
        /// Method invoked when the component is ready to start.
        /// </summary>
        /// <returns>="Task".</returns>
        protected override async Task OnInitializedAsync()
        {
            if (string.IsNullOrEmpty(ID))
            {
                ID = SfBaseUtils.GenerateID(IDPREFIX);
            }

            await base.OnInitializedAsync();
            SidebarCloseOnDocumentClick = CloseOnDocumentClick;
            SidebarIsOpen = IsOpen;
            SidebarPosition = Position;
            SliderShowBackdrop = ShowBackdrop;
            SidebarType = Type;
            SidebarWidth = Width;
            ScriptModules = SfScriptModules.SfSidebar;
            GetClass();
            GetStyle();
            SetDock();
            UpdateAttributes();
        }

        /// <summary>
        /// Method invoked when any changes in component state occurs.
        /// </summary>
        /// <returns>="Task".</returns>
        protected async override Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            SidebarCloseOnDocumentClick = NotifyPropertyChanges(nameof(CloseOnDocumentClick), CloseOnDocumentClick, SidebarCloseOnDocumentClick);
            SidebarPosition = NotifyPropertyChanges(nameof(Position), Position, SidebarPosition);
            SliderShowBackdrop = NotifyPropertyChanges(nameof(ShowBackdrop), ShowBackdrop, SliderShowBackdrop);
            SidebarType = NotifyPropertyChanges(nameof(Type), Type, SidebarType);
            SidebarWidth = NotifyPropertyChanges(nameof(Width), Width, SidebarWidth);

            if (EnablePersistence && IsRendered && !SfBaseUtils.Equals(IsOpen, SidebarIsOpen))
            {
                await SetLocalStorage(ID, SerializeModel());
            }

            SidebarIsOpen = NotifyPropertyChanges(nameof(IsOpen), IsOpen, SidebarIsOpen);
            if (PropertyChanges.Count > 0)
            {
                await SidebarPropertyChange(PropertyChanges);
            }

            UpdateAttributes();
        }

        /// <summary>
        /// Method invoked after each time the component has been rendered.
        /// </summary>
        /// <param name="firstRender">Set to true for the first time component rendering; otherwise gets false.</param>
        /// <returns>="Task".</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                await SfBaseUtils.InvokeEvent<object>(Created, null);
                isVisible = openState;
            }
        }

        internal override async Task OnAfterScriptRendered()
        {
            isScriptRendered = true;
            isDeviceMode = await InvokeMethod<bool>("sfBlazor.isDevice", false, null);
            await PersistProperties();
            isMediaQueryOpen = await InvokeMethod<bool>("sfBlazor.Sidebar.initialize", false, new object[] { element, DotnetObjectReference, GetProperties() });
            await SidebarInitRender();
            if (EnablePersistence)
            {
                GetClass();
                GetStyle();
                UpdateAttributes();
                StateHasChanged();
            }

            await SetSidebarType();
        }

        private async Task PersistProperties()
        {
            if (EnablePersistence)
            {
                PersistenceValues localStorageValue = await InvokeMethod<PersistenceValues>("window.localStorage.getItem", true, new object[] { ID });
                if (localStorageValue == null)
                {
                    await SetLocalStorage(ID, SerializeModel());
                }
                else
                {
                    IsOpen = SidebarIsOpen = await SfBaseUtils.UpdateProperty<bool>(localStorageValue.IsOpen, SidebarIsOpen, IsOpenChanged);
                }
            }
        }
    }
}