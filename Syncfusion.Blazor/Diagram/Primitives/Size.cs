using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Diagram
{
    /// <summary>
    /// Describes the size of an object.
    /// </summary>
    public class DiagramSize
    {
        /// <summary>
        /// Gets or sets the width of an object.
        /// </summary>
        [JsonPropertyName("width")]
        public double? Width { get; set; }
        /// <summary>
        /// Gets or sets the height of an object.
        /// </summary>
        [JsonPropertyName("height")]
        public double? Height { get; set; }
        internal DiagramSize Clone()
        {
            return new DiagramSize() { Width = this.Width, Height = this.Height };
        }

        internal void Dispose()
        {
            Width = null;
            Height = null;
        }
    }
}
