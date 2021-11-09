using System;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Diagram.Internal
{
    /// <summary>
    /// Represents the serialization and deserialization of the shape. 
    /// </summary>
    public class ShapeJsonConverter : JsonConverter<Shape>
    {
        /// <summary>
        /// Determines if the specified type can be converted. 
        /// </summary>
        /// <param name="typeToConvert">Type</param>
        /// <returns>IsAssignableFrom</returns>
        public override bool CanConvert(Type typeToConvert) =>
            typeof(Shape).IsAssignableFrom(typeToConvert);
        /// <summary>
        /// Reads and converts the JSON to type T. 
        /// </summary>
        /// <param name="reader">Utf8JsonReader</param>
        /// <param name="typeToConvert">Type</param>
        /// <param name="options">JsonSerializerOptions</param>
        /// <returns>Shape</returns>
        public override Shape Read(
            ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }
            Shape shape = new Shape();
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return shape;
                }
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    string propertyName = reader.GetString();
                    reader.Read();
                    if (propertyName == "type")
                    {
                        Shapes type = (Shapes)Enum.Parse(typeof(Shapes), reader.GetString());
                        shape = type switch
                        {
                            Shapes.Basic => new BasicShape(),
                            Shapes.Path => new PathShape(),
                            Shapes.Image => new ImageShape(),
                            Shapes.Flow => new FlowShape(),
                            Shapes.Bpmn => new BpmnShape(),
                            Shapes.HTML => new Shape(),
                            Shapes.SVG => new Shape(),
                            _ => throw new NotImplementedException()
                        };
                    }
                    switch (propertyName)
                    {
                        case "shape":
                            if (shape is BasicShape shape1)
                            {
                                BasicShapeType basicShape = (BasicShapeType)Enum.Parse(typeof(BasicShapeType), reader.GetString());
                                shape1.Shape = basicShape;
                            }
                            else if (shape
                                is FlowShape flowShape)
                            {
                                FlowShapeType shapes = (FlowShapeType)Enum.Parse(typeof(FlowShapeType), reader.GetInt32().ToString(CultureInfo.InvariantCulture));
                                flowShape.Shape = shapes;
                            }
                            else
                            {
                                BpmnShapes bpmnShape = (BpmnShapes)Enum.Parse(typeof(BpmnShapes), reader.GetInt32().ToString(CultureInfo.InvariantCulture));
                                ((BpmnShape)shape).Shape = bpmnShape;
                            }
                            break;
                        case "cornerRadius":
                            double cRadius = reader.GetDouble();
                            ((BasicShape)shape).CornerRadius = cRadius;
                            break;
                        case "points":
                            DiagramPoint[] pts = JsonSerializer.Deserialize<DiagramPoint[]>(reader.GetString());
                            ((BasicShape)shape).Points = pts;
                            break;
                        case "polygonPath":
                            string polygonPath = reader.GetString();
                            ((BasicShape)shape).PolygonPath = polygonPath;
                            break;
                        case "type":
                            Shapes shpaes = (Shapes)Enum.Parse(typeof(Shapes), reader.GetString());
                            shape.Type = shpaes;
                            break;
                        case "data":
                            string data = reader.GetString();
                            ((PathShape)shape).Data = data;
                            break;
                        case "source":
                            string source = reader.GetString();
                            ((ImageShape)shape).Source = source;
                            break;
                        case "align":
                            ImageAlignment align = (ImageAlignment)Enum.Parse(typeof(ImageAlignment), reader.GetInt32().ToString(CultureInfo.InvariantCulture));
                            ((ImageShape)shape).ImageAlign = align;
                            break;
                        case "scale":
                            Scale scale = JsonSerializer.Deserialize<Scale>(reader.GetString());
                            ((ImageShape)shape).Scale = scale;
                            break;
                        case "annotation":
                            BpmnAnnotation bpmnShapeAnnotation = JsonSerializer.Deserialize<BpmnAnnotation>(reader.GetString());
                            ((BpmnShape)shape).Annotation = bpmnShapeAnnotation;
                            break;
                        case "annotations":
                            DiagramObjectCollection<BpmnAnnotation> bpmnShapeAnnotations = JsonSerializer.Deserialize<DiagramObjectCollection<BpmnAnnotation>>(reader.GetString());
                            ((BpmnShape)shape).Annotations = bpmnShapeAnnotations;
                            break;
                        case "events":
                            BpmnSubEvent bpmnShapeEvents = JsonSerializer.Deserialize<BpmnSubEvent>(reader.GetString());
                            ((BpmnShape)shape).Events = bpmnShapeEvents;
                            break;
                        case "gateway":
                            BpmnGateway bpmnShapeGateway = JsonSerializer.Deserialize<BpmnGateway>(reader.GetString());
                            ((BpmnShape)shape).Gateway = bpmnShapeGateway;
                            break;
                        case "activity":
                            BpmnActivity bpmnShapeActivity = JsonSerializer.Deserialize<BpmnActivity>(reader.GetString());
                            ((BpmnShape)shape).Activity = bpmnShapeActivity;
                            break;
                        case "dataObject":
                            BpmnDataObject dataObject = JsonSerializer.Deserialize<BpmnDataObject>(reader.GetString());
                            ((BpmnShape)shape).DataObject = dataObject;
                            break;
                        case "annotationId":
                            int annotationId = reader.GetInt32();
                            ((BpmnShape)shape).annotationId = annotationId;
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
        /// <param name="value">Shape</param>
        /// <param name="options">JsonSerializerOptions</param>
        public override void Write(Utf8JsonWriter writer, Shape value, JsonSerializerOptions options)
        {
            if (writer != null && value != null)
            {
                writer.WriteStartObject();
                writer.WriteString("type", value.Type.ToString());
                if (value is BasicShape basicShape)
                {
                    writer.WriteString("shape", basicShape.Shape.ToString());
                    writer.WriteString("points", JsonSerializer.Serialize<DiagramPoint[]>(basicShape.Points));
                    writer.WriteNumber("cornerRadius", basicShape.CornerRadius);
                }
                else if (value is PathShape pathShape)
                {
                    writer.WriteString("data", pathShape.Data.ToString());
                }
                else if (value is ImageShape imageShape)
                {
                    writer.WriteString("source", imageShape.Source.ToString());
                    writer.WriteNumber("align", (int)imageShape.ImageAlign);
                    writer.WriteString("scale", JsonSerializer.Serialize<Scale>(imageShape.Scale));
                }
                else if (value is FlowShape flowShape)
                {
                    writer.WriteNumber("shape", (int)flowShape.Shape);
                }
                else if (value is BpmnShape bpmnShapes)
                {
                    writer.WriteNumber("shape", (int)bpmnShapes.Shape);
                    writer.WriteString("annotation", JsonSerializer.Serialize<BpmnAnnotation>(bpmnShapes.Annotation));
                    writer.WriteString("annotations", JsonSerializer.Serialize<DiagramObjectCollection<BpmnAnnotation>>(bpmnShapes.Annotations));
                    if (bpmnShapes.Shape == BpmnShapes.Event)
                        writer.WriteString("events", JsonSerializer.Serialize(bpmnShapes.Events));
                    else if (bpmnShapes.Shape == BpmnShapes.Gateway)
                        writer.WriteString("gateway", JsonSerializer.Serialize(bpmnShapes.Gateway));
                    else if (bpmnShapes.Shape == BpmnShapes.Activity)
                        writer.WriteString("activity", JsonSerializer.Serialize(bpmnShapes.Activity));
                    else if (bpmnShapes.Shape == BpmnShapes.DataObject)
                        writer.WriteString("dataObject", JsonSerializer.Serialize(bpmnShapes.DataObject));
                    writer.WriteNumber("annotationId", (int)bpmnShapes.annotationId);
                }
                writer.WriteEndObject();
            }
        }
    }
}
