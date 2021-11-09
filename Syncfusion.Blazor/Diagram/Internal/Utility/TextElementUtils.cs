using System.Collections.ObjectModel;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Diagram.Internal
{
    internal class TextElementUtils
    {
        public string Content { get; set; }
        public TextStyle Style { get; set; }
        public DiagramSize Bounds { get; set; }
        public DiagramSize NodeSize { get; set; }
        public TextBounds WrapBounds { get; set; }
        public ObservableCollection<SubTextElement> ChildNodes { get; set; }

        internal void Dispose()
        {
            Content = null;
            if (Style != null)
            {
                Style.Dispose();
                Style = null;
            }
            if (Bounds != null)
            {
                Bounds.Dispose();
                Bounds = null;
            }
            if (NodeSize != null)
            {
                NodeSize.Dispose();
                NodeSize = null;
            }
            if (WrapBounds != null)
            {
                //WrapBounds.Dispose();
                WrapBounds = null;
            }
            if (ChildNodes != null)
            {
                for (int i = 0; i < ChildNodes.Count; i++)
                {
                    //ChildNodes[i].Dispose();
                    ChildNodes[i] = null;
                }
                ChildNodes.Clear();
                ChildNodes = null;
            }
        }

    }

    internal class TextElementBounds
    {
        internal string X { get; set; }
        internal string Y { get; set; }
        internal string Text { get; set; }

    }
    internal class TextElementUtilsSerialize
    {
        public string Content { get; set; }
        public object Style { get; set; }
        public DiagramSize Bounds { get; set; }
        public DiagramSize NodeSize { get; set; }

        internal void Dispose()
        {
            if (Content != null)
            {
                Content = null;
            }
            if (Style != null)
            {
                Style = null;
            }
            if (Bounds != null)
            {
                Bounds.Dispose();
                Bounds = null;
            }
            if (NodeSize != null)
            {
                NodeSize.Dispose();
                NodeSize = null;
            }
        }
    }

    [JsonConverter(typeof(TextElementStyleJsonConverter))]
    internal class TextElementStyle
    {
        [JsonPropertyName("bold")]
        public bool Bold { get; set; }
        [JsonPropertyName("fontFamily")]
        public string FontFamily { get; set; } = "Arial";
        [JsonPropertyName("fontSize")]
        public double FontSize { get; set; } = 12.0;
        [JsonPropertyName("italic")]
        public bool Italic { get; set; }
        [JsonPropertyName("textAlign")]
        public TextAlign TextAlign { get; set; } = TextAlign.Center;
        [JsonPropertyName("textDecoration")]
        public TextDecoration TextDecoration { get; set; } = TextDecoration.None;
        [JsonPropertyName("textOverflow")]
        public TextOverflow TextOverflow { get; set; } = TextOverflow.Wrap;
        [JsonPropertyName("textWrapping")]
        public TextWrap TextWrapping { get; set; } = TextWrap.WrapWithOverflow;
        [JsonPropertyName("whiteSpace")]
        public WhiteSpace WhiteSpace { get; set; } = WhiteSpace.CollapseSpace;
    }
}
