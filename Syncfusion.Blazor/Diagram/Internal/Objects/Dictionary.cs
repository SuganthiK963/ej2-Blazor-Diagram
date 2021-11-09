using System.Collections.Generic;
using System.Linq;

namespace Syncfusion.Blazor.Diagram.Internal
{
    internal static class Dictionary
    {
        static internal Dictionary<string, ShapeInformation> DefaultShapes;

        /// <summary>
        /// Defines the BpmnShapes.
        /// </summary>
        static internal Dictionary<string, ShapeInformation> BpmnShapes;

        /// <summary>
        /// Defines the BpmnTriggerShapes.
        /// </summary>
        static internal Dictionary<string, ShapeInformation> BpmnTriggerShapes;
        /// <summary>
        /// Defines the BpmnGatewayShapes.
        /// </summary>
        static internal Dictionary<string, ShapeInformation> BpmnGatewayShapes;
        /// <summary>
        /// Defines the BpmnLoopShapes.
        /// </summary>
        static internal Dictionary<string, ShapeInformation> BpmnLoopShapes;
        /// <summary>
        /// Defines the BpmnTaskShapes.
        /// </summary>
        static internal Dictionary<string, ShapeInformation> BpmnTaskShapes;
        static internal Dictionary<Actions, string> DefaultCursor;
        static internal Dictionary<string, DiagramRect> MeasureCustomBounds { get; set; } = new Dictionary<string, DiagramRect>();
        static internal Dictionary<string, List<DiagramPoint>> MeasureCustomPathPoints { get; set; } = new Dictionary<string, List<DiagramPoint>>();

        static internal Dictionary<string, TextElementUtils> MeasureTextBounds { get; set; } = new Dictionary<string, TextElementUtils>();
        static internal Dictionary<string, DiagramSize> MeasureImageBounds { get; set; } = new Dictionary<string, DiagramSize>();
        static internal Dictionary<string, DiagramSize> MeasureNativeELementBounds { get; set; } = new Dictionary<string, DiagramSize>();
        internal static string GetShapeData(string shape)
        {
            if (shape != "None")
            {
                return DefaultShapes[shape.ToString()].Path;
            }
            return string.Empty;
        }
        /// <summary>
        /// The GetBpmnTriggerShapePathData.
        /// </summary>
        /// <param name="shape">The shape<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        internal static string GetBpmnTriggerShapePathData(string shape)
        {
            if (shape != "None")
            {
                return BpmnTriggerShapes[shape.ToString()].Path;
            }
            return string.Empty;
        }

        /// <summary>
        /// The GetBpmnGatewayShapePathData.
        /// </summary>
        /// <param name="shape">The shape<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        internal static string GetBpmnGatewayShapePathData(string shape)
        {
            if (shape != "None")
            {
                return BpmnGatewayShapes[shape.ToString()].Path;
            }
            return string.Empty;
        }

        /// <summary>
        /// The GetBpmnTaskShapePathData.
        /// </summary>
        /// <param name="shape">The shape<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        internal static string GetBpmnTaskShapePathData(string shape)
        {
            if (shape != "None")
            {
                return BpmnTaskShapes[shape.ToString()].Path;
            }
            return string.Empty;
        }

        /// <summary>
        /// The GetBpmnLoopShapePathData.
        /// </summary>
        /// <param name="shape">The shape<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        internal static string GetBpmnLoopShapePathData(string shape)
        {
            if (shape != "None")
            {
                return BpmnLoopShapes[shape.ToString()].Path;
            }
            return string.Empty;
        }

        /// <summary>
        /// The GetBpmnShapePathData.
        /// </summary>
        /// <param name="shape">The shape<see cref="string"/>.</param>
        /// <returns>The <see cref="string"/>.</returns>
        internal static string GetBpmnShapePathData(string shape)
        {
            if (shape != "None")
            {
                return BpmnShapes[shape.ToString()].Path;
            }
            return string.Empty;
        }

        /// <summary>
        /// The GetPathPointsCollection.
        /// </summary>
        /// <param name="data">The data<see cref="string"/>.</param>
        /// <returns>The <see cref="List{Point}"/>.</returns>
        internal static List<DiagramPoint> GetPathPointsCollection(string data)
        {
            KeyValuePair<string, ShapeInformation>? defaultShapeData = DefaultShapes.FirstOrDefault(x => x.Value.Path == data);
            KeyValuePair<string, ShapeInformation>? pathShapeData = BpmnShapes.FirstOrDefault(x => x.Value.Path == data);
            KeyValuePair<string, ShapeInformation>? bpmnTriggerShapeData = BpmnTriggerShapes.FirstOrDefault(x => x.Value.Path == data);
            KeyValuePair<string, ShapeInformation>? bpmnGatewayShapeData = BpmnGatewayShapes.FirstOrDefault(x => x.Value.Path == data);
            KeyValuePair<string, ShapeInformation>? bpmnTaskShapeData = BpmnTaskShapes.FirstOrDefault(x => x.Value.Path == data);
            KeyValuePair<string, ShapeInformation>? bpmnShapeData = BpmnShapes.FirstOrDefault(x => x.Value.Path == data);
            KeyValuePair<string, ShapeInformation>? bpmnLoopData = BpmnLoopShapes.FirstOrDefault(x => x.Value.Path == data);
            KeyValuePair<string, List<DiagramPoint>>? customData = MeasureCustomPathPoints.FirstOrDefault(x => x.Key == data);
            if (defaultShapeData.HasValue && defaultShapeData.Value.Value != null)
            {
                return defaultShapeData.Value.Value.Points;
            }
            else if (customData.HasValue && customData.Value.Value != null)
            {
                return customData.Value.Value;
            }
            else if (pathShapeData.HasValue && pathShapeData.Value.Value != null)
            {
                return pathShapeData.Value.Value.Points;
            }
            else if (bpmnTriggerShapeData.HasValue && bpmnTriggerShapeData.Value.Value != null)
            {
                return bpmnTriggerShapeData.Value.Value.Points;
            }
            else if (bpmnGatewayShapeData.HasValue && bpmnGatewayShapeData.Value.Value != null)
            {
                return bpmnGatewayShapeData.Value.Value.Points;
            }
            else if (bpmnTaskShapeData.HasValue && bpmnTaskShapeData.Value.Value != null)
            {
                return bpmnTaskShapeData.Value.Value.Points;
            }
            else if (bpmnShapeData.HasValue && bpmnShapeData.Value.Value != null)
            {
                return bpmnShapeData.Value.Value.Points;
            }
            else if (bpmnLoopData.HasValue && bpmnLoopData.Value.Value != null)
            {
                return bpmnLoopData.Value.Value.Points;
            }
            return null;
        }

        /// <summary>
        /// The GetPathPointsCollection.
        /// </summary>
        /// <param name="data">The data<see cref="string"/>.</param>
        /// <returns>The <see cref="List{Point}"/>.</returns>
        internal static List<DiagramPoint> GetCustomPathPointsCollection(string data)
        {
            KeyValuePair<string, List<DiagramPoint>>? customData = MeasureCustomPathPoints.FirstOrDefault(x => x.Key == data);
            if (customData.HasValue && customData.Value.Value != null)
            {
                return customData.Value.Value;
            }
            else
            {
                return null;
            }
        }

        internal static DiagramRect GetMeasurePathBounds(string data)
        {
            KeyValuePair<string, ShapeInformation>? defaultShapeData = DefaultShapes.FirstOrDefault(x => x.Value.Path == data);
            KeyValuePair<string, ShapeInformation>? bpmnTriggerShapeData = BpmnTriggerShapes.FirstOrDefault(x => x.Value.Path == data);
            KeyValuePair<string, ShapeInformation>? bpmnGatewayShapeData = BpmnGatewayShapes.FirstOrDefault(x => x.Value.Path == data);
            KeyValuePair<string, ShapeInformation>? bpmnTaskShapeData = BpmnTaskShapes.FirstOrDefault(x => x.Value.Path == data);
            KeyValuePair<string, ShapeInformation>? bpmnShapeData = BpmnShapes.FirstOrDefault(x => x.Value.Path == data);
            KeyValuePair<string, ShapeInformation>? bpmnLoopData = BpmnLoopShapes.FirstOrDefault(x => x.Value.Path == data);
            if (defaultShapeData.HasValue && defaultShapeData.Value.Value != null)
            {
                return defaultShapeData.Value.Value.Bounds;
            }
            else if (bpmnTriggerShapeData.HasValue && bpmnTriggerShapeData.Value.Value != null)
            {
                return bpmnTriggerShapeData.Value.Value.Bounds;
            }
            else if (bpmnGatewayShapeData.HasValue && bpmnGatewayShapeData.Value.Value != null)
            {
                return bpmnGatewayShapeData.Value.Value.Bounds;
            }
            else if (bpmnTaskShapeData.HasValue && bpmnTaskShapeData.Value.Value != null)
            {
                return bpmnTaskShapeData.Value.Value.Bounds;
            }
            else if (bpmnShapeData.HasValue && bpmnShapeData.Value.Value != null)
            {
                return bpmnShapeData.Value.Value.Bounds;
            }
            else if (bpmnLoopData.HasValue && bpmnLoopData.Value.Value != null)
            {
                return bpmnLoopData.Value.Value.Bounds;
            }
            else
            {
                KeyValuePair<string, DiagramRect>? customData = MeasureCustomBounds.FirstOrDefault(x => x.Key == data);
                return customData.HasValue ? customData.Value.Value : null;
            }
        }
        internal static TextElementUtils GetMeasureTextBounds(string data)
        {
            if (!MeasureTextBounds.ContainsKey(data))
                return null;
            return MeasureTextBounds[data];
        }
        internal static DiagramSize GetMeasureImageBounds(string data)
        {
            if (!MeasureImageBounds.ContainsKey(data))
                return null;
            else
            {
                return MeasureImageBounds[data];
            }
        }

        internal static string GetCursorValue(Actions actions)
        {
            if (DefaultCursor.ContainsKey(actions))
            {
                return DefaultCursor[actions];
            }
            return null;
        }

        internal static DiagramSize GetMeasureNativeElementBounds(string data)
        {
            if (!MeasureNativeELementBounds.ContainsKey(data))
                return null;
            else
            {
                return MeasureNativeELementBounds[data];
            }
        }

        internal static void Dispose()
        {
            if (DefaultShapes != null)
            {
                foreach (string index in DefaultShapes.Keys)
                {
                    ShapeInformation value = DefaultShapes[index];
                    value.Dispose();
                    value = null;
                }
                DefaultShapes.Clear();
                DefaultShapes = null;
            }
            if (BpmnShapes != null)
            {
                foreach (string index in BpmnShapes.Keys)
                {
                    ShapeInformation value = BpmnShapes[index];
                    value.Dispose();
                    value = null;
                }
                BpmnShapes.Clear();
                BpmnShapes = null;
            }
            if (BpmnTriggerShapes != null)
            {
                foreach (string index in BpmnTriggerShapes.Keys)
                {
                    ShapeInformation value = BpmnTriggerShapes[index];
                    value.Dispose();
                    value = null;
                }
                BpmnTriggerShapes.Clear();
                BpmnTriggerShapes = null;
            }
            if (BpmnGatewayShapes != null)
            {
                foreach (string index in BpmnGatewayShapes.Keys)
                {
                    ShapeInformation value = BpmnGatewayShapes[index];
                    value.Dispose();
                    value = null;
                }
                BpmnGatewayShapes.Clear();
                BpmnGatewayShapes = null;
            }
            if (BpmnLoopShapes != null)
            {
                foreach (string index in BpmnLoopShapes.Keys)
                {
                    ShapeInformation value = BpmnLoopShapes[index];
                    value.Dispose();
                    value = null;
                }
                BpmnLoopShapes.Clear();
                BpmnLoopShapes = null;
            }
            if (BpmnTaskShapes != null)
            {
                foreach (string index in BpmnTaskShapes.Keys)
                {
                    ShapeInformation value = BpmnTaskShapes[index];
                    value.Dispose();
                    value = null;
                }
                BpmnTaskShapes.Clear();
                BpmnTaskShapes = null;
            }
            if (DefaultCursor != null)
            {
                DefaultCursor.Clear();
                DefaultCursor = null;
            }
            if (MeasureCustomBounds != null)
            {
                foreach (string index in MeasureCustomBounds.Keys)
                {
                    DiagramRect value = MeasureCustomBounds[index];
                    value.Dispose();
                    value = null;
                }
                MeasureCustomBounds.Clear();
                MeasureCustomBounds = null;
            }
            if (MeasureCustomPathPoints != null)
            {
                foreach (string index in MeasureCustomPathPoints.Keys)
                {
                    List<DiagramPoint> value = MeasureCustomPathPoints[index];
                    for (int i = 0; i < value.Count; i++)
                    {
                        value[i].Dispose();
                        value[i] = null;
                    }
                    value.Clear();
                    value = null;
                }
                MeasureCustomPathPoints.Clear();
                MeasureCustomPathPoints = null;
            }
            if (MeasureTextBounds != null)
            {
                foreach (string index in MeasureTextBounds.Keys)
                {
                    TextElementUtils value = MeasureTextBounds[index];
                    value.Dispose();
                    value = null;
                }
                MeasureTextBounds.Clear();
                MeasureTextBounds = null;
            }
            if (MeasureImageBounds != null)
            {
                foreach (string index in MeasureImageBounds.Keys)
                {
                    DiagramSize value = MeasureImageBounds[index];
                    value.Dispose();
                    value = null;
                }
                MeasureImageBounds.Clear();
                MeasureImageBounds = null;
            }
            if (MeasureNativeELementBounds != null)
            {
                foreach (string index in MeasureNativeELementBounds.Keys)
                {
                    DiagramSize value = MeasureNativeELementBounds[index];
                    value.Dispose();
                    value = null;
                }
                MeasureNativeELementBounds.Clear();
                MeasureNativeELementBounds = null;
            }
        }
    }

    internal class ShapeInformation
    {
        internal string Path { get; set; }
        internal DiagramRect Bounds { get; set; }
        internal List<DiagramPoint> Points { get; set; }

        internal void Dispose()
        {
            if (Path != null)
            {
                Path = null;
            }
            if (Bounds != null)
            {
                Bounds.Dispose();
                Bounds = null;
            }
            if (Points != null)
            {
                for (int i = 0; i < Points.Count; i++)
                {
                    Points[i].Dispose();
                    Points[i] = null;
                }
                Points.Clear();
                Points = null;
            }
        }
    }
}
