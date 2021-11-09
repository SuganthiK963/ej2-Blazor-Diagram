using System;
using System.Text;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Components;

namespace Syncfusion.Blazor.Internal
{
    /// <summary>
    /// An interface for implementing JSInteropAdaptor.
    /// </summary>
    public interface IJSInteropAdaptor : IDisposable
    {
        public void Init();

        public DotNetObjectReference<object> Create();

        public DotNetObjectReference<object> GetRef();
    }

    /// <summary>
    /// Custom handler of JSInterop to invoke the JavaScript methods with DotNetObjectReference.
    /// </summary>
    public class JSInteropAdaptor : ComponentBase, IJSInteropAdaptor
    {
        public void Init() => _dotnetRef = Create();

        private DotNetObjectReference<object> _dotnetRef { get; set; }

        public virtual DotNetObjectReference<object> Create() => DotNetObjectReference.Create<object>(this);

        public DotNetObjectReference<object> GetRef() => _dotnetRef ?? Create();

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _dotnetRef?.Dispose();
            }
        }
    }
}
