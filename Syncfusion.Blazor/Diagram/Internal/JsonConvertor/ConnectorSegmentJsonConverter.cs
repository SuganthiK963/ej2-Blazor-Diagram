using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Diagram.Internal
{
    /// <summary>
    /// Represents the serialization and deserialization of the ConnectorSegment. 
    /// </summary>
    public class ConnectorSegmentJsonConverter : JsonConverter<ConnectorSegment>
    {
        /// <summary>
        /// Determines if the specified type can be converted. 
        /// </summary>
        /// <param name="typeToConvert">Type</param>
        /// <returns>IsAssignableFrom</returns>
        public override bool CanConvert(Type typeToConvert) =>
            typeof(ConnectorSegment).IsAssignableFrom(typeToConvert);
        /// <summary>
        /// Reads and converts the JSON to type T. 
        /// </summary>
        /// <param name="reader">Utf8JsonReader</param>
        /// <param name="typeToConvert">Type</param>
        /// <param name="options">JsonSerializerOptions</param>
        /// <returns>ConnectorSegment</returns>
        public override ConnectorSegment Read(
            ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }
            ConnectorSegment segment = new ConnectorSegment();
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return segment;
                }
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    string propertyName = reader.GetString();
                    reader.Read();
                    if (propertyName == "type")
                    {
                        ConnectorSegmentType type = (ConnectorSegmentType)Enum.Parse(typeof(ConnectorSegmentType), reader.GetString());
                        segment = type switch
                        {
                            ConnectorSegmentType.Bezier => new BezierSegment(),
                            ConnectorSegmentType.Straight => new StraightSegment(),
                            ConnectorSegmentType.Orthogonal => new OrthogonalSegment(),
                            ConnectorSegmentType.Polyline => throw new NotImplementedException(),
                            _ => throw new NotImplementedException()
                        };
                    }
                    switch (propertyName)
                    {
                        case "type":
                            ConnectorSegmentType segments = (ConnectorSegmentType)Enum.Parse(typeof(ConnectorSegmentType), reader.GetString());
                            segment.Type = segments;
                            break;
                        case "allowDrag":
                            bool allowdrag = reader.GetBoolean();
                            segment.AllowDrag = allowdrag;
                            break;
                        case "point1":
                            DiagramPoint pt1 = JsonSerializer.Deserialize<DiagramPoint>(reader.GetString());
                            ((BezierSegment)segment).Point1 = pt1;
                            break;
                        case "point2":
                            DiagramPoint pt2 = JsonSerializer.Deserialize<DiagramPoint>(reader.GetString());
                            ((BezierSegment)segment).Point2 = pt2;
                            break;
                        case "vector1":
                            Vector vector1 = JsonSerializer.Deserialize<Vector>(reader.GetString());
                            ((BezierSegment)segment).Vector1 = vector1;
                            break;
                        case "vector2":
                            Vector vector2 = JsonSerializer.Deserialize<Vector>(reader.GetString());
                            ((BezierSegment)segment).Vector2 = vector2;
                            break;
                        case "length":
                            double length; _ = double.TryParse(reader.GetString(), out length);
                            ((OrthogonalSegment)segment).Length = (double?)length;
                            break;
                        case "direction":
                            Direction? direction = (Direction?)Enum.Parse(typeof(Direction), reader.GetString());
                            ((OrthogonalSegment)segment).Direction = direction;
                            break;
                        case "point":
                            DiagramPoint pt = JsonSerializer.Deserialize<DiagramPoint>(reader.GetString());
                            ((StraightSegment)segment).Point = pt;
                            break;
                    }
                }
            }
            throw new JsonException();
        }
        /// <summary>
        /// Writes a specified value as JSON. 
        /// </summary>
        /// <param name="writer">Utf8JsonWriter</param>
        /// <param name="value">ConnectorSegment</param>
        /// <param name="options">JsonSerializerOptions</param>
        public override void Write(Utf8JsonWriter writer, ConnectorSegment value, JsonSerializerOptions options)
        {
            if (writer != null && value != null)
            {
                writer.WriteStartObject();
                writer.WriteString("type", value.Type.ToString());
                writer.WriteBoolean("allowDrag", value.AllowDrag);
                if (value is BezierSegment bezier)
                {
                    writer.WriteString("point1", JsonSerializer.Serialize(bezier.Point1));
                    writer.WriteString("point2", JsonSerializer.Serialize(bezier.Point2));
                    writer.WriteString("vector1", JsonSerializer.Serialize(bezier.Vector1));
                    writer.WriteString("vector2", JsonSerializer.Serialize(bezier.Vector2));
                }
                if (value is OrthogonalSegment orthogonal)
                {
                    if (orthogonal.Length != null)
                        writer.WriteString("length", orthogonal.Length.ToString());
                    if (orthogonal.Direction != null)
                        writer.WriteString("direction", orthogonal.Direction.ToString());
                }
                if (value is StraightSegment straight)
                {
                    writer.WriteString("point", JsonSerializer.Serialize(straight.Point));
                }
                writer.WriteEndObject();
            }
        }
    }
}
