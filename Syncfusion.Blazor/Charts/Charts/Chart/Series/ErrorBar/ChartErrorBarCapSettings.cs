using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// Specifies the configuration of the error bar cap settings.
    /// </summary>
    public partial class ChartErrorBarCapSettings : ChartSubComponent
    {
        #region ERRORBAR PRIVATE FIELDS

        private string color;
        private double width = 1;
        private double length = 10;
        private double opacity = 1;

        #endregion

        [CascadingParameter]
        private ChartErrorBarSettings Parent { get; set; }

        #region ERRORBAR API

        /// <summary>
        ///  The stroke color of the cap, which accepts value in hex, rgba as a valid CSS color string.
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
                    if (Parent?.Renderer != null)
                    {
                        Parent.Renderer.UpdateCapCustomization(nameof(Color));
                    }
                }
            }
        }

        /// <summary>
        /// The length of the error bar in pixels.
        /// </summary>
        [Parameter]
        public double Length
        {
            get
            {
                return length;
            }

            set
            {
                if (length != value)
                {
                    length = value;
                    if (Parent?.Renderer != null)
                    {
                        Parent.Renderer.UpdateCapCustomization(nameof(Length));
                    }
                }
            }
        }

        /// <summary>
        /// The opacity of the cap.
        /// </summary>
        [Parameter]
        public double Opacity
        {
            get
            {
                return opacity;
            }

            set
            {
                if (opacity != value)
                {
                    opacity = value;
                    if (Parent?.Renderer != null)
                    {
                        Parent.Renderer.UpdateCapCustomization(nameof(Opacity));
                    }
                }
            }
        }

        /// <summary>
        /// The width of the error bar in pixels.
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
                    if (Parent?.Renderer != null)
                    {
                        Parent.Renderer.UpdateCapCustomization(nameof(Width));
                    }
                }
            }
        }
        #endregion

        protected override void OnInitialized()
        {
            base.OnInitialized();
            Parent = (ChartErrorBarSettings)Tracker;
            Parent.UpdateErrorBarProperty("ErrorBarCap", this);
        }

        internal override void ComponentDispose()
        {
            Parent = null;
        }
    }
}