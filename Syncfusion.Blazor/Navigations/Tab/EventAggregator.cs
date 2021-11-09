using System;
using System.Collections.Generic;

namespace Syncfusion.Blazor.Navigations.Internal
{
    internal class EventAggregator
    {
        private Dictionary<string, Action<ToolbarEventArgs>> eventList = new Dictionary<string, Action<ToolbarEventArgs>>();

        public void Notify(string name, ToolbarEventArgs args)
        {
            if (eventList.ContainsKey(name))
            {
                eventList[name].Invoke(args);
            }
        }

        public void Add(string name, Action<ToolbarEventArgs> handler)
        {
            if (!eventList.TryAdd(name, handler))
            {
                eventList[name] = handler;
            }
        }
    }
}