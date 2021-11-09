using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Runtime.Serialization;
using System.Dynamic;

namespace Syncfusion.Blazor.Data
{
    /// <summary>
    /// Specifies the FilterType to be used in LINQ methods.
    /// </summary>
    public enum FilterType
    {
        /// <summary>
        /// Performs LessThan operation.
        /// </summary>
        LessThan,

        /// <summary>
        /// Performs LessThan Or Equal operation.
        /// </summary>
        LessThanOrEqual,

        /// <summary>
        /// Checks Equals on the operands.
        /// </summary>
        Equals,

        /// <summary>
        /// Checks for Not Equals on the operands.
        /// </summary>
        NotEquals,

        /// <summary>
        /// Checks for Greater Than or Equal on the operands.
        /// </summary>
        GreaterThanOrEqual,

        /// <summary>
        /// Checks for Greater Than on the operands.
        /// </summary>
        GreaterThan,

        /// <summary>
        /// Checks for StartsWith on the string operands.
        /// </summary>
        StartsWith,

        /// <summary>
        /// Checks for EndsWith on the string operands.
        /// </summary>
        EndsWith,

        /// <summary>
        /// Checks for Contains on the string operands.
        /// </summary>
        Contains,

        /// <summary>
        /// Returns invalid type
        /// </summary>
        Undefined,

        /// <summary>
        /// Checks for Between two date on the operands.
        /// </summary>
        Between
    }

    /// <summary>
    /// Specifies the Filter Behaviour for the filter predicates.
    /// </summary>
    public enum FilterBehavior
    {
        /// <summary>
        /// Parses only StronglyTyped values.
        /// </summary>
        StronglyTyped,

        /// <summary>
        /// Parses all values by converting them as string.
        /// </summary>
        StringTyped
    }

    /// <summary>
    /// Specifies the Filter Behaviour for the filter predicates.
    /// </summary>
    public enum ColumnFilter
    {
        /// <summary>
        /// Parses only StronglyTyped values.
        /// </summary>
        Value,

        /// <summary>
        /// Parses all values by converting them as string.
        /// </summary>
        DisplayText
    }

    /// <summary>
    /// Defines the sort column.
    /// </summary>
    public class SortColumn
    {
        public SortColumn()
        {
            SortDirection = ListSortDirection.Ascending;
        }

        /// <summary>
        /// Specifies the column name.
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// Specifies the sort direction.
        /// </summary>
        public ListSortDirection SortDirection { get; set; }
    }

    /// <summary>
    /// Defines the dynamic class.
    /// </summary>
    /// <exclude/>
    public abstract class DynamicClass
    {
        public override string ToString()
        {
           PropertyInfo[] props = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
           StringBuilder sb = new StringBuilder();
           sb.Append('{');
           for (int i = 0; i < props.Length; i++)
            {
                if (i > 0)
                {
                    sb.Append(", ");
                }

                sb.Append(props[i].Name);
                sb.Append('=');
                sb.Append(props[i].GetValue(this, null));
            }

           sb.Append('}');
           return sb.ToString();
        }
    }

    /// <summary>
    /// Specifies the dynamic property.
    /// </summary>
    /// <exclude/>
    public class DynamicProperty
    {
        private string name;
        private Type type;

        public DynamicProperty(string name, Type type)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            this.name = name;
            this.type = type;
        }

        public string Name
        {
            get { return name; }
        }

        public Type Type
        {
            get { return type; }
        }
    }

    internal class Signature : IEquatable<Signature>
    {
        public int hashCode;
        public DynamicProperty[] properties;

        public Signature(IEnumerable<DynamicProperty> properties)
        {
            this.properties = properties.ToArray();
            hashCode = 0;
            foreach (DynamicProperty p in properties)
            {
                hashCode ^= p.Name.GetHashCode(StringComparison.Ordinal) ^ p.Type.GetHashCode();
            }
        }

        public bool Equals(Signature other)
        {
            if (properties.Length != other.properties.Length)
            {
                return false;
            }

            for (int i = 0; i < properties.Length; i++)
            {
                if (properties[i].Name != other.properties[i].Name ||
                    properties[i].Type != other.properties[i].Type)
                {
                    return false;
                }
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            return obj is Signature ? Equals((Signature)obj) : false;
        }

        public override int GetHashCode()
        {
            return hashCode;
        }
    }

    internal class EnumerationValue
    {
        internal static object GetValueFromEnumMember(string description, Type EnumType)
        {
            var type = EnumType;
            if (!type.IsEnum)
            {
                throw new InvalidOperationException();
            }

            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(
                    field,
                    typeof(EnumMemberAttribute)) as EnumMemberAttribute;
                if (attribute != null)
                {
                    if (attribute.Value == description)
                    {
                        return field.GetValue(null);
                    }
                }
                else
                {
                    if (field.Name == description)
                    {
                        return field.GetValue(null);
                    }
                }
            }

            return null;
        }
    }

    internal class DataSetMemberBinder : SetMemberBinder
    {
        public DataSetMemberBinder(string name, bool ignoreCase)
            : base(name, ignoreCase)
        {
        }

        public override DynamicMetaObject FallbackSetMember(DynamicMetaObject target, DynamicMetaObject value, DynamicMetaObject errorSuggestion)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Defines the group context class.
    /// </summary>
    /// <exclude/>
    public class GroupContext
    {
        public GroupContext()
        {
        }

        public List<GroupContext> ChildGroups { get; set; }

        public int Count { get; set; }

        public IEnumerable Details { get; set; }

        public object Key
        {
            get
            {
                if (Details != null)
                {
                    var type = Details.GetType();
                    var value = type.GetProperties().Where(pi => pi.Name == "Key").FirstOrDefault();
                    var result = value.GetValue(Details, null);
                    return result;
                }

                return null;
            }
        }
    }
}