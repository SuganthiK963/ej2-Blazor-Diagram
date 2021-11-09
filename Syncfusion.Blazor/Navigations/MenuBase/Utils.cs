using System.Reflection;

namespace Syncfusion.Blazor.Navigations.Internal
{
    public static class Utils
    {
        public static T GetItemProperties<T, TItem>(TItem item, string propName)
        {
            PropertyInfo prop = item.GetType().GetProperty(propName);
            if (prop != null)
            {
                return (T)prop.GetValue(item);
            }
            else
            {
                return default;
            }
        }
    }
}
