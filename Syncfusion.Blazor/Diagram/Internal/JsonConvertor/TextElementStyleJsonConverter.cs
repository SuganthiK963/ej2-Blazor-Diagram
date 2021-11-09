using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Diagram.Internal
{
    internal class TextElementStyleJsonConverter : JsonConverter<TextElementStyle>
    {
        public override bool CanConvert(Type typeToConvert) =>
            typeof(TextElementStyle).IsAssignableFrom(typeToConvert);

        public override TextElementStyle Read(
            ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }
            TextElementStyle style = new TextElementStyle();
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return style;
                }
            }
            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, TextElementStyle value, JsonSerializerOptions options)
        {
            if (writer != null && value != null)
            {
                writer.WriteStartObject();
                if (value.Bold)
                    writer.WriteBoolean("bold", value.Bold);
                if (value.Italic)
                    writer.WriteBoolean("italic", value.Italic);
                if (value.FontSize != 12.0)
                    writer.WriteString("fontSize", value.FontSize.ToString(CultureInfo.InvariantCulture));
                if (value.TextAlign != TextAlign.Center)
                    writer.WriteString("textAlign", value.TextAlign.ToString());
                if (value.TextDecoration != TextDecoration.None)
                    writer.WriteString("textDecoration", value.TextDecoration.ToString());
                if (value.TextOverflow != TextOverflow.Wrap)
                    writer.WriteString("textOverflow", value.TextOverflow.ToString());
                if (value.TextWrapping != TextWrap.WrapWithOverflow)
                    writer.WriteString("textWrapping", value.TextWrapping.ToString());
                if (value.WhiteSpace != WhiteSpace.CollapseSpace)
                    writer.WriteString("whiteSpace", value.WhiteSpace.ToString());
                writer.WriteEndObject();
            }
        }
    }

}
