using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Diagram.Internal
{
    /// <summary>
    ///  Represents the serialization and deserialization of the Gradient. 
    /// </summary>
    public class GradientJsonConverter : JsonConverter<GradientBrush>
    {
        /// <summary>
        /// Determines if the specified type can be converted. 
        /// </summary>
        /// <param name="typeToConvert">Type</param>
        /// <returns>IsAssignableFrom</returns>
        public override bool CanConvert(Type typeToConvert) =>
            typeof(GradientBrush).IsAssignableFrom(typeToConvert);
        /// <summary>
        /// Reads and converts the JSON to type Gradient.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        /// <returns>The converted value.</returns>
        public override GradientBrush Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }
            GradientBrush gradient = null;
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return gradient;
                }
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    string propertyName = reader.GetString();
                    reader.Read();
                    if (propertyName == "type")
                    {
                        GradientType type = (GradientType)Enum.Parse(typeof(GradientType), reader.GetString());
                        gradient = type switch
                        {
                            GradientType.Linear => new LinearGradientBrush(),
                            GradientType.Radial => new RadialGradientBrush(),
                            _ => throw new NotImplementedException(),
                        };
                    }
                    switch (propertyName)
                    {
                        case "type":
                            GradientType type = (GradientType)Enum.Parse(typeof(GradientType), reader.GetString());
                            gradient.BrushType = type;
                            break;
                        case "stops":
                            DiagramObjectCollection<GradientStop> stops = JsonSerializer.Deserialize<DiagramObjectCollection<GradientStop>>(reader.GetString());
                            gradient.GradientStops = stops;
                            break;
                        case "x1":
                            double x1 = reader.GetDouble();
                            ((LinearGradientBrush)gradient).X1 = x1;
                            break;
                        case "y1":
                            double y1 = reader.GetDouble();
                            ((LinearGradientBrush)gradient).Y1 = y1;
                            break;
                        case "x2":
                            double x2 = reader.GetDouble();
                            ((LinearGradientBrush)gradient).X2 = x2;
                            break;
                        case "y2":
                            double y2 = reader.GetDouble();
                            ((LinearGradientBrush)gradient).Y2 = y2;
                            break;
                        case "cx":
                            double cx = reader.GetDouble();
                            ((RadialGradientBrush)gradient).CX = cx;
                            break;
                        case "cy":
                            double cy = reader.GetDouble();
                            ((RadialGradientBrush)gradient).CY = cy;
                            break;
                        case "fx":
                            double fx = reader.GetDouble();
                            ((RadialGradientBrush)gradient).FX = fx;
                            break;
                        case "fy":
                            double fy = reader.GetDouble();
                            ((RadialGradientBrush)gradient).FY = fy;
                            break;
                        case "r":
                            double r = reader.GetDouble();
                            ((RadialGradientBrush)gradient).R = r;
                            break;
                    }
                }
            }
            throw new JsonException();
        }
        /// <summary>
        /// Writes a specified value as JSON.
        /// </summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="value">The value to convert to JSON.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        public override void Write(Utf8JsonWriter writer, GradientBrush value, JsonSerializerOptions options)
        {
            if (writer != null && value != null)
            {
                writer.WriteStartObject();
                writer.WriteString("type", value.BrushType.ToString());
                writer.WriteString("stops", JsonSerializer.Serialize(value.GradientStops));
                if (value is LinearGradientBrush linear)
                {
                    writer.WriteNumber("x1", linear.X1);
                    writer.WriteNumber("y1", linear.Y1);
                    writer.WriteNumber("x2", linear.X2);
                    writer.WriteNumber("y2", linear.Y2);
                }
                else if (value is RadialGradientBrush radial)
                {
                    writer.WriteNumber("cx", radial.CX);
                    writer.WriteNumber("cy", radial.CY);
                    writer.WriteNumber("fx", radial.FX);
                    writer.WriteNumber("fy", radial.FY);
                    writer.WriteNumber("r", radial.R);
                }
                writer.WriteEndObject();
            }
        }
    }
}
