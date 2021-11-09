window.sf = window.sf || {};
var chartsbase = (function (exports) {
'use strict';

/**
 * Common directory file
 */
var Common = /** @class */ (function () {
    function Common() {
    }
    return Common;
}());

/**
 * Specifies the chart constant value
 */
/** @private */
var loaded = 'loaded';
/** @private */
var legendClick = 'legendClick';
/** @private */
var load = 'load';
/** @private */
var animationComplete = 'animationComplete';
/** @private */
var legendRender = 'legendRender';
/** @private */
var textRender = 'textRender';
/** @private */
var pointRender = 'pointRender';
/** @private */
var seriesRender = 'seriesRender';
/** @private */
var axisLabelRender = 'axisLabelRender';
/** @private */
var axisRangeCalculated = 'axisRangeCalculated';
/** @private */
var axisMultiLabelRender = 'axisMultiLabelRender';
/** @private */
var tooltipRender = 'tooltipRender';
/** @private */
var sharedTooltipRender = 'sharedTooltipRender';
/** @private */
var chartMouseMove = 'chartMouseMove';
/** @private */
var chartMouseClick = 'chartMouseClick';
/** @private */
var pointClick = 'pointClick';
/** @private */
var pointDoubleClick = 'pointDoubleClick';
/** @private */
var pointMove = 'pointMove';
/** @private */
var chartMouseLeave = 'chartMouseLeave';
/** @private */
var chartMouseDown = 'chartMouseDown';
/** @private */
var chartMouseUp = 'chartMouseUp';
/** @private */
var zoomComplete = 'zoomComplete';
/** @private */
var dragComplete = 'dragComplete';
/** @private */
var selectionComplete = 'selectionComplete';
/** @private */
var resized = 'resized';
/** @private */
var beforePrint = 'beforePrint';
/** @private */
var annotationRender = 'annotationRender';
/** @private */
var scrollStart = 'scrollStart';
/** @private */
var scrollEnd = 'scrollEnd';
/** @private */
var scrollChanged = 'scrollChanged';
/** @private */
var stockEventRender = 'stockEventRender';
/** @private */
var multiLevelLabelClick = 'multiLevelLabelClick';
/** @private */
var dragStart = 'dragStart';
/** @private */
var drag = 'drag';
/** @private */
var dragEnd = 'dragEnd';
/*** @private*/
var regSub = /~\d+~/g;
/*** @private*/
var regSup = /\^\d+\^/g;
/** @private */
var beforeExport = 'beforeExport';
/** @private */
var afterExport = 'afterExport';
/** @private */
var bulletChartMouseClick = 'chartMouseClick';
/** @private */
var onZooming = 'onZooming';

/**
 * Specifies Chart Themes
 */

(function (Theme) {
    /** @private */
    Theme.axisLabelFont = {
        size: '12px',
        fontWeight: 'Normal',
        color: null,
        fontStyle: 'Normal',
        fontFamily: 'Segoe UI'
    };
    /** @private */
    Theme.axisTitleFont = {
        size: '14px',
        fontWeight: 'Normal',
        color: null,
        fontStyle: 'Normal',
        fontFamily: 'Segoe UI'
    };
    /** @private */
    Theme.chartTitleFont = {
        size: '15px',
        fontWeight: '500',
        color: null,
        fontStyle: 'Normal',
        fontFamily: 'Segoe UI'
    };
    /** @private */
    Theme.chartSubTitleFont = {
        size: '11px',
        fontWeight: '500',
        color: null,
        fontStyle: 'Normal',
        fontFamily: 'Segoe UI'
    };
    /** @private */
    Theme.crosshairLabelFont = {
        size: '13px',
        fontWeight: 'Normal',
        color: null,
        fontStyle: 'Normal',
        fontFamily: 'Segoe UI'
    };
    /** @private */
    Theme.tooltipLabelFont = {
        size: '13px',
        fontWeight: 'Normal',
        color: null,
        fontStyle: 'Normal',
        fontFamily: 'Segoe UI'
    };
    /** @private */
    Theme.legendLabelFont = {
        size: '13px',
        fontWeight: 'Normal',
        color: null,
        fontStyle: 'Normal',
        fontFamily: 'Segoe UI'
    };
    /** @private */
    Theme.stripLineLabelFont = {
        size: '12px',
        fontWeight: 'Regular',
        color: '#353535',
        fontStyle: 'Normal',
        fontFamily: 'Segoe UI'
    };
    /** @private */
    Theme.stockEventFont = {
        size: '13px',
        fontWeight: 'Normal',
        color: null,
        fontStyle: 'Normal',
        fontFamily: 'Segoe UI'
    };
})(exports.Theme || (exports.Theme = {}));
/** @private */
function getSeriesColor(theme) {
    var palette;
    switch (theme) {
        case 'Fabric':
            palette = ['#4472c4', '#ed7d31', '#ffc000', '#70ad47', '#5b9bd5',
                '#c1c1c1', '#6f6fe2', '#e269ae', '#9e480e', '#997300'];
            break;
        case 'Bootstrap4':
            palette = ['#a16ee5', '#f7ce69', '#55a5c2', '#7ddf1e', '#ff6ea6',
                '#7953ac', '#b99b4f', '#407c92', '#5ea716', '#b91c52'];
            break;
        case 'Bootstrap':
            palette = ['#a16ee5', '#f7ce69', '#55a5c2', '#7ddf1e', '#ff6ea6',
                '#7953ac', '#b99b4f', '#407c92', '#5ea716', '#b91c52'];
            break;
        case 'HighContrastLight':
        case 'Highcontrast':
        case 'HighContrast':
            palette = ['#79ECE4', '#E98272', '#DFE6B6', '#C6E773', '#BA98FF',
                '#FA83C3', '#00C27A', '#43ACEF', '#D681EF', '#D8BC6E'];
            break;
        case 'MaterialDark':
            palette = ['#00bdae', '#404041', '#357cd2', '#e56590', '#f8b883',
                '#70ad47', '#dd8abd', '#7f84e8', '#7bb4eb', '#ea7a57'];
            break;
        case 'FabricDark':
            palette = ['#4472c4', '#ed7d31', '#ffc000', '#70ad47', '#5b9bd5',
                '#c1c1c1', '#6f6fe2', '#e269ae', '#9e480e', '#997300'];
            break;
        case 'BootstrapDark':
            palette = ['#a16ee5', '#f7ce69', '#55a5c2', '#7ddf1e', '#ff6ea6',
                '#7953ac', '#b99b4f', '#407c92', '#5ea716', '#b91c52'];
            break;
        // palette = ['#B586FF', '#71F9A3', '#FF9572', '#5BD5FF', '#F9F871',
        //     '#B6F971', '#8D71F9', '#FF6F91', '#FFC75F', '#D55DB1'];
        // break;
        default:
            palette = ['#00bdae', '#404041', '#357cd2', '#e56590', '#f8b883',
                '#70ad47', '#dd8abd', '#7f84e8', '#7bb4eb', '#ea7a57'];
            break;
    }
    return palette;
}
/** @private */
function getThemeColor(theme) {
    var style;
    var darkBackground = theme === 'MaterialDark' ? '#303030' : (theme === 'FabricDark' ? '#201F1F' : '1A1A1A');
    switch (theme) {
        case 'HighContrastLight':
        case 'Highcontrast':
        case 'HighContrast':
            style = {
                axisLabel: '#ffffff',
                axisTitle: '#ffffff',
                axisLine: '#ffffff',
                majorGridLine: '#BFBFBF',
                minorGridLine: '#969696',
                majorTickLine: '#BFBFBF',
                minorTickLine: '#969696',
                chartTitle: '#ffffff',
                legendLabel: '#ffffff',
                background: '#000000',
                areaBorder: '#ffffff',
                errorBar: '#ffffff',
                crosshairLine: '#ffffff',
                crosshairFill: '#ffffff',
                crosshairLabel: '#000000',
                tooltipFill: '#ffffff',
                tooltipBoldLabel: '#000000',
                tooltipLightLabel: '#000000',
                tooltipHeaderLine: '#969696',
                markerShadow: '#BFBFBF',
                selectionRectFill: 'rgba(255, 217, 57, 0.3)',
                selectionRectStroke: '#ffffff',
                selectionCircleStroke: '#FFD939'
            };
            break;
        case 'MaterialDark':
        case 'FabricDark':
        case 'BootstrapDark':
            style = {
                axisLabel: '#DADADA', axisTitle: '#ffffff',
                axisLine: ' #6F6C6C',
                majorGridLine: '#414040',
                minorGridLine: '#514F4F',
                majorTickLine: '#414040',
                minorTickLine: ' #4A4848',
                chartTitle: '#ffffff',
                legendLabel: '#DADADA',
                background: darkBackground,
                areaBorder: ' #9A9A9A',
                errorBar: '#ffffff',
                crosshairLine: '#F4F4F4',
                crosshairFill: '#F4F4F4',
                crosshairLabel: '#282727',
                tooltipFill: '#F4F4F4',
                tooltipBoldLabel: '#282727',
                tooltipLightLabel: '#333232',
                tooltipHeaderLine: '#9A9A9A',
                markerShadow: null,
                selectionRectFill: 'rgba(56,169,255, 0.1)',
                selectionRectStroke: '#38A9FF',
                selectionCircleStroke: '#282727'
            };
            break;
        case 'Bootstrap4':
            style = {
                axisLabel: '#212529', axisTitle: '#ffffff', axisLine: '#CED4DA', majorGridLine: '#CED4DA',
                minorGridLine: '#DEE2E6', majorTickLine: '#ADB5BD', minorTickLine: '#CED4DA', chartTitle: '#212529', legendLabel: '#212529',
                background: '#FFFFFF', areaBorder: '#DEE2E6', errorBar: '#000000', crosshairLine: '#6C757D', crosshairFill: '#495057',
                crosshairLabel: '#FFFFFF', tooltipFill: 'rgba(0, 0, 0, 0.9)', tooltipBoldLabel: 'rgba(255,255,255)',
                tooltipLightLabel: 'rgba(255,255,255, 0.9)', tooltipHeaderLine: 'rgba(255,255,255, 0.2)', markerShadow: null,
                selectionRectFill: 'rgba(255,255,255, 0.1)', selectionRectStroke: 'rgba(0, 123, 255)', selectionCircleStroke: '#495057'
            };
            break;
        default:
            style = {
                axisLabel: '#686868',
                axisTitle: '#424242',
                axisLine: '#b5b5b5',
                majorGridLine: '#dbdbdb',
                minorGridLine: '#eaeaea',
                majorTickLine: '#b5b5b5',
                minorTickLine: '#d6d6d6',
                chartTitle: '#424242',
                legendLabel: '#353535',
                background: '#FFFFFF',
                areaBorder: 'Gray',
                errorBar: '#000000',
                crosshairLine: '#4f4f4f',
                crosshairFill: '#4f4f4f',
                crosshairLabel: '#e5e5e5',
                tooltipFill: 'rgba(0, 8, 22, 0.75)',
                tooltipBoldLabel: '#ffffff',
                tooltipLightLabel: '#dbdbdb',
                tooltipHeaderLine: '#ffffff',
                markerShadow: null,
                selectionRectFill: 'rgba(41, 171, 226, 0.1)',
                selectionRectStroke: '#29abe2',
                selectionCircleStroke: '#29abe2'
            };
            break;
    }
    return style;
}
/** @private */
function getScrollbarThemeColor(theme) {
    var scrollStyle;
    switch (theme) {
        case 'HighContrastLight':
            scrollStyle = {
                backRect: '#333',
                thumb: '#bfbfbf',
                circle: '#fff',
                circleHover: '#685708',
                arrow: '#333',
                grip: '#333',
                arrowHover: '#fff',
                backRectBorder: '#969696',
            };
            break;
        case 'Bootstrap':
            scrollStyle = {
                backRect: '#f5f5f5',
                thumb: '#e6e6e6',
                circle: '#fff',
                circleHover: '#eee',
                arrow: '#8c8c8c',
                grip: '#8c8c8c'
            };
            break;
        case 'Fabric':
            scrollStyle = {
                backRect: '#f8f8f8',
                thumb: '#eaeaea',
                circle: '#fff',
                circleHover: '#eaeaea',
                arrow: '#a6a6a6',
                grip: '#a6a6a6'
            };
            break;
        default:
            scrollStyle = {
                backRect: '#f5f5f5',
                thumb: '#e0e0e0',
                circle: '#fff',
                circleHover: '#eee',
                arrow: '#9e9e9e',
                grip: '#9e9e9e'
            };
            break;
    }
    return scrollStyle;
}

var __extends$1 = (undefined && undefined.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
/**
 * Defines the appearance of the connectors
 */
var Connector = /** @class */ (function (_super) {
    __extends$1(Connector, _super);
    function Connector() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    __decorate([
        sf.base.Property('Line')
    ], Connector.prototype, "type", void 0);
    __decorate([
        sf.base.Property(null)
    ], Connector.prototype, "color", void 0);
    __decorate([
        sf.base.Property(1)
    ], Connector.prototype, "width", void 0);
    __decorate([
        sf.base.Property(null)
    ], Connector.prototype, "length", void 0);
    __decorate([
        sf.base.Property('')
    ], Connector.prototype, "dashArray", void 0);
    return Connector;
}(sf.base.ChildProperty));
/**
 * Configures the fonts in charts.
 */
var Font = /** @class */ (function (_super) {
    __extends$1(Font, _super);
    function Font() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    __decorate([
        sf.base.Property('Normal')
    ], Font.prototype, "fontStyle", void 0);
    __decorate([
        sf.base.Property('16px')
    ], Font.prototype, "size", void 0);
    __decorate([
        sf.base.Property('Normal')
    ], Font.prototype, "fontWeight", void 0);
    __decorate([
        sf.base.Property('')
    ], Font.prototype, "color", void 0);
    __decorate([
        sf.base.Property('Center')
    ], Font.prototype, "textAlignment", void 0);
    __decorate([
        sf.base.Property('Segoe UI')
    ], Font.prototype, "fontFamily", void 0);
    __decorate([
        sf.base.Property(1)
    ], Font.prototype, "opacity", void 0);
    __decorate([
        sf.base.Property('Trim')
    ], Font.prototype, "textOverflow", void 0);
    return Font;
}(sf.base.ChildProperty));
/**
 * Configures the borders in the chart.
 */
var Border = /** @class */ (function (_super) {
    __extends$1(Border, _super);
    function Border() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    __decorate([
        sf.base.Property('')
    ], Border.prototype, "color", void 0);
    __decorate([
        sf.base.Property(1)
    ], Border.prototype, "width", void 0);
    return Border;
}(sf.base.ChildProperty));
/**
 * Configures the marker position in the chart.
 */
var Offset = /** @class */ (function (_super) {
    __extends$1(Offset, _super);
    function Offset() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    __decorate([
        sf.base.Property(0)
    ], Offset.prototype, "x", void 0);
    __decorate([
        sf.base.Property(0)
    ], Offset.prototype, "y", void 0);
    return Offset;
}(sf.base.ChildProperty));
/**
 * Configures the chart area.
 */
var ChartArea = /** @class */ (function (_super) {
    __extends$1(ChartArea, _super);
    function ChartArea() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    __decorate([
        sf.base.Complex({}, Border)
    ], ChartArea.prototype, "border", void 0);
    __decorate([
        sf.base.Property('transparent')
    ], ChartArea.prototype, "background", void 0);
    __decorate([
        sf.base.Property(1)
    ], ChartArea.prototype, "opacity", void 0);
    __decorate([
        sf.base.Property(null)
    ], ChartArea.prototype, "backgroundImage", void 0);
    return ChartArea;
}(sf.base.ChildProperty));
/**
 * Configures the chart margins.
 */
var Margin = /** @class */ (function (_super) {
    __extends$1(Margin, _super);
    function Margin() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    __decorate([
        sf.base.Property(10)
    ], Margin.prototype, "left", void 0);
    __decorate([
        sf.base.Property(10)
    ], Margin.prototype, "right", void 0);
    __decorate([
        sf.base.Property(10)
    ], Margin.prototype, "top", void 0);
    __decorate([
        sf.base.Property(10)
    ], Margin.prototype, "bottom", void 0);
    return Margin;
}(sf.base.ChildProperty));
/**
 * Configures the animation behavior for chart series.
 */
var Animation$1 = /** @class */ (function (_super) {
    __extends$1(Animation$$1, _super);
    function Animation$$1() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    __decorate([
        sf.base.Property(true)
    ], Animation$$1.prototype, "enable", void 0);
    __decorate([
        sf.base.Property(1000)
    ], Animation$$1.prototype, "duration", void 0);
    __decorate([
        sf.base.Property(0)
    ], Animation$$1.prototype, "delay", void 0);
    return Animation$$1;
}(sf.base.ChildProperty));
/**
 * Series and point index
 * @public
 */
var Indexes = /** @class */ (function (_super) {
    __extends$1(Indexes, _super);
    function Indexes() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    __decorate([
        sf.base.Property(0)
    ], Indexes.prototype, "series", void 0);
    __decorate([
        sf.base.Property(0)
    ], Indexes.prototype, "point", void 0);
    return Indexes;
}(sf.base.ChildProperty));
/**
 * Column series rounded corner options
 */
var CornerRadius = /** @class */ (function (_super) {
    __extends$1(CornerRadius, _super);
    function CornerRadius() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    __decorate([
        sf.base.Property(0)
    ], CornerRadius.prototype, "topLeft", void 0);
    __decorate([
        sf.base.Property(0)
    ], CornerRadius.prototype, "topRight", void 0);
    __decorate([
        sf.base.Property(0)
    ], CornerRadius.prototype, "bottomLeft", void 0);
    __decorate([
        sf.base.Property(0)
    ], CornerRadius.prototype, "bottomRight", void 0);
    return CornerRadius;
}(sf.base.ChildProperty));
/**
 * Configures the Empty Points of series
 */
var EmptyPointSettings = /** @class */ (function (_super) {
    __extends$1(EmptyPointSettings, _super);
    function EmptyPointSettings() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    __decorate([
        sf.base.Property(null)
    ], EmptyPointSettings.prototype, "fill", void 0);
    __decorate([
        sf.base.Complex({ color: 'transparent', width: 0 }, Border)
    ], EmptyPointSettings.prototype, "border", void 0);
    __decorate([
        sf.base.Property('Gap')
    ], EmptyPointSettings.prototype, "mode", void 0);
    return EmptyPointSettings;
}(sf.base.ChildProperty));
/**
 * Configures the drag settings of series
 */
var DragSettings = /** @class */ (function (_super) {
    __extends$1(DragSettings, _super);
    function DragSettings() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    __decorate([
        sf.base.Property(false)
    ], DragSettings.prototype, "enable", void 0);
    __decorate([
        sf.base.Property(null)
    ], DragSettings.prototype, "minY", void 0);
    __decorate([
        sf.base.Property(null)
    ], DragSettings.prototype, "maxY", void 0);
    __decorate([
        sf.base.Property(null)
    ], DragSettings.prototype, "fill", void 0);
    return DragSettings;
}(sf.base.ChildProperty));
/**
 * Configures the ToolTips in the chart.
 * @public
 */
var TooltipSettings = /** @class */ (function (_super) {
    __extends$1(TooltipSettings, _super);
    function TooltipSettings() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    __decorate([
        sf.base.Property(false)
    ], TooltipSettings.prototype, "enable", void 0);
    __decorate([
        sf.base.Property(true)
    ], TooltipSettings.prototype, "enableMarker", void 0);
    __decorate([
        sf.base.Property(false)
    ], TooltipSettings.prototype, "shared", void 0);
    __decorate([
        sf.base.Property(null)
    ], TooltipSettings.prototype, "fill", void 0);
    __decorate([
        sf.base.Property(null)
    ], TooltipSettings.prototype, "header", void 0);
    __decorate([
        sf.base.Property(0.75)
    ], TooltipSettings.prototype, "opacity", void 0);
    __decorate([
        sf.base.Complex(exports.Theme.tooltipLabelFont, Font)
    ], TooltipSettings.prototype, "textStyle", void 0);
    __decorate([
        sf.base.Property(null)
    ], TooltipSettings.prototype, "format", void 0);
    __decorate([
        sf.base.Property(null)
    ], TooltipSettings.prototype, "template", void 0);
    __decorate([
        sf.base.Property(true)
    ], TooltipSettings.prototype, "enableAnimation", void 0);
    __decorate([
        sf.base.Property(300)
    ], TooltipSettings.prototype, "duration", void 0);
    __decorate([
        sf.base.Property(1000)
    ], TooltipSettings.prototype, "fadeOutDuration", void 0);
    __decorate([
        sf.base.Property(false)
    ], TooltipSettings.prototype, "enableTextWrap", void 0);
    __decorate([
        sf.base.Complex({ color: '#cccccc', width: 0.5 }, Border)
    ], TooltipSettings.prototype, "border", void 0);
    return TooltipSettings;
}(sf.base.ChildProperty));
/**
 * button settings in period selector
 */
var Periods = /** @class */ (function (_super) {
    __extends$1(Periods, _super);
    function Periods() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    __decorate([
        sf.base.Property('Years')
    ], Periods.prototype, "intervalType", void 0);
    __decorate([
        sf.base.Property(1)
    ], Periods.prototype, "interval", void 0);
    __decorate([
        sf.base.Property(null)
    ], Periods.prototype, "text", void 0);
    __decorate([
        sf.base.Property(false)
    ], Periods.prototype, "selected", void 0);
    return Periods;
}(sf.base.ChildProperty));
/**
 * Period Selector Settings
 */
var PeriodSelectorSettings = /** @class */ (function (_super) {
    __extends$1(PeriodSelectorSettings, _super);
    function PeriodSelectorSettings() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    __decorate([
        sf.base.Property(43)
    ], PeriodSelectorSettings.prototype, "height", void 0);
    __decorate([
        sf.base.Property('Bottom')
    ], PeriodSelectorSettings.prototype, "position", void 0);
    __decorate([
        sf.base.Collection([], Periods)
    ], PeriodSelectorSettings.prototype, "periods", void 0);
    return PeriodSelectorSettings;
}(sf.base.ChildProperty));

/**
 * Numeric Range.
 * @private
 */
var DoubleRange = /** @class */ (function () {
    function DoubleRange(start, end) {
        /*
          if (!isNaN(start) && !isNaN(end)) {
           this.mIsEmpty = true;
          } else {
              this.mIsEmpty = false;
          }*/
        if (start < end) {
            this.mStart = start;
            this.mEnd = end;
        }
        else {
            this.mStart = end;
            this.mEnd = start;
        }
    }
    Object.defineProperty(DoubleRange.prototype, "start", {
        //private mIsEmpty: boolean;
        /** @private */
        get: function () {
            return this.mStart;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DoubleRange.prototype, "end", {
        /** @private */
        get: function () {
            return this.mEnd;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DoubleRange.prototype, "delta", {
        /*
          get isEmpty(): boolean {
             return this.mIsEmpty;
         }*/
        /** @private */
        get: function () {
            return (this.mEnd - this.mStart);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(DoubleRange.prototype, "median", {
        /** @private */
        get: function () {
            return this.mStart + (this.mEnd - this.mStart) / 2;
        },
        enumerable: true,
        configurable: true
    });
    return DoubleRange;
}());

/**
 * Numeric module is used to render numeric axis.
 */
var Double = /** @class */ (function () {
    /**
     * Constructor for the dateTime module.
     * @private
     */
    function Double(chart) {
        this.isColumn = 0;
        this.chart = chart;
    }
    /**
     * Numeric Nice Interval for the axis.
     * @private
     */
    Double.prototype.calculateNumericNiceInterval = function (axis, delta, size) {
        var actualDesiredIntervalsCount = getActualDesiredIntervalsCount(size, axis);
        var niceInterval = delta / actualDesiredIntervalsCount;
        if (!sf.base.isNullOrUndefined(axis.desiredIntervals)) {
            return niceInterval;
        }
        var minInterval = Math.pow(10, Math.floor(logBase(niceInterval, 10)));
        for (var _i = 0, _a = axis.intervalDivs; _i < _a.length; _i++) {
            var interval = _a[_i];
            var currentInterval = minInterval * interval;
            if (actualDesiredIntervalsCount < (delta / currentInterval)) {
                break;
            }
            niceInterval = currentInterval;
        }
        return niceInterval;
    };
    /**
     * Actual Range for the axis.
     * @private
     */
    Double.prototype.getActualRange = function (axis, size) {
        this.initializeDoubleRange(axis);
        if ((!axis.startFromZero) && (this.isColumn > 0)) {
            axis.actualRange.interval = axis.interval || this.calculateNumericNiceInterval(axis, axis.doubleRange.delta, size);
            axis.actualRange.max = axis.doubleRange.end + axis.actualRange.interval;
            if ((axis.doubleRange.start - axis.actualRange.interval < 0 && axis.doubleRange.start > 0)) {
                axis.actualRange.min = 0;
            }
            else {
                axis.actualRange.min = axis.doubleRange.start - axis.actualRange.interval;
            }
        }
        else {
            axis.actualRange.interval = axis.interval || this.calculateNumericNiceInterval(axis, axis.doubleRange.delta, size);
            axis.actualRange.min = axis.doubleRange.start;
            axis.actualRange.max = axis.doubleRange.end;
        }
    };
    /**
     * Range for the axis.
     * @private
     */
    Double.prototype.initializeDoubleRange = function (axis) {
        //Axis Min
        if (axis.minimum !== null) {
            this.min = axis.minimum;
        }
        else if (this.min === null || this.min === Number.POSITIVE_INFINITY) {
            this.min = 0;
        }
        // Axis Max
        if (axis.maximum !== null) {
            this.max = axis.maximum;
        }
        else if (this.max === null || this.max === Number.NEGATIVE_INFINITY) {
            this.max = 5;
        }
        if (this.min === this.max) {
            this.max = axis.valueType.indexOf('Category') > -1 ? this.max : this.min + 1;
        }
        axis.doubleRange = new DoubleRange(this.min, this.max);
        axis.actualRange = {};
    };
    /**
     * The function to calculate the range and labels for the axis.
     * @return {void}
     * @private
     */
    Double.prototype.calculateRangeAndInterval = function (size, axis) {
        this.calculateRange(axis, size);
        this.getActualRange(axis, size);
        this.applyRangePadding(axis, size);
        this.calculateVisibleLabels(axis, this.chart);
    };
    /**
     * Calculate Range for the axis.
     * @private
     */
    Double.prototype.calculateRange = function (axis, size) {
        /*! Generate axis range */
        this.min = null;
        this.max = null;
        if (!setRange(axis)) {
            for (var _i = 0, _a = axis.series; _i < _a.length; _i++) {
                var series_1 = _a[_i];
                if (!series_1.visible) {
                    continue;
                }
                this.paddingInterval = 0;
                axis.maxPointLength = series_1.points.length;
                if (((series_1.type.indexOf('Column') > -1 || series_1.type.indexOf('Histogram') > -1) && axis.orientation === 'Horizontal')
                    || (series_1.type.indexOf('Bar') > -1 && axis.orientation === 'Vertical')) {
                    if ((series_1.xAxis.valueType === 'Double' || series_1.xAxis.valueType === 'DateTime')
                        && series_1.xAxis.rangePadding === 'Auto') {
                        this.paddingInterval = getMinPointsDelta(series_1.xAxis, axis.series) * 0.5;
                    }
                }
                //For xRange
                if (axis.orientation === 'Horizontal') {
                    if (this.chart.requireInvertedAxis) {
                        this.yAxisRange(axis, series_1);
                    }
                    else {
                        this.findMinMax(series_1.xMin - this.paddingInterval, series_1.xMax + this.paddingInterval);
                    }
                }
                // For yRange
                if (axis.orientation === 'Vertical') {
                    this.isColumn += (series_1.type === 'Column' || series_1.type === 'Bar' || series_1.drawType === 'Column') ? 1 : 0;
                    if (this.chart.requireInvertedAxis) {
                        this.findMinMax(series_1.xMin - this.paddingInterval, series_1.xMax + this.paddingInterval);
                    }
                    else {
                        this.yAxisRange(axis, series_1);
                    }
                }
            }
        }
    };
    Double.prototype.yAxisRange = function (axis, series) {
        if (series.dragSettings.enable && this.chart.dragY) {
            if (this.chart.dragY >= axis.visibleRange.max) {
                series.yMax = this.chart.dragY + axis.visibleRange.interval;
            }
            if (this.chart.dragY <= axis.visibleRange.min) {
                series.yMin = this.chart.dragY - axis.visibleRange.interval;
            }
        }
        this.findMinMax(series.yMin, series.yMax);
    };
    Double.prototype.findMinMax = function (min, max) {
        if (this.min === null || this.min > min) {
            this.min = min;
        }
        if (this.max === null || this.max < max) {
            this.max = max;
        }
    };
    /**
     * Apply padding for the range.
     * @private
     */
    Double.prototype.applyRangePadding = function (axis, size) {
        var start = axis.actualRange.min;
        var end = axis.actualRange.max;
        if (!setRange(axis)) {
            var interval = axis.actualRange.interval;
            var padding = axis.getRangePadding(this.chart);
            if (padding === 'Additional' || padding === 'Round') {
                this.findAdditional(axis, start, end, interval);
            }
            else if (padding === 'Normal') {
                this.findNormal(axis, start, end, interval, size);
            }
            else {
                this.updateActualRange(axis, start, end, interval);
            }
        }
        axis.actualRange.delta = axis.actualRange.max - axis.actualRange.min;
        this.calculateVisibleRange(size, axis);
    };
    Double.prototype.updateActualRange = function (axis, minimum, maximum, interval) {
        axis.actualRange = {
            min: axis.minimum != null ? axis.minimum : minimum,
            max: axis.maximum != null ? axis.maximum : maximum,
            interval: axis.interval != null ? axis.interval : interval,
            delta: axis.actualRange.delta
        };
    };
    Double.prototype.findAdditional = function (axis, start, end, interval) {
        var minimum;
        var maximum;
        minimum = Math.floor(start / interval) * interval;
        maximum = Math.ceil(end / interval) * interval;
        if (axis.rangePadding === 'Additional') {
            minimum -= interval;
            maximum += interval;
        }
        this.updateActualRange(axis, minimum, maximum, interval);
    };
    Double.prototype.findNormal = function (axis, start, end, interval, size) {
        var remaining;
        var minimum;
        var maximum;
        var startValue = start;
        if (start < 0) {
            startValue = 0;
            minimum = start + (start * 0.05);
            remaining = interval + (minimum % interval);
            if ((0.365 * interval) >= remaining) {
                minimum -= interval;
            }
            if (minimum % interval < 0) {
                minimum = (minimum - interval) - (minimum % interval);
            }
        }
        else {
            minimum = start < ((5.0 / 6.0) * end) ? 0 : (start - (end - start) * 0.5);
            if (minimum % interval > 0) {
                minimum -= (minimum % interval);
            }
        }
        maximum = (end > 0) ? (end + (end - startValue) * 0.05) : (end - (end - startValue) * 0.05);
        remaining = interval - (maximum % interval);
        if ((0.365 * interval) >= remaining) {
            maximum += interval;
        }
        if (maximum % interval > 0) {
            maximum = (maximum + interval) - (maximum % interval);
        }
        axis.doubleRange = new DoubleRange(minimum, maximum);
        if (minimum === 0) {
            interval = this.calculateNumericNiceInterval(axis, axis.doubleRange.delta, size);
            maximum = Math.ceil(maximum / interval) * interval;
        }
        this.updateActualRange(axis, minimum, maximum, interval);
    };
    /**
     * Calculate visible range for axis.
     * @private
     */
    Double.prototype.calculateVisibleRange = function (size, axis) {
        axis.visibleRange = {
            max: axis.actualRange.max, min: axis.actualRange.min,
            delta: axis.actualRange.delta, interval: axis.actualRange.interval
        };
        if (this.chart.chartAreaType === 'Cartesian') {
            var isLazyLoad = sf.base.isNullOrUndefined(axis.zoomingScrollBar) ? false : axis.zoomingScrollBar.isLazyLoad;
            if ((axis.zoomFactor < 1 || axis.zoomPosition > 0) && !isLazyLoad) {
                axis.calculateVisibleRange(size);
                axis.visibleRange.interval = (axis.enableAutoIntervalOnZooming && axis.valueType !== 'Category') ?
                    this.calculateNumericNiceInterval(axis, axis.doubleRange.delta, size)
                    : axis.visibleRange.interval;
            }
        }
        axis.triggerRangeRender(this.chart, axis.visibleRange.min, axis.visibleRange.max, axis.visibleRange.interval);
    };
    /**
     * Calculate label for the axis.
     * @private
     */
    Double.prototype.calculateVisibleLabels = function (axis, chart) {
        /*! Generate axis labels */
        axis.visibleLabels = [];
        var tempInterval = axis.visibleRange.min;
        var labelStyle;
        var controlName = chart.getModuleName();
        var isPolarRadar = controlName === 'chart' && chart.chartAreaType === 'PolarRadar';
        if (!isPolarRadar && (axis.zoomFactor < 1 || axis.zoomPosition > 0 || this.paddingInterval)) {
            tempInterval = axis.visibleRange.min - (axis.visibleRange.min % axis.visibleRange.interval);
        }
        var format = this.getFormat(axis);
        var isCustom = format.match('{value}') !== null;
        var intervalDigits = 0;
        var formatDigits = 0;
        if (axis.labelFormat && axis.labelFormat.indexOf('n') > -1) {
            formatDigits = parseInt(axis.labelFormat.substring(1, axis.labelFormat.length), 10);
        }
        axis.format = chart.intl.getNumberFormat({
            format: isCustom ? '' : format,
            useGrouping: chart.useGroupingSeparator
        });
        axis.startLabel = axis.format(axis.visibleRange.min);
        axis.endLabel = axis.format(axis.visibleRange.max);
        if (axis.visibleRange.interval && (axis.visibleRange.interval + '').indexOf('.') >= 0) {
            intervalDigits = (axis.visibleRange.interval + '').split('.')[1].length;
        }
        labelStyle = (sf.base.extend({}, sf.base.getValue('properties', axis.labelStyle), null, true));
        for (; tempInterval <= axis.visibleRange.max; tempInterval += axis.visibleRange.interval) {
            if (withIn(tempInterval, axis.visibleRange)) {
                triggerLabelRender(chart, tempInterval, this.formatValue(axis, isCustom, format, tempInterval), labelStyle, axis);
            }
        }
        if (tempInterval && (tempInterval + '').indexOf('.') >= 0 && (tempInterval + '').split('.')[1].length > 10) {
            tempInterval = (tempInterval + '').split('.')[1].length > (formatDigits || intervalDigits) ?
                +tempInterval.toFixed(formatDigits || intervalDigits) : tempInterval;
            if (tempInterval <= axis.visibleRange.max) {
                triggerLabelRender(chart, tempInterval, this.formatValue(axis, isCustom, format, tempInterval), labelStyle, axis);
            }
        }
        if (axis.getMaxLabelWidth) {
            axis.getMaxLabelWidth(this.chart);
        }
    };
    /**
     * Format of the axis label.
     * @private
     */
    Double.prototype.getFormat = function (axis) {
        if (axis.labelFormat) {
            if (axis.labelFormat.indexOf('p') === 0 && axis.labelFormat.indexOf('{value}') === -1 && axis.isStack100) {
                return '{value}%';
            }
            return axis.labelFormat;
        }
        return axis.isStack100 ? '{value}%' : '';
    };
    /**
     * Formatted the axis label.
     * @private
     */
    Double.prototype.formatValue = function (axis, isCustom, format, tempInterval) {
        return isCustom ? format.replace('{value}', axis.format(tempInterval))
            : axis.format(tempInterval);
    };
    return Double;
}());

var __extends$3 = (undefined && undefined.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate$2 = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
/**
 * Configures the Annotation for chart.
 */
var ChartAnnotationSettings = /** @class */ (function (_super) {
    __extends$3(ChartAnnotationSettings, _super);
    function ChartAnnotationSettings() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    __decorate$2([
        sf.base.Property('0')
    ], ChartAnnotationSettings.prototype, "x", void 0);
    __decorate$2([
        sf.base.Property('0')
    ], ChartAnnotationSettings.prototype, "y", void 0);
    __decorate$2([
        sf.base.Property(null)
    ], ChartAnnotationSettings.prototype, "content", void 0);
    __decorate$2([
        sf.base.Property('Center')
    ], ChartAnnotationSettings.prototype, "horizontalAlignment", void 0);
    __decorate$2([
        sf.base.Property('Pixel')
    ], ChartAnnotationSettings.prototype, "coordinateUnits", void 0);
    __decorate$2([
        sf.base.Property('Chart')
    ], ChartAnnotationSettings.prototype, "region", void 0);
    __decorate$2([
        sf.base.Property('Middle')
    ], ChartAnnotationSettings.prototype, "verticalAlignment", void 0);
    __decorate$2([
        sf.base.Property(null)
    ], ChartAnnotationSettings.prototype, "xAxisName", void 0);
    __decorate$2([
        sf.base.Property(null)
    ], ChartAnnotationSettings.prototype, "yAxisName", void 0);
    __decorate$2([
        sf.base.Property(null)
    ], ChartAnnotationSettings.prototype, "description", void 0);
    return ChartAnnotationSettings;
}(sf.base.ChildProperty));
/**
 * label border properties.
 */
var LabelBorder = /** @class */ (function (_super) {
    __extends$3(LabelBorder, _super);
    function LabelBorder() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    __decorate$2([
        sf.base.Property('')
    ], LabelBorder.prototype, "color", void 0);
    __decorate$2([
        sf.base.Property(1)
    ], LabelBorder.prototype, "width", void 0);
    __decorate$2([
        sf.base.Property('Rectangle')
    ], LabelBorder.prototype, "type", void 0);
    return LabelBorder;
}(sf.base.ChildProperty));
/**
 * categories for multi level labels
 */
var MultiLevelCategories = /** @class */ (function (_super) {
    __extends$3(MultiLevelCategories, _super);
    function MultiLevelCategories() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    __decorate$2([
        sf.base.Property(null)
    ], MultiLevelCategories.prototype, "start", void 0);
    __decorate$2([
        sf.base.Property(null)
    ], MultiLevelCategories.prototype, "end", void 0);
    __decorate$2([
        sf.base.Property('')
    ], MultiLevelCategories.prototype, "text", void 0);
    __decorate$2([
        sf.base.Property(null)
    ], MultiLevelCategories.prototype, "maximumTextWidth", void 0);
    __decorate$2([
        sf.base.Property(null)
    ], MultiLevelCategories.prototype, "customAttributes", void 0);
    __decorate$2([
        sf.base.Property('')
    ], MultiLevelCategories.prototype, "type", void 0);
    return MultiLevelCategories;
}(sf.base.ChildProperty));
/**
 * Strip line properties
 */
var StripLineSettings = /** @class */ (function (_super) {
    __extends$3(StripLineSettings, _super);
    function StripLineSettings() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    __decorate$2([
        sf.base.Property(true)
    ], StripLineSettings.prototype, "visible", void 0);
    __decorate$2([
        sf.base.Property(false)
    ], StripLineSettings.prototype, "startFromAxis", void 0);
    __decorate$2([
        sf.base.Property(null)
    ], StripLineSettings.prototype, "start", void 0);
    __decorate$2([
        sf.base.Property(null)
    ], StripLineSettings.prototype, "end", void 0);
    __decorate$2([
        sf.base.Property(null)
    ], StripLineSettings.prototype, "size", void 0);
    __decorate$2([
        sf.base.Property('#808080')
    ], StripLineSettings.prototype, "color", void 0);
    __decorate$2([
        sf.base.Property(null)
    ], StripLineSettings.prototype, "dashArray", void 0);
    __decorate$2([
        sf.base.Property('Auto')
    ], StripLineSettings.prototype, "sizeType", void 0);
    __decorate$2([
        sf.base.Property(false)
    ], StripLineSettings.prototype, "isRepeat", void 0);
    __decorate$2([
        sf.base.Property(null)
    ], StripLineSettings.prototype, "repeatEvery", void 0);
    __decorate$2([
        sf.base.Property(null)
    ], StripLineSettings.prototype, "repeatUntil", void 0);
    __decorate$2([
        sf.base.Property(false)
    ], StripLineSettings.prototype, "isSegmented", void 0);
    __decorate$2([
        sf.base.Property(null)
    ], StripLineSettings.prototype, "segmentStart", void 0);
    __decorate$2([
        sf.base.Property(null)
    ], StripLineSettings.prototype, "segmentEnd", void 0);
    __decorate$2([
        sf.base.Property(null)
    ], StripLineSettings.prototype, "segmentAxisName", void 0);
    __decorate$2([
        sf.base.Complex({ color: 'transparent', width: 1 }, Border)
    ], StripLineSettings.prototype, "border", void 0);
    __decorate$2([
        sf.base.Property('')
    ], StripLineSettings.prototype, "text", void 0);
    __decorate$2([
        sf.base.Property(null)
    ], StripLineSettings.prototype, "rotation", void 0);
    __decorate$2([
        sf.base.Property('Middle')
    ], StripLineSettings.prototype, "horizontalAlignment", void 0);
    __decorate$2([
        sf.base.Property('Middle')
    ], StripLineSettings.prototype, "verticalAlignment", void 0);
    __decorate$2([
        sf.base.Complex(exports.Theme.stripLineLabelFont, Font)
    ], StripLineSettings.prototype, "textStyle", void 0);
    __decorate$2([
        sf.base.Property('Behind')
    ], StripLineSettings.prototype, "zIndex", void 0);
    __decorate$2([
        sf.base.Property(1)
    ], StripLineSettings.prototype, "opacity", void 0);
    return StripLineSettings;
}(sf.base.ChildProperty));
/**
 * MultiLevelLabels properties
 */
var MultiLevelLabels = /** @class */ (function (_super) {
    __extends$3(MultiLevelLabels, _super);
    function MultiLevelLabels() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    __decorate$2([
        sf.base.Property('Center')
    ], MultiLevelLabels.prototype, "alignment", void 0);
    __decorate$2([
        sf.base.Property('Wrap')
    ], MultiLevelLabels.prototype, "overflow", void 0);
    __decorate$2([
        sf.base.Complex(exports.Theme.axisLabelFont, Font)
    ], MultiLevelLabels.prototype, "textStyle", void 0);
    __decorate$2([
        sf.base.Complex({ color: null, width: 1, type: 'Rectangle' }, LabelBorder)
    ], MultiLevelLabels.prototype, "border", void 0);
    __decorate$2([
        sf.base.Collection([], MultiLevelCategories)
    ], MultiLevelLabels.prototype, "categories", void 0);
    return MultiLevelLabels;
}(sf.base.ChildProperty));
/**
 * Specifies range for scrollbarSettings property
 * @public
 */
var ScrollbarSettingsRange = /** @class */ (function (_super) {
    __extends$3(ScrollbarSettingsRange, _super);
    function ScrollbarSettingsRange() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    __decorate$2([
        sf.base.Property(null)
    ], ScrollbarSettingsRange.prototype, "minimum", void 0);
    __decorate$2([
        sf.base.Property(null)
    ], ScrollbarSettingsRange.prototype, "maximum", void 0);
    return ScrollbarSettingsRange;
}(sf.base.ChildProperty));
/**
 * Scrollbar Settings Properties for Lazy Loading
 */
var ScrollbarSettings = /** @class */ (function (_super) {
    __extends$3(ScrollbarSettings, _super);
    function ScrollbarSettings() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    __decorate$2([
        sf.base.Property(false)
    ], ScrollbarSettings.prototype, "enable", void 0);
    __decorate$2([
        sf.base.Property(null)
    ], ScrollbarSettings.prototype, "pointsLength", void 0);
    __decorate$2([
        sf.base.Complex({}, ScrollbarSettingsRange)
    ], ScrollbarSettings.prototype, "range", void 0);
    return ScrollbarSettings;
}(sf.base.ChildProperty));

var __extends$2 = (undefined && undefined.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate$1 = (undefined && undefined.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var axisPadding = 10;
/**
 * Configures the `rows` of the chart.
 */
var Row = /** @class */ (function (_super) {
    __extends$2(Row, _super);
    function Row() {
        /**
         * The height of the row as a string accept input both as '100px' and '100%'.
         * If specified as '100%, row renders to the full height of its chart.
         * @default '100%'
         */
        var _this = _super !== null && _super.apply(this, arguments) || this;
        /** @private */
        _this.axes = [];
        /** @private */
        _this.nearSizes = [];
        /** @private */
        _this.farSizes = [];
        return _this;
    }
    /**
     * Measure the row size
     * @return {void}
     * @private
     */
    Row.prototype.computeSize = function (axis, clipRect, scrollBarHeight) {
        var width = 0;
        var innerPadding = 5;
        if (axis.visible && axis.internalVisibility) {
            width += (axis.findTickSize(axis.crossInAxis) + scrollBarHeight +
                axis.findLabelSize(axis.crossInAxis, innerPadding) + axis.lineStyle.width * 0.5);
        }
        if (axis.opposedPosition) {
            this.farSizes.push(width);
        }
        else {
            this.nearSizes.push(width);
        }
    };
    __decorate$1([
        sf.base.Property('100%')
    ], Row.prototype, "height", void 0);
    __decorate$1([
        sf.base.Complex({}, Border)
    ], Row.prototype, "border", void 0);
    return Row;
}(sf.base.ChildProperty));
/**
 * Configures the `columns` of the chart.
 */
var Column = /** @class */ (function (_super) {
    __extends$2(Column, _super);
    function Column() {
        /**
         * The width of the column as a string accepts input both as like '100px' or '100%'.
         * If specified as '100%, column renders to the full width of its chart.
         * @default '100%'
         */
        var _this = _super !== null && _super.apply(this, arguments) || this;
        /** @private */
        _this.axes = [];
        /** @private */
        _this.nearSizes = [];
        /** @private */
        _this.farSizes = [];
        /** @private */
        _this.padding = 0;
        return _this;
    }
    /**
     * Measure the column size
     * @return {void}
     * @private
     */
    Column.prototype.computeSize = function (axis, clipRect, scrollBarHeight) {
        var height = 0;
        var innerPadding = 5;
        if (axis.visible && axis.internalVisibility) {
            height += (axis.findTickSize(axis.crossInAxis) + scrollBarHeight +
                axis.findLabelSize(axis.crossInAxis, innerPadding) + axis.lineStyle.width * 0.5);
        }
        if (axis.opposedPosition) {
            this.farSizes.push(height);
        }
        else {
            this.nearSizes.push(height);
        }
    };
    __decorate$1([
        sf.base.Property('100%')
    ], Column.prototype, "width", void 0);
    __decorate$1([
        sf.base.Complex({}, Border)
    ], Column.prototype, "border", void 0);
    return Column;
}(sf.base.ChildProperty));
/**
 * Configures the major grid lines in the `axis`.
 */
var MajorGridLines = /** @class */ (function (_super) {
    __extends$2(MajorGridLines, _super);
    function MajorGridLines() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    __decorate$1([
        sf.base.Property(1)
    ], MajorGridLines.prototype, "width", void 0);
    __decorate$1([
        sf.base.Property('')
    ], MajorGridLines.prototype, "dashArray", void 0);
    __decorate$1([
        sf.base.Property(null)
    ], MajorGridLines.prototype, "color", void 0);
    return MajorGridLines;
}(sf.base.ChildProperty));
/**
 * Configures the minor grid lines in the `axis`.
 */
var MinorGridLines = /** @class */ (function (_super) {
    __extends$2(MinorGridLines, _super);
    function MinorGridLines() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    __decorate$1([
        sf.base.Property(0.7)
    ], MinorGridLines.prototype, "width", void 0);
    __decorate$1([
        sf.base.Property('')
    ], MinorGridLines.prototype, "dashArray", void 0);
    __decorate$1([
        sf.base.Property(null)
    ], MinorGridLines.prototype, "color", void 0);
    return MinorGridLines;
}(sf.base.ChildProperty));
/**
 * Configures the axis line of a chart.
 */
var AxisLine = /** @class */ (function (_super) {
    __extends$2(AxisLine, _super);
    function AxisLine() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    __decorate$1([
        sf.base.Property(1)
    ], AxisLine.prototype, "width", void 0);
    __decorate$1([
        sf.base.Property('')
    ], AxisLine.prototype, "dashArray", void 0);
    __decorate$1([
        sf.base.Property(null)
    ], AxisLine.prototype, "color", void 0);
    return AxisLine;
}(sf.base.ChildProperty));
/**
 * Configures the major tick lines.
 */
var MajorTickLines = /** @class */ (function (_super) {
    __extends$2(MajorTickLines, _super);
    function MajorTickLines() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    __decorate$1([
        sf.base.Property(1)
    ], MajorTickLines.prototype, "width", void 0);
    __decorate$1([
        sf.base.Property(5)
    ], MajorTickLines.prototype, "height", void 0);
    __decorate$1([
        sf.base.Property(null)
    ], MajorTickLines.prototype, "color", void 0);
    return MajorTickLines;
}(sf.base.ChildProperty));
/**
 * Configures the minor tick lines.
 */
var MinorTickLines = /** @class */ (function (_super) {
    __extends$2(MinorTickLines, _super);
    function MinorTickLines() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    __decorate$1([
        sf.base.Property(0.7)
    ], MinorTickLines.prototype, "width", void 0);
    __decorate$1([
        sf.base.Property(5)
    ], MinorTickLines.prototype, "height", void 0);
    __decorate$1([
        sf.base.Property(null)
    ], MinorTickLines.prototype, "color", void 0);
    return MinorTickLines;
}(sf.base.ChildProperty));
/**
 * Configures the crosshair ToolTip.
 */
var CrosshairTooltip = /** @class */ (function (_super) {
    __extends$2(CrosshairTooltip, _super);
    function CrosshairTooltip() {
        return _super !== null && _super.apply(this, arguments) || this;
    }
    __decorate$1([
        sf.base.Property(false)
    ], CrosshairTooltip.prototype, "enable", void 0);
    __decorate$1([
        sf.base.Property(null)
    ], CrosshairTooltip.prototype, "fill", void 0);
    __decorate$1([
        sf.base.Complex(exports.Theme.crosshairLabelFont, Font)
    ], CrosshairTooltip.prototype, "textStyle", void 0);
    return CrosshairTooltip;
}(sf.base.ChildProperty));
/**
 * Configures the axes in the chart.
 * @public
 */
var Axis = /** @class */ (function (_super) {
    __extends$2(Axis, _super);
    // tslint:disable-next-line:no-any
    function Axis(parent, propName, defaultValue, isArray) {
        var _this = _super.call(this, parent, propName, defaultValue, isArray) || this;
        /** @private */
        _this.visibleLabels = [];
        /** @private */
        _this.series = [];
        /** @private */
        _this.rect = new sf.svgbase.Rect(undefined, undefined, 0, 0);
        /** @private */
        _this.axisBottomLine = null;
        /** @private */
        _this.intervalDivs = [10, 5, 2, 1];
        /** @private */
        _this.angle = _this.labelRotation;
        /** @private */
        _this.isStack100 = false;
        /** @private */
        _this.crossAt = null;
        /** @private */
        _this.updatedRect = null;
        /** @private */
        _this.multiLevelLabelHeight = 0;
        /** @private */
        _this.isChart = true;
        /** @private */
        _this.isIntervalInDecimal = true;
        /**
         * @private
         * Task: BLAZ-2044
         * This property used to hide the axis when series hide from legend click
         */
        _this.internalVisibility = true;
        return _this;
    }
    /**
     * The function used to find tick size.
     * @return {number}
     * @private
     */
    Axis.prototype.findTickSize = function (crossAxis) {
        if (this.tickPosition === 'Inside') {
            return 0;
        }
        if (crossAxis && (!crossAxis.visibleRange || this.isInside(crossAxis.visibleRange))) {
            return 0;
        }
        return this.majorTickLines.height;
    };
    /**
     * The function used to find axis position.
     * @return {number}
     * @private
     */
    Axis.prototype.isInside = function (range) {
        return (inside(this.crossAt, range) ||
            (!this.opposedPosition && this.crossAt >= range.max) || (this.opposedPosition && this.crossAt <= range.min));
    };
    /**
     * The function used to find label Size.
     * @return {number}
     * @private
     */
    Axis.prototype.findLabelSize = function (crossAxis, innerPadding) {
        var titleSize = 0;
        if (this.title) {
            titleSize = sf.svgbase.measureText(this.title, this.titleStyle).height + innerPadding;
        }
        if (this.labelPosition === 'Inside') {
            return titleSize + innerPadding;
        }
        var diff;
        var value;
        var labelSize = titleSize + innerPadding + axisPadding +
            ((this.orientation === 'Vertical') ? this.maxLabelSize.width : this.maxLabelSize.height) + this.multiLevelLabelHeight;
        if (crossAxis && this.placeNextToAxisLine) {
            var range = crossAxis.visibleRange;
            var size = (crossAxis.orientation === 'Horizontal') ? crossAxis.rect.width : crossAxis.rect.height;
            if (!range || !size) {
                return 0;
            }
            else if (this.isInside(range)) {
                value = this.findDifference(crossAxis);
                diff = (value) * (size / range.delta);
                diff = (value) * ((size - (diff < labelSize ? (labelSize - diff) : 0)) / range.delta);
                labelSize = (diff < labelSize) ? (labelSize - diff) : 0;
            }
        }
        return labelSize;
    };
    /**
     * The function used to find axis position.
     * @return {number}
     * @private
     */
    Axis.prototype.updateCrossValue = function (chart) {
        var value = this.crossAt;
        if (value === null || !this.isInside(this.crossInAxis.visibleRange)) {
            this.updatedRect = this.rect;
            return null;
        }
        var range = this.crossInAxis.visibleRange;
        if (!this.opposedPosition) {
            if (this.crossAt > range.max) {
                value = range.max;
            }
        }
        else {
            if (this.crossAt < range.min) {
                value = range.min;
            }
        }
        this.updatedRect = sf.base.extend({}, this.rect, null, true);
        if (this.orientation === 'Horizontal') {
            value = this.crossInAxis.rect.height - (valueToCoefficient(value, this.crossInAxis) * this.crossInAxis.rect.height);
            this.updatedRect.y = this.crossInAxis.rect.y + value;
        }
        else {
            value = valueToCoefficient(value, this.crossInAxis) * this.crossInAxis.rect.width;
            this.updatedRect.x = this.crossInAxis.rect.x + value;
        }
    };
    Axis.prototype.findDifference = function (crossAxis) {
        var value = 0;
        if (this.opposedPosition) {
            value = crossAxis.isInversed ? crossAxis.visibleRange.min : crossAxis.visibleRange.max;
        }
        else {
            value = crossAxis.isInversed ? crossAxis.visibleRange.max : crossAxis.visibleRange.min;
        }
        return Math.abs(this.crossAt - value);
    };
    /**
     * Calculate visible range for axis.
     * @return {void}
     * @private
     */
    Axis.prototype.calculateVisibleRange = function (size) {
        if (this.zoomFactor < 1 || this.zoomPosition > 0) {
            var baseRange = this.actualRange;
            var start = void 0;
            var end = void 0;
            if (!this.isInversed) {
                start = this.actualRange.min + this.zoomPosition * this.actualRange.delta;
                end = start + this.zoomFactor * this.actualRange.delta;
            }
            else {
                start = this.actualRange.max - (this.zoomPosition * this.actualRange.delta);
                end = start - (this.zoomFactor * this.actualRange.delta);
            }
            if (start < baseRange.min) {
                end = end + (baseRange.min - start);
                start = baseRange.min;
            }
            if (end > baseRange.max) {
                start = start - (end - baseRange.max);
                end = baseRange.max;
            }
            this.doubleRange = new DoubleRange(start, end);
            this.visibleRange = { min: this.doubleRange.start, max: this.doubleRange.end,
                delta: this.doubleRange.delta, interval: this.visibleRange.interval };
        }
    };
    /**
     * Triggers the event.
     * @return {void}
     * @private
     */
    Axis.prototype.triggerRangeRender = function (chart, minimum, maximum, interval) {
        var argsData;
        argsData = {
            cancel: false, name: axisRangeCalculated, axis: this,
            minimum: minimum, maximum: maximum, interval: interval
        };
        chart.trigger(axisRangeCalculated, argsData);
        if (!argsData.cancel) {
            this.visibleRange = { min: argsData.minimum, max: argsData.maximum, interval: argsData.interval,
                delta: argsData.maximum - argsData.minimum };
        }
    };
    /**
     * Calculate padding for the axis.
     * @return {string}
     * @private
     */
    Axis.prototype.getRangePadding = function (chart) {
        var padding = this.rangePadding;
        if (padding !== 'Auto') {
            return padding;
        }
        switch (this.orientation) {
            case 'Horizontal':
                if (chart.requireInvertedAxis) {
                    padding = (this.isStack100 || this.baseModule.chart.stockChart ? 'Round' : 'Normal');
                }
                else {
                    padding = 'None';
                }
                break;
            case 'Vertical':
                if (!chart.requireInvertedAxis) {
                    padding = (this.isStack100 || this.baseModule.chart.stockChart ? 'Round' : 'Normal');
                }
                else {
                    padding = 'None';
                }
                break;
        }
        return padding;
    };
    /**
     * Calculate maximum label width for the axis.
     * @return {void}
     * @private
     */
    // tslint:disable-next-line:max-func-body-length
    Axis.prototype.getMaxLabelWidth = function (chart) {
        var pointX;
        var previousEnd = 0;
        var isIntersect = false;
        var isAxisLabelBreak;
        this.angle = this.labelRotation;
        this.maxLabelSize = new sf.svgbase.Size(0, 0);
        var action = this.labelIntersectAction;
        var label;
        for (var i = 0, len = this.visibleLabels.length; i < len; i++) {
            label = this.visibleLabels[i];
            isAxisLabelBreak = isBreakLabel(label.originalText);
            if (isAxisLabelBreak) {
                label.size = sf.svgbase.measureText(label.originalText.replace(/<br>/g, ' '), this.labelStyle);
                label.breakLabelSize = sf.svgbase.measureText(this.enableTrim ? label.text.join('<br>') : label.originalText, this.labelStyle);
            }
            else {
                label.size = sf.svgbase.measureText(label.text, this.labelStyle);
            }
            var width = isAxisLabelBreak ? label.breakLabelSize.width : label.size.width;
            if (width > this.maxLabelSize.width) {
                this.maxLabelSize.width = width;
                this.rotatedLabel = label.text;
            }
            var height = isAxisLabelBreak ? label.breakLabelSize.height : label.size.height;
            if (height > this.maxLabelSize.height) {
                this.maxLabelSize.height = height;
            }
            if (isAxisLabelBreak) {
                label.text = this.enableTrim ? label.text : label.originalText.split('<br>');
            }
            if (action === 'None' || action === 'Hide' || action === 'Trim') {
                continue;
            }
            if ((action !== 'None' || this.angle % 360 === 0) && this.orientation === 'Horizontal' &&
                this.rect.width > 0 && !isIntersect) {
                var width1 = isAxisLabelBreak ? label.breakLabelSize.width : label.size.width;
                var height1 = isAxisLabelBreak ? label.breakLabelSize.height : label.size.height;
                pointX = (valueToCoefficient(label.value, this) * this.rect.width) + this.rect.x;
                pointX -= width1 / 2;
                if (this.edgeLabelPlacement === 'Shift') {
                    if (i === 0 && pointX < this.rect.x) {
                        pointX = this.rect.x;
                    }
                    if (i === this.visibleLabels.length - 1 && ((pointX + width1) > (this.rect.x + this.rect.width))) {
                        pointX = this.rect.x + this.rect.width - width1;
                    }
                }
                switch (action) {
                    case 'MultipleRows':
                        if (i > 0) {
                            this.findMultiRows(i, pointX, label, isAxisLabelBreak);
                        }
                        break;
                    case 'Rotate45':
                    case 'Rotate90':
                        if (i > 0 && (!this.isInversed ? pointX <= previousEnd : pointX + width1 >= previousEnd)) {
                            this.angle = (action === 'Rotate45') ? 45 : 90;
                            isIntersect = true;
                        }
                        break;
                    default:
                        if (isAxisLabelBreak) {
                            var result = void 0;
                            var result1 = [];
                            var str = void 0;
                            for (var index = 0; index < label.text.length; index++) {
                                result = textWrap(label.text[index], this.rect.width / this.visibleLabels.length, this.labelStyle);
                                if (result.length > 1) {
                                    for (var j = 0; j < result.length; j++) {
                                        str = result[j];
                                        result1.push(str);
                                    }
                                }
                                else {
                                    result1.push(result[0]);
                                }
                            }
                            label.text = result1;
                        }
                        else {
                            label.text = textWrap(label.text, this.rect.width / this.visibleLabels.length, this.labelStyle);
                        }
                        var height_1 = (height1 * label.text.length);
                        if (height_1 > this.maxLabelSize.height) {
                            this.maxLabelSize.height = height_1;
                        }
                        break;
                }
                previousEnd = this.isInversed ? pointX : pointX + width1;
            }
        }
        if (this.angle !== 0 && this.orientation === 'Horizontal') {
            //I264474: Fix for datasource bind im mounted console error ocurred
            this.rotatedLabel = sf.base.isNullOrUndefined(this.rotatedLabel) ? '' : this.rotatedLabel;
            if (isBreakLabel(this.rotatedLabel)) {
                this.maxLabelSize = sf.svgbase.measureText(this.rotatedLabel, this.labelStyle);
            }
            else {
                this.maxLabelSize = rotateTextSize(this.labelStyle, this.rotatedLabel, this.angle, chart);
            }
        }
        if (chart.multiLevelLabelModule && this.multiLevelLabels.length > 0) {
            chart.multiLevelLabelModule.getMultilevelLabelsHeight(this);
        }
    };
    /**
     * Finds the multiple rows for axis.
     * @return {void}
     */
    Axis.prototype.findMultiRows = function (length, currentX, currentLabel, isBreakLabels) {
        var label;
        var pointX;
        var width2;
        var store = [];
        var isMultiRows;
        for (var i = length - 1; i >= 0; i--) {
            label = this.visibleLabels[i];
            width2 = isBreakLabels ? label.breakLabelSize.width : label.size.width;
            pointX = (valueToCoefficient(label.value, this) * this.rect.width) + this.rect.x;
            isMultiRows = !this.isInversed ? currentX < (pointX + width2 * 0.5) :
                currentX + currentLabel.size.width > (pointX - width2 * 0.5);
            if (isMultiRows) {
                store.push(label.index);
                currentLabel.index = (currentLabel.index > label.index) ? currentLabel.index : label.index + 1;
            }
            else {
                currentLabel.index = store.indexOf(label.index) > -1 ? currentLabel.index : label.index;
            }
        }
        var height = ((isBreakLabels ? currentLabel.breakLabelSize.height : currentLabel.size.height) * currentLabel.index) +
            (5 * (currentLabel.index - 1));
        if (height > this.maxLabelSize.height) {
            this.maxLabelSize.height = height;
        }
    };
    /**
     * Finds the default module for axis.
     * @return {void}
     * @private
     */
    Axis.prototype.getModule = function (chart) {
        if (this.valueType === 'Double') {
            this.baseModule = new Double(chart);
        }
        else {
            this.baseModule = chart[firstToLowerCase(this.valueType) + 'Module'];
        }
    };
    __decorate$1([
        sf.base.Complex(exports.Theme.axisLabelFont, Font)
    ], Axis.prototype, "labelStyle", void 0);
    __decorate$1([
        sf.base.Complex({}, CrosshairTooltip)
    ], Axis.prototype, "crosshairTooltip", void 0);
    __decorate$1([
        sf.base.Property('')
    ], Axis.prototype, "title", void 0);
    __decorate$1([
        sf.base.Complex(exports.Theme.axisTitleFont, Font)
    ], Axis.prototype, "titleStyle", void 0);
    __decorate$1([
        sf.base.Property('')
    ], Axis.prototype, "labelFormat", void 0);
    __decorate$1([
        sf.base.Property('')
    ], Axis.prototype, "skeleton", void 0);
    __decorate$1([
        sf.base.Property('DateTime')
    ], Axis.prototype, "skeletonType", void 0);
    __decorate$1([
        sf.base.Property(0)
    ], Axis.prototype, "plotOffset", void 0);
    __decorate$1([
        sf.base.Property(null)
    ], Axis.prototype, "plotOffsetLeft", void 0);
    __decorate$1([
        sf.base.Property(null)
    ], Axis.prototype, "plotOffsetTop", void 0);
    __decorate$1([
        sf.base.Property(null)
    ], Axis.prototype, "plotOffsetRight", void 0);
    __decorate$1([
        sf.base.Property(null)
    ], Axis.prototype, "plotOffsetBottom", void 0);
    __decorate$1([
        sf.base.Property(false)
    ], Axis.prototype, "isIndexed", void 0);
    __decorate$1([
        sf.base.Property(10)
    ], Axis.prototype, "logBase", void 0);
    __decorate$1([
        sf.base.Property(0)
    ], Axis.prototype, "columnIndex", void 0);
    __decorate$1([
        sf.base.Property(0)
    ], Axis.prototype, "rowIndex", void 0);
    __decorate$1([
        sf.base.Property(1)
    ], Axis.prototype, "span", void 0);
    __decorate$1([
        sf.base.Property(null)
    ], Axis.prototype, "desiredIntervals", void 0);
    __decorate$1([
        sf.base.Property(3)
    ], Axis.prototype, "maximumLabels", void 0);
    __decorate$1([
        sf.base.Property(1)
    ], Axis.prototype, "zoomFactor", void 0);
    __decorate$1([
        sf.base.Property(0)
    ], Axis.prototype, "zoomPosition", void 0);
    __decorate$1([
        sf.base.Property(true)
    ], Axis.prototype, "enableScrollbarOnZooming", void 0);
    __decorate$1([
        sf.base.Property(false)
    ], Axis.prototype, "opposedPosition", void 0);
    __decorate$1([
        sf.base.Property(true)
    ], Axis.prototype, "enableAutoIntervalOnZooming", void 0);
    __decorate$1([
        sf.base.Property('Auto')
    ], Axis.prototype, "rangePadding", void 0);
    __decorate$1([
        sf.base.Property('Double')
    ], Axis.prototype, "valueType", void 0);
    __decorate$1([
        sf.base.Property('None')
    ], Axis.prototype, "edgeLabelPlacement", void 0);
    __decorate$1([
        sf.base.Property('Auto')
    ], Axis.prototype, "intervalType", void 0);
    __decorate$1([
        sf.base.Property('BetweenTicks')
    ], Axis.prototype, "labelPlacement", void 0);
    __decorate$1([
        sf.base.Property('Outside')
    ], Axis.prototype, "tickPosition", void 0);
    __decorate$1([
        sf.base.Property('Outside')
    ], Axis.prototype, "labelPosition", void 0);
    __decorate$1([
        sf.base.Property('')
    ], Axis.prototype, "name", void 0);
    __decorate$1([
        sf.base.Property(true)
    ], Axis.prototype, "visible", void 0);
    __decorate$1([
        sf.base.Property(0)
    ], Axis.prototype, "minorTicksPerInterval", void 0);
    __decorate$1([
        sf.base.Property(0)
    ], Axis.prototype, "labelRotation", void 0);
    __decorate$1([
        sf.base.Property(null)
    ], Axis.prototype, "crossesAt", void 0);
    __decorate$1([
        sf.base.Property(true)
    ], Axis.prototype, "placeNextToAxisLine", void 0);
    __decorate$1([
        sf.base.Property(null)
    ], Axis.prototype, "crossesInAxis", void 0);
    __decorate$1([
        sf.base.Property(null)
    ], Axis.prototype, "minimum", void 0);
    __decorate$1([
        sf.base.Property(null)
    ], Axis.prototype, "maximum", void 0);
    __decorate$1([
        sf.base.Property(null)
    ], Axis.prototype, "interval", void 0);
    __decorate$1([
        sf.base.Property(34)
    ], Axis.prototype, "maximumLabelWidth", void 0);
    __decorate$1([
        sf.base.Property(false)
    ], Axis.prototype, "enableTrim", void 0);
    __decorate$1([
        sf.base.Property(5)
    ], Axis.prototype, "labelPadding", void 0);
    __decorate$1([
        sf.base.Complex({}, MajorTickLines)
    ], Axis.prototype, "majorTickLines", void 0);
    __decorate$1([
        sf.base.Complex({}, MinorTickLines)
    ], Axis.prototype, "minorTickLines", void 0);
    __decorate$1([
        sf.base.Complex({}, MajorGridLines)
    ], Axis.prototype, "majorGridLines", void 0);
    __decorate$1([
        sf.base.Complex({}, MinorGridLines)
    ], Axis.prototype, "minorGridLines", void 0);
    __decorate$1([
        sf.base.Complex({}, AxisLine)
    ], Axis.prototype, "lineStyle", void 0);
    __decorate$1([
        sf.base.Property('Trim')
    ], Axis.prototype, "labelIntersectAction", void 0);
    __decorate$1([
        sf.base.Property(false)
    ], Axis.prototype, "isInversed", void 0);
    __decorate$1([
        sf.base.Property(100)
    ], Axis.prototype, "coefficient", void 0);
    __decorate$1([
        sf.base.Property(0)
    ], Axis.prototype, "startAngle", void 0);
    __decorate$1([
        sf.base.Property(true)
    ], Axis.prototype, "startFromZero", void 0);
    __decorate$1([
        sf.base.Property(null)
    ], Axis.prototype, "description", void 0);
    __decorate$1([
        sf.base.Property(2)
    ], Axis.prototype, "tabIndex", void 0);
    __decorate$1([
        sf.base.Collection([], StripLineSettings)
    ], Axis.prototype, "stripLines", void 0);
    __decorate$1([
        sf.base.Collection([], MultiLevelLabels)
    ], Axis.prototype, "multiLevelLabels", void 0);
    __decorate$1([
        sf.base.Complex({ color: null, width: 0, type: 'Rectangle' }, LabelBorder)
    ], Axis.prototype, "border", void 0);
    __decorate$1([
        sf.base.Complex({}, ScrollbarSettings)
    ], Axis.prototype, "scrollbarSettings", void 0);
    return Axis;
}(sf.base.ChildProperty));
/** @private */
var VisibleLabels = /** @class */ (function () {
    function VisibleLabels(text, value, labelStyle, originalText, size, breakLabelSize, index) {
        if (size === void 0) { size = new sf.svgbase.Size(0, 0); }
        if (breakLabelSize === void 0) { breakLabelSize = new sf.svgbase.Size(0, 0); }
        if (index === void 0) { index = 1; }
        this.text = text;
        this.originalText = originalText;
        this.value = value;
        this.labelStyle = labelStyle;
        this.size = size;
        this.breakLabelSize = breakLabelSize;
        this.index = 1;
    }
    return VisibleLabels;
}());

var __extends = (undefined && undefined.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
/**
 * Function to sort the dataSource, by default it sort the data in ascending order.
 * @param  {Object} data
 * @param  {string} fields
 * @param  {boolean} isDescending
 * @returns Object
 */

/** @private */
function isBreakLabel(label) {
    return label.indexOf('<br>') !== -1;
}

/** @private */
function rotateTextSize(font, text, angle, chart) {
    var renderer = new sf.svgbase.SvgRenderer(chart.element.id);
    var box;
    var options;
    var htmlObject;
    options = {
        'font-size': font.size,
        'font-style': font.fontStyle,
        'font-family': font.fontFamily,
        'font-weight': font.fontWeight,
        'transform': 'rotate(' + angle + ', 0, 0)',
        'text-anchor': 'middle'
    };
    htmlObject = renderer.createText(options, text);
    if (!chart.delayRedraw && !chart.redraw) {
        chart.element.appendChild(chart.svgObject);
    }
    chart.svgObject.appendChild(htmlObject);
    box = htmlObject.getBoundingClientRect();
    sf.base.remove(htmlObject);
    if (!chart.delayRedraw && !chart.redraw) {
        sf.base.remove(chart.svgObject);
    }
    return new sf.svgbase.Size((box.right - box.left), (box.bottom - box.top));
}
/** @private */

/** @private */
function logBase(value, base) {
    return Math.log(value) / Math.log(base);
}
/** @private */

/** @private */
function inside(value, range) {
    return (value < range.max) && (value > range.min);
}
/** @private */
function withIn(value, range) {
    return (value <= range.max) && (value >= range.min);
}
/** @private */

/** @private */

/** @private */

/** @private */

/** @private */

/** @private */

/** @private */

/** @private */

/** @private */

/**
 * Helper function to determine whether there is an intersection between the two polygons described
 * by the lists of vertices. Uses the Separating Axis Theorem
 *
 * @param a an array of connected points [{x:, y:}, {x:, y:},...] that form a closed polygon
 * @param b an array of connected points [{x:, y:}, {x:, y:},...] that form a closed polygon
 * @return true if there is any intersection between the 2 polygons, false otherwise
 */

/** @private */

/** @private */

/** @private */
function valueToCoefficient(value, axis) {
    var range = axis.visibleRange;
    var result = (value - range.min) / (range.delta);
    return axis.isInversed ? (1 - result) : result;
}
/** @private */

/**
 * method to find series, point index by element id
 * @private
 */

/** @private */

/** @private */

/** @private */

/** @private */

//Within bounds
/** @private */

/** @private */

/** @private */

/** @private */

/** @private */
function firstToLowerCase(str) {
    return str.substr(0, 1).toLowerCase() + str.substr(1);
}
/** @private */

/** @private */
function getMinPointsDelta(axis, seriesCollection) {
    var minDelta = Number.MAX_VALUE;
    var xValues;
    var minVal;
    var seriesMin;
    for (var index = 0; index < seriesCollection.length; index++) {
        var series = seriesCollection[index];
        xValues = [];
        if (series.visible &&
            (axis.name === series.xAxisName || (axis.name === 'primaryXAxis' && series.xAxisName === null)
                || (axis.name === series.chart.primaryXAxis.name && !series.xAxisName))) {
            xValues = series.points.map(function (point, index) {
                return point.xValue;
            });
            xValues.sort(function (first, second) { return first - second; });
            if (xValues.length === 1) {
                seriesMin = (axis.valueType === 'DateTime' && series.xMin === series.xMax) ? (series.xMin - 2592000000) : series.xMin;
                minVal = xValues[0] - (!sf.base.isNullOrUndefined(seriesMin) ?
                    seriesMin : axis.visibleRange.min);
                if (minVal !== 0) {
                    minDelta = Math.min(minDelta, minVal);
                }
            }
            else {
                for (var index_1 = 0; index_1 < xValues.length; index_1++) {
                    var value = xValues[index_1];
                    if (index_1 > 0 && value) {
                        minVal = value - xValues[index_1 - 1];
                        if (minVal !== 0) {
                            minDelta = Math.min(minDelta, minVal);
                        }
                    }
                }
            }
        }
    }
    if (minDelta === Number.MAX_VALUE) {
        minDelta = 1;
    }
    return minDelta;
}
/** @private */

/**
 * Animation Effect Calculation Started Here
 * @param currentTime
 * @param startValue
 * @param endValue
 * @param duration
 * @private
 */

/**
 * Animation Effect Calculation End
 * @private
 */

/**
 * Animate the rect element
 */

/**
 * Animation after legend click a path
 * @param element element to be animated
 * @param direction current direction of the path
 * @param previousDirection previous direction of the path
 */

/**
 * To append the clip rect element
 * @param redraw
 * @param options
 * @param renderer
 * @param clipPath
 */

/**
 * Triggers the event.
 * @return {void}
 * @private
 */
function triggerLabelRender(chart, tempInterval, text, labelStyle, axis) {
    var argsData;
    argsData = {
        cancel: false, name: axisLabelRender, axis: axis,
        text: text, value: tempInterval, labelStyle: labelStyle
    };
    chart.trigger(axisLabelRender, argsData);
    if (!argsData.cancel) {
        var isLineBreakLabels = argsData.text.indexOf('<br>') !== -1;
        var text_1 = (axis.enableTrim) ? (isLineBreakLabels ?
            lineBreakLabelTrim(axis.maximumLabelWidth, argsData.text, axis.labelStyle) :
            textTrim(axis.maximumLabelWidth, argsData.text, axis.labelStyle)) : argsData.text;
        axis.visibleLabels.push(new VisibleLabels(text_1, argsData.value, argsData.labelStyle, argsData.text));
    }
}
/**
 * The function used to find whether the range is set.
 * @return {boolean}
 * @private
 */
function setRange(axis) {
    return (axis.minimum != null && axis.maximum != null);
}
/**
 * Calculate desired interval for the axis.
 * @return {void}
 * @private
 */
function getActualDesiredIntervalsCount(availableSize, axis) {
    var size = axis.orientation === 'Horizontal' ? availableSize.width : availableSize.height;
    if (sf.base.isNullOrUndefined(axis.desiredIntervals)) {
        var desiredIntervalsCount = (axis.orientation === 'Horizontal' ? 0.533 : 1) * axis.maximumLabels;
        desiredIntervalsCount = Math.max((size * (desiredIntervalsCount / 100)), 1);
        return desiredIntervalsCount;
    }
    else {
        return axis.desiredIntervals;
    }
}
/**
 * Animation for template
 * @private
 */

/** @private */

/** @private */
// tslint:disable-next-line:max-func-body-length

/** @private */

/** @private */

/** @private */
function getElement(id) {
    return document.getElementById(id);
}
/** @private */

/** @private */

/** @private */

/** @private */

/** @private */

/** @private */

/** @private */

/**
 * Method to append child element
 * @param parent
 * @param childElement
 * @param isReplace
 */

/** @private */

/** @private */

/** @private */

/** @private */

/** @private */

/** @private */

/** @private */

/** @private */

/** @private */

/** @private */

/** @private */

/** @private */

/** @private */

/** @private */

/** @private */
// tslint:disable-next-line:max-func-body-length

/** @private */
function textTrim(maxWidth, text, font) {
    var label = text;
    var size = sf.svgbase.measureText(text, font).width;
    if (size > maxWidth) {
        var textLength = text.length;
        for (var i = textLength - 1; i >= 0; --i) {
            label = text.substring(0, i) + '...';
            size = sf.svgbase.measureText(label, font).width;
            if (size <= maxWidth) {
                return label;
            }
        }
    }
    return label;
}
/**
 * To trim the line break label
 * @param maxWidth
 * @param text
 * @param font
 */
function lineBreakLabelTrim(maxWidth, text, font) {
    var labelCollection = [];
    var breakLabels = text.split('<br>');
    for (var i = 0; i < breakLabels.length; i++) {
        text = breakLabels[i];
        var size = sf.svgbase.measureText(text, font).width;
        if (size > maxWidth) {
            var textLength = text.length;
            for (var i_1 = textLength - 1; i_1 >= 0; --i_1) {
                text = text.substring(0, i_1) + '...';
                size = sf.svgbase.measureText(text, font).width;
                if (size <= maxWidth) {
                    labelCollection.push(text);
                    break;
                }
            }
        }
        else {
            labelCollection.push(text);
        }
    }
    return labelCollection;
}
/** @private */

/** @private */

/** @private */

/** @private */

/**
 * Method to calculate the width and height of the chart
 */


/**
 * To calculate chart title and height
 * @param title
 * @param style
 * @param width
 */

/**
 * Method to calculate x position of title
 */

/**
 * Method to find new text and element size based on textOverflow
 */
function textWrap(currentLabel, maximumWidth, font) {
    var textCollection = currentLabel.split(' ');
    var label = '';
    var labelCollection = [];
    var text;
    for (var i = 0, len = textCollection.length; i < len; i++) {
        text = textCollection[i];
        if (sf.svgbase.measureText(label.concat(text), font).width < maximumWidth) {
            label = label.concat((label === '' ? '' : ' ') + text);
        }
        else {
            if (label !== '') {
                labelCollection.push(textTrim(maximumWidth, label, font));
                label = text;
            }
            else {
                labelCollection.push(textTrim(maximumWidth, text, font));
                text = '';
            }
        }
        if (label && i === len - 1) {
            labelCollection.push(textTrim(maximumWidth, label, font));
        }
    }
    return labelCollection;
}
/**
 * Method to support the subscript and superscript value to text
 */

/**
 * Method to reset the blazor templates
 */

/** @private */
var RectOption = /** @class */ (function (_super) {
    __extends(RectOption, _super);
    function RectOption(id, fill, border, opacity, rect, rx, ry, transform, dashArray) {
        var _this = _super.call(this, id, fill, border.width, border.color, opacity, dashArray) || this;
        _this.y = rect.y;
        _this.x = rect.x;
        _this.height = rect.height;
        _this.width = rect.width;
        _this.rx = rx ? rx : 0;
        _this.ry = ry ? ry : 0;
        _this.transform = transform ? transform : '';
        _this.stroke = (border.width !== 0 && _this.stroke !== '') ? border.color : 'transparent';
        return _this;
    }
    return RectOption;
}(sf.svgbase.PathOption));
/** @private */
var CircleOption = /** @class */ (function (_super) {
    __extends(CircleOption, _super);
    function CircleOption(id, fill, border, opacity, cx, cy, r) {
        var _this = _super.call(this, id, fill, border.width, border.color, opacity) || this;
        _this.cy = cy;
        _this.cx = cx;
        _this.r = r;
        return _this;
    }
    return CircleOption;
}(sf.svgbase.PathOption));

/**
 * Period selector class
 */
var PeriodSelector = /** @class */ (function () {
    //constructor for period selector
    function PeriodSelector(control) {
        this.control = {};
        this.rootControl = control;
    }
    /**
     * To set the control values
     * @param control
     */
    PeriodSelector.prototype.setControlValues = function (control) {
        if (control.getModuleName() === 'rangeNavigator') {
            this.control.periods = this.rootControl.periodSelectorSettings.periods;
            this.control.seriesXMax = control.chartSeries.xMax;
            this.control.seriesXMin = control.chartSeries.xMin;
            this.control.rangeSlider = control.rangeSlider;
            this.control.rangeNavigatorControl = control;
            this.control.endValue = control.endValue;
            this.control.startValue = control.startValue;
        }
        else {
            this.control.periods = this.rootControl.periods;
            this.control.endValue = this.control.seriesXMax = control.seriesXMax;
            this.control.startValue = this.control.seriesXMin = control.seriesXMin;
            this.control.rangeNavigatorControl = this.rootControl.rangeNavigator;
            if (this.control.rangeNavigatorControl) {
                this.control.rangeSlider = this.rootControl.rangeNavigator.rangeSlider;
            }
        }
        this.control.element = control.element;
        this.control.disableRangeSelector = control.disableRangeSelector;
    };
    /**
     *  To initialize the period selector properties
     */
    PeriodSelector.prototype.appendSelector = function (options, x) {
        if (x === void 0) { x = 0; }
        this.renderSelectorElement(null, options, x);
        this.renderSelector();
    };
    /**
     * renderSelector div
     * @param control
     */
    PeriodSelector.prototype.renderSelectorElement = function (control, options, x) {
        //render border
        this.periodSelectorSize = control ? this.periodSelectorSize : new sf.svgbase.Rect(x, this.rootControl.titleSize.height, options.width, options.height);
        var thumbSize;
        var element;
        if (control) {
            thumbSize = control.themeStyle.thumbWidth;
            element = control.element;
        }
        else {
            thumbSize = options.thumbSize;
            element = options.element;
        }
        if (getElement(element.id + '_Secondary_Element')) {
            sf.base.remove(getElement(element.id + '_Secondary_Element'));
        }
        this.periodSelectorDiv = sf.base.createElement('div', {
            id: element.id + '_Secondary_Element',
            styles: 'width: ' + (this.periodSelectorSize.width - thumbSize) + 'px;height: ' +
                this.periodSelectorSize.height + 'px;top:' +
                this.periodSelectorSize.y + 'px;left:' +
                (this.periodSelectorSize.x + thumbSize / 2) + 'px; position: absolute'
        });
        element.appendChild(this.periodSelectorDiv);
    };
    /**
     * renderSelector elements
     */
    // tslint:disable-next-line:max-func-body-length
    PeriodSelector.prototype.renderSelector = function () {
        var _this = this;
        this.setControlValues(this.rootControl);
        var enableCustom = true;
        var controlId = this.control.element.id;
        var selectorElement = sf.base.createElement('div', { id: controlId + '_selector' });
        this.periodSelectorDiv.appendChild(selectorElement);
        var buttons = this.control.periods;
        var selector = this.updateCustomElement();
        var buttonStyles = 'text-transform: none; text-overflow: unset';
        for (var i = 0; i < buttons.length; i++) {
            selector.push({ align: 'Left', text: buttons[i].text });
        }
        if (this.rootControl.getModuleName() === 'stockChart') {
            enableCustom = this.rootControl.enableCustomRange;
        }
        var selctorArgs;
        if (enableCustom) {
            this.calendarId = controlId + '_calendar';
            selector.push({ template: '<button id=' + this.calendarId + '></button>', align: 'Right' });
        }
        selctorArgs = {
            selector: selector, name: 'RangeSelector', cancel: false, enableCustomFormat: true, content: 'Date Range'
        };
        if (this.rootControl.getModuleName() === 'stockChart') {
            selector.push({ template: sf.base.createElement('button', { id: controlId + '_reset', innerHTML: 'Reset',
                    styles: buttonStyles, className: 'e-dropdown-btn e-btn' }),
                align: 'Right' });
            if (this.rootControl.exportType.indexOf('Print') > -1) {
                selector.push({ template: sf.base.createElement('button', { id: controlId + '_print', innerHTML: 'Print', styles: buttonStyles,
                        className: 'e-dropdown-btn e-btn' }),
                    align: 'Right' });
            }
            if (this.rootControl.exportType.length) {
                selector.push({ template: sf.base.createElement('button', { id: controlId + '_export', innerHTML: 'Export', styles: buttonStyles,
                        className: 'e-dropdown-btn e-btn' }),
                    align: 'Right' });
            }
        }
        this.rootControl.trigger('selectorRender', selctorArgs);
        this.toolbar = new sf.navigations.Toolbar({
            items: selctorArgs.selector, height: this.periodSelectorSize.height,
            clicked: function (args) {
                _this.buttonClick(args, _this.control);
            }, created: function () {
                _this.nodes = _this.toolbar.element.querySelectorAll('.e-toolbar-left')[0];
                if (sf.base.isNullOrUndefined(_this.selectedIndex)) {
                    buttons.map(function (period, index) {
                        if (period.selected) {
                            _this.control.startValue = _this.changedRange(period.intervalType, _this.control.endValue, period.interval).getTime();
                            _this.selectedIndex = (_this.nodes.childNodes.length - buttons.length) + index;
                        }
                    });
                }
                _this.setSelectedStyle(_this.selectedIndex);
            }
        });
        var isStringTemplate = 'isStringTemplate';
        this.toolbar[isStringTemplate] = true;
        this.toolbar.appendTo(selectorElement);
        this.triggerChange = true;
        if (enableCustom) {
            this.datePicker = new sf.calendars.DateRangePicker({
                min: new Date(this.control.seriesXMin),
                max: new Date(this.control.seriesXMax),
                format: 'dd\'\/\'MM\'\/\'yyyy', placeholder: 'Select a range',
                showClearButton: false, startDate: new Date(this.control.startValue),
                endDate: new Date(this.control.endValue),
                created: function (args) {
                    if (selctorArgs.enableCustomFormat) {
                        var datePickerElement = document.getElementsByClassName('e-date-range-wrapper')[0];
                        datePickerElement.style.display = 'none';
                        datePickerElement.insertAdjacentElement('afterend', sf.base.createElement('div', {
                            id: 'customRange',
                            innerHTML: selctorArgs.content, className: 'e-btn e-dropdown-btn',
                            styles: 'font-family: "Segoe UI"; font-size: 14px; font-weight: 500; text-transform: none '
                        }));
                        getElement('customRange').insertAdjacentElement('afterbegin', (sf.base.createElement('span', {
                            id: 'dateIcon', className: 'e-input-group-icon e-range-icon e-btn-icon e-icons',
                            styles: 'font-size: 16px; min-height: 0px; margin: -3px 0 0 0; outline: none; min-width: 30px'
                            // fix for date range icon alignment issue.
                        })));
                        document.getElementById('customRange').onclick = function () {
                            _this.datePicker.show(getElement('customRange'));
                        };
                    }
                },
                change: function (args) {
                    if (_this.triggerChange) {
                        if (_this.control.rangeSlider && args.event) {
                            _this.control.rangeSlider.performAnimation(args.startDate.getTime(), args.endDate.getTime(), _this.control.rangeNavigatorControl);
                        }
                        else if (args.event) {
                            _this.rootControl.rangeChanged(args.startDate.getTime(), args.endDate.getTime());
                        }
                        _this.nodes = _this.toolbar.element.querySelectorAll('.e-toolbar-left')[0];
                        if (!_this.rootControl.resizeTo && _this.control.rangeSlider && _this.control.rangeSlider.isDrag) {
                            /**
                             * Issue: While disabling range navigator console error throws
                             * Fix:Check with rangeSlider present or not. Then checked with isDrag.
                             */
                            for (var i = 0, length_1 = _this.nodes.childNodes.length; i < length_1; i++) {
                                _this.nodes.childNodes[i].childNodes[0].classList.remove('e-active');
                                _this.nodes.childNodes[i].childNodes[0].classList.remove('e-active');
                            }
                        }
                    }
                }
            });
            this.datePicker.appendTo('#' + this.calendarId);
        }
    };
    PeriodSelector.prototype.updateCustomElement = function () {
        var selector = [];
        var controlId = this.rootControl.element.id;
        var buttonStyles = 'text-transform: none; text-overflow: unset';
        if (this.rootControl.getModuleName() === 'stockChart') {
            if (this.rootControl.seriesType.length) {
                selector.push({ template: sf.base.createElement('button', { id: controlId + '_seriesType', innerHTML: 'Series',
                        styles: buttonStyles }),
                    align: 'Left' });
            }
            if (this.rootControl.indicatorType.length) {
                selector.push({ template: sf.base.createElement('button', { id: controlId + '_indicatorType', innerHTML: 'Indicators',
                        styles: buttonStyles }),
                    align: 'Left' });
            }
            if (this.rootControl.trendlineType.length) {
                selector.push({ template: sf.base.createElement('button', { id: controlId + '_trendType', innerHTML: 'Trendline',
                        styles: buttonStyles }),
                    align: 'Left' });
            }
        }
        return selector;
    };
    /**
     * To set and deselect the acrive style
     * @param buttons
     */
    PeriodSelector.prototype.setSelectedStyle = function (selectedIndex) {
        if (this.control.disableRangeSelector || this.rootControl.getModuleName() === 'stockChart') {
            for (var i = 0, length_2 = this.nodes.childNodes.length; i < length_2; i++) {
                this.nodes.childNodes[i].childNodes[0].classList.remove('e-active');
                this.nodes.childNodes[i].childNodes[0].classList.remove('e-active');
            }
            this.nodes.childNodes[selectedIndex].childNodes[0].classList.add('e-flat');
            this.nodes.childNodes[selectedIndex].childNodes[0].classList.add('e-active');
        }
    };
    /**
     * Button click handling
     */
    PeriodSelector.prototype.buttonClick = function (args, control) {
        var _this = this;
        var toolBarItems = this.toolbar.items;
        var clickedEle = args.item;
        var slider = this.control.rangeSlider;
        var updatedStart;
        var updatedEnd;
        var buttons = this.control.periods;
        var button = buttons.filter(function (btn) { return (btn.text === clickedEle.text); });
        buttons.map(function (period, index) {
            if (period.text === args.item.text) {
                _this.selectedIndex = (_this.nodes.childNodes.length - buttons.length) + index;
            }
        });
        if (args.item.text !== '') {
            this.setSelectedStyle(this.selectedIndex);
        }
        if (clickedEle.text.toLowerCase() === 'all') {
            updatedStart = control.seriesXMin;
            updatedEnd = control.seriesXMax;
            if (slider) {
                slider.performAnimation(updatedStart, updatedEnd, this.control.rangeNavigatorControl);
            }
            else {
                this.rootControl.rangeChanged(updatedStart, updatedEnd);
            }
        }
        else if (clickedEle.text.toLowerCase() === 'ytd') {
            if (slider) {
                updatedStart = new Date(new Date(slider.currentEnd).getFullYear().toString()).getTime();
                updatedEnd = slider.currentEnd;
                slider.performAnimation(updatedStart, updatedEnd, this.control.rangeNavigatorControl);
            }
            else {
                updatedStart = new Date(new Date(this.rootControl.currentEnd).getFullYear().toString()).getTime();
                updatedEnd = this.rootControl.currentEnd;
                this.rootControl.rangeChanged(updatedStart, updatedEnd);
            }
        }
        else if (clickedEle.text.toLowerCase() !== '') {
            if (slider) {
                updatedStart = this.changedRange(button[0].intervalType, slider.currentEnd, button[0].interval).getTime();
                updatedEnd = slider.currentEnd;
                slider.performAnimation(updatedStart, updatedEnd, this.control.rangeNavigatorControl);
            }
            else {
                updatedStart = this.changedRange(button[0].intervalType, this.rootControl.currentEnd, button[0].interval).getTime();
                updatedEnd = this.rootControl.currentEnd;
                this.rootControl.rangeChanged(updatedStart, updatedEnd);
            }
        }
        if (this.rootControl.getModuleName() === 'stockChart') {
            this.rootControl.zoomChange = false;
        }
        if (getElement(this.calendarId + '_popup') && !sf.base.Browser.isDevice) {
            var element = getElement(this.calendarId + '_popup');
            element.querySelectorAll('.e-range-header')[0].style.display = 'none';
        }
    };
    /**
     *
     * @param type updatedRange for selector
     * @param end
     * @param interval
     */
    PeriodSelector.prototype.changedRange = function (type, end, interval) {
        var result = new Date(end);
        switch (type) {
            case 'Quarter':
                result.setMonth(result.getMonth() - (3 * interval));
                break;
            case 'Months':
                result.setMonth(result.getMonth() - interval);
                break;
            case 'Weeks':
                result.setDate(result.getDate() - (interval * 7));
                break;
            case 'Days':
                result.setDate(result.getDate() - interval);
                break;
            case 'Hours':
                result.setHours(result.getHours() - interval);
                break;
            case 'Minutes':
                result.setMinutes(result.getMinutes() - interval);
                break;
            case 'Seconds':
                result.setSeconds(result.getSeconds() - interval);
                break;
            default:
                result.setFullYear(result.getFullYear() - interval);
                break;
        }
        return result;
    };
    
    /**
     * Get module name
     */
    PeriodSelector.prototype.getModuleName = function () {
        return 'PeriodSelector';
    };
    /**
     * To destroy the period selector.
     * @return {void}
     * @private
     */
    PeriodSelector.prototype.destroy = function () {
        /**
         * destroy method
         */
    };
    return PeriodSelector;
}());

/**
 * Chart and accumulation common files
 */

exports.Common = Common;
exports.loaded = loaded;
exports.legendClick = legendClick;
exports.load = load;
exports.animationComplete = animationComplete;
exports.legendRender = legendRender;
exports.textRender = textRender;
exports.pointRender = pointRender;
exports.seriesRender = seriesRender;
exports.axisLabelRender = axisLabelRender;
exports.axisRangeCalculated = axisRangeCalculated;
exports.axisMultiLabelRender = axisMultiLabelRender;
exports.tooltipRender = tooltipRender;
exports.sharedTooltipRender = sharedTooltipRender;
exports.chartMouseMove = chartMouseMove;
exports.chartMouseClick = chartMouseClick;
exports.pointClick = pointClick;
exports.pointDoubleClick = pointDoubleClick;
exports.pointMove = pointMove;
exports.chartMouseLeave = chartMouseLeave;
exports.chartMouseDown = chartMouseDown;
exports.chartMouseUp = chartMouseUp;
exports.zoomComplete = zoomComplete;
exports.dragComplete = dragComplete;
exports.selectionComplete = selectionComplete;
exports.resized = resized;
exports.beforePrint = beforePrint;
exports.annotationRender = annotationRender;
exports.scrollStart = scrollStart;
exports.scrollEnd = scrollEnd;
exports.scrollChanged = scrollChanged;
exports.stockEventRender = stockEventRender;
exports.multiLevelLabelClick = multiLevelLabelClick;
exports.dragStart = dragStart;
exports.drag = drag;
exports.dragEnd = dragEnd;
exports.regSub = regSub;
exports.regSup = regSup;
exports.beforeExport = beforeExport;
exports.afterExport = afterExport;
exports.bulletChartMouseClick = bulletChartMouseClick;
exports.onZooming = onZooming;
exports.getSeriesColor = getSeriesColor;
exports.getThemeColor = getThemeColor;
exports.getScrollbarThemeColor = getScrollbarThemeColor;
exports.PeriodSelector = PeriodSelector;

return exports;

});
sfBlazor.modules["chartsbase"] = "charts.Common";
window.sf.charts = window.sf.base.extend({}, window.sf.charts, chartsbase({}));
