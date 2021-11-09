using System.ComponentModel;

namespace Syncfusion.Blazor.Charts
{
    /// <summary>
    /// The interface specifies the properties of the spark line tooltip.
    /// </summary>
    public interface ISparklineTooltip
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