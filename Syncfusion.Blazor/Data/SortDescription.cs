using System;

namespace Syncfusion.Blazor.Data
{
    /// <summary>
    ///  Defines the direction and the property name to be used as the criteria for
    ///  sorting a collection.
    /// </summary>
    /// <exclude/>
    public struct SortDescription: IEquatable<SortDescription>
    {
        /// <summary>
        /// Initializes a new instance of the System.ComponentModel.SortDescription structure.
        /// </summary>
        /// <param name="propertyName">The name of the property to sort the list by.</param>
        /// <param name="direction">The sort order.</param>
        public SortDescription(string propertyName, ListSortDirection direction)
        {
            this.direction = direction;
            this.propertyName = propertyName;
        }

        /// <summary>
        /// Compares two System.ComponentModel.SortDescription objects for value inequality.
        /// </summary>
        /// <param name="sd1">The first instance to compare.</param>
        /// <param name="sd2">The second instance to compare.</param>
        /// <returns>bool.</returns>
        public static bool operator !=(SortDescription sd1, SortDescription sd2)
        {
            return !sd1.Equals(sd2);
        }

        /// <summary>
        /// Compares two System.ComponentModel.SortDescription objects for value equality.
        /// </summary>
        /// <param name="sd1">The first instance to compare.</param>
        /// <param name="sd2">The second instance to compare.</param>
        /// <returns>true.</returns>
        public static bool operator ==(SortDescription sd1, SortDescription sd2)
        {
            return sd1.Equals(sd2);
        }

        private ListSortDirection direction;

        /// <summary>
        /// Gets or sets a value that indicates whether to sort in ascending or descending
        ///     order.
        /// </summary>
        public ListSortDirection Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        private string propertyName;

        /// <summary>
        /// Gets or sets the property name being used as the sorting criteria.
        /// </summary>
        public string PropertyName
        {
            get { return propertyName; }
            set { propertyName = value; }
        }

        /// <summary>
        /// Compares the specified instance and the current instance of System.ComponentModel.SortDescription
        ///     for value equality.
        /// </summary>
        /// <param name="obj">The System.ComponentModel.SortDescription instance to compare.</param>
        /// <returns>true.</returns>
        public override bool Equals(object obj)
        {
            return true;
        }
        /// <summary>
        /// Compares the specified instance and the current instance of System.ComponentModel.SortDescription
        ///     for value equality.
        /// </summary>
        /// <param name="other">The System.ComponentModel.SortDescription instance to compare.</param>
        /// <returns>true.</returns>
        public bool Equals(SortDescription other) => true;

        /// <summary>
        /// Returns the hash code.
        /// </summary>
        /// <returns>int.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
