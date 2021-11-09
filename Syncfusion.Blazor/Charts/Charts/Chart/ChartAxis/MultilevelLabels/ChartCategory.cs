using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Options to customize the category for the multilevel labels.
    /// </summary>
    public class ChartCategory : ChartSubComponent
    {
        #region STRIPLINE COMPONENT BACKING FIELDS
        private bool isPropertyChanged;
        private object start;
        private object end;
        private string text = string.Empty;
        #endregion

        [CascadingParameter]
        private ChartCategories Parent { get; set; }

        /// <summary>
        /// multi level labels custom data.
        /// </summary>
        [Parameter]
        public object CustomAttributes { get; set; }

        /// <summary>
        /// End value of the multi level labels.
        /// </summary>
        [Parameter]
        public object End
        {
            get
            {
                return end;
            }

            set
            {
                if (end == null || !end.Equals(value))
                {
                    end = value;
                    isPropertyChanged = Parent != null;
                }
            }
        }

        /// <summary>
        /// Maximum width of the text for multi level labels.
        /// </summary>
        [Parameter]
        public double MaximumTextWidth { get; set; }

        /// <summary>
        /// Start value of the multi level labels.
        /// </summary>
        [Parameter]
        public object Start
        {
            get
            {
                return start;
            }

            set
            {
                if (start == null || !start.Equals(value))
                {
                    start = value;
                    isPropertyChanged = Parent != null;
                }
            }
        }

        /// <summary>
        /// multi level labels text.
        /// </summary>
        [Parameter]
        public string Text
        {
            get
            {
                return text;
            }

            set
            {
                if (text != value)
                {
                    text = value;
                    isPropertyChanged = Parent != null;
                }
            }
        }

        /// <summary>
        /// Border type for labels.
        /// Rectangle
        /// Without Top Border
        /// Without Top and BottomBorder
        /// Without Border
        /// Brace
        /// CurlyBrace.
        /// </summary>
        [Parameter]
        public BorderType Type { get; set; } = BorderType.Auto;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Parent = (ChartCategories)Tracker;
            Parent.Categories.Add(this);
            if (Parent.Axis.Renderer != null)
            {
                Parent.Chart.DelayLayoutChange();
            }
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            if (isPropertyChanged)
            {
                isPropertyChanged = false;
                Parent.Chart.AxisContainer.UpdateAxisRendering();
            }
        }

        internal override void ComponentDispose()
        {
            Parent.Categories?.Remove(this);
            if (Parent.Axis?.Renderer != null)
            {
                Parent.Chart.DelayLayoutChange();
            }

            Parent = null;
            ChildContent = null;
        }
    }
}