using System;
using System.Collections.Generic;

namespace Syncfusion.Blazor.SplitButtons.Internal
{
    public static class Utils
    {
        private const string ID = "id";
        private const string TYPE = "type";
        private const string BTN = "button";

        public static Dictionary<string, object> GetBtnAttributes(Dictionary<string, object> attr, string compCls, string id)
        {
            if (attr == null)
            {
                attr = new Dictionary<string, object>();
            }

            if (!attr.ContainsKey(ID))
            {
                attr.Add(ID, string.IsNullOrEmpty(id) ? compCls + Guid.NewGuid().ToString() : id);
            }

            if (attr.ContainsKey(ID))
            {
                attr[ID] = string.IsNullOrEmpty((string)attr[ID]) ? compCls + Guid.NewGuid().ToString() : (string)attr[ID];
            }

            if (!attr.ContainsKey(TYPE))
            {
                attr.Add(TYPE, BTN);
            }

            return attr;
        }
    }
}