using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Internal;

namespace Syncfusion.Blazor.CircularGauge.Internal
{
    /// <summary>
    /// Specifies the properties and methods to render annotations.
    /// </summary>
    internal class AnnotationRenderer
    {
        private CultureInfo culture = CultureInfo.InvariantCulture;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnnotationRenderer"/> class.
        /// </summary>
        /// <param name="parent"> to access the annotation render class fields. </param>
        public AnnotationRenderer(SfCircularGauge parent)
        {
            Parent = parent;
        }

        /// <summary>
        /// Gets or sets the properties of the Circular Gauge.
        /// </summary>
        internal SfCircularGauge Parent { get; set; }

        /// <summary>
        /// Gets or sets the properties to render annotation.
        /// </summary>
        internal Annotation AnnotationSetting { get; set; } = new Annotation();

        /// <summary>
        /// Gets or sets the properties to render annotation.
        /// </summary>
        internal List<Annotation> AnnotationCollection { get; set; } = new List<Annotation>();

        /// <summary>
        /// Creates the annotations in the Circular Gauge.
        /// </summary>
        /// <param name="axis">Specifies the axis of the annotation to be rendered.</param>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        internal async Task RenderAnnotation(CircularGaugeAxis axis)
        {
            for (int i = 0; i < axis.Annotations.Count; i++)
            {
                Annotation annotationSetting = new Annotation();
                await CreateTemplate(axis.Annotations[i], i, Parent.AxisRenderer.AxisSetting.AxisIndex, annotationSetting, axis);
                if (axis.Annotations[i].ContentTemplate != null)
                {
                    annotationSetting.IsTemplate = true;
                }
                else
                {
                    annotationSetting.Visibility = "visibility:hidden";
                }

                AnnotationCollection.Add(annotationSetting);
                AnnotationSetting = annotationSetting;
            }
        }

        /// <summary>
        /// Renders the annotation template.
        /// </summary>
        /// <param name="annotationValue">Specifies the value of the annotation.</param>
        /// <param name="annotationIndex">Specifies the index of the annotation.</param>
        /// <param name="axisIndex">represents the axis index of the annotation.</param>
        /// <returns> <see cref="Task"/> representing the asynchronous operation.</returns>
        internal RenderFragment RenderSecondaryElements(CircularGaugeAnnotation annotationValue, double annotationIndex, double axisIndex)
        {
            Parent.AxisRenderer.AxisCollection[(int)axisIndex].Annotations[(int)annotationIndex].AnnotationLocation =
                AxisRenderer.GetLocationFromAngle(annotationValue.Angle - 90, SfCircularGauge.StringToNumber(annotationValue.Radius, Parent.AxisRenderer.AxisSetting.CurrentRadius), Parent.AxisRenderer.AxisSetting.MidPoint);
            RenderFragment fragment = builder =>
            {
                int seq = 0;
                builder.OpenElement(seq++, "div");
                builder.AddAttribute(seq++, "id", Parent.ID + "_Axis_" + axisIndex + "_Annotation_" + annotationIndex + "_ContentTemplate");
                builder.AddContent(seq++, annotationValue.ContentTemplate);
                builder.CloseElement();
            };
            return fragment;
        }

        /// <summary>
        /// Disposes the property values during the destroy of the component that is hold up for the execution of the component.
        /// </summary>
        internal void Dispose()
        {
            AnnotationCollection = null;
            AnnotationSetting = null;
            Parent = null;
        }

        private async Task CreateTemplate(CircularGaugeAnnotation annotation, int annotationIndex, int axisIndex, Annotation annotationSetting, CircularGaugeAxis axis)
        {
            string content = axis.AxisValues.AnnotationContent[annotationIndex];
            AnnotationRenderEventArgs eventArgs = new AnnotationRenderEventArgs() { Cancel = false, Content = content };
            await SfBaseUtils.InvokeEvent<AnnotationRenderEventArgs>(Parent.CircularGaugeEvents?.AnnotationRendering, eventArgs);
            annotationSetting.AnnotationLocation = AxisRenderer.GetLocationFromAngle(annotation.Angle - 90, SfCircularGauge.StringToNumber(annotation.Radius, Parent.AxisRenderer.AxisSetting.CurrentRadius), Parent.AxisRenderer.AxisSetting.MidPoint);
            annotationSetting.AnnotationID = Parent.ID + "_Axis_" + axisIndex + "_Annotation_" + annotationIndex;
            annotationSetting.ContentTemplate = content;
            annotationSetting.Description = !string.IsNullOrEmpty(annotation.Description) ? annotation.Description : "Annotation";
            annotationSetting.LeftPosition = annotationSetting.AnnotationLocation.X.ToString(culture);
            annotationSetting.TopPosition = annotationSetting.AnnotationLocation.Y.ToString(culture);
            string fontSize = annotation.TextStyle != null && !string.IsNullOrEmpty(annotation.TextStyle.Size) ? annotation.TextStyle.Size : "12px";
            string fontWeight = annotation.TextStyle != null && !string.IsNullOrEmpty(annotation.TextStyle.FontWeight) ? annotation.TextStyle.FontWeight : "normal";
            string fontStyle = annotation.TextStyle != null && !string.IsNullOrEmpty(annotation.TextStyle.FontStyle) ? annotation.TextStyle.FontStyle : "normal";
            string fontFamily = annotation.TextStyle != null && !string.IsNullOrEmpty(annotation.TextStyle.FontFamily) ? annotation.TextStyle.FontFamily : Parent.ThemeStyles.FontFamily;
            double opacity = annotation.TextStyle != null ? annotation.TextStyle.Opacity : 1;
            string color = annotation.TextStyle != null ? annotation.TextStyle.Color : "#686868";
            annotationSetting.AnnotationPosition = "left:" + annotationSetting.LeftPosition + "px" + ";" + "top:" + annotationSetting.TopPosition + "px";
            annotationSetting.AnnotationTextStyle = "font-size:" + fontSize + "; font-style:" + fontStyle + "; font-weight:" + fontWeight + "; font-family:" + fontFamily +
                "; opacity:" + opacity + "; color:" + color + ";";
            annotationSetting.AnnotationTemplateStyles = "position:absolute; z-index:" + annotation.ZIndex + ";transform:" +
                (annotation.AutoAngle ? "rotate(" + (annotation.Angle - 90) + "deg)" : "rotate(0deg)") + annotationSetting.AnnotationPosition + ";";
        }
    }
}
