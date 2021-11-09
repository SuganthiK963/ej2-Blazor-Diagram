using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    public class ChartAxisLabelBorder : ChartSubComponent
    {
        #region BACKING FIELDS
        private string color = string.Empty;
        private BorderType type;
        private double width;
        private bool isPropertyChanged;
        #endregion

        [CascadingParameter]
        private ChartAxis Axis { get; set; }

        /// <summary>
        /// The color of the border that accepts value in hex and rgba as a valid CSS color string.
        /// </summary>
        [Parameter]
        public string Color
        {
            get
            {
                return color;
            }

            set
            {
                if (color != value)
                {
                    color = value;
                    isPropertyChanged = Axis != null;
                }
            }
        }

        /// <summary>
        /// Border type for labels
        /// Rectangle
        /// Without Top Border
        /// Without Top and BottomBorder
        /// Without Border
        /// Brace
        /// CurlyBrace.
        /// </summary>
        [Parameter]
        public BorderType Type
        {
            get
            {
                return type;
            }

            set
            {
                if (type != value)
                {
                    type = value;
                    isPropertyChanged = Axis != null;
                }
            }
        }

        /// <summary>
        /// Specifies the border width of axis labels.
        /// </summary>
        [Parameter]
        public double Width
        {
            get
            {
                return width;
            }

            set
            {
                if (width != value)
                {
                    width = value;
                    isPropertyChanged = Axis != null;
                }
            }
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Axis = (ChartAxis)Tracker;
            Axis.UpdateAxisProperties("Border", this);
        }
    }
}
