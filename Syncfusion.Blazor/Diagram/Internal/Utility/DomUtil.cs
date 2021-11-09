using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Syncfusion.Blazor.Diagram.Internal
{
    internal static class DomUtil
    {
        internal static IJSRuntime JSRuntime { get; set; }

        private const string CREATEMEASUREELEMENTS = "sfBlazor.Diagram.createMeasureElements";
        private const string MEASUREBOUNDS = "sfBlazor.Diagram.measureBounds";
        private const string PATHPOINTS = "sfBlazor.Diagram.pathPoints";
        private const string UPDATEZOOMPANTOOL = "sfBlazor.Diagram.updateZoomPanTool";
        private const string UPDATEINNERLAYERSIZE = "sfBlazor.Diagram.updateInnerLayerSize";
        private const string UPDATEGRIDLINES = "sfBlazor.Diagram.updateGridlines";
        private const string OPENURL = "sfBlazor.Diagram.openUrl";
        private const string TEXTEDIT = "sfBlazor.Diagram.textEdit";

        internal static void CreateMeasureElements(IJSRuntime jsRuntime, bool zoomValue, string[] layersList, string width, string height, string DiagramContentID, DotNetObjectReference<DiagramEventHandler> selfReference, TransformFactor transform = null, DiagramSize patternSize = null, List<string> gridLinePathdata = null, List<DiagramPoint> gridLineDots = null)
        {
            JSRuntime = jsRuntime;
#pragma warning disable CA2012 // Use ValueTasks correctly
            JSRuntime.InvokeAsync<object>(CREATEMEASUREELEMENTS, new object[] { zoomValue, layersList, width, height, DiagramContentID, selfReference, transform, patternSize, gridLinePathdata, gridLineDots }).ConfigureAwait(true);
#pragma warning restore CA2012 // Use ValueTasks correctly
        }
        internal static void OpenUrl(string url)
        {
#pragma warning disable CA2012 // Use ValueTasks correctly
            JSRuntime.InvokeAsync<object>(OPENURL, url).ConfigureAwait(true);
#pragma warning disable CA2012 // Use ValueTasks correctly
        }
        internal static async Task TextEdit(TextElementUtils textElementUtils, DiagramPoint centerPointValue, DiagramRect nodeBounds, TransformFactor transform, bool scale, string annotationId)
        {
            await JSRuntime.InvokeAsync<object>(TEXTEDIT, textElementUtils, centerPointValue, nodeBounds, transform, scale, annotationId).ConfigureAwait(true);
        }
        internal static async Task<Dictionary<string, DiagramRect>> MeasureBounds(Dictionary<string, string> data, Dictionary<string, TextElementUtils> textData, Dictionary<string, string> imageData, Dictionary<string, string> nativeData)
        {
            Dictionary<string, DiagramRect> measureBoundsCollection = null;
            Dictionary<string, TextElementUtilsSerialize> textDataSerialize = DomUtil.SerializeTextStyle(textData);
            object obj = await JSRuntime.InvokeAsync<object>(MEASUREBOUNDS, new object[] { data, textDataSerialize, imageData, nativeData }).ConfigureAwait(true);
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                WriteIndented = true,
                IgnoreNullValues = true
            };
            Dictionary<string, object> dataObj = JsonSerializer.Deserialize<Dictionary<string, object>>(obj.ToString(), options);
            if (dataObj["Path"].ToString() != "{}")
            {
                measureBoundsCollection = JsonSerializer.Deserialize<Dictionary<string, DiagramRect>>(dataObj["Path"].ToString(), options);
                UpdateMeasuredPathData(measureBoundsCollection, Dictionary.MeasureCustomBounds);
            }
            if (dataObj["Image"].ToString() != "{}")
            {
                Dictionary<string, DiagramSize> measuredImageData = JsonSerializer.Deserialize<Dictionary<string, DiagramSize>>(dataObj["Image"].ToString(), options);
                UpdateMeasuredImageData(measuredImageData, Dictionary.MeasureImageBounds);
            }
            if (dataObj["Native"].ToString() != "{}")
            {
                Dictionary<string, DiagramSize> measuredNativeData = JsonSerializer.Deserialize<Dictionary<string, DiagramSize>>(dataObj["Native"].ToString(), options);
                UpdateMeasuredNativeData(measuredNativeData, Dictionary.MeasureNativeELementBounds);
            }
            if (dataObj["Text"].ToString() != "{}")
            {
                Dictionary<string, TextElementUtils> measuredTextData = JsonSerializer.Deserialize<Dictionary<string, TextElementUtils>>(dataObj["Text"].ToString(), options);
                UpdateMeasuredTextData(measuredTextData, Dictionary.MeasureTextBounds, textData, textDataSerialize);
            }
            if (textDataSerialize != null)
            {
                foreach (string index in textDataSerialize.Keys)
                {
                    TextElementUtilsSerialize value = textDataSerialize[index];
                    value.Dispose();
                    value = null;
                }
                textDataSerialize.Clear();
                textDataSerialize = null;
            }
            return measureBoundsCollection;
        }
        internal static void UpdateMeasuredTextData(Dictionary<string, TextElementUtils> currentMeasuredData, Dictionary<string, TextElementUtils> currentData, Dictionary<string, TextElementUtils> textData, Dictionary<string, TextElementUtilsSerialize> textDataSerialize)
        {
            foreach (string key in currentMeasuredData.Keys)
            {
                if (currentData.ContainsKey(key))
                    currentData[key] = currentMeasuredData[key];
                else
                    currentData.Add(key, currentMeasuredData[key]);
            }
            if (textData != null)
            {
                foreach (string key in textData.Keys)
                {
                    if (!currentData.ContainsKey(key))
                    {
                        TextElementUtils textElementUtilsSerialize = textData[key];
                        string style = TextElementStyleSerialize(textElementUtilsSerialize.Style).ToString();
                        KeyValuePair<string, TextElementUtilsSerialize>? textData1 = textDataSerialize.FirstOrDefault(x => x.Value.Content == textElementUtilsSerialize.Content && Equals(x.Value.Bounds.Width, textElementUtilsSerialize.Bounds.Width) && Equals(x.Value.Bounds.Height, textElementUtilsSerialize.Bounds.Height) &&
                                Equals(x.Value.NodeSize.Width, textElementUtilsSerialize.NodeSize.Width) && Equals(x.Value.NodeSize.Height, textElementUtilsSerialize.NodeSize.Height) &&
                                x.Value.Style.ToString() == style);
                        if (textData1.HasValue && textData1.Value.Value != null && !currentData.ContainsKey(key) && currentMeasuredData.ContainsKey(textData1.Value.Key))
                        {
                            currentData.Add(key, currentMeasuredData[textData1.Value.Key]);
                        }
                    }
                }
            }
        }
        private static void UpdateMeasuredPathData(Dictionary<string, DiagramRect> currentMeasuredData, Dictionary<string, DiagramRect> currentData)
        {
            foreach (string key in currentMeasuredData.Keys)
            {
                if (currentData.ContainsKey(key))
                    currentData[key] = currentMeasuredData[key];
                else
                    currentData.Add(key, currentMeasuredData[key]);
            }
        }
        private static void UpdateMeasuredImageData(Dictionary<string, DiagramSize> currentMeasuredData, Dictionary<string, DiagramSize> currentData)
        {
            foreach (string key in currentMeasuredData.Keys)
            {
                if (currentData.ContainsKey(key))
                    currentData[key] = currentMeasuredData[key];
                else
                    currentData.Add(key, currentMeasuredData[key]);
            }
        }
        private static void UpdateMeasuredNativeData(Dictionary<string, DiagramSize> currentMeasuredData, Dictionary<string, DiagramSize> currentData)
        {
            foreach (string key in currentMeasuredData.Keys)
            {
                if (currentData.ContainsKey(key))
                    currentData[key] = currentMeasuredData[key];
                else
                    currentData.Add(key, currentMeasuredData[key]);
            }
        }
        private static void UpdateMeasuredPathPointData(Dictionary<string, List<DiagramPoint>> currentMeasuredData, Dictionary<string, List<DiagramPoint>> currentData)
        {
            foreach (string key in currentMeasuredData.Keys)
            {
                if (currentData.ContainsKey(key))
                    currentData[key] = currentMeasuredData[key];
                else
                    currentData.Add(key, currentMeasuredData[key]);
            }
        }
        internal static DiagramRect MeasurePath(string data)
        {
            return Dictionary.GetMeasurePathBounds(data);
        }
        internal static TextElementUtils MeasureText(string data)
        {
            return Dictionary.GetMeasureTextBounds(data);
        }
        internal static DiagramSize MeasureImage(string data)
        {
            return Dictionary.GetMeasureImageBounds(data);
        }
        internal static DiagramSize MeasureNativeElement(string data)
        {
            return Dictionary.GetMeasureNativeElementBounds(data);
        }
        internal static double SetChildPosition(SubTextElement temp, ObservableCollection<SubTextElement> childNodes, int i, TextAttributes options)
        {
            if (childNodes.Count >= 1 && temp.X == 0 &&
                (options.TextOverflow == TextOverflow.Clip || options.TextOverflow == TextOverflow.Ellipsis) &&
                (options.TextWrapping == TextWrap.Wrap || options.TextWrapping == TextWrap.WrapWithOverflow))
            {
                temp.X = i > 0 && childNodes[i - 1] != null ? childNodes[i - 1].X : -(temp.Width / 2);
                return temp.X;
            }
            return temp.X;
        }
        // internal static async Task<List<DiagramPoint>> FindSegmentPoints(PathElement element) {
        //     DiagramRect pathBounds = element.AbsoluteBounds;
        //     string pathData = UpdatePath(element, pathBounds, element, null);
        //     object obj = await JSRuntime.InvokeAsync<object>(FINDSEGMENTPOINTS, new object[] { pathData }).ConfigureAwait(true);
        //     List<DiagramPoint> points = JsonSerializer.Deserialize<List<DiagramPoint>>(obj.ToString());
        //     return points;
        // }
        internal static string UpdatePath(string element, DiagramRect bounds)
        {
            double initX = 0; double initY = 0;
            DiagramRect bBox = bounds;
            double scaleX = 300 / bounds.Width;
            double scaleY = 300 / bounds.Height;
            bool isScale = true;

            List<PathSegment> arrayCollection = PathUtil.ProcessPathData(element);
            arrayCollection = PathUtil.SplitArrayCollection(arrayCollection);
            string newPathString = PathUtil.TransformPath(arrayCollection, scaleX, scaleY, isScale, bBox.X, bBox.Y, initX, initY);
            return newPathString;
        }
        internal static List<DiagramPoint> TranslatePoints(PathElement element, List<DiagramPoint> points)
        {
            List<DiagramPoint> translatedPts = new List<DiagramPoint>() { };
            int i;
            for (i = 0; i < points.Count; i++)
            {
                DiagramPoint point = points[i];
                Matrix matrix = null;
                double angle = element.RotationAngle + element.ParentTransform;
                if (angle != 0)
                {
                    matrix = Matrix.IdentityMatrix();
                    Matrix.RotateMatrix(matrix, angle, element.OffsetX, element.OffsetY);
                }
                if (matrix != null)
                {
                    point = Matrix.TransformPointByMatrix(matrix, point);
                }
                translatedPts.Add(point);
            }
            return translatedPts;
        }

        internal static async Task<Dictionary<string, List<DiagramPoint>>> PathPoints(Dictionary<string, string> pathPoint)
        {
            object obj = await JSRuntime.InvokeAsync<object>(PATHPOINTS, pathPoint);
            Dictionary<string, List<DiagramPoint>> measurePathPointsCollection = JsonSerializer.Deserialize<Dictionary<string, List<DiagramPoint>>>(obj.ToString());
            UpdateMeasuredPathPointData(measurePathPointsCollection, Dictionary.MeasureCustomPathPoints);
            return measurePathPointsCollection;
        }
        internal static async Task<DiagramRect> UpdateInnerLayerSize(string[] layersList, string width, string height, DiagramPoint scrollValues, TransformFactor gTransform = null, DiagramSize patternSize = null, List<string> gridLinePathData = null, List<DiagramPoint> gridLineDotsData = null, List<object> selectorAttributes = null, string id = null)
        {
            object obj = await JSRuntime.InvokeAsync<object>(UPDATEINNERLAYERSIZE, new object[] { layersList, width, height, scrollValues, gTransform, patternSize, gridLinePathData, gridLineDotsData, selectorAttributes, id}).ConfigureAwait(true);
            if (obj != null)
            {
                DiagramRect scrollBounds = JsonSerializer.Deserialize<DiagramRect>(obj.ToString());
                return scrollBounds;
            }
            return null;
        }

        internal static void UpdateZoomPanTool(bool value)
        {
#pragma warning disable CA2012 // Use ValueTasks correctly
            JSRuntime.InvokeVoidAsync(UPDATEZOOMPANTOOL, value);
#pragma warning restore CA2012 // Use ValueTasks correctly
        }

        private static Dictionary<string, TextElementUtilsSerialize> SerializeTextStyle(Dictionary<string, TextElementUtils> data)
        {
            Dictionary<string, TextElementUtilsSerialize> serializeResult = null;
            if (data != null)
            {
                serializeResult = new Dictionary<string, TextElementUtilsSerialize>();
                foreach (string index in data.Keys)
                {
                    TextElementUtils textElementUtils = data[index];
                    if (textElementUtils.Style != null)
                    {
                        TextElementUtilsSerialize textElementUtilsSerialize = new TextElementUtilsSerialize()
                        {
                            Content = textElementUtils.Content,
                            Bounds = textElementUtils.Bounds,
                            NodeSize = textElementUtils.NodeSize,
                            Style = TextElementStyleSerialize(textElementUtils.Style)
                        };
                        KeyValuePair<string, TextElementUtilsSerialize>? textData = serializeResult.FirstOrDefault(x => x.Value.Content == textElementUtilsSerialize.Content && Equals(x.Value.Bounds.Width, textElementUtilsSerialize.Bounds.Width) && Equals(x.Value.Bounds.Height, textElementUtilsSerialize.Bounds.Height) &&
                            Equals(x.Value.NodeSize.Width, textElementUtilsSerialize.NodeSize.Width) && Equals(x.Value.NodeSize.Height, textElementUtilsSerialize.NodeSize.Height) &&
                            x.Value.Style.ToString() == textElementUtilsSerialize.Style.ToString());
                        if (!(textData.HasValue && textData.Value.Value != null))
                        {
                            serializeResult.Add(index, textElementUtilsSerialize);
                        }
                    }
                }
            }
            return serializeResult;
        }
        internal static object TextElementStyleSerialize(TextStyle style)
        {
            TextElementStyle textElementStyle = new TextElementStyle()
            {
                Bold = style.Bold,
                FontFamily = style.FontFamily,
                FontSize = style.FontSize,
                Italic = style.Italic,
                TextAlign = style.TextAlign,
                TextDecoration = style.TextDecoration,
                TextOverflow = style.TextOverflow,
                TextWrapping = style.TextWrapping,
                WhiteSpace = style.WhiteSpace
            };
            string jsonData = JsonSerializer.Serialize(textElementStyle, new JsonSerializerOptions
            {
                WriteIndented = true,
                IgnoreNullValues = true
            });
            object result = JsonSerializer.Deserialize<object>(jsonData);
            return result;
        }

        internal static List<ITouches> AddTouchPointer(List<ITouches> touchList, ITouches[] touches)
        {
            touchList = new List<ITouches>() { };
            for(int i=0; i<touches.Length; i++)
            {
                touchList.Add(new ITouches() { PageX = touches[i].ClientX, PageY = touches[i].ClientY });
            }
            return touchList;
        }

        internal static void UpdateGridLines(string diagramPatternId, DiagramSize patternSize, List<string> pathData, List<DiagramPoint> dotsData)
        {
#pragma warning disable CA2012 // Use ValueTasks correctly
            JSRuntime.InvokeVoidAsync(UPDATEGRIDLINES, new object[] { diagramPatternId, patternSize, pathData, dotsData });
#pragma warning restore CA2012 // Use ValueTasks correctly
        }
    }
}
