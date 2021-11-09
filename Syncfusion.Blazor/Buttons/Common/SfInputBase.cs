using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Syncfusion.Blazor.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Syncfusion.Blazor.Lists")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.DropDowns")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.Navigations")]
[assembly: InternalsVisibleTo("Syncfusion.Blazor.SplitButtons")]

namespace Syncfusion.Blazor.Buttons
{
    /// <summary>
    /// A base class for form input components.
    /// </summary>
    public abstract class SfInputBase<TChecked> : SfBaseComponent
    {
        internal string idValue;
        private const string RIPPLE = "e-ripple-container";
        internal ElementReference input;
        internal ElementReference rippleElement;

        [CascadingParameter]
        private EditContext CascadedEditContext { get; set; } = default;

        /// <exclude/>
        /// <summary>
        /// Defines the caption for the input, that describes the purpose of the input including HTML and its customization.
        /// </summary>
        [Parameter]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Defines class/multiple classes separated by a space in the component wrapper element.
        /// You can add custom styles to the component by using this property.
        /// </summary>
        [Parameter]
        public string CssClass { get; set; }

        /// <summary>
        /// Defines name attribute for the input element.
        /// </summary>
        [Parameter]
        public string Name { get; set; }

        /// <summary>
        /// Defines value attribute for the input element.
        /// </summary>
        [Parameter]
        public string Value { get; set; }

        /// <summary>
        /// Specifies a value that indicates whether the component is checked or not.
        /// </summary>
        [Parameter]
        public TChecked Checked { get; set; }

        private TChecked isChecked { get; set; }

        /// <exclude/>
        [Parameter]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public EventCallback<TChecked> CheckedChanged { get; set; }

        /// <exclude/>
        [Parameter]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public Expression<Func<TChecked>> CheckedExpression { get; set; }

        /// <summary>
        /// Specifies a value that indicates whether the component is disabled or not.
        /// When set to true, the component will be in disabled state.
        /// </summary>
        [Parameter]
        public bool Disabled { get; set; }

        /// <summary>
        /// Enable or disable rendering component in right to left direction.
        /// </summary>
        [Parameter]
        public bool EnableRtl { get; set; }

        /// <summary>
        /// Enable or disable persisting component's state between page reloads.
        /// </summary>
        [Parameter]
        public bool EnablePersistence { get; set; }

        internal Dictionary<string, object> htmlAttr;
        /// <exclude/>
        /// <summary>
        /// You can add the additional html attributes such as title , native events etc., to the element.
        /// </summary>
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> HtmlAttributes
        {
            get { return htmlAttr; }
            set { htmlAttr = value; }
        }

        /// <summary>
        /// Triggers once the component rendering is completed.
        /// </summary>
        [Parameter]
        public EventCallback<object> Created { get; set; }
        private const string CHECKED = "Checked";
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            isChecked = Checked;
        }

        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            isChecked = NotifyPropertyChanges(CHECKED, Checked, isChecked, true);
            if (IsRendered)
            {
                await UpdateCheckState(Checked);
            }

            InitRender();
            UpdateHTMLAttributes();
        }

        private void UpdateHTMLAttributes()
        {
            if (this.HtmlAttributes != null)
            {
                foreach (var item in (Dictionary<string, object>)this.HtmlAttributes)
                {
                    if (item.Key == "id")
                    {
                        idValue = item.Value?.ToString();
                    }
                }
            }
        }

        protected async Task UpdateCheckState(TChecked state)
        {
            Checked = isChecked = await SfBaseUtils.UpdateProperty(state, isChecked, CheckedChanged, CascadedEditContext, CheckedExpression);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                if (EnablePersistence)
                {
                    var localStorageValue = await InvokeMethod<string>("window.localStorage.getItem", false, new object[] { idValue });
                    localStorageValue = string.IsNullOrEmpty(localStorageValue) ? null : localStorageValue;
                    if (!(localStorageValue == null && Checked != null))
                    {
                        if (localStorageValue == "null") {
                            var persistValue = (TChecked)(object)null;
                            Checked = isChecked = await SfBaseUtils.UpdateProperty(persistValue, isChecked, CheckedChanged, CascadedEditContext, CheckedExpression);
                        }
                        else
                        {
                            var persistValue = (TChecked)SfBaseUtils.ChangeType(localStorageValue, typeof(TChecked));
                            Checked = isChecked = await SfBaseUtils.UpdateProperty(persistValue, isChecked, CheckedChanged, CascadedEditContext, CheckedExpression);
                        }
                    }
                }
                await SfBaseUtils.InvokeEvent<object>(Created, null);
            }
        }

        internal override async Task OnAfterScriptRendered()
        {
            if (SyncfusionService.options.EnableRippleEffect)
            {
                RippleSettings ripple = new RippleSettings() { Duration = 400, Selector = "." + RIPPLE, IsCenterRipple = true };
                await SfBaseUtils.RippleEffect(JSRuntime, rippleElement, ripple);
            }
        }

        protected virtual void InitRender()
        {
        }

        /// <summary>
        /// Sets the focus to component.
        /// </summary>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public async Task FocusIn()
        {
            await InvokeMethod("sfBlazor.focusButton", input);
        }

        /// <summary>
        /// Sets the focus to component.
        /// </summary>
        public async Task FocusAsync()
        {
           await FocusIn();
        }

        protected async Task SetLocalStorage(string persistId, TChecked isChecked)
        {
            await InvokeMethod("window.localStorage.setItem", new object[] { persistId, isChecked });
        }

        protected async Task<string> GetPersistData()
        {
            return await InvokeMethod<string>("window.localStorage.getItem", false, new object[] { idValue });
        }
    }
}
