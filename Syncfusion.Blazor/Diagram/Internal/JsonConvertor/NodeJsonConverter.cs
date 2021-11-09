using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Syncfusion.Blazor.Diagram.Internal
{
    /// <summary>
    /// Represents the serialization and deserialization of the Node. 
    /// </summary>
    public class NodeJsonConverter : JsonConverter<Node>
    {
        /// <summary>
        /// Determines if the specified type can be converted. 
        /// </summary>
        /// <param name="typeToConvert">Type</param>
        /// <returns>IsAssignableFrom</returns>
        public override bool CanConvert(Type typeToConvert) =>
            typeof(Node).IsAssignableFrom(typeToConvert);
        /// <summary>
        /// Reads and converts the JSON to type T. 
        /// </summary>
        /// <param name="reader">Utf8JsonReader</param>
        /// <param name="typeToConvert">Type</param>
        /// <param name="options">JsonSerializerOptions</param>
        /// <returns>Node</returns>
        public override Node Read(
            ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }
            Node node = new Node();
            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndObject)
                {
                    return node;
                }
                if (reader.TokenType == JsonTokenType.PropertyName)
                {
                    string propertyName = reader.GetString();
                    reader.Read();
                    switch (propertyName)
                    {
                        case "children":
                            node = new NodeGroup();
                            string[] children = JsonSerializer.Deserialize<string[]>(reader.GetString());
                            (node as NodeGroup).Children = children;
                            break;
                        case "borderWidth":
                            double borderWidth = reader.GetDouble();
                            node.BorderWidth = borderWidth;
                            break;
                        case "zIndex":
                            int zIndex = reader.GetInt32();
                            node.ZIndex = zIndex;
                            break;
                        case "offsetX":
                            double offsetX = reader.GetDouble();
                            node.OffsetX = offsetX;
                            break;
                        case "offsetY":
                            double offsetY = reader.GetDouble();
                            node.OffsetY = offsetY;
                            break;
                        case "width":
                            double? width = (double?)reader.GetDouble();
                            node.Width = width;
                            break;
                        case "height":
                            double? height = (double?)reader.GetDouble();
                            node.Height = height;
                            break;
                        case "maxWidth":
                            double? maxWidth = (double?)reader.GetDouble();
                            node.MaxWidth = maxWidth;
                            break;
                        case "maxHeight":
                            double? maxHeight = (double?)reader.GetDouble();
                            node.MaxHeight = maxHeight;
                            break;
                        case "minHeight":
                            double? minHeight = (double?)reader.GetDouble();
                            node.MinHeight = minHeight;
                            break;
                        case "minWidth":
                            double? minWidth = (double?)reader.GetDouble();
                            node.MinWidth = minWidth;
                            break;
                        case "rotateAngle":
                            double rotateAngle = reader.GetDouble();
                            node.RotationAngle = rotateAngle;
                            break;
                        case "additionalInfo":
                            Dictionary<string, object> addInfo = JsonSerializer.Deserialize<Dictionary<string, object>>(reader.GetString());
                            node.AdditionalInfo = addInfo;
                            break;
                        case "annotations":
                            DiagramObjectCollection<ShapeAnnotation> annotations = JsonSerializer.Deserialize<DiagramObjectCollection<ShapeAnnotation>>(reader.GetString());
                            node.Annotations = annotations;
                            break;
                        case "ports":
                            DiagramObjectCollection<PointPort> ports = JsonSerializer.Deserialize<DiagramObjectCollection<PointPort>>(reader.GetString());
                            node.Ports = ports;
                            break;
                        case "fixedUserHandles":
                            DiagramObjectCollection<NodeFixedUserHandle> fixedUserHandles = JsonSerializer.Deserialize<DiagramObjectCollection<NodeFixedUserHandle>>(reader.GetString());
                            node.FixedUserHandles = fixedUserHandles;
                            break;
                        case "inEdges":
                            List<string> inEdges = JsonSerializer.Deserialize<List<string>>(reader.GetString());
                            node.InEdges = inEdges;
                            break;
                        case "outEdges":
                            List<string> outEdges = JsonSerializer.Deserialize<List<string>>(reader.GetString());
                            node.OutEdges = outEdges;
                            break;
                        case "shadow":
                            Shadow shadow = JsonSerializer.Deserialize<Shadow>(reader.GetString());
                            node.Shadow = shadow;
                            break;
                        case "shape":
                            JsonSerializerOptions settings = new JsonSerializerOptions()
                            {
                                WriteIndented = true
                            };
                            settings.Converters.Add(new ShapeJsonConverter());
                            Shape shape = JsonSerializer.Deserialize<Shape>(reader.GetString(), settings);
                            node.Shape = shape;
                            break;
                        case "pivot":
                            DiagramPoint pivot = JsonSerializer.Deserialize<DiagramPoint>(reader.GetString());
                            node.Pivot = pivot;
                            break;
                        case "style":
                            ShapeStyle style = JsonSerializer.Deserialize<ShapeStyle>(reader.GetString());
                            node.Style = style;
                            break;
                        case "margin":
                            Margin margin = JsonSerializer.Deserialize<Margin>(reader.GetString());
                            node.Margin = margin;
                            break;
                        case "processId":
                            string processId = reader.GetString();
                            node.ProcessId = processId;
                            break;
                        case "id":
                            string id = reader.GetString();
                            node.ID = id;
                            break;
                        case "parentId":
                            string parentId = reader.GetString();
                            node.ParentId = parentId;
                            break;
                        case "borderColor":
                            string borderColor = reader.GetString();
                            node.BorderColor = borderColor;
                            break;
                        case "backgroundColor":
                            string backgroundColor = reader.GetString();
                            node.BackgroundColor = backgroundColor;
                            break;
                        case "verticalAlignment":
                            VerticalAlignment verticalAlignment = (VerticalAlignment)Enum.Parse(typeof(VerticalAlignment), reader.GetString());
                            node.VerticalAlignment = verticalAlignment;
                            break;
                        case "horizontalAlignment":
                            HorizontalAlignment horizontalAlignment = (HorizontalAlignment)Enum.Parse(typeof(HorizontalAlignment), reader.GetString());
                            node.HorizontalAlignment = horizontalAlignment;
                            break;
                        case "flip":
                            FlipDirection flip = (FlipDirection)Enum.Parse(typeof(FlipDirection), reader.GetString());
                            node.Flip = flip;
                            break;
                        case "constraints":
                            NodeConstraints constraints = (NodeConstraints)Enum.Parse(typeof(NodeConstraints), reader.GetString());
                            node.Constraints = constraints;
                            break;
                        case "data":
                            object data = JsonSerializer.Deserialize<object>(reader.GetString());
                            node.Data = data;
                            break;
                        case "isExpanded":
                            bool isExpanded = reader.GetBoolean();
                            node.IsExpanded = isExpanded;
                            break;
                        case "canShapeLayout":
                            bool excludeFromLayout = reader.GetBoolean();
                            node.CanAutoLayout = excludeFromLayout;
                            break;
                        case "isVisible":
                            bool visible = reader.GetBoolean();
                            node.IsVisible = visible;
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
        /// <param name="value">Node</param>
        /// <param name="options">JsonSerializerOptions></param>
        public override void Write(Utf8JsonWriter writer, Node value, JsonSerializerOptions options)
        {
            if (writer != null && value != null)
            {
                writer.WriteStartObject();

                if (value is NodeGroup)
                    writer.WriteString("children", JsonSerializer.Serialize((value as NodeGroup).Children));

                writer.WriteString("borderColor", value.BorderColor);
                writer.WriteString("id", value.ID);
                writer.WriteString("parentId", value.ParentId);
                writer.WriteString("backgroundColor", value.BackgroundColor);
                writer.WriteString("verticalAlignment", value.VerticalAlignment.ToString());
                writer.WriteString("horizontalAlignment", value.HorizontalAlignment.ToString());
                writer.WriteString("flip", value.Flip.ToString());
                if (value.Data != null)
                    writer.WriteString("data", JsonSerializer.Serialize(value.Data));
                writer.WriteString("constraints", value.Constraints.ToString());
                writer.WriteBoolean("isExpanded", value.IsExpanded);
                writer.WriteBoolean("canShapeLayout", value.CanAutoLayout);
                writer.WriteBoolean("isVisible", value.IsVisible);

                writer.WriteNumber("borderWidth", value.BorderWidth);
                writer.WriteNumber("zIndex", value.ZIndex);
                writer.WriteNumber("offsetX", value.OffsetX);
                writer.WriteNumber("offsetY", value.OffsetY);
                if (value.Width != null)
                    writer.WriteNumber("width", value.Width.Value);
                if (value.Height != null)
                    writer.WriteNumber("height", value.Height.Value);
                if (value.MaxWidth != null)
                    writer.WriteNumber("maxWidth", value.MaxWidth.Value);
                if (value.MaxHeight != null)
                    writer.WriteNumber("maxHeight", value.MaxHeight.Value);
                if (value.MinHeight != null)
                    writer.WriteNumber("minHeight", value.MinHeight.Value);
                if (value.MinWidth != null)
                    writer.WriteNumber("minWidth", value.MinWidth.Value);
                writer.WriteNumber("rotateAngle", value.RotationAngle);
                if (value.AdditionalInfo != null)
                    writer.WriteString("additionalInfo", JsonSerializer.Serialize(value.AdditionalInfo));
                if (value.Annotations != null)
                    writer.WriteString("annotations", JsonSerializer.Serialize(value.Annotations));
                if (value.Ports != null)
                    writer.WriteString("ports", JsonSerializer.Serialize(value.Ports));
                if (value.FixedUserHandles != null)
                    writer.WriteString("fixedUserHandles", JsonSerializer.Serialize(value.FixedUserHandles));
                if (value.InEdges != null)
                    writer.WriteString("inEdges", JsonSerializer.Serialize(value.InEdges));
                if (value.OutEdges != null)
                    writer.WriteString("outEdges", JsonSerializer.Serialize(value.OutEdges));
                if (value.Pivot != null)
                    writer.WriteString("pivot", JsonSerializer.Serialize(value.Pivot));
                if (!string.IsNullOrEmpty(value.ProcessId))
                    writer.WriteString("processId", value.ProcessId);
                if (value.Shadow != null)
                    writer.WriteString("shadow", JsonSerializer.Serialize(value.Shadow));
                if (value.Shape != null)
                {
                    JsonSerializerOptions settings = new JsonSerializerOptions()
                    {
                        WriteIndented = true
                    };
                    settings.Converters.Add(new ShapeJsonConverter());
                    writer.WriteString("shape", JsonSerializer.Serialize(value.Shape, settings));
                }
                if (value.Style != null)
                    writer.WriteString("style", JsonSerializer.Serialize(value.Style));
                if (value.Margin != null)
                    writer.WriteString("margin", JsonSerializer.Serialize(value.Margin));
                writer.WriteEndObject();
            }
        }
    }
}
