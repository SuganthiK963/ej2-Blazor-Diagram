using Microsoft.AspNetCore.Components.Rendering;
using Syncfusion.Blazor.Charts.Chart.Internal;
using Syncfusion.Blazor.DataVizCommon;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Syncfusion.Blazor.Charts.Internal
{
    public class ChartTitleRenderer : ChartRenderer
    {
        private Rect availableRect;

        private List<string> titleCollection = new List<string>();

        private List<string> subtitleCollection = new List<string>();

        private double maxWidth;

        private CultureInfo culture = CultureInfo.InvariantCulture;

        private Size titleSize = new Size(0, 0);

        private Size subTitleSize = new Size(0, 0);

        internal ChartTitleStyle TitleStyle { get; set; }

        internal ChartSubTitleStyle SubTitleStyle { get; set; }

        private string Description
        {
            get
            {
                return Owner.Description;
            }
        }

        private double TabIndex
        {
            get
            {
                return Owner.TabIndex;
            }
        }

        protected override void OnInitialized()
        {
            Owner.ChartTitleRenderer = this;
            AddToRenderQueue(this);
        }

        internal string GetTitleFontKey()
        {
            SetDefaultTitleStyle();
            return TitleStyle.FontWeight + Constants.UNDERSCORE + TitleStyle.FontStyle + Constants.UNDERSCORE + TitleStyle.FontFamily;
        }

        internal string GetSubTitleFontKey()
        {
            SetDefaultSubTitleStyle();
            return SubTitleStyle.FontWeight + Constants.UNDERSCORE + SubTitleStyle.FontStyle + Constants.UNDERSCORE + SubTitleStyle.FontFamily;
        }

        private ChartFontOptions GetTitleFontOptions()
        {
            return new ChartFontOptions { Color = TitleStyle.Color, Size = TitleStyle.Size, FontFamily = TitleStyle.FontFamily, FontWeight = TitleStyle.FontWeight, FontStyle = TitleStyle.FontStyle, TextAlignment = TitleStyle.TextAlignment, TextOverflow = TitleStyle.TextOverflow };
        }

        private ChartFontOptions GetSubTitleFontOptions()
        {
            return new ChartFontOptions { Color = SubTitleStyle.Color, Size = SubTitleStyle.Size, FontFamily = SubTitleStyle.FontFamily, FontWeight = SubTitleStyle.FontWeight, FontStyle = SubTitleStyle.FontStyle, TextAlignment = SubTitleStyle.TextAlignment, TextOverflow = SubTitleStyle.TextOverflow };
        }

        public override void HandleChartSizeChange(Rect rect)
        {
            if (rect != null && !string.IsNullOrEmpty(Owner.Title))
            {
                SetTitleCollection(rect);
                double titleHeight = (titleSize.Height * titleCollection.Count) + 15;
                if (!string.IsNullOrEmpty(Owner.SubTitle))
                {
                    double titleWidth;
                    foreach (string titleText in titleCollection)
                    {
                        titleWidth = ChartHelper.MeasureText(titleText, GetTitleFontOptions()).Width;
                        maxWidth = titleWidth > maxWidth ? titleWidth : maxWidth;
                    }

                    subtitleCollection = ChartHelper.GetTitle(Owner.SubTitle, GetSubTitleFontOptions(), maxWidth);
                    subTitleSize = ChartHelper.MeasureText(Owner.SubTitle, GetSubTitleFontOptions());
                    titleHeight += (subTitleSize.Height * subtitleCollection.Count) + 15;
                }

                rect.Height -= titleHeight;
                rect.Y += titleHeight;
                RendererShouldRender = true;
                if (availableRect != rect)
                {
                    availableRect = rect;
                }
            }
        }

        internal void SetTitleCollection(Rect rect)
        {
            titleCollection = ChartHelper.GetTitle(Owner.Title, GetTitleFontOptions(), rect.Width);
            titleSize = ChartHelper.MeasureText(Owner.Title, GetTitleFontOptions());
        }

        private void SetDefaultTitleStyle()
        {
            if (TitleStyle != null)
            {
                return;
            }

            TitleStyle = new ChartTitleStyle();
        }

        private void SetDefaultSubTitleStyle()
        {
            if (SubTitleStyle != null)
            {
                return;
            }

            SubTitleStyle = new ChartSubTitleStyle();
        }

        internal void OnThemeChanged()
        {
            RendererShouldRender = true;
            ProcessRenderQueue();
        }

        protected override void BuildRenderTree(RenderTreeBuilder builder)
        {
            if (availableRect != null && builder != null)
            {
                RenderTitle(builder);
                RendererShouldRender = false;
            }
        }

        private void RenderTitle(RenderTreeBuilder builder)
        {
            if (!string.IsNullOrEmpty(Owner.Title))
            {
                string textAnchor = ChartHelper.GetTextAnchor(TitleStyle.TextAlignment, Owner.EnableRTL);
                TextOptions titleOptions = new TextOptions(
                    Convert.ToString(ChartHelper.TitlePositionX(new Rect(Owner.Margin.Left, 0, Owner.AvailableSize.Width - Owner.Margin.Left - Owner.Margin.Right, 0), TitleStyle.TextAlignment), culture),
                    Convert.ToString(Owner.Margin.Top + (titleSize.Height * 3 / 4), culture),
                    TitleStyle.Color ?? Owner.ChartThemeStyle.ChartTitle,
                    GetTitleFontOptions(),
                    Owner.Title,
                    textAnchor,
                    Owner.ID + "_ChartTitle",
                    string.Empty,
                    string.Empty,
                    "auto",
                    !string.IsNullOrEmpty(Description) ? Description : Owner.Title,
                    TabIndex.ToString(culture));

                titleOptions.TextCollection = titleCollection;
                titleOptions.Font = GetTitleFontOptions();
                ChartHelper.TextElement(builder, Owner.SvgRenderer, titleOptions);
                if (!string.IsNullOrEmpty(Owner.SubTitle))
                {
                    RenderSubTitle(builder, titleOptions);
                }
            }
        }

        private void RenderSubTitle(RenderTreeBuilder builder, TextOptions options)
        {
            string textAnchor = ChartHelper.GetTextAnchor(SubTitleStyle.TextAlignment, Owner.EnableRTL);
            TextOptions subtitleOptions = new TextOptions(
                Convert.ToString(ChartHelper.TitlePositionX(new Rect((TitleStyle.TextAlignment == Alignment.Center) ? (Convert.ToDouble(options.X, null) - (maxWidth * 0.5)) : (TitleStyle.TextAlignment == Alignment.Far) ? Convert.ToDouble(options.X, null) - maxWidth : Convert.ToDouble(options.X, null), 0, maxWidth, 0), SubTitleStyle.TextAlignment), culture),
                Convert.ToString((Convert.ToDouble(options.Y, null) * titleCollection.Count) + (subTitleSize.Height * 3 / 4) + 10, culture),
                SubTitleStyle.Color ?? Owner.ChartThemeStyle.ChartTitle,
                SubTitleStyle.GetFontOptions(),
                subtitleCollection[0],
                textAnchor,
                Owner.ID + "_ChartSubTitle",
                string.Empty,
                string.Empty,
                "auto",
                !string.IsNullOrEmpty(Description) ? Description : Owner.SubTitle,
                TabIndex.ToString(culture));
            subtitleOptions.TextCollection = subtitleCollection;
            subtitleOptions.Font = GetSubTitleFontOptions();
            ChartHelper.TextElement(builder, Owner.SvgRenderer, subtitleOptions);
        }
    }
}
