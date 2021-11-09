using System;
using System.Threading.Tasks;
using System.ComponentModel;
using Syncfusion.Blazor.Internal;
using System.Collections.Generic;
using Syncfusion.Blazor.Calendars.Internal;
using System.Linq;

namespace Syncfusion.Blazor.Calendars
{
    /// <summary>
    /// The Calendar is a graphical user interface component that displays a Gregorian Calendar, and allows a user to select a date.
    /// </summary>
    public partial class SfCalendar<TValue> : CalendarBase<TValue>
    {
        internal const string VALUE = "Value";
        internal const string VALUECHANGE = "ValueChange";
        internal const string CALENDAR_VALUES = "Values";
        internal const string ROOT = "e-control e-calendar e-lib";
        internal const string WEEK_NUMBER = "e-week-number";
        internal const string RTL = "e-rtl";
        internal const string DAYHEADERLONG = "e-calendar-day-header-lg";
        internal const string OVERLAY = "e-overlay";
        internal const string CLASS = "class";

        private Dictionary<string, object> containerAttr = new Dictionary<string, object>();

        internal CalendarBaseRender<TValue> CalendarBase { get; set; }

        internal CalendarEvents<TValue> CalendarEvents { get; set; }

        protected override string RootClass { get; set; }

        private TimeSpan TimeSpan { get; set; }

        private Type PropertyType { get; set; }

        /// <exclude/>
        internal DateTime CurrentDate { get; set; }

        protected override string ComponentReference { get; set; } = "SfCalendar";
        /// <summary>
        /// Triggers while initial rendering of the component.
        /// </summary>
        /// <returns>Task.</returns>
        protected override async Task OnInitializedAsync()
        {
            PropertyType = typeof(TValue);
            TimeSpan = new TimeSpan(0, 0, 0);
            PreviousDate = Value == null ? default(TValue) : (TValue)SfBaseUtils.ChangeType(Value, PropertyType);

            await base.OnInitializedAsync();
            Calendar_CssClass = CssClass;
            Calendar_Enabled = Enabled;
            CalendarBase_Value = Value;
            CalendarBase_Depth = Depth;
            CalendarBase_Start = Start;
            Calendar_Values = Values;
            CalendarBase_Min = Min;
            CalendarBase_Max = Max;
            CalendarBase_FirstDayOfWeek = FirstDayOfWeek;
            if (string.IsNullOrEmpty(ID))
            {
                ID = "calendar-" + Guid.NewGuid().ToString();
            }

            RootClass = ROOT;
            SetEnabled();
        }

        /// <summary>
        /// Triggers while dynamically changing the properties of the component.
        /// </summary>
        /// <returns>Task.</returns>
        protected override async Task OnParametersSetAsync()
        {
            await base.OnParametersSetAsync();
            await PropertyParametersSet();
            InitRender();
            if (PropertyChanges.Count > 0)
            {
                if (PropertyChanges.ContainsKey(nameof(Values)))
                {
                    await UpdateCalendarProperty(nameof(Value), Values.LastOrDefault());
                }
                if (PropertyChanges.ContainsKey(VALUE))
                {
                    await UpdateCalendarProperty(VALUE, Value);
                }

                if (PropertyChanges.ContainsKey(nameof(CssClass)))
                {
                    RootClass = string.IsNullOrEmpty(RootClass) ? RootClass : SfBaseUtils.RemoveClass(RootClass, Calendar_CssClass);
                    Calendar_CssClass = CssClass;
                }

                if (PropertyChanges.ContainsKey(nameof(Enabled)))
                {
                    Calendar_Enabled = Enabled;
                    SetEnabled();
                }
            }

            SetCssClass();
        }

        /// <summary>
        /// Triggers after the component is rendered.
        /// </summary>
        /// <param name="firstRender">True if the component is rendered for the first time.</param>
        /// <returns>Task.</returns>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await base.OnAfterRenderAsync(firstRender);
            if (firstRender)
            {
                if (EnablePersistence)
                {
                    var localStorageValue = await InvokeMethod<string>("window.localStorage.getItem", false, new object[] { ID });
                    localStorageValue = (string.IsNullOrEmpty(localStorageValue) || localStorageValue == "null") ? null : localStorageValue;
                    if (!(localStorageValue == null && Value != null))
                    {
                        var persistValue = (TValue)SfBaseUtils.ChangeType(localStorageValue, typeof(TValue));
                        NotifyPropertyChanges(nameof(Value), persistValue, CalendarBase_Value);
                        await UpdateCalendarProperty(VALUE, persistValue);
                    }

                    StateHasChanged();
                }

                await SfBaseUtils.InvokeEvent<object>(CalendarEvents?.Created, null);
            }
        }

        /// <summary>
        /// Triggers while dynamically changing the properties of the component.
        /// </summary>
        /// <returns>Task.</returns>
        private async Task PropertyParametersSet()
        {
            CalendarBase_Start = NotifyPropertyChanges(nameof(Start), Start, CalendarBase_Start);
            CalendarBase_Depth = NotifyPropertyChanges(nameof(Depth), Depth, CalendarBase_Depth);
            CalendarBase_FirstDayOfWeek = NotifyPropertyChanges(nameof(FirstDayOfWeek), FirstDayOfWeek, CalendarBase_FirstDayOfWeek);
            CalendarBase_Min = NotifyPropertyChanges(nameof(Min), Min, CalendarBase_Min);
            CalendarBase_Max = NotifyPropertyChanges(nameof(Max), Max, CalendarBase_Max);
            NotifyPropertyChanges(nameof(Enabled), Enabled, Calendar_Enabled);
            NotifyPropertyChanges(nameof(CssClass), CssClass, Calendar_CssClass);
            CalendarBase_Value = this.NotifyPropertyChanges(VALUE, Value, CalendarBase_Value, true);
            Calendar_Values = this.NotifyPropertyChanges(nameof(Values), Values, Calendar_Values, true);
            await Task.CompletedTask;
        }

        private void InitRender()
        {
            RootClass = !(Min <= Max) ? SfBaseUtils.AddClass(RootClass, OVERLAY) :
                SfBaseUtils.RemoveClass(RootClass, OVERLAY);
            if (IsMultiSelection && Values != null && Values.Length > 0)
            {
                Value = GenericValue(Values[Values.Length - 1]);
                PreviousValues = Values.Length;
            }

            RootClass = (DayHeaderFormat == DayHeaderFormats.Wide) ? SfBaseUtils.AddClass(RootClass, DAYHEADERLONG) :
                SfBaseUtils.RemoveClass(RootClass, DAYHEADERLONG);
            RootClass = (EnableRtl || SyncfusionService.options.EnableRtl) ? SfBaseUtils.AddClass(RootClass, RTL) : SfBaseUtils.RemoveClass(RootClass, RTL);
            if (HtmlAttributes != null)
            {
                foreach (var item in HtmlAttributes)
                {
                    if (containerAttributes.IndexOf(item.Key) < 0)
                    {
                        containerAttr = SfBaseUtils.UpdateDictionary(item.Key, item.Value, containerAttr);
                    }
                    else
                    {
                        if (item.Key == CLASS)
                        {
                            RootClass = SfBaseUtils.AddClass(RootClass, item.Value.ToString());
                        }
                        else
                        {
                            containerAttr = SfBaseUtils.UpdateDictionary(item.Key, item.Value, containerAttr);
                        }
                    }
                }
            }

            RootClass = WeekNumber ? SfBaseUtils.AddClass(RootClass, WEEK_NUMBER) : SfBaseUtils.RemoveClass(RootClass, WEEK_NUMBER);
        }

        private void SetCssClass()
        {
            if (!string.IsNullOrEmpty(CssClass))
            {
                RootClass = SfBaseUtils.AddClass(RootClass, CssClass);
            }
        }

        private void SetEnabled()
        {
            if (!Enabled)
            {
                RootClass = SfBaseUtils.AddClass(RootClass, DISABLE);
            }
            else
            {
                RootClass = RootClass.Replace(DISABLE, string.Empty, StringComparison.Ordinal);
            }
        }

        /// <exclude/>
        internal override async Task UpdateCalendarProperty(string key, object dateValue)
        {
            if (Enabled)
            {
                if (key == VALUE)
                {
                    TValue tempValue = (TValue)SfBaseUtils.ChangeType(dateValue, typeof(TValue));
                    Value = CalendarBase_Value = await SfBaseUtils.UpdateProperty(tempValue, CalendarBase_Value, ValueChanged, CalendarEditContext, ValueExpression);
                }
                else
                {
                    Values = Calendar_Values = await SfBaseUtils.UpdateProperty(dateValue as DateTime[], Calendar_Values, ValuesChanged, CalendarEditContext, ValuesExpression);
                }
            }
        }

        internal override void BindNavigateEvent(NavigatedEventArgs eventArgs)
        {
            _ = SfBaseUtils.InvokeEvent<NavigatedEventArgs>(CalendarEvents?.Navigated, eventArgs);
        }

        internal override async Task BindRenderDayEvent(RenderDayCellEventArgs eventArgs)
        {
            await SfBaseUtils.InvokeEvent<RenderDayCellEventArgs>(CalendarEvents?.OnRenderDayCell, eventArgs);
        }

        /// <summary>
        /// Triggers when calendar value is changed.
        /// </summary>
        /// <param name="args">Specifies the event arguments.</param>
        protected override void ChangeEvent(EventArgs args)
        {
            ChangedArgs.Name = VALUECHANGE;
            if (EnablePersistence)
            {
                _ = SetLocalStorage(ID, Value);
            }

            _ = SfBaseUtils.InvokeEvent<ChangedEventArgs<TValue>>(CalendarEvents?.ValueChange, ChangedArgs);
            PreviousDate = (TValue)SfBaseUtils.ChangeType(Value, typeof(TValue));
        }

        internal override async Task InvokeDeSelectEvent(DeSelectedEventArgs<TValue> args)
        {
            await SfBaseUtils.InvokeEvent<DeSelectedEventArgs<TValue>>(CalendarEvents?.DeSelected, args);
        }

        internal override async Task InvokeSelectEvent(SelectedEventArgs<TValue> args)
        {
            await SfBaseUtils.InvokeEvent<SelectedEventArgs<TValue>>(CalendarEvents?.Selected, args);
        }
    }
}