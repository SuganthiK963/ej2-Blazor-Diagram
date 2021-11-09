using System;
using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Inputs
{
    /// <summary>
    /// Specifies the list of files that will be preloaded on rendering of uploader component.
    /// </summary>
    public class UploaderUploadedFiles : SfBaseComponent
    {
        [CascadingParameter]
        private UploaderFiles Parent { get; set; }

        [CascadingParameter]
        private SfUploader BaseParent { get; set; }

        /// <summary>
        /// Specifies the content that has to be passed.
        /// </summary>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Specifies the name of the file.
        /// </summary>
        [Parameter]
        public string Name { get; set; } = string.Empty;

        private string name { get; set; }

        /// <summary>
        /// Specifies the size of the file.
        /// </summary>
        [Parameter]
        public double Size { get; set; }

        private double size { get; set; }

        /// <summary>
        /// Specifies the type of the file.
        /// </summary>
        [Parameter]
        public string Type { get; set; } = string.Empty;

        private string type { get; set; }

        /// <summary>
        /// Triggers while initial rendering of the component.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent.UpdateChildProperty(this);
            name = Name;
            size = Size;
            type = Type;
            await BaseParent.CallStateHasChangedAsync();
        }

        /// <summary>
        /// Triggers while dynamically updating the component properties.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            Name = name = NotifyPropertyChanges(nameof(Name), Name, name);
            Size = size = NotifyPropertyChanges(nameof(Size), Size, size);
            Type = type = NotifyPropertyChanges(nameof(Type), Type, type);
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            BaseParent = null;
        }
    }
}