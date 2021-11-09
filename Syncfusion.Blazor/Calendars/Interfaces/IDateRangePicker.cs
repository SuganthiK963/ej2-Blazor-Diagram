using System.Threading.Tasks;

namespace Syncfusion.Blazor.Calendars
{
    /// <summary>
    /// Interface for <see cref="IDateRangePicker" />.
    /// </summary>
    public interface IDateRangePicker
    {
        /// <summary>
        /// Method updates the children properties of the component.
        /// </summary>
        /// <param name="presetValue">Specifies the value of preset.</param>
        public void UpdateChildProperties(object presetValue);

        /// <summary>
        /// Task which invokes statehaschanged asynchronous method.
        /// </summary>
        /// <returns>The <see cref="Task"/>.</returns>
        public Task CallStateHasChangedAsync();
    }
}
