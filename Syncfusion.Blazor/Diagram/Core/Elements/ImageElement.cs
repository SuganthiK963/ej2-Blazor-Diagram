using Syncfusion.Blazor.Diagram.Internal;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Diagram
{
    /// <summary>
    /// Represents the class that defines a basic image elements.
    /// </summary>
    public class ImageElement : DiagramElement
    {

        private string imageSource;
        /// <summary>
        /// Creates a new instance of the <see cref="ImageElement"/> from the given ImageElement.
        /// </summary>
        /// <param name="src">basic image element.</param>
        public ImageElement(ImageElement src) : base(src)
        {
            if (src != null)
            {
                imageSource = src.imageSource;
                Stretch = src.Stretch;
                ImageScale = src.ImageScale;
                ImageAlign = src.ImageAlign;
                ImageSize = src.ImageSize;
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageElement"/> class.
        /// </summary>
        public ImageElement() : base()
        {
        }

        /// <summary>
        /// Gets or sets the source of the image element.
        /// </summary>
        [JsonPropertyName("source")]
        public string Source
        {
            get
            {
                return this.imageSource;
            }
            set
            {
                if (this.imageSource != value)
                {
                    this.imageSource = value;
                    this.IsDirt = true;
                }
            }
        }
        /// <summary>
        /// Gets or sets how the image will be scaled to fit within the bounds specified by the image scale property
        /// </summary>
        public Scale ImageScale { get; set; } = Scale.None;
        /// <summary>
        /// Gets or sets the alignment of the image within the bounds specified.
        /// </summary>
        public ImageAlignment ImageAlign { get; set; } = ImageAlignment.None;
        /// <summary>
        /// Gets or sets the image element, which determines how the content fits into the available space
        /// </summary>
        internal Stretch Stretch { get; set; } = Stretch.Stretch;
        /// <summary>
        /// Gets or sets the actual size of the image element to be rendered.
        /// </summary>
        public DiagramSize ImageSize { get; set; }

        /// <summary>
        /// Measures minimum space that is required to render the image
        /// </summary>
        /// <param name="availableSize"></param>
        /// <returns></returns>
        internal override DiagramSize Measure(DiagramSize availableSize)
        {
            if (this.IsDirt && (this.Stretch != Stretch.Stretch || this.Width == null && this.Height == null))
            {
                this.ImageSize = DomUtil.MeasureImage(this.ID);
                this.IsDirt = false;
            }
            if (this.Width != null || this.Height != null)
            {
                this.DesiredSize = new DiagramSize() { Width = this.Width, Height = this.Height };
                this.ImageSize = this.DesiredSize;
            }
            else
            {
                this.DesiredSize = this.ImageSize;
            }
            this.DesiredSize = this.ValidateDesiredSize(this.DesiredSize, availableSize);
            return this.DesiredSize;
        }

        /// <summary>
        /// Arranges the image element
        /// </summary>
        /// <param name="desiredSize"></param>
        /// <param name="isStack"></param>
        internal override DiagramSize Arrange(DiagramSize desiredSize, bool? isStack)
        {
            this.ActualSize = desiredSize;
            this.UpdateBounds();
            this.IsDirt = false;
            return this.ActualSize;
        }
        /// <summary>
        /// Creates a new image element that is a copy of the current image element
        /// </summary>
        public override object Clone()
        {
            return new ImageElement(this);
        }

        internal override void Dispose()
        {
            base.Dispose();
            imageSource = null;
            if (ImageSize != null)
            {
                ImageSize.Dispose();
                ImageSize = null;
            }
        }

    }
}
