using System;
using System.ComponentModel;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Syncfusion.Blazor.Inputs
{
    /// <summary>
    /// Specifies the list of files that will be preloaded on rendering of uploader component.
    /// </summary>
    public partial class UploaderFiles : SfBaseComponent
    {
        [CascadingParameter]
        private SfUploader Parent { get; set; }

        /// <summary>
        /// Specifies the content that has to be passed.
        /// </summary>
        /// <exclude/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        /// <summary>
        /// Specifies the uploaded file list.
        /// </summary>
        public List<UploaderUploadedFiles> Files { get; set; } = new List<UploaderUploadedFiles>();

        internal void UpdateChildProperty(UploaderUploadedFiles file)
        {
            Files.Add(file);
        }

        /// <summary>
        /// Triggers while initial rendering of the component.
        /// </summary>
        /// <returns>Task.</returns>
        /// <exclude/>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            Parent?.UpdateChildProperties("Files", Files);
        }

        internal override void ComponentDispose()
        {
            Parent = null;
            Files = null;
        }
    }
}