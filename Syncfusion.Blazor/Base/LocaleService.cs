using System;
using System.Linq;
using Newtonsoft.Json;
using System.Globalization;
using System.Collections.Generic;

namespace Syncfusion.Blazor.Internal
{
    /// <summary>
    /// Implements the Syncfusion Localizer for injecting its service.
    /// </summary>
    internal class SyncfusionStringLocalizer : ISyncfusionStringLocalizer
    {
        /// <summary>
        /// Gets the <see cref="System.Resources.ResourceManager" /> for localization.
        /// </summary>
        public System.Resources.ResourceManager ResourceManager
        {
            get
            {
                return null;
            }
        }

        /// <summary>
        /// Get localized text from resource file.
        /// </summary>
        /// <param name="key">Property key to return localized value.</param>
        /// <returns>Locale text.</returns>
        public string GetText(string key)
        {
            return ResourceManager?.GetString(key, CultureInfo.CurrentCulture);
        }
    }

    /// <summary>
    /// Maintains the Localizer details for performing Localization.
    /// </summary>
    internal class LocalizerDetails : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocalizerDetails"/> class.
        /// </summary>
        /// <param name="resourceManager">Resource manager instance.</param>
        /// <param name="culture">culture information.</param>
        /// <param name="service">Syncfusion Blazor service.</param>
        /// <param name="keys">Locale key values.</param>
        internal LocalizerDetails(System.Resources.ResourceManager resourceManager, CultureInfo culture, SyncfusionBlazorService service, List<string> keys)
        {
            Manager = resourceManager;
            Culture = culture;
            SyncfusionService = service;
            LocaleKeys = keys;
        }

        /// <summary>
        /// Gets or sets culture.
        /// </summary>
        protected CultureInfo Culture { get; set; }

        /// <summary>
        /// Gets or sets locale keys.
        /// </summary>
        protected List<string> LocaleKeys { get; set; }

        /// <summary>
        /// Gets or sets Syncfusion service.
        /// </summary>
        protected SyncfusionBlazorService SyncfusionService { get; set; }

        /// <summary>
        /// Gets or sets resource manager.
        /// </summary>
        protected System.Resources.ResourceManager Manager { get; set; }

        /// <summary>
        /// Returns the locale text for blazor components from the resource file.
        /// </summary>
        /// <returns>Locale text.</returns>
        internal string GetLocaleText()
        {
            var locale = string.Empty;
            if (LocaleKeys == null)
            {
                return locale;
            }

            var cultureName = Intl.CurrentCulture.Name;
            if (!SyncfusionService.LoadedLocale.ContainsKey(cultureName))
            {
                SyncfusionService.LoadedLocale[cultureName] = new List<string>();
            }

            var componentLocale = new Dictionary<string, Dictionary<string, string>>();
            foreach (var key in LocaleKeys)
            {
                if (SyncfusionService.LoadedLocale.ContainsKey(cultureName) && SyncfusionService.LoadedLocale[cultureName].Contains(key))
                {
                    continue;
                }
                else
                {
                    SyncfusionService.LoadedLocale[cultureName].Add(key);
                }

                GetMappingLocale(key, componentLocale);
            }

            if (componentLocale.Count > 0)
            {
                locale = "{\"" + Culture + "\": " + JsonConvert.SerializeObject(componentLocale) + " }";
            }

            componentLocale = null;
            return locale;
        }

        /// <summary>
        /// Returns the locale text from the ResourceManager.
        /// </summary>
        /// <param name="localeKey">locale key to retrieve the locale value.</param>
        /// <returns>Locale content.</returns>
        internal string GetString(string localeKey)
        {
            return Manager?.GetString(localeKey, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Updates the components locale dictionary.
        /// </summary>
        /// <param name="key">Component name.</param>
        /// <param name="componentLocale">List of components locale collections in dictionary.</param>
#pragma warning disable CA1502
        protected void GetMappingLocale(string key, Dictionary<string, Dictionary<string, string>> componentLocale)
#pragma warning disable CA1502
        {
            var localeProperties = new Dictionary<string, string>();
            switch (key)
            {
                case "pivotview":
                    localeProperties.Add("grandTotal", GetString("PivotView_GrandTotal"));
                    localeProperties.Add("total", GetString("PivotView_Total"));
                    localeProperties.Add("value", GetString("PivotView_Value"));
                    localeProperties.Add("noValue", GetString("PivotView_NoValue"));
                    localeProperties.Add("row", GetString("PivotView_Row"));
                    localeProperties.Add("column", GetString("PivotView_Column"));
                    localeProperties.Add("collapse", GetString("PivotView_Collapse"));
                    localeProperties.Add("expand", GetString("PivotView_Expand"));
                    localeProperties.Add("rowAxisPrompt", GetString("PivotView_RowAxisWatermark"));
                    localeProperties.Add("columnAxisPrompt", GetString("PivotView_ColumnAxisWatermark"));
                    localeProperties.Add("valueAxisPrompt", GetString("PivotView_ValueAxisWatermark"));
                    localeProperties.Add("filterAxisPrompt", GetString("PivotView_FilterAxisWatermark"));
                    localeProperties.Add("filter", GetString("PivotView_Filter"));
                    localeProperties.Add("filtered", GetString("PivotView_Filtered"));
                    localeProperties.Add("sort", GetString("PivotView_Sort"));
                    localeProperties.Add("filters", GetString("PivotView_Filters"));
                    localeProperties.Add("rows", GetString("PivotView_Rows"));
                    localeProperties.Add("columns", GetString("PivotView_Columns"));
                    localeProperties.Add("values", GetString("PivotView_Values"));
                    localeProperties.Add("close", GetString("PivotView_Close"));
                    localeProperties.Add("cancel", GetString("PivotView_Cancel"));
                    localeProperties.Add("delete", GetString("PivotView_Delete"));
                    localeProperties.Add("CalculatedField", GetString("PivotView_CalculatedField"));
                    localeProperties.Add("createCalculatedField", GetString("PivotView_CreateCalculatedField"));
                    localeProperties.Add("fieldName", GetString("PivotView_CalculatedField_NameWatermark"));
                    localeProperties.Add("error", GetString("PivotView_Error"));
                    localeProperties.Add("invalidFormula", GetString("PivotView_InvalidFormula"));
                    localeProperties.Add("dropText", GetString("PivotView_CalculatedField_ExampleWatermark"));
                    localeProperties.Add("dropTextMobile", GetString("PivotView_CalculatedField_MobileWatermark"));
                    localeProperties.Add("dropAction", GetString("PivotView_CalculatedField_DropMessage"));
                    localeProperties.Add("alert", GetString("PivotView_Alert"));
                    localeProperties.Add("warning", GetString("PivotView_Warning"));
                    localeProperties.Add("ok", GetString("PivotView_OK"));
                    localeProperties.Add("search", GetString("PivotView_Search"));
                    localeProperties.Add("drag", GetString("PivotView_Drag"));
                    localeProperties.Add("remove", GetString("PivotView_Remove"));
                    localeProperties.Add("allFields", GetString("PivotView_AllFields"));
                    localeProperties.Add("formula", GetString("PivotView_Formula"));
                    localeProperties.Add("addToRow", GetString("PivotView_AddToRow"));
                    localeProperties.Add("addToColumn", GetString("PivotView_AddToColumn"));
                    localeProperties.Add("addToValue", GetString("PivotView_AddToValue"));
                    localeProperties.Add("addToFilter", GetString("PivotView_AddToFilter"));
                    localeProperties.Add("emptyData", GetString("PivotView_EmptyRecordsMessage"));
                    localeProperties.Add("fieldExist", GetString("PivotView_CalculatedField_ExistMessage"));
                    localeProperties.Add("confirmText", GetString("PivotView_CalculatedField_ConfirmMessage"));
                    localeProperties.Add("noMatches", GetString("PivotView_NoMatchesMessage"));
                    localeProperties.Add("format", GetString("PivotView_Summaries"));
                    localeProperties.Add("edit", GetString("PivotView_Edit"));
                    localeProperties.Add("clear", GetString("PivotView_Clear"));
                    localeProperties.Add("formulaField", GetString("PivotView_CalculatedField_DragDropMessage"));
                    localeProperties.Add("dragField", GetString("PivotView_CalculatedField_DragMessage"));
                    localeProperties.Add("clearFilter", GetString("PivotView_ClearFilter"));
                    localeProperties.Add("by", GetString("PivotView_By"));
                    localeProperties.Add("all", GetString("PivotView_All"));
                    localeProperties.Add("multipleItems", GetString("PivotView_MultipleItems"));
                    localeProperties.Add("member", GetString("PivotView_Member"));
                    localeProperties.Add("label", GetString("PivotView_Label"));
                    localeProperties.Add("date", GetString("PivotView_Date"));
                    localeProperties.Add("enterValue", GetString("PivotView_EnterValue"));
                    localeProperties.Add("chooseDate", GetString("PivotView_EnterDate"));
                    localeProperties.Add("Before", GetString("PivotView_Before"));
                    localeProperties.Add("BeforeOrEqualTo", GetString("PivotView_BeforeOrEqualTo"));
                    localeProperties.Add("After", GetString("PivotView_After"));
                    localeProperties.Add("AfterOrEqualTo", GetString("PivotView_AfterOrEqualTo"));
                    localeProperties.Add("labelTextContent", GetString("PivotView_LabelTextMessage"));
                    localeProperties.Add("dateTextContent", GetString("PivotView_DateTextMessage"));
                    localeProperties.Add("valueTextContent", GetString("PivotView_ValueTextMessage"));
                    localeProperties.Add("Equals", GetString("PivotView_Equals"));
                    localeProperties.Add("DoesNotEquals", GetString("PivotView_DoesNotEquals"));
                    localeProperties.Add("BeginWith", GetString("PivotView_BeginWith"));
                    localeProperties.Add("DoesNotBeginWith", GetString("PivotView_DoesNotBeginWith"));
                    localeProperties.Add("EndsWith", GetString("PivotView_EndsWith"));
                    localeProperties.Add("DoesNotEndsWith", GetString("PivotView_DoesNotEndsWith"));
                    localeProperties.Add("Contains", GetString("PivotView_Contains"));
                    localeProperties.Add("DoesNotContains", GetString("PivotView_DoesNotContains"));
                    localeProperties.Add("GreaterThan", GetString("PivotView_GreaterThan"));
                    localeProperties.Add("GreaterThanOrEqualTo", GetString("PivotView_GreaterThanOrEqualTo"));
                    localeProperties.Add("LessThan", GetString("PivotView_LessThan"));
                    localeProperties.Add("LessThanOrEqualTo", GetString("PivotView_LessThanOrEqualTo"));
                    localeProperties.Add("Between", GetString("PivotView_Between"));
                    localeProperties.Add("NotBetween", GetString("PivotView_NotBetween"));
                    localeProperties.Add("And", GetString("PivotView_And"));
                    localeProperties.Add("Sum", GetString("PivotView_Sum"));
                    localeProperties.Add("Count", GetString("PivotView_Count"));
                    localeProperties.Add("DistinctCount", GetString("PivotView_DistinctCount"));
                    localeProperties.Add("Product", GetString("PivotView_Product"));
                    localeProperties.Add("Avg", GetString("PivotView_Avg"));
                    localeProperties.Add("Min", GetString("PivotView_Min"));
                    localeProperties.Add("SampleVar", GetString("PivotView_SampleVar"));
                    localeProperties.Add("PopulationVar", GetString("PivotView_PopulationVar"));
                    localeProperties.Add("RunningTotals", GetString("PivotView_RunningTotals"));
                    localeProperties.Add("Max", GetString("PivotView_Max"));
                    localeProperties.Add("Index", GetString("PivotView_Index"));
                    localeProperties.Add("SampleStDev", GetString("PivotView_SampleStDev"));
                    localeProperties.Add("PopulationStDev", GetString("PivotView_PopulationStDev"));
                    localeProperties.Add("PercentageOfRowTotal", GetString("PivotView_PercentageOfRowTotal"));
                    localeProperties.Add("PercentageOfParentTotal", GetString("PivotView_PercentageOfParentTotal"));
                    localeProperties.Add("PercentageOfParentColumnTotal", GetString("PivotView_PercentageOfParentColumnTotal"));
                    localeProperties.Add("PercentageOfParentRowTotal", GetString("PivotView_PercentageOfParentRowTotal"));
                    localeProperties.Add("DifferenceFrom", GetString("PivotView_DifferenceFrom"));
                    localeProperties.Add("PercentageOfDifferenceFrom", GetString("PivotView_PercentageOfDifferenceFrom"));
                    localeProperties.Add("PercentageOfGrandTotal", GetString("PivotView_PercentageOfGrandTotal"));
                    localeProperties.Add("PercentageOfColumnTotal", GetString("PivotView_PercentageOfColumnTotal"));
                    localeProperties.Add("NotEquals", GetString("PivotView_NotEquals"));
                    localeProperties.Add("AllValues", GetString("PivotView_AllValues"));
                    localeProperties.Add("conditionalFormating", GetString("PivotView_ConditionalFormating"));
                    localeProperties.Add("apply", GetString("PivotView_Apply"));
                    localeProperties.Add("condition", GetString("PivotView_AddCondition"));
                    localeProperties.Add("formatLabel", GetString("PivotView_Format"));
                    localeProperties.Add("valueFieldSettings", GetString("PivotView_ValueFieldSettings"));
                    localeProperties.Add("baseField", GetString("PivotView_BaseField"));
                    localeProperties.Add("baseItem", GetString("PivotView_BaseItem"));
                    localeProperties.Add("summarizeValuesBy", GetString("PivotView_SummarizeValuesBy"));
                    localeProperties.Add("sourceName", GetString("PivotView_FieldNameMessage"));
                    localeProperties.Add("sourceCaption", GetString("PivotView_FieldCaptionMessage"));
                    localeProperties.Add("example", GetString("PivotView_Example"));
                    localeProperties.Add("editorDataLimitMsg", GetString("PivotView_MemberLimitMessage"));
                    localeProperties.Add("details", GetString("PivotView_Details"));
                    localeProperties.Add("manageRecords", GetString("PivotView_ManageRecords"));
                    localeProperties.Add("Years", GetString("PivotView_Years"));
                    localeProperties.Add("Quarters", GetString("PivotView_Quarters"));
                    localeProperties.Add("Months", GetString("PivotView_Months"));
                    localeProperties.Add("Days", GetString("PivotView_Days"));
                    localeProperties.Add("Hours", GetString("PivotView_Hours"));
                    localeProperties.Add("Minutes", GetString("PivotView_Minutes"));
                    localeProperties.Add("Seconds", GetString("PivotView_Seconds"));
                    localeProperties.Add("save", GetString("PivotView_SaveReport"));
                    localeProperties.Add("new", GetString("PivotView_NewReport"));
                    localeProperties.Add("load", GetString("PivotView_LoadReport"));
                    localeProperties.Add("saveAs", GetString("PivotView_SaveAsReport"));
                    localeProperties.Add("rename", GetString("PivotView_RenameReport"));
                    localeProperties.Add("deleteReport", GetString("PivotView_DeleteReport"));
                    localeProperties.Add("export", GetString("PivotView_Export"));
                    localeProperties.Add("subTotals", GetString("PivotView_Subtotals"));
                    localeProperties.Add("grandTotals", GetString("PivotView_GrandTotals"));
                    localeProperties.Add("reportName", GetString("PivotView_ReportNameMessage"));
                    localeProperties.Add("pdf", GetString("PivotView_PDF"));
                    localeProperties.Add("excel", GetString("PivotView_Excel"));
                    localeProperties.Add("csv", GetString("PivotView_CSV"));
                    localeProperties.Add("png", GetString("PivotView_PNG"));
                    localeProperties.Add("jpeg", GetString("PivotView_JPEG"));
                    localeProperties.Add("svg", GetString("PivotView_SVG"));
                    localeProperties.Add("mdxQuery", GetString("PivotView_MdxQuery"));
                    localeProperties.Add("showSubTotals", GetString("PivotView_ShowSubtotals"));
                    localeProperties.Add("doNotShowSubTotals", GetString("PivotView_DoNotShowSubtotals"));
                    localeProperties.Add("showSubTotalsRowsOnly", GetString("PivotView_ShowRowSubtotalsOnly"));
                    localeProperties.Add("showSubTotalsColumnsOnly", GetString("PivotView_ShowColumnSubtotalsOnly"));
                    localeProperties.Add("showGrandTotals", GetString("PivotView_ShowGrandTotals"));
                    localeProperties.Add("doNotShowGrandTotals", GetString("PivotView_DoNotShowGrandTotals"));
                    localeProperties.Add("showGrandTotalsRowsOnly", GetString("PivotView_ShowRowGrandTotalsOnly"));
                    localeProperties.Add("showGrandTotalsColumnsOnly", GetString("PivotView_ShowColumnGrandTotalsOnly"));
                    localeProperties.Add("fieldList", GetString("PivotView_ShowFieldList"));
                    localeProperties.Add("grid", GetString("PivotView_ShowTable"));
                    localeProperties.Add("toolbarFormatting", GetString("PivotView_ConditionalFormatting"));
                    localeProperties.Add("chart", GetString("PivotView_Chart"));
                    localeProperties.Add("reportMsg", GetString("PivotView_ValidReportNameMessage"));
                    localeProperties.Add("reportList", GetString("PivotView_ReportList"));
                    localeProperties.Add("removeConfirm", GetString("PivotView_RemoveReportConfirmMessage"));
                    localeProperties.Add("emptyReport", GetString("PivotView_EmptyReportMessage"));
                    localeProperties.Add("bar", GetString("PivotView_Bar"));
                    localeProperties.Add("line", GetString("PivotView_Line"));
                    localeProperties.Add("area", GetString("PivotView_Area"));
                    localeProperties.Add("scatter", GetString("PivotView_Scatter"));
                    localeProperties.Add("polar", GetString("PivotView_Polar"));
                    localeProperties.Add("of", GetString("PivotView_Of"));
                    localeProperties.Add("emptyFormat", GetString("PivotView_NoFormatMessage"));
                    localeProperties.Add("emptyInput", GetString("PivotView_NoInputMessage"));
                    localeProperties.Add("newReportConfirm", GetString("PivotView_NewReportConfirmMessage"));
                    localeProperties.Add("emptyReportName", GetString("PivotView_EnterReportNameMessage"));
                    localeProperties.Add("qtr", GetString("PivotView_Quarter"));
                    localeProperties.Add("null", GetString("PivotView_Null"));
                    localeProperties.Add("undefined", GetString("PivotView_Undefined"));
                    localeProperties.Add("groupOutOfRange", GetString("PivotView_OutOfRange"));
                    localeProperties.Add("fieldDropErrorAction", GetString("PivotView_FieldDropErrorMessage"));
                    localeProperties.Add("MoreOption", GetString("PivotView_MoreOption"));
                    localeProperties.Add("aggregate", GetString("PivotView_Aggregate"));
                    localeProperties.Add("drillThrough", GetString("PivotView_DrillThrough"));
                    localeProperties.Add("ascending", GetString("PivotView_Ascending"));
                    localeProperties.Add("descending", GetString("PivotView_Descending"));
                    localeProperties.Add("number", GetString("PivotView_Number"));
                    localeProperties.Add("currency", GetString("PivotView_Currency"));
                    localeProperties.Add("percentage", GetString("PivotView_Percentage"));
                    localeProperties.Add("formatType", GetString("PivotView_FormatType"));
                    localeProperties.Add("customText", GetString("PivotView_CurrencySymbol"));
                    localeProperties.Add("symbolPosition", GetString("PivotView_SymbolPosition"));
                    localeProperties.Add("left", GetString("PivotView_Left"));
                    localeProperties.Add("right", GetString("PivotView_Right"));
                    localeProperties.Add("grouping", GetString("PivotView_Grouping"));
                    localeProperties.Add("true", GetString("PivotView_True"));
                    localeProperties.Add("false", GetString("PivotView_False"));
                    localeProperties.Add("decimalPlaces", GetString("PivotView_DecimalPlaces"));
                    localeProperties.Add("numberFormat", GetString("PivotView_NumberFormatting"));
                    localeProperties.Add("memberType", GetString("PivotView_FieldType"));
                    localeProperties.Add("formatString", GetString("PivotView_FormatString"));
                    localeProperties.Add("expressionField", GetString("PivotView_Expression"));
                    localeProperties.Add("customFormat", GetString("PivotView_CustomFormatMessage"));
                    localeProperties.Add("selectedHierarchy", GetString("PivotView_ParentHierarchy"));
                    localeProperties.Add("olapDropText", GetString("PivotView_CalculatedField_OLAPExampleWatermark"));
                    localeProperties.Add("Percent", GetString("PivotView_Percent"));
                    localeProperties.Add("Custom", GetString("PivotView_Custom"));
                    localeProperties.Add("Measure", GetString("PivotView_Measure"));
                    localeProperties.Add("Dimension", GetString("PivotView_Dimension"));
                    localeProperties.Add("Standard", GetString("PivotView_Standard"));
                    localeProperties.Add("blank", GetString("sfBlazorLocale[PivotView_Blank"));
                    localeProperties.Add("fieldTooltip", GetString("sfBlazorLocale[PivotView_CalculatedField_Tooltip"));
                    localeProperties.Add("QuarterYear", GetString("PivotView_QuarterYear"));
                    localeProperties.Add("fieldTitle", GetString("PivotView_FieldName"));
                    localeProperties.Add("drillError", GetString("PivotView_DrillThroughErrorMessage"));
                    localeProperties.Add("caption", GetString("PivotView_FieldCaption"));
                    localeProperties.Add("copy", GetString("PivotView_Copy"));
                    localeProperties.Add("defaultReport", GetString("PivotView_SampleReport"));
                    localeProperties.Add("customFormatString", GetString("PivotView_CustomFormat"));
                    localeProperties.Add("invalidFormat", GetString("PivotView_InvalidFormat"));
                    localeProperties.Add("group", GetString("PivotView_Group"));
                    localeProperties.Add("unGroup", GetString("PivotView_Ungroup"));
                    localeProperties.Add("invalidSelection", GetString("PivotView_InvalidGroupSelectionMessage"));
                    localeProperties.Add("groupName", GetString("PivotView_GroupCaptionMessage"));
                    localeProperties.Add("captionName", GetString("PivotView_GroupFieldCaptionMessage"));
                    localeProperties.Add("selectedItems", GetString("PivotView_SelectedItems"));
                    localeProperties.Add("groupFieldCaption", GetString("PivotView_GroupFieldCaption"));
                    localeProperties.Add("groupTitle", GetString("PivotView_GroupName"));
                    localeProperties.Add("startAt", GetString("PivotView_StartAt"));
                    localeProperties.Add("endAt", GetString("PivotView_EndAt"));
                    localeProperties.Add("groupBy", GetString("PivotView_IntervalBy"));
                    localeProperties.Add("selectGroup", GetString("PivotView_SelectGroups"));
                    localeProperties.Add("multipleAxes", GetString("PivotView_MultipleAxes"));
                    localeProperties.Add("chartTypeSettings", GetString("PivotView_ChartTypeSettings"));
                    localeProperties.Add("ChartType", GetString("PivotView_ChartType"));
                    localeProperties.Add("yes", GetString("PivotView_Yes"));
                    localeProperties.Add("no", GetString("PivotView_No"));
                    localeProperties.Add("numberFormatMenu", GetString("PivotView_NumberFormatMenu"));
                    localeProperties.Add("conditionalFormatingMenu", GetString("PivotView_ConditionalFormatingMenu"));
                    localeProperties.Add("removeCalculatedField", GetString("PivotView_CalculatedField_RemoveMessage"));
                    localeProperties.Add("stackingarea", GetString("PivotView_StackedArea"));
                    localeProperties.Add("stackingcolumn", GetString("PivotView_StackedColumn"));
                    localeProperties.Add("stackingbar", GetString("PivotView_StackedBar"));
                    localeProperties.Add("stepline", GetString("PivotView_StepLine"));
                    localeProperties.Add("steparea", GetString("PivotView_StepArea"));
                    localeProperties.Add("splinearea", GetString("PivotView_SplineArea"));
                    localeProperties.Add("spline", GetString("PivotView_Spline"));
                    localeProperties.Add("stackingcolumn100", GetString("PivotView_StackedColumn100"));
                    localeProperties.Add("stackingbar100", GetString("PivotView_StackedBar100"));
                    localeProperties.Add("stackingarea100", GetString("PivotView_StackedArea100"));
                    localeProperties.Add("bubble", GetString("PivotView_Bubble"));
                    localeProperties.Add("pareto", GetString("PivotView_Pareto"));
                    localeProperties.Add("radar", GetString("PivotView_Radar"));
                    localeProperties.Add("editCalculatedField", GetString("PivotView_CalculatedField_EditTooltipMessage"));
                    localeProperties.Add("clearCalculatedField", GetString("PivotView_CalculatedField_ClearTooltipMessage"));
                    break;
                case "pivotfieldlist":
                    localeProperties.Add("staticFieldList", GetString("PivotFieldList_StaticFieldList"));
                    localeProperties.Add("fieldList", GetString("PivotFieldList_FieldList"));
                    localeProperties.Add("dropFilterPrompt", GetString("PivotFieldList_FilterAxisWatermark"));
                    localeProperties.Add("dropColPrompt", GetString("PivotFieldList_ColumnAxisWatermark"));
                    localeProperties.Add("dropRowPrompt", GetString("PivotFieldList_RowAxisWatermark"));
                    localeProperties.Add("dropValPrompt", GetString("PivotFieldList_ValueAxisWatermark"));
                    localeProperties.Add("Add field here", GetString("PivotFieldList_AddFieldMessage"));
                    localeProperties.Add("Choose field", GetString("PivotFieldList_ChooseFieldMessage"));
                    localeProperties.Add("centerHeader", GetString("PivotFieldList_DragFieldsMessage"));
                    localeProperties.Add("add", GetString("PivotFieldList_Add"));
                    localeProperties.Add("drag", GetString("PivotFieldList_Drag"));
                    localeProperties.Add("filter", GetString("PivotFieldList_Filter"));
                    localeProperties.Add("filtered", GetString("PivotFieldList_Filtered"));
                    localeProperties.Add("sort", GetString("PivotFieldList_Sort"));
                    localeProperties.Add("remove", GetString("PivotFieldList_Remove"));
                    localeProperties.Add("filters", GetString("PivotFieldList_Filters"));
                    localeProperties.Add("rows", GetString("PivotFieldList_Rows"));
                    localeProperties.Add("columns", GetString("PivotFieldList_Columns"));
                    localeProperties.Add("values", GetString("PivotFieldList_Values"));
                    localeProperties.Add("CalculatedField", GetString("PivotFieldList_CalculatedField"));
                    localeProperties.Add("createCalculatedField", GetString("PivotFieldList_CreateCalculatedField"));
                    localeProperties.Add("fieldName", GetString("PivotFieldList_CalculatedField_NameWatermark"));
                    localeProperties.Add("error", GetString("PivotFieldList_Error"));
                    localeProperties.Add("invalidFormula", GetString("PivotFieldList_InvalidFormula"));
                    localeProperties.Add("dropText", GetString("PivotFieldList_CalculatedField_ExampleWatermark"));
                    localeProperties.Add("dropTextMobile", GetString("PivotFieldList_CalculatedField_MobileWatermark"));
                    localeProperties.Add("dropAction", GetString("PivotFieldList_CalculatedField_DropMessage"));
                    localeProperties.Add("search", GetString("PivotFieldList_Search"));
                    localeProperties.Add("close", GetString("PivotFieldList_Close"));
                    localeProperties.Add("cancel", GetString("PivotFieldList_Cancel"));
                    localeProperties.Add("delete", GetString("PivotFieldList_Delete"));
                    localeProperties.Add("alert", GetString("PivotFieldList_Alert"));
                    localeProperties.Add("warning", GetString("PivotFieldList_Warning"));
                    localeProperties.Add("ok", GetString("PivotFieldList_OK"));
                    localeProperties.Add("allFields", GetString("PivotFieldList_AllFields"));
                    localeProperties.Add("formula", GetString("PivotFieldList_Formula"));
                    localeProperties.Add("fieldExist", GetString("PivotFieldList_CalculatedField_ExistMessage"));
                    localeProperties.Add("confirmText", GetString("PivotFieldList_CalculatedField_ConfirmMessage"));
                    localeProperties.Add("noMatches", GetString("PivotFieldList_NoMatchesMessage"));
                    localeProperties.Add("format", GetString("PivotFieldList_Summaries"));
                    localeProperties.Add("edit", GetString("PivotFieldList_Edit"));
                    localeProperties.Add("clear", GetString("PivotFieldList_Clear"));
                    localeProperties.Add("formulaField", GetString("PivotFieldList_CalculatedField_DragDropMessage"));
                    localeProperties.Add("dragField", GetString("PivotFieldList_CalculatedField_DragMessage"));
                    localeProperties.Add("clearFilter", GetString("PivotFieldList_ClearFilter"));
                    localeProperties.Add("by", GetString("PivotFieldList_By"));
                    localeProperties.Add("enterValue", GetString("PivotFieldList_EnterValue"));
                    localeProperties.Add("chooseDate", GetString("PivotFieldList_EnterDate"));
                    localeProperties.Add("all", GetString("PivotFieldList_All"));
                    localeProperties.Add("multipleItems", GetString("PivotFieldList_MultipleItems"));
                    localeProperties.Add("Equals", GetString("PivotFieldList_Equals"));
                    localeProperties.Add("DoesNotEquals", GetString("PivotFieldList_DoesNotEquals"));
                    localeProperties.Add("BeginWith", GetString("PivotFieldList_BeginWith"));
                    localeProperties.Add("DoesNotBeginWith", GetString("PivotFieldList_DoesNotBeginWith"));
                    localeProperties.Add("EndsWith", GetString("PivotFieldList_EndsWith"));
                    localeProperties.Add("DoesNotEndsWith", GetString("PivotFieldList_DoesNotEndsWith"));
                    localeProperties.Add("Contains", GetString("PivotFieldList_Contains"));
                    localeProperties.Add("DoesNotContains", GetString("PivotFieldList_DoesNotContains"));
                    localeProperties.Add("GreaterThan", GetString("PivotFieldList_GreaterThan"));
                    localeProperties.Add("GreaterThanOrEqualTo", GetString("PivotFieldList_GreaterThanOrEqualTo"));
                    localeProperties.Add("LessThan", GetString("PivotFieldList_LessThan"));
                    localeProperties.Add("LessThanOrEqualTo", GetString("PivotFieldList_LessThanOrEqualTo"));
                    localeProperties.Add("Between", GetString("PivotFieldList_Between"));
                    localeProperties.Add("NotBetween", GetString("PivotFieldList_NotBetween"));
                    localeProperties.Add("Before", GetString("PivotFieldList_Before"));
                    localeProperties.Add("BeforeOrEqualTo", GetString("PivotFieldList_BeforeOrEqualTo"));
                    localeProperties.Add("After", GetString("PivotFieldList_After"));
                    localeProperties.Add("AfterOrEqualTo", GetString("PivotFieldList_AfterOrEqualTo"));
                    localeProperties.Add("member", GetString("PivotFieldList_Member"));
                    localeProperties.Add("label", GetString("PivotFieldList_Label"));
                    localeProperties.Add("date", GetString("PivotFieldList_Date"));
                    localeProperties.Add("value", GetString("PivotFieldList_Value"));
                    localeProperties.Add("labelTextContent", GetString("PivotFieldList_LabelTextMessage"));
                    localeProperties.Add("dateTextContent", GetString("PivotFieldList_DateTextMessage"));
                    localeProperties.Add("valueTextContent", GetString("PivotFieldList_ValueTextMessage"));
                    localeProperties.Add("And", GetString("PivotFieldList_And"));
                    localeProperties.Add("Sum", GetString("PivotFieldList_Sum"));
                    localeProperties.Add("Count", GetString("PivotFieldList_Count"));
                    localeProperties.Add("DistinctCount", GetString("PivotFieldList_DistinctCount"));
                    localeProperties.Add("Product", GetString("PivotFieldList_Product"));
                    localeProperties.Add("Avg", GetString("PivotFieldList_Avg"));
                    localeProperties.Add("Min", GetString("PivotFieldList_Min"));
                    localeProperties.Add("Max", GetString("PivotFieldList_Max"));
                    localeProperties.Add("Index", GetString("PivotFieldList_Index"));
                    localeProperties.Add("SampleStDev", GetString("PivotFieldList_SampleStDev"));
                    localeProperties.Add("PopulationStDev", GetString("PivotFieldList_PopulationStDev"));
                    localeProperties.Add("SampleVar", GetString("PivotFieldList_SampleVar"));
                    localeProperties.Add("PopulationVar", GetString("PivotFieldList_PopulationVar"));
                    localeProperties.Add("RunningTotals", GetString("PivotFieldList_RunningTotals"));
                    localeProperties.Add("DifferenceFrom", GetString("PivotFieldList_DifferenceFrom"));
                    localeProperties.Add("PercentageOfDifferenceFrom", GetString("PivotFieldList_PercentageOfDifferenceFrom"));
                    localeProperties.Add("PercentageOfGrandTotal", GetString("PivotFieldList_PercentageOfGrandTotal"));
                    localeProperties.Add("PercentageOfColumnTotal", GetString("PivotFieldList_PercentageOfColumnTotal"));
                    localeProperties.Add("PercentageOfRowTotal", GetString("PivotFieldList_PercentageOfRowTotal"));
                    localeProperties.Add("PercentageOfParentTotal", GetString("PivotFieldList_PercentageOfParentTotal"));
                    localeProperties.Add("PercentageOfParentColumnTotal", GetString("PivotFieldList_PercentageOfParentColumnTotal"));
                    localeProperties.Add("PercentageOfParentRowTotal", GetString("PivotFieldList_PercentageOfParentRowTotal"));
                    localeProperties.Add("Years", GetString("PivotFieldList_Years"));
                    localeProperties.Add("Quarters", GetString("PivotFieldList_Quarters"));
                    localeProperties.Add("Months", GetString("PivotFieldList_Months"));
                    localeProperties.Add("Days", GetString("PivotFieldList_Days"));
                    localeProperties.Add("Hours", GetString("PivotFieldList_Hours"));
                    localeProperties.Add("Minutes", GetString("PivotFieldList_Minutes"));
                    localeProperties.Add("Seconds", GetString("PivotFieldList_Seconds"));
                    localeProperties.Add("apply", GetString("PivotFieldList_Apply"));
                    localeProperties.Add("valueFieldSettings", GetString("PivotFieldList_ValueFieldSettings"));
                    localeProperties.Add("sourceName", GetString("PivotFieldList_FieldNameMessage"));
                    localeProperties.Add("sourceCaption", GetString("PivotFieldList_FieldCaptionMessage"));
                    localeProperties.Add("summarizeValuesBy", GetString("PivotFieldList_SummarizeValuesBy"));
                    localeProperties.Add("baseField", GetString("PivotFieldList_BaseField"));
                    localeProperties.Add("baseItem", GetString("PivotFieldList_BaseItem"));
                    localeProperties.Add("example", GetString("PivotFieldList_Example"));
                    localeProperties.Add("editorDataLimitMsg", GetString("PivotFieldList_MemberLimitMessage"));
                    localeProperties.Add("deferLayoutUpdate", GetString("PivotFieldList_DeferLayoutUpdate"));
                    localeProperties.Add("null", GetString("PivotFieldList_Null"));
                    localeProperties.Add("undefined", GetString("PivotFieldList_Undefined"));
                    localeProperties.Add("groupOutOfRange", GetString("PivotFieldList_OutOfRange"));
                    localeProperties.Add("fieldDropErrorAction", GetString("PivotFieldList_FieldDropErrorMessage"));
                    localeProperties.Add("MoreOption", GetString("PivotFieldList_MoreOption"));
                    localeProperties.Add("memberType", GetString("PivotFieldList_FieldType"));
                    localeProperties.Add("selectedHierarchy", GetString("PivotFieldList_ParentHierarchy"));
                    localeProperties.Add("expressionField", GetString("PivotFieldList_Expression"));
                    localeProperties.Add("olapDropText", GetString("PivotFieldList_CalculatedField_OLAPExampleWatermark"));
                    localeProperties.Add("customFormat", GetString("PivotFieldList_CustomFormat"));
                    localeProperties.Add("Measure", GetString("PivotFieldList_Measure"));
                    localeProperties.Add("Dimension", GetString("PivotFieldList_Dimension"));
                    localeProperties.Add("Standard", GetString("PivotFieldList_Standard"));
                    localeProperties.Add("Currency", GetString("PivotFieldList_Currency"));
                    localeProperties.Add("Percent", GetString("PivotFieldList_Percent"));
                    localeProperties.Add("Custom", GetString("PivotFieldList_Custom"));
                    localeProperties.Add("blank", GetString("PivotFieldList_Blank"));
                    localeProperties.Add("fieldTooltip", GetString("PivotFieldList_CalculatedField_Tooltip"));
                    localeProperties.Add("fieldTitle", GetString("PivotFieldList_FieldName"));
                    localeProperties.Add("QuarterYear", GetString("PivotFieldList_QuarterYear"));
                    localeProperties.Add("caption", GetString("PivotFieldList_FieldCaption"));
                    localeProperties.Add("copy", GetString("PivotFieldList_Copy"));
                    localeProperties.Add("group", GetString("PivotFieldList_Group"));
                    localeProperties.Add("removeCalculatedField", GetString("PivotFieldList_CalculatedField_RemoveMessage"));
                    localeProperties.Add("yes", GetString("PivotFieldList_Yes"));
                    localeProperties.Add("no", GetString("PivotFieldList_No"));
                    localeProperties.Add("numberFormatString", GetString("PivotFieldList_NumberFormat_ExampleWatermark"));
                    localeProperties.Add("sortNone", GetString("PivotFieldList_SortNone_TooltipMessage"));
                    localeProperties.Add("sortAscending", GetString("PivotFieldList_SortAscending_TooltipMessage"));
                    localeProperties.Add("sortDescending", GetString("PivotFieldList_SortDescending_TooltipMessage"));
                    localeProperties.Add("editCalculatedField", GetString("PivotFieldList_CalculatedField_EditTooltipMessage"));
                    localeProperties.Add("clearCalculatedField", GetString("PivotFieldList_CalculatedField_ClearTooltipMessage"));
                    localeProperties.Add("of", GetString("PivotFieldList_Of"));
                    localeProperties.Add("formatLabel", GetString("PivotFieldList_Format"));
                    break;
                case "inplace-editor":
                    localeProperties.Add("save", GetString("InPlaceEditor_Save"));
                    localeProperties.Add("cancel", GetString("InPlaceEditor_Cancel"));
                    localeProperties.Add("loadingText", GetString("InPlaceEditor_LoadingText"));
                    localeProperties.Add("editIcon", GetString("InPlaceEditor_EditIcon"));
                    localeProperties.Add("editAreaClick", GetString("InPlaceEditor_EditAreaClick"));
                    localeProperties.Add("editAreaDoubleClick", GetString("InPlaceEditor_EditAreaDoubleClick"));
                    break;
                case "maps":
                    localeProperties.Add("ZoomIn", GetString("Maps_ZoomIn"));
                    localeProperties.Add("Zoom", GetString("Maps_Zoom"));
                    localeProperties.Add("ZoomOut", GetString("Maps_ZoomOut"));
                    localeProperties.Add("Pan", GetString("Maps_Pan"));
                    localeProperties.Add("Reset", GetString("Maps_Reset"));
                    break;
                case "chart":
                    localeProperties.Add("ZoomIn", GetString("Chart_ZoomIn"));
                    localeProperties.Add("Zoom", GetString("Chart_Zoom"));
                    localeProperties.Add("ZoomOut", GetString("Chart_ZoomOut"));
                    localeProperties.Add("Pan", GetString("Chart_Pan"));
                    localeProperties.Add("Reset", GetString("Chart_Reset"));
                    localeProperties.Add("ResetZoom", GetString("Chart_ResetZoom"));
                    break;
                case "diagram":
                    localeProperties.Add("Copy", GetString("Diagram_Copy"));
                    localeProperties.Add("Cut", GetString("Diagram_Cut"));
                    localeProperties.Add("Paste", GetString("Diagram_Paste"));
                    localeProperties.Add("Undo", GetString("Diagram_Undo"));
                    localeProperties.Add("Redo", GetString("Diagram_Redo"));
                    localeProperties.Add("SelectAll", GetString("Diagram_SelectAll"));
                    localeProperties.Add("Grouping", GetString("Diagram_Grouping"));
                    localeProperties.Add("Group", GetString("Diagram_Group"));
                    localeProperties.Add("UnGroup", GetString("Diagram_UnGroup"));
                    localeProperties.Add("Order", GetString("Diagram_Order"));
                    localeProperties.Add("BringToFront", GetString("Diagram_BringToFront"));
                    localeProperties.Add("MoveForward", GetString("Diagram_MoveForward"));
                    localeProperties.Add("SendToBack", GetString("Diagram_SendToBack"));
                    localeProperties.Add("SendBackward", GetString("Diagram_SendBackward"));
                    break;
                case "documenteditor":
                    localeProperties.Add("Table", GetString("DocumentEditor_Table"));
                    localeProperties.Add("Row", GetString("DocumentEditor_Row"));
                    localeProperties.Add("Cell", GetString("DocumentEditor_Cell"));
                    localeProperties.Add("Ok", GetString("DocumentEditor_Ok"));
                    localeProperties.Add("Cancel", GetString("DocumentEditor_Cancel"));
                    localeProperties.Add("Size", GetString("DocumentEditor_Size"));
                    localeProperties.Add("Preferred Width", GetString("DocumentEditor_PreferredWidth"));
                    localeProperties.Add("Points", GetString("DocumentEditor_Points"));
                    localeProperties.Add("Percent", GetString("DocumentEditor_Percent"));
                    localeProperties.Add("Measure in", GetString("DocumentEditor_MeasureIn"));
                    localeProperties.Add("Alignment", GetString("DocumentEditor_Alignment"));
                    localeProperties.Add("Left", GetString("DocumentEditor_Left"));
                    localeProperties.Add("Center", GetString("DocumentEditor_Center"));
                    localeProperties.Add("Right", GetString("DocumentEditor_Right"));
                    localeProperties.Add("Justify", GetString("DocumentEditor_Justify"));
                    localeProperties.Add("Indent from left", GetString("DocumentEditor_IndentFromLeft"));
                    localeProperties.Add("Borders and Shading", GetString("DocumentEditor_BordersAndShading"));
                    localeProperties.Add("Options", GetString("DocumentEditor_Options"));
                    localeProperties.Add("Specify height", GetString("DocumentEditor_SpecifyHeight"));
                    localeProperties.Add("At least", GetString("DocumentEditor_AtLeast"));
                    localeProperties.Add("Exactly", GetString("DocumentEditor_Exactly"));
                    localeProperties.Add("Row height is", GetString("DocumentEditor_RowHeightIs"));
                    localeProperties.Add("Allow row to break across pages", GetString("DocumentEditor_AllowRowToBreakAcrossPages"));
                    localeProperties.Add("Repeat as header row at the top of each page", GetString("DocumentEditor_RepeatAsHeaderRowAtTheTopOfEachPage"));
                    localeProperties.Add("Vertical alignment", GetString("DocumentEditor_VerticalAlignment"));
                    localeProperties.Add("Top", GetString("DocumentEditor_Top"));
                    localeProperties.Add("Bottom", GetString("DocumentEditor_Bottom"));
                    localeProperties.Add("Default cell margins", GetString("DocumentEditor_DefaultCellMargins"));
                    localeProperties.Add("Default cell spacing", GetString("DocumentEditor_DefaultCellSpacing"));
                    localeProperties.Add("Allow spacing between cells", GetString("DocumentEditor_AllowSpacingBetweenCells"));
                    localeProperties.Add("Cell margins", GetString("DocumentEditor_CellMargins"));
                    localeProperties.Add("Same as the whole table", GetString("DocumentEditor_SameAsTheWholeTable"));
                    localeProperties.Add("Borders", GetString("DocumentEditor_Borders"));
                    localeProperties.Add("None", GetString("DocumentEditor_None"));
                    localeProperties.Add("Style", GetString("DocumentEditor_Style"));
                    localeProperties.Add("Width", GetString("DocumentEditor_Width"));
                    localeProperties.Add("Height", GetString("DocumentEditor_Height"));
                    localeProperties.Add("Letter", GetString("DocumentEditor_Letter"));
                    localeProperties.Add("Tabloid", GetString("DocumentEditor_Tabloid"));
                    localeProperties.Add("Legal", GetString("DocumentEditor_Legal"));
                    localeProperties.Add("Statement", GetString("DocumentEditor_Statement"));
                    localeProperties.Add("Executive", GetString("DocumentEditor_Executive"));
                    localeProperties.Add("A3", GetString("DocumentEditor_A3"));
                    localeProperties.Add("A4", GetString("DocumentEditor_A4"));
                    localeProperties.Add("A5", GetString("DocumentEditor_A5"));
                    localeProperties.Add("B4", GetString("DocumentEditor_B4"));
                    localeProperties.Add("B5", GetString("DocumentEditor_B5"));
                    localeProperties.Add("Custom Size", GetString("DocumentEditor_CustomSize"));
                    localeProperties.Add("Different odd and even", GetString("DocumentEditor_DifferentOddAndEven"));
                    localeProperties.Add("Different first page", GetString("DocumentEditor_DifferentFirstPage"));
                    localeProperties.Add("From edge", GetString("DocumentEditor_FromEdge"));
                    localeProperties.Add("Header", GetString("DocumentEditor_Header"));
                    localeProperties.Add("Footer", GetString("DocumentEditor_Footer"));
                    localeProperties.Add("Margin", GetString("DocumentEditor_Margin"));
                    localeProperties.Add("Paper", GetString("DocumentEditor_Paper"));
                    localeProperties.Add("Layout", GetString("DocumentEditor_Layout"));
                    localeProperties.Add("Orientation", GetString("DocumentEditor_Orientation"));
                    localeProperties.Add("Landscape", GetString("DocumentEditor_Landscape"));
                    localeProperties.Add("Portrait", GetString("DocumentEditor_Portrait"));
                    localeProperties.Add("Show page numbers", GetString("DocumentEditor_ShowPageNumbers"));
                    localeProperties.Add("Right align page numbers", GetString("DocumentEditor_RightAlignPageNumbers"));
                    localeProperties.Add("Nothing", GetString("DocumentEditor_Nothing"));
                    localeProperties.Add("Tab leader", GetString("DocumentEditor_TabLeader"));
                    localeProperties.Add("Show levels", GetString("DocumentEditor_ShowLevels"));
                    localeProperties.Add("Use hyperlinks instead of page numbers", GetString("DocumentEditor_UseHyperlinksInsteadOfPageNumbers"));
                    localeProperties.Add("Build table of contents from", GetString("DocumentEditor_BuildTableOfContentsFrom"));
                    localeProperties.Add("Styles", GetString("DocumentEditor_Styles"));
                    localeProperties.Add("Available styles", GetString("DocumentEditor_AvailableStyles"));
                    localeProperties.Add("TOC level", GetString("DocumentEditor_TOCLevel"));
                    localeProperties.Add("Heading", GetString("DocumentEditor_Heading"));
                    localeProperties.Add("Heading 1", GetString("DocumentEditor_Heading1"));
                    localeProperties.Add("Heading 2", GetString("DocumentEditor_Heading2"));
                    localeProperties.Add("Heading 3", GetString("DocumentEditor_Heading3"));
                    localeProperties.Add("Heading 4", GetString("DocumentEditor_Heading4"));
                    localeProperties.Add("Heading 5", GetString("DocumentEditor_Heading5"));
                    localeProperties.Add("Heading 6", GetString("DocumentEditor_Heading6"));
                    localeProperties.Add("List Paragraph", GetString("DocumentEditor_ListParagraph"));
                    localeProperties.Add("Normal", GetString("DocumentEditor_Normal"));
                    localeProperties.Add("Outline levels", GetString("DocumentEditor_OutlineLevels"));
                    localeProperties.Add("Table entry fields", GetString("DocumentEditor_TableEntryFields"));
                    localeProperties.Add("Modify", GetString("DocumentEditor_Modify"));
                    localeProperties.Add("Color", GetString("DocumentEditor_Color"));
                    localeProperties.Add("Setting", GetString("DocumentEditor_Setting"));
                    localeProperties.Add("Box", GetString("DocumentEditor_Box"));
                    localeProperties.Add("All", GetString("DocumentEditor_All"));
                    localeProperties.Add("Custom", GetString("DocumentEditor_Custom"));
                    localeProperties.Add("Preview", GetString("DocumentEditor_Preview"));
                    localeProperties.Add("Shading", GetString("DocumentEditor_Shading"));
                    localeProperties.Add("Fill", GetString("DocumentEditor_Fill"));
                    localeProperties.Add("Apply To", GetString("DocumentEditor_ApplyTo"));
                    localeProperties.Add("Table Properties", GetString("DocumentEditor_TableProperties"));
                    localeProperties.Add("Cell Options", GetString("DocumentEditor_CellOptions"));
                    localeProperties.Add("Table Options", GetString("DocumentEditor_TableOptions"));
                    localeProperties.Add("Insert Table", GetString("DocumentEditor_InsertTable"));
                    localeProperties.Add("Number of columns", GetString("DocumentEditor_NumberOfColumns"));
                    localeProperties.Add("Number of rows", GetString("DocumentEditor_NumberOfRows"));
                    localeProperties.Add("Text to display", GetString("DocumentEditor_TextToDisplay"));
                    localeProperties.Add("Address", GetString("DocumentEditor_Address"));
                    localeProperties.Add("Insert Hyperlink", GetString("DocumentEditor_InsertHyperlink"));
                    localeProperties.Add("Edit Hyperlink", GetString("DocumentEditor_EditHyperlink"));
                    localeProperties.Add("Insert", GetString("DocumentEditor_Insert"));
                    localeProperties.Add("General", GetString("DocumentEditor_General"));
                    localeProperties.Add("Indentation", GetString("DocumentEditor_Indentation"));
                    localeProperties.Add("Before text", GetString("DocumentEditor_BeforeText"));
                    localeProperties.Add("Special", GetString("DocumentEditor_Special"));
                    localeProperties.Add("First line", GetString("DocumentEditor_FirstLine"));
                    localeProperties.Add("Hanging", GetString("DocumentEditor_Hanging"));
                    localeProperties.Add("After text", GetString("DocumentEditor_AfterText"));
                    localeProperties.Add("By", GetString("DocumentEditor_By"));
                    localeProperties.Add("Before", GetString("DocumentEditor_Before"));
                    localeProperties.Add("Line Spacing", GetString("DocumentEditor_LineSpacing"));
                    localeProperties.Add("After", GetString("DocumentEditor_After"));
                    localeProperties.Add("At", GetString("DocumentEditor_At"));
                    localeProperties.Add("Multiple", GetString("DocumentEditor_Multiple"));
                    localeProperties.Add("Spacing", GetString("DocumentEditor_Spacing"));
                    localeProperties.Add("Define new Multilevel list", GetString("DocumentEditor_DefineNewMultilevelList"));
                    localeProperties.Add("List level", GetString("DocumentEditor_ListLevel"));
                    localeProperties.Add("Choose level to modify", GetString("DocumentEditor_ChooseLevelToModify"));
                    localeProperties.Add("Level", GetString("DocumentEditor_Level"));
                    localeProperties.Add("Number format", GetString("DocumentEditor_NumberFormat"));
                    localeProperties.Add("Number style for this level", GetString("DocumentEditor_NumberStyleForThisLevel"));
                    localeProperties.Add("Enter formatting for number", GetString("DocumentEditor_EnterFormattingForNumber"));
                    localeProperties.Add("Start at", GetString("DocumentEditor_StartAt"));
                    localeProperties.Add("Restart list after", GetString("DocumentEditor_RestartListAfter"));
                    localeProperties.Add("Position", GetString("DocumentEditor_Position"));
                    localeProperties.Add("Text indent at", GetString("DocumentEditor_TextIndentAt"));
                    localeProperties.Add("Aligned at", GetString("DocumentEditor_AlignedAt"));
                    localeProperties.Add("Follow number with", GetString("DocumentEditor_FollowNumberWith"));
                    localeProperties.Add("Tab character", GetString("DocumentEditor_TabCharacter"));
                    localeProperties.Add("Space", GetString("DocumentEditor_Space"));
                    localeProperties.Add("Arabic", GetString("DocumentEditor_Arabic"));
                    localeProperties.Add("UpRoman", GetString("DocumentEditor_UpRoman"));
                    localeProperties.Add("LowRoman", GetString("DocumentEditor_LowRoman"));
                    localeProperties.Add("UpLetter", GetString("DocumentEditor_UpLetter"));
                    localeProperties.Add("LowLetter", GetString("DocumentEditor_LowLetter"));
                    localeProperties.Add("Number", GetString("DocumentEditor_Number"));
                    localeProperties.Add("Leading zero", GetString("DocumentEditor_LeadingZero"));
                    localeProperties.Add("Bullet", GetString("DocumentEditor_Bullet"));
                    localeProperties.Add("Ordinal", GetString("DocumentEditor_Ordinal"));
                    localeProperties.Add("Ordinal Text", GetString("DocumentEditor_OrdinalText"));
                    localeProperties.Add("For East", GetString("DocumentEditor_ForEast"));
                    localeProperties.Add("No Restart", GetString("DocumentEditor_NoRestart"));
                    localeProperties.Add("Font", GetString("DocumentEditor_Font"));
                    localeProperties.Add("Font style", GetString("DocumentEditor_FontStyle"));
                    localeProperties.Add("Underline style", GetString("DocumentEditor_UnderlineStyle"));
                    localeProperties.Add("Font color", GetString("DocumentEditor_FontColor"));
                    localeProperties.Add("Effects", GetString("DocumentEditor_Effects"));
                    localeProperties.Add("Strikethrough", GetString("DocumentEditor_Strikethrough"));
                    localeProperties.Add("Superscript", GetString("DocumentEditor_Superscript"));
                    localeProperties.Add("Subscript", GetString("DocumentEditor_Subscript"));
                    localeProperties.Add("Double strikethrough", GetString("DocumentEditor_DoubleStrikethrough"));
                    localeProperties.Add("Regular", GetString("DocumentEditor_Regular"));
                    localeProperties.Add("Bold", GetString("DocumentEditor_Bold"));
                    localeProperties.Add("Italic", GetString("DocumentEditor_Italic"));
                    localeProperties.Add("Cut", GetString("DocumentEditor_Cut"));
                    localeProperties.Add("Copy", GetString("DocumentEditor_Copy"));
                    localeProperties.Add("Paste", GetString("DocumentEditor_Paste"));
                    localeProperties.Add("Hyperlink", GetString("DocumentEditor_Hyperlink"));
                    localeProperties.Add("Open Hyperlink", GetString("DocumentEditor_OpenHyperlink"));
                    localeProperties.Add("Copy Hyperlink", GetString("DocumentEditor_CopyHyperlink"));
                    localeProperties.Add("Remove Hyperlink", GetString("DocumentEditor_RemoveHyperlink"));
                    localeProperties.Add("Paragraph", GetString("DocumentEditor_Paragraph"));
                    localeProperties.Add("Linked Style", GetString("DocumentEditor_LinkedStyle"));
                    localeProperties.Add("Character", GetString("DocumentEditor_Character"));
                    localeProperties.Add("Merge Cells", GetString("DocumentEditor_MergeCells"));
                    localeProperties.Add("Insert Above", GetString("DocumentEditor_InsertAbove"));
                    localeProperties.Add("Insert Below", GetString("DocumentEditor_InsertBelow"));
                    localeProperties.Add("Insert Left", GetString("DocumentEditor_InsertLeft"));
                    localeProperties.Add("Insert Right", GetString("DocumentEditor_InsertRight"));
                    localeProperties.Add("Delete", GetString("DocumentEditor_Delete"));
                    localeProperties.Add("Delete Table", GetString("DocumentEditor_DeleteTable"));
                    localeProperties.Add("Delete Row", GetString("DocumentEditor_DeleteRow"));
                    localeProperties.Add("Delete Column", GetString("DocumentEditor_DeleteColumn"));
                    localeProperties.Add("File Name", GetString("DocumentEditor_FileName"));
                    localeProperties.Add("Format Type", GetString("DocumentEditor_FormatType"));
                    localeProperties.Add("Save", GetString("DocumentEditor_Save"));
                    localeProperties.Add("Navigation", GetString("DocumentEditor_Navigation"));
                    localeProperties.Add("Results", GetString("DocumentEditor_Results"));
                    localeProperties.Add("Replace", GetString("DocumentEditor_Replace"));
                    localeProperties.Add("Replace All", GetString("DocumentEditor_ReplaceAll"));
                    localeProperties.Add("We replaced all", GetString("DocumentEditor_WeReplacedAll"));
                    localeProperties.Add("Find", GetString("DocumentEditor_Find"));
                    localeProperties.Add("No matches", GetString("DocumentEditor_NoMatches"));
                    localeProperties.Add("All Done", GetString("DocumentEditor_AllDone"));
                    localeProperties.Add("Result", GetString("DocumentEditor_Result"));
                    localeProperties.Add("of", GetString("DocumentEditor_Of"));
                    localeProperties.Add("instances", GetString("DocumentEditor_Instances"));
                    localeProperties.Add("with", GetString("DocumentEditor_With"));
                    localeProperties.Add("Click to follow link", GetString("DocumentEditor_ClickToFollowLink"));
                    localeProperties.Add("Continue Numbering", GetString("DocumentEditor_ContinueNumbering"));
                    localeProperties.Add("Bookmark name", GetString("DocumentEditor_BookmarkName"));
                    localeProperties.Add("Close", GetString("DocumentEditor_Close"));
                    localeProperties.Add("Restart At", GetString("DocumentEditor_RestartAt"));
                    localeProperties.Add("Properties", GetString("DocumentEditor_Properties"));
                    localeProperties.Add("Name", GetString("DocumentEditor_Name"));
                    localeProperties.Add("Style type", GetString("DocumentEditor_StyleType"));
                    localeProperties.Add("Style based on", GetString("DocumentEditor_StyleBasedOn"));
                    localeProperties.Add("Style for following paragraph", GetString("DocumentEditor_StyleForFollowingParagraph"));
                    localeProperties.Add("Formatting", GetString("DocumentEditor_Formatting"));
                    localeProperties.Add("Numbering and Bullets", GetString("DocumentEditor_NumberingAndBullets"));
                    localeProperties.Add("Numbering", GetString("DocumentEditor_Numbering"));
                    localeProperties.Add("Update Field", GetString("DocumentEditor_UpdateField"));
                    localeProperties.Add("Edit Field", GetString("DocumentEditor_EditField"));
                    localeProperties.Add("Bookmark", GetString("DocumentEditor_Bookmark"));
                    localeProperties.Add("Page Setup", GetString("DocumentEditor_PageSetup"));
                    localeProperties.Add("No bookmarks found", GetString("DocumentEditor_NoBookmarksFound"));
                    localeProperties.Add("Number format tooltip information", GetString("DocumentEditor_NumberFormatTooltipInformation"));
                    localeProperties.Add("Format", GetString("DocumentEditor_Format"));
                    localeProperties.Add("Create New Style", GetString("DocumentEditor_CreateNewStyle"));
                    localeProperties.Add("Modify Style", GetString("DocumentEditor_ModifyStyle"));
                    localeProperties.Add("New", GetString("DocumentEditor_New"));
                    localeProperties.Add("Bullets", GetString("DocumentEditor_Bullets"));
                    localeProperties.Add("Use bookmarks", GetString("DocumentEditor_UseBookmarks"));
                    localeProperties.Add("Table of Contents", GetString("DocumentEditor_TableOfContents"));
                    localeProperties.Add("AutoFit", GetString("DocumentEditor_AutoFit"));
                    localeProperties.Add("AutoFit to Contents", GetString("DocumentEditor_AutoFitToContents"));
                    localeProperties.Add("AutoFit to Window", GetString("DocumentEditor_AutoFitToWindow"));
                    localeProperties.Add("Fixed Column Width", GetString("DocumentEditor_FixedColumnWidth"));
                    localeProperties.Add("Reset", GetString("DocumentEditor_Reset"));
                    localeProperties.Add("Match case", GetString("DocumentEditor_MatchCase"));
                    localeProperties.Add("Whole words", GetString("DocumentEditor_WholeWords"));
                    localeProperties.Add("Add", GetString("DocumentEditor_Add"));
                    localeProperties.Add("Go To", GetString("DocumentEditor_GoTo"));
                    localeProperties.Add("Search for", GetString("DocumentEditor_SearchFor"));
                    localeProperties.Add("Replace with", GetString("DocumentEditor_ReplaceWith"));
                    localeProperties.Add("TOC 1", GetString("DocumentEditor_TOC1"));
                    localeProperties.Add("TOC 2", GetString("DocumentEditor_TOC2"));
                    localeProperties.Add("TOC 3", GetString("DocumentEditor_TOC3"));
                    localeProperties.Add("TOC 4", GetString("DocumentEditor_TOC4"));
                    localeProperties.Add("TOC 5", GetString("DocumentEditor_TOC5"));
                    localeProperties.Add("TOC 6", GetString("DocumentEditor_TOC6"));
                    localeProperties.Add("TOC 7", GetString("DocumentEditor_TOC7"));
                    localeProperties.Add("TOC 8", GetString("DocumentEditor_TOC8"));
                    localeProperties.Add("TOC 9", GetString("DocumentEditor_TOC9"));
                    localeProperties.Add("Right-to-left", GetString("DocumentEditor_RightToLeft"));
                    localeProperties.Add("Left-to-right", GetString("DocumentEditor_LeftToRight"));
                    localeProperties.Add("Direction", GetString("DocumentEditor_Direction"));
                    localeProperties.Add("Table direction", GetString("DocumentEditor_TableDirection"));
                    localeProperties.Add("Indent from right", GetString("DocumentEditor_IndentFromRight"));
                    localeProperties.Add("Contextual Spacing", GetString("DocumentEditor_ContextualSpacing"));
                    localeProperties.Add("Password Mismatch", GetString("DocumentEditor_PasswordMismatch"));
                    localeProperties.Add("Restrict Editing", GetString("DocumentEditor_RestrictEditing"));
                    localeProperties.Add("Formatting restrictions", GetString("DocumentEditor_FormattingRestrictions"));
                    localeProperties.Add("Allow formatting", GetString("DocumentEditor_AllowFormatting"));
                    localeProperties.Add("Editing restrictions", GetString("DocumentEditor_EditingRestrictions"));
                    localeProperties.Add("Read only", GetString("DocumentEditor_ReadOnly"));
                    localeProperties.Add("Exceptions Optional", GetString("DocumentEditor_ExceptionsOptional"));
                    localeProperties.Add("Select Part Of Document And User", GetString("DocumentEditor_SelectPartOfDocumentAndUser"));
                    localeProperties.Add("Everyone", GetString("DocumentEditor_Everyone"));
                    localeProperties.Add("More users", GetString("DocumentEditor_MoreUsers"));
                    localeProperties.Add("Add Users", GetString("DocumentEditor_AddUsers"));
                    localeProperties.Add("Enforcing Protection", GetString("DocumentEditor_EnforcingProtection"));
                    localeProperties.Add("Start Enforcing Protection", GetString("DocumentEditor_StartEnforcingProtection"));
                    localeProperties.Add("Enter User", GetString("DocumentEditor_EnterUser"));
                    localeProperties.Add("Users", GetString("DocumentEditor_Users"));
                    localeProperties.Add("Enter new password", GetString("DocumentEditor_EnterNewPassword"));
                    localeProperties.Add("Reenter new password to confirm", GetString("DocumentEditor_ReenterNewPasswordToConfirm"));
                    localeProperties.Add("Your permissions", GetString("DocumentEditor_YourPermissions"));
                    localeProperties.Add("Protected Document", GetString("DocumentEditor_ProtectedDocument"));
                    localeProperties.Add("You may format text only with certain styles", GetString("DocumentEditor_YouMayFormatTextOnlyWithCertainStyles"));
                    localeProperties.Add("Stop Protection", GetString("DocumentEditor_StopProtection"));
                    localeProperties.Add("Password", GetString("DocumentEditor_Password"));
                    localeProperties.Add("Spelling Editor", GetString("DocumentEditor_SpellingEditor"));
                    localeProperties.Add("Spelling", GetString("DocumentEditor_Spelling"));
                    localeProperties.Add("Spell Check", GetString("DocumentEditor_SpellCheck"));
                    localeProperties.Add("Underline errors", GetString("DocumentEditor_UnderlineErrors"));
                    localeProperties.Add("Ignore", GetString("DocumentEditor_Ignore"));
                    localeProperties.Add("Ignore all", GetString("DocumentEditor_IgnoreAll"));
                    localeProperties.Add("Add to Dictionary", GetString("DocumentEditor_AddToDictionary"));
                    localeProperties.Add("Change", GetString("DocumentEditor_Change"));
                    localeProperties.Add("Change All", GetString("DocumentEditor_ChangeAll"));
                    localeProperties.Add("Suggestions", GetString("DocumentEditor_Suggestions"));
                    localeProperties.Add("The password is incorrect", GetString("DocumentEditor_ThePasswordIsIncorrect"));
                    localeProperties.Add("Error in establishing connection with web server", GetString("DocumentEditor_ErrorInEstablishingConnectionWithWebServer"));
                    localeProperties.Add("Highlight the regions I can edit", GetString("DocumentEditor_HighlightTheRegionsICanEdit"));
                    localeProperties.Add("Show All Regions I Can Edit", GetString("DocumentEditor_ShowAllRegionsICanEdit"));
                    localeProperties.Add("Find Next Region I Can Edit", GetString("DocumentEditor_FindNextRegionICanEdit"));
                    localeProperties.Add("Keep source formatting", GetString("DocumentEditor_KeepSourceFormatting"));
                    localeProperties.Add("Match destination formatting", GetString("DocumentEditor_MatchDestinationFormatting"));
                    localeProperties.Add("Text only", GetString("DocumentEditor_TextOnly"));
                    localeProperties.Add("Comments", GetString("DocumentEditor_Comments"));
                    localeProperties.Add("Type your comment", GetString("DocumentEditor_TypeYourComment"));
                    localeProperties.Add("Post", GetString("DocumentEditor_Post"));
                    localeProperties.Add("Reply", GetString("DocumentEditor_Reply"));
                    localeProperties.Add("New Comment", GetString("DocumentEditor_NewComment"));
                    localeProperties.Add("Edit", GetString("DocumentEditor_Edit"));
                    localeProperties.Add("Resolve", GetString("DocumentEditor_Resolve"));
                    localeProperties.Add("Reopen", GetString("DocumentEditor_Reopen"));
                    localeProperties.Add("No comments in this document", GetString("DocumentEditor_NoCommentsInThisDocument"));
                    localeProperties.Add("more", GetString("DocumentEditor_More"));
                    localeProperties.Add("Type your comment here", GetString("DocumentEditor_TypeYourCommentHere"));
                    localeProperties.Add("Next Comment", GetString("DocumentEditor_NextComment"));
                    localeProperties.Add("Previous Comment", GetString("DocumentEditor_PreviousComment"));
                    localeProperties.Add("Un-posted comments", GetString("DocumentEditor_UnPostedComments"));
                    localeProperties.Add("Discard Comment", GetString("DocumentEditor_DiscardComment"));
                    localeProperties.Add("No Headings", GetString("DocumentEditor_NoHeadings"));
                    localeProperties.Add("Add Headings", GetString("DocumentEditor_AddHeadings"));
                    localeProperties.Add("More Options", GetString("DocumentEditor_MoreOptions"));
                    localeProperties.Add("Click to see this comment", GetString("DocumentEditor_ClickToSeeThisComment"));
                    localeProperties.Add("Drop Down Form Field", GetString("DocumentEditor_DropDownFormField"));
                    localeProperties.Add("Drop-down items", GetString("DocumentEditor_DropDownItems"));
                    localeProperties.Add("Items in drop-down list", GetString("DocumentEditor_ItemsInDropDownList"));
                    localeProperties.Add("ADD", GetString("DocumentEditor_ADD"));
                    localeProperties.Add("REMOVE", GetString("DocumentEditor_REMOVE"));
                    localeProperties.Add("Field settings", GetString("DocumentEditor_FieldSettings"));
                    localeProperties.Add("Tooltip", GetString("DocumentEditor_Tooltip"));
                    localeProperties.Add("Drop-down enabled", GetString("DocumentEditor_DropDownEnabled"));
                    localeProperties.Add("Check Box Form Field", GetString("DocumentEditor_CheckBoxFormField"));
                    localeProperties.Add("Check box size", GetString("DocumentEditor_CheckBoxSize"));
                    localeProperties.Add("Auto", GetString("DocumentEditor_Auto"));
                    localeProperties.Add("Default value", GetString("DocumentEditor_DefaultValue"));
                    localeProperties.Add("Not checked", GetString("DocumentEditor_NotChecked"));
                    localeProperties.Add("Checked", GetString("DocumentEditor_Checked"));
                    localeProperties.Add("Check box enabled", GetString("DocumentEditor_CheckBoxEnabled"));
                    localeProperties.Add("Text Form Field", GetString("DocumentEditor_TextFormField"));
                    localeProperties.Add("Type", GetString("DocumentEditor_Type"));
                    localeProperties.Add("Default text", GetString("DocumentEditor_DefaultText"));
                    localeProperties.Add("Maximum length", GetString("DocumentEditor_MaximumLength"));
                    localeProperties.Add("Text format", GetString("DocumentEditor_TextFormat"));
                    localeProperties.Add("Fill-in enabled", GetString("DocumentEditor_FillInEnabled"));
                    localeProperties.Add("Default number", GetString("DocumentEditor_DefaultNumber"));
                    localeProperties.Add("Default date", GetString("DocumentEditor_DefaultDate"));
                    localeProperties.Add("Date format", GetString("DocumentEditor_DateFormat"));
                    localeProperties.Add("Merge Track", GetString("DocumentEditor_MergeTrack"));
                    localeProperties.Add("UnTrack", GetString("DocumentEditor_UnTrack"));
                    localeProperties.Add("Accept", GetString("DocumentEditor_Accept"));
                    localeProperties.Add("Reject", GetString("DocumentEditor_Reject"));
                    localeProperties.Add("Previous Changes", GetString("DocumentEditor_PreviousChanges"));
                    localeProperties.Add("Next Changes", GetString("DocumentEditor_NextChanges"));
                    localeProperties.Add("Inserted", GetString("DocumentEditor_Inserted"));
                    localeProperties.Add("Deleted", GetString("DocumentEditor_Deleted"));
                    localeProperties.Add("Changes", GetString("DocumentEditor_Changes"));
                    localeProperties.Add("Accept all", GetString("DocumentEditor_AcceptAll"));
                    localeProperties.Add("Reject all", GetString("DocumentEditor_RejectAll"));
                    localeProperties.Add("No changes", GetString("DocumentEditor_NoChanges"));
                    localeProperties.Add("Accept Changes", GetString("DocumentEditor_AcceptChanges"));
                    localeProperties.Add("Reject Changes", GetString("DocumentEditor_RejectChanges"));
                    localeProperties.Add("User", GetString("DocumentEditor_User"));
                    localeProperties.Add("View", GetString("DocumentEditor_View"));
                    break;
                case "documenteditorcontainer":
                    localeProperties.Add("New", GetString("DocumentEditorContainer_New"));
                    localeProperties.Add("Open", GetString("DocumentEditorContainer_Open"));
                    localeProperties.Add("Undo", GetString("DocumentEditorContainer_Undo"));
                    localeProperties.Add("Redo", GetString("DocumentEditorContainer_Redo"));
                    localeProperties.Add("Image", GetString("DocumentEditorContainer_Image"));
                    localeProperties.Add("Table", GetString("DocumentEditorContainer_Table"));
                    localeProperties.Add("Link", GetString("DocumentEditorContainer_Link"));
                    localeProperties.Add("Bookmark", GetString("DocumentEditorContainer_Bookmark"));
                    localeProperties.Add("Table of Contents", GetString("DocumentEditorContainer_TableOfContents"));
                    localeProperties.Add("HEADING - - - - 1", GetString("DocumentEditorContainer_HEADING1"));
                    localeProperties.Add("HEADING - - - - 2", GetString("DocumentEditorContainer_HEADING2"));
                    localeProperties.Add("HEADING - - - - 3", GetString("DocumentEditorContainer_HEADING3"));
                    localeProperties.Add("Header", GetString("DocumentEditorContainer_Header"));
                    localeProperties.Add("Footer", GetString("DocumentEditorContainer_Footer"));
                    localeProperties.Add("Page Setup", GetString("DocumentEditorContainer_PageSetup"));
                    localeProperties.Add("Page Number", GetString("DocumentEditorContainer_PageNumber"));
                    localeProperties.Add("Break", GetString("DocumentEditorContainer_Break"));
                    localeProperties.Add("Find", GetString("DocumentEditorContainer_Find"));
                    localeProperties.Add("Local Clipboard", GetString("DocumentEditorContainer_LocalClipboard"));
                    localeProperties.Add("Restrict Editing", GetString("DocumentEditorContainer_RestrictEditing"));
                    localeProperties.Add("Upload from computer", GetString("DocumentEditorContainer_UploadFromComputer"));
                    localeProperties.Add("By URL", GetString("DocumentEditorContainer_ByURL"));
                    localeProperties.Add("Page Break", GetString("DocumentEditorContainer_PageBreak"));
                    localeProperties.Add("Section Break", GetString("DocumentEditorContainer_SectionBreak"));
                    localeProperties.Add("Header And Footer", GetString("DocumentEditorContainer_HeaderAndFooter"));
                    localeProperties.Add("Options", GetString("DocumentEditorContainer_Options"));
                    localeProperties.Add("Levels", GetString("DocumentEditorContainer_Levels"));
                    localeProperties.Add("Different First Page", GetString("DocumentEditorContainer_DifferentFirstPage"));
                    localeProperties.Add("Different header and footer for odd and even pages", GetString("DocumentEditorContainer_DifferentHeaderAndFooterForOddAndEvenPages"));
                    localeProperties.Add("Different Odd And Even Pages", GetString("DocumentEditorContainer_DifferentOddAndEvenPages"));
                    localeProperties.Add("Different header and footer for first page", GetString("DocumentEditorContainer_DifferentHeaderAndFooterForFirstPage"));
                    localeProperties.Add("Position", GetString("DocumentEditorContainer_Position"));
                    localeProperties.Add("Header from Top", GetString("DocumentEditorContainer_HeaderFromTop"));
                    localeProperties.Add("Footer from Bottom", GetString("DocumentEditorContainer_FooterFromBottom"));
                    localeProperties.Add("Distance from top of the page to top of the header", GetString("DocumentEditorContainer_DistanceFromTopOfThePageToTopOfTheHeader"));
                    localeProperties.Add("Distance from bottom of the page to bottom of the footer", GetString("DocumentEditorContainer_DistanceFromBottomOfThePageToBottomOfTheFooter"));
                    localeProperties.Add("Aspect ratio", GetString("DocumentEditorContainer_AspectRatio"));
                    localeProperties.Add("W", GetString("DocumentEditorContainer_W"));
                    localeProperties.Add("H", GetString("DocumentEditorContainer_H"));
                    localeProperties.Add("Width", GetString("DocumentEditorContainer_Width"));
                    localeProperties.Add("Height", GetString("DocumentEditorContainer_Height"));
                    localeProperties.Add("Text", GetString("DocumentEditorContainer_Text"));
                    localeProperties.Add("Paragraph", GetString("DocumentEditorContainer_Paragraph"));
                    localeProperties.Add("Fill", GetString("DocumentEditorContainer_Fill"));
                    localeProperties.Add("Fill color", GetString("DocumentEditorContainer_FillColor"));
                    localeProperties.Add("Border Style", GetString("DocumentEditorContainer_BorderStyle"));
                    localeProperties.Add("Outside borders", GetString("DocumentEditorContainer_OutsideBorders"));
                    localeProperties.Add("All borders", GetString("DocumentEditorContainer_AllBorders"));
                    localeProperties.Add("Inside borders", GetString("DocumentEditorContainer_InsideBorders"));
                    localeProperties.Add("Left border", GetString("DocumentEditorContainer_LeftBorder"));
                    localeProperties.Add("Inside vertical border", GetString("DocumentEditorContainer_InsideVerticalBorder"));
                    localeProperties.Add("Right border", GetString("DocumentEditorContainer_RightBorder"));
                    localeProperties.Add("Top border", GetString("DocumentEditorContainer_TopBorder"));
                    localeProperties.Add("Inside horizontal border", GetString("DocumentEditorContainer_InsideHorizontalBorder"));
                    localeProperties.Add("Bottom border", GetString("DocumentEditorContainer_BottomBorder"));
                    localeProperties.Add("Border color", GetString("DocumentEditorContainer_BorderColor"));
                    localeProperties.Add("Border width", GetString("DocumentEditorContainer_BorderWidth"));
                    localeProperties.Add("Cell", GetString("DocumentEditorContainer_Cell"));
                    localeProperties.Add("Merge cells", GetString("DocumentEditorContainer_MergeCells"));
                    localeProperties.Add("Insert Or Delete", GetString("DocumentEditorContainer_InsertOrDelete"));
                    localeProperties.Add("Insert columns to the left", GetString("DocumentEditorContainer_InsertColumnsToTheLeft"));
                    localeProperties.Add("Insert columns to the right", GetString("DocumentEditorContainer_InsertColumnsToTheRight"));
                    localeProperties.Add("Insert rows above", GetString("DocumentEditorContainer_InsertRowsAbove"));
                    localeProperties.Add("Insert rows below", GetString("DocumentEditorContainer_InsertRowsBelow"));
                    localeProperties.Add("Delete rows", GetString("DocumentEditorContainer_DeleteRows"));
                    localeProperties.Add("Delete columns", GetString("DocumentEditorContainer_DeleteColumns"));
                    localeProperties.Add("Cell Margin", GetString("DocumentEditorContainer_CellMargin"));
                    localeProperties.Add("Top", GetString("DocumentEditorContainer_Top"));
                    localeProperties.Add("Bottom", GetString("DocumentEditorContainer_Bottom"));
                    localeProperties.Add("Left", GetString("DocumentEditorContainer_Left"));
                    localeProperties.Add("Right", GetString("DocumentEditorContainer_Right"));
                    localeProperties.Add("Align Text", GetString("DocumentEditorContainer_AlignText"));
                    localeProperties.Add("Align top", GetString("DocumentEditorContainer_AlignTop"));
                    localeProperties.Add("Align bottom", GetString("DocumentEditorContainer_AlignBottom"));
                    localeProperties.Add("Align center", GetString("DocumentEditorContainer_AlignCenter"));
                    localeProperties.Add("Number of heading or outline levels to be shown in table of contents", GetString("DocumentEditorContainer_NumberOfHeadingOrOutlineLevelsToBeShownInTableOfContents"));
                    localeProperties.Add("Show page numbers", GetString("DocumentEditorContainer_ShowPageNumbers"));
                    localeProperties.Add("Show page numbers in table of contents", GetString("DocumentEditorContainer_ShowPageNumbersInTableOfContents"));
                    localeProperties.Add("Right align page numbers", GetString("DocumentEditorContainer_RightAlignPageNumbers"));
                    localeProperties.Add("Right align page numbers in table of contents", GetString("DocumentEditorContainer_RightAlignPageNumbersInTableOfContents"));
                    localeProperties.Add("Use hyperlinks", GetString("DocumentEditorContainer_UseHyperlinks"));
                    localeProperties.Add("Use hyperlinks instead of page numbers", GetString("DocumentEditorContainer_UseHyperlinksInsteadOfPageNumbers"));
                    localeProperties.Add("Font", GetString("DocumentEditorContainer_Font"));
                    localeProperties.Add("Font Size", GetString("DocumentEditorContainer_FontSize"));
                    localeProperties.Add("Font color", GetString("DocumentEditorContainer_FontColor"));
                    localeProperties.Add("Text highlight color", GetString("DocumentEditorContainer_TextHighlightColor"));
                    localeProperties.Add("Clear all formatting", GetString("DocumentEditorContainer_ClearAllFormatting"));
                    localeProperties.Add("Bold Tooltip", GetString("DocumentEditorContainer_BoldTooltip"));
                    localeProperties.Add("Italic Tooltip", GetString("DocumentEditorContainer_ItalicTooltip"));
                    localeProperties.Add("Underline Tooltip", GetString("DocumentEditorContainer_UnderlineTooltip"));
                    localeProperties.Add("Strikethrough", GetString("DocumentEditorContainer_Strikethrough"));
                    localeProperties.Add("Superscript Tooltip", GetString("DocumentEditorContainer_SuperscriptTooltip"));
                    localeProperties.Add("Subscript Tooltip", GetString("DocumentEditorContainer_SubscriptTooltip"));
                    localeProperties.Add("Align left Tooltip", GetString("DocumentEditorContainer_AlignLeftTooltip"));
                    localeProperties.Add("Center Tooltip", GetString("DocumentEditorContainer_CenterTooltip"));
                    localeProperties.Add("Align right Tooltip", GetString("DocumentEditorContainer_AlignRightTooltip"));
                    localeProperties.Add("Justify Tooltip", GetString("DocumentEditorContainer_JustifyTooltip"));
                    localeProperties.Add("Decrease indent", GetString("DocumentEditorContainer_DecreaseIndent"));
                    localeProperties.Add("Increase indent", GetString("DocumentEditorContainer_IncreaseIndent"));
                    localeProperties.Add("Line spacing", GetString("DocumentEditorContainer_LineSpacing"));
                    localeProperties.Add("Bullets", GetString("DocumentEditorContainer_Bullets"));
                    localeProperties.Add("Numbering", GetString("DocumentEditorContainer_Numbering"));
                    localeProperties.Add("Styles", GetString("DocumentEditorContainer_Styles"));
                    localeProperties.Add("Manage Styles", GetString("DocumentEditorContainer_ManageStyles"));
                    localeProperties.Add("Page", GetString("DocumentEditorContainer_Page"));
                    localeProperties.Add("of", GetString("DocumentEditorContainer_Of"));
                    localeProperties.Add("Fit one page", GetString("DocumentEditorContainer_FitOnePage"));
                    localeProperties.Add("Spell Check", GetString("DocumentEditorContainer_SpellCheck"));
                    localeProperties.Add("Underline errors", GetString("DocumentEditorContainer_UnderlineErrors"));
                    localeProperties.Add("Fit page width", GetString("DocumentEditorContainer_FitPageWidth"));
                    localeProperties.Add("Update", GetString("DocumentEditorContainer_Update"));
                    localeProperties.Add("Cancel", GetString("DocumentEditorContainer_Cancel"));
                    localeProperties.Add("Insert", GetString("DocumentEditorContainer_Insert"));
                    localeProperties.Add("No Border", GetString("DocumentEditorContainer_NoBorder"));
                    localeProperties.Add("Create a new document", GetString("DocumentEditorContainer_CreateANewDocument"));
                    localeProperties.Add("Open a document", GetString("DocumentEditorContainer_OpenADocument"));
                    localeProperties.Add("Undo Tooltip", GetString("DocumentEditorContainer_UndoTooltip"));
                    localeProperties.Add("Redo Tooltip", GetString("DocumentEditorContainer_RedoTooltip"));
                    localeProperties.Add("Insert inline picture from a file", GetString("DocumentEditorContainer_InsertInlinePictureFromAFile"));
                    localeProperties.Add("Insert a table into the document", GetString("DocumentEditorContainer_InsertATableIntoTheDocument"));
                    localeProperties.Add("Create Hyperlink", GetString("DocumentEditorContainer_CreateHyperlink"));
                    localeProperties.Add("Insert a bookmark in a specific place in this document", GetString("DocumentEditorContainer_InsertABookmarkInASpecificPlaceInThisDocument"));
                    localeProperties.Add("Provide an overview of your document by adding a table of contents", GetString("DocumentEditorContainer_ProvideAnOverviewOfYourDocumentByAddingATableOfContents"));
                    localeProperties.Add("Add or edit the header", GetString("DocumentEditorContainer_AddOrEditTheHeader"));
                    localeProperties.Add("Add or edit the footer", GetString("DocumentEditorContainer_AddOrEditTheFooter"));
                    localeProperties.Add("Open the page setup dialog", GetString("DocumentEditorContainer_OpenThePageSetupDialog"));
                    localeProperties.Add("Add page numbers", GetString("DocumentEditorContainer_AddPageNumbers"));
                    localeProperties.Add("Find Text", GetString("DocumentEditorContainer_FindText"));
                    localeProperties.Add("Toggle between the internal clipboard and system clipboard", GetString("DocumentEditorContainer_ToggleBetweenTheInternalClipboardAndSystemClipboard"));
                    localeProperties.Add("Current Page Number", GetString("DocumentEditorContainer_CurrentPageNumber"));
                    localeProperties.Add("Read only", GetString("DocumentEditorContainer_ReadOnly"));
                    localeProperties.Add("Protections", GetString("DocumentEditorContainer_Protections"));
                    localeProperties.Add("Error in establishing connection with web server", GetString("DocumentEditorContainer_ErrorInEstablishingConnectionWithWebServer"));
                    localeProperties.Add("Single", GetString("DocumentEditorContainer_Single"));
                    localeProperties.Add("Double", GetString("DocumentEditorContainer_Double"));
                    localeProperties.Add("New comment", GetString("DocumentEditorContainer_NewComment"));
                    localeProperties.Add("Comments", GetString("DocumentEditorContainer_Comments"));
                    localeProperties.Add("Print layout", GetString("DocumentEditorContainer_PrintLayout"));
                    localeProperties.Add("Web layout", GetString("DocumentEditorContainer_WebLayout"));
                    localeProperties.Add("Form Fields", GetString("DocumentEditorContainer_FormFields"));
                    localeProperties.Add("Text Form", GetString("DocumentEditorContainer_TextForm"));
                    localeProperties.Add("Check Box", GetString("DocumentEditorContainer_CheckBox"));
                    localeProperties.Add("DropDown", GetString("DocumentEditorContainer_DropDown"));
                    localeProperties.Add("Update Fields", GetString("DocumentEditorContainer_UpdateFields"));
                    localeProperties.Add("Update cross reference fields", GetString("DocumentEditorContainer_UpdateCrossReferenceFields"));
                    localeProperties.Add("Track Changes", GetString("DocumentEditorContainer_Track_changes"));
                    localeProperties.Add("TrackChanges", GetString("DocumentEditorContainer_TrackChanges"));
                    localeProperties.Add("No bookmarks found", GetString("DocumentEditorContainer_NoBookmarksFound"));
                    break;
                case "PdfViewer":
                    localeProperties.Add("PdfViewer", GetString("PdfViewer"));
                    localeProperties.Add("Cancel", GetString("PdfViewer_Cancel"));
                    localeProperties.Add("Download file", GetString("PdfViewer_DownloadFile"));
                    localeProperties.Add("Download", GetString("PdfViewer_Download"));
                    localeProperties.Add("Enter Password", GetString("PdfViewer_EnterPassword"));
                    localeProperties.Add("File Corrupted", GetString("PdfViewer_FileCorrupted"));
                    localeProperties.Add("File Corrupted Content", GetString("PdfViewer_FileCorruptedContent"));
                    localeProperties.Add("Fit Page", GetString("PdfViewer_FitPage"));
                    localeProperties.Add("Fit Width", GetString("PdfViewer_FitWidth"));
                    localeProperties.Add("Automatic", GetString("PdfViewer_Automatic"));
                    localeProperties.Add("Go To First Page", GetString("PdfViewer_GoToFirstPage"));
                    localeProperties.Add("Invalid Password", GetString("PdfViewer_InvalidPassword"));
                    localeProperties.Add("Next Page", GetString("PdfViewer_NextPage"));
                    localeProperties.Add("OK", GetString("PdfViewer_OK"));
                    localeProperties.Add("Open", GetString("PdfViewer_Open"));
                    localeProperties.Add("Page Number", GetString("PdfViewer_PageNumber"));
                    localeProperties.Add("Previous Page", GetString("PdfViewer_PreviousPage"));
                    localeProperties.Add("Go To Last Page", GetString("PdfViewer_GoToLastPage"));
                    localeProperties.Add("Zoom", GetString("PdfViewer_Zoom"));
                    localeProperties.Add("Zoom In", GetString("PdfViewer_ZoomIn"));
                    localeProperties.Add("Zoom Out", GetString("PdfViewer_ZoomOut"));
                    localeProperties.Add("Page Thumbnails", GetString("PdfViewer_PageThumbnails"));
                    localeProperties.Add("Bookmarks", GetString("PdfViewer_Bookmarks"));
                    localeProperties.Add("Print", GetString("PdfViewer_Print"));
                    localeProperties.Add("Password Protected", GetString("PdfViewer_PasswordProtected"));
                    localeProperties.Add("Copy", GetString("PdfViewer_Copy"));
                    localeProperties.Add("Text Selection", GetString("PdfViewer_TextSelection"));
                    localeProperties.Add("Panning", GetString("PdfViewer_Panning"));
                    localeProperties.Add("Text Search", GetString("PdfViewer_TextSearch"));
                    localeProperties.Add("Find in document", GetString("PdfViewer_Findindocument"));
                    localeProperties.Add("Match case", GetString("PdfViewer_Matchcase"));
                    localeProperties.Add("Apply", GetString("PdfViewer_Apply]"));
                    localeProperties.Add("GoToPage", GetString("PdfViewer_GoToPage"));
                    localeProperties.Add("No matches", GetString("PdfViewer_Nomatches"));
                    localeProperties.Add("No Text Found", GetString("PdfViewer_NoTextFound"));
                    localeProperties.Add("Undo", GetString("PdfViewer_Undo"));
                    localeProperties.Add("Redo", GetString("PdfViewer_Redo"));
                    localeProperties.Add("Annotation", GetString("PdfViewer_Annotation"));
                    localeProperties.Add("Highlight", GetString("PdfViewer_Highlight"));
                    localeProperties.Add("Underline", GetString("PdfViewer_Underline"));
                    localeProperties.Add("Strikethrough", GetString("PdfViewer_Strikethrough"));
                    localeProperties.Add("Delete", GetString("PdfViewer_Delete"));
                    localeProperties.Add("Opacity", GetString("PdfViewer_Opacity"));
                    localeProperties.Add("Color edit", GetString("PdfViewer_ColorEdit"));
                    localeProperties.Add("Opacity edit", GetString("PdfViewer_OpacityEdit"));
                    localeProperties.Add("Highlight context", GetString("PdfViewer_HighlightContext"));
                    localeProperties.Add("Underline context", GetString("PdfViewer_UnderlineContext"));
                    localeProperties.Add("Strikethrough context", GetString("PdfViewer_StrikethroughContext"));
                    localeProperties.Add("Server error", GetString("PdfViewer_Servererror"));
                    localeProperties.Add("Client error", GetString("PdfViewer_Clienterror"));
                    localeProperties.Add("Open text", GetString("PdfViewer_OpenText"));
                    localeProperties.Add("First text", GetString("PdfViewer_FirstText"));
                    localeProperties.Add("Previous text", GetString("PdfViewer_PreviousText"));
                    localeProperties.Add("Next text", GetString("PdfViewer_NextText"));
                    localeProperties.Add("Last text", GetString("PdfViewer_LastText"));
                    localeProperties.Add("Zoom in text", GetString("PdfViewer_ZoomInText"));
                    localeProperties.Add("Zoom out text", GetString("PdfViewer_ZoomOutText"));
                    localeProperties.Add("Selection text", GetString("PdfViewer_SelectionText"));
                    localeProperties.Add("Pan text", GetString("PdfViewer_PanText"));
                    localeProperties.Add("Print text", GetString("PdfViewer_PrintText"));
                    localeProperties.Add("Search text", GetString("PdfViewer_SearchText"));
                    localeProperties.Add("Annotation Edit text", GetString("PdfViewer_AnnotationEditText"));
                    localeProperties.Add("Line Thickness", GetString("PdfViewer_LineThickness"));
                    localeProperties.Add("Line Properties", GetString("PdfViewer_LineProperties"));
                    localeProperties.Add("Start Arrow", GetString("PdfViewer_StartArrow"));
                    localeProperties.Add("End Arrow", GetString("PdfViewer_EndArrow"));
                    localeProperties.Add("Line Style", GetString("PdfViewer_LineStyle"));
                    localeProperties.Add("Fill Color", GetString("PdfViewer_FillColor"));
                    localeProperties.Add("Line Color", GetString("PdfViewer_LineColor"));
                    localeProperties.Add("None", GetString("PdfViewer_None"));
                    localeProperties.Add("Open Arrow", GetString("PdfViewer_OpenArrow"));
                    localeProperties.Add("Closed Arrow", GetString("PdfViewer_ClosedArrow"));
                    localeProperties.Add("Round Arrow", GetString("PdfViewer_RoundArrow"));
                    localeProperties.Add("Square Arrow", GetString("PdfViewer_SquareArrow"));
                    localeProperties.Add("Diamond Arrow", GetString("PdfViewer_DiamondArrow"));
                    localeProperties.Add("Cut", GetString("PdfViewer_Cut"));
                    localeProperties.Add("Paste", GetString("PdfViewer_Paste"));
                    localeProperties.Add("Delete Context", GetString("PdfViewer_DeleteContext"));
                    localeProperties.Add("Properties", GetString("PdfViewer_Properties"));
                    localeProperties.Add("Add Stamp", GetString("PdfViewer_AddStamp"));
                    localeProperties.Add("Add Shapes", GetString("PdfViewer_AddShapes"));
                    localeProperties.Add("Stroke edit", GetString("PdfViewer_StrokeEdit"));
                    localeProperties.Add("Change thickness", GetString("PdfViewer_ChangeThickness"));
                    localeProperties.Add("Add line", GetString("PdfViewer_Addline"));
                    localeProperties.Add("Add arrow", GetString("PdfViewer_AddArrow"));
                    localeProperties.Add("Add rectangle", GetString("PdfViewer_AddRectangle"));
                    localeProperties.Add("Add circle", GetString("PdfViewer_AddCircle"));
                    localeProperties.Add("Add polygon", GetString("PdfViewer_AddPolygon"));
                    localeProperties.Add("Add Comments", GetString("PdfViewer_AddComments"));
                    localeProperties.Add("Comments", GetString("PdfViewer_Comments"));
                    localeProperties.Add("No Comments Yet", GetString("PdfViewer_NoCommentsYet"));
                    localeProperties.Add("Accepted", GetString("PdfViewer_Accepted"));
                    localeProperties.Add("Completed", GetString("PdfViewer_Completed"));
                    localeProperties.Add("Cancelled", GetString("PdfViewer_Cancelled"));
                    localeProperties.Add("Rejected", GetString("PdfViewer_Rejected"));
                    localeProperties.Add("Leader Length", GetString("PdfViewer_LeaderLength"));
                    localeProperties.Add("Scale Ratio", GetString("PdfViewer_ScaleRatio"));
                    localeProperties.Add("Calibrate", GetString("PdfViewer_Calibrate"));
                    localeProperties.Add("Calibrate Distance", GetString("PdfViewer_CalibrateDistance"));
                    localeProperties.Add("Calibrate Perimeter", GetString("PdfViewer_CalibratePerimeter"));
                    localeProperties.Add("Calibrate Area", GetString("PdfViewer_CalibrateArea"));
                    localeProperties.Add("Calibrate Radius", GetString("PdfViewer_CalibrateRadius"));
                    localeProperties.Add("Calibrate Volume", GetString("PdfViewer_CalibrateVolume"));
                    localeProperties.Add("Depth", GetString("PdfViewer_Depth"));
                    localeProperties.Add("Closed", GetString("PdfViewer_Closed"));
                    localeProperties.Add("Round", GetString("PdfViewer_Round"));
                    localeProperties.Add("Square", GetString("PdfViewer_Square"));
                    localeProperties.Add("Diamond", GetString("PdfViewer_Diamond"));
                    localeProperties.Add("Edit", GetString("PdfViewer_Edit"));
                    localeProperties.Add("Comment", GetString("PdfViewer_Comment"));
                    localeProperties.Add("Comment Panel", GetString("PdfViewer_CommentPanel"));
                    localeProperties.Add("Set Status", GetString("PdfViewer_SetStatus"));
                    localeProperties.Add("Post", GetString("PdfViewer_Post"));
                    localeProperties.Add("Page", GetString("PdfViewer_Page"));
                    localeProperties.Add("Add a comment", GetString("PdfViewer_AddComment"));
                    localeProperties.Add("Add a reply", GetString("PdfViewer_AddReply"));
                    localeProperties.Add("Import Annotations", GetString("PdfViewer_ImportAnnotations"));
                    localeProperties.Add("Export Annotations", GetString("PdfViewer_ExportAnnotations"));
                    localeProperties.Add("Add", GetString("PdfViewer_Add"));
                    localeProperties.Add("Clear", GetString("PdfViewer_Clear"));
                    localeProperties.Add("Bold", GetString("PdfViewer_Bold"));
                    localeProperties.Add("Italic", GetString("PdfViewer_Italic"));
                    localeProperties.Add("Strikethroughs", GetString("PdfViewer_Strikethroughs"));
                    localeProperties.Add("Underlines", GetString("PdfViewer_Underlines"));
                    localeProperties.Add("Superscript", GetString("PdfViewer_Superscript"));
                    localeProperties.Add("Subscript", GetString("PdfViewer_Subscript"));
                    localeProperties.Add("Align left", GetString("PdfViewer_AlignLeft"));
                    localeProperties.Add("Align right", GetString("PdfViewer_AlignRight"));
                    localeProperties.Add("Center", GetString("PdfViewer_Center"));
                    localeProperties.Add("Justify", GetString("PdfViewer_Justify"));
                    localeProperties.Add("Font color", GetString("PdfViewer_FontColor"));
                    localeProperties.Add("Text Align", GetString("PdfViewer_TextAlign"));
                    localeProperties.Add("Text Properties", GetString("PdfViewer_TextProperties"));
                    localeProperties.Add("Draw Signature", GetString("PdfViewer_DrawSignature"));
                    localeProperties.Add("Create", GetString("PdfViewer_Create"));
                    localeProperties.Add("Font family", GetString("PdfViewer_FontFamily"));
                    localeProperties.Add("Font size", GetString("PdfViewer_FontSize"));
                    localeProperties.Add("Free Text", GetString("PdfViewer_FreeText"));
                    localeProperties.Add("Import Failed", GetString("PdfViewer_ImportFailed"));
                    localeProperties.Add("File not found", GetString("PdfViewer_FileNotFound"));
                    localeProperties.Add("Export Failed", GetString("PdfViewer_ExportFailed"));
                    break;
            }

            var filteredLocale = localeProperties.Where(text => text.Value != null).ToDictionary(keyPare => keyPare.Key, keyPare => keyPare.Value);
            if (filteredLocale.Count > 0)
            {
                componentLocale.Add(key, filteredLocale);
            }

            filteredLocale = null;
        }

        /// <summary>
        /// Dispose the unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            LocaleKeys = null;
        }
    }
}
