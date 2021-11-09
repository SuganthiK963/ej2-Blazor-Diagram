using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.DataVizCommon
{
    public class SvgClass : ComponentBase
    {
        [Parameter]
#pragma warning disable CA1716
        public virtual string Class { get; set; } = string.Empty;
#pragma warning restore CA1716

        [Parameter]
        public virtual string Id { get; set; }

        internal virtual void ChangeClass(string classname, bool? isAdd, bool isReset = false)
        {
            Class = Class?.Trim();
            if (isReset)
            {
                Class = classname;
            }
            else
            {
                if (isAdd == true && !Class.Contains(classname, System.StringComparison.Ordinal))
                {
                    Class += string.IsNullOrEmpty(Class) ? classname : " " + classname;
                }
                else if (isAdd == false && !string.IsNullOrEmpty(Class) && !string.IsNullOrEmpty(classname))
                {
                    Class = Class.Replace(classname, string.Empty, System.StringComparison.Ordinal);
                }
            }
            StateHasChanged();
        }
    }
}