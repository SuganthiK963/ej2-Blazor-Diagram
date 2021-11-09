using System.ComponentModel;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// The interface specifies the properties of the bullet chart tooltip.
    /// </summary>
    public interface IBulletChartTooltip
    {
        /// <summary>
        /// Specifies to update the dependent class value.
        /// </summary>
        /// <param name="key">Represents the class name.</param>
        /// <param name="keyValue">Represents the value of class.</param>
        /// <exclude />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void UpdateChildProperties(string key, object keyValue);
    }
}
