using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSuelen.src.Engine
{
    public class ObjectBase : IDisposable
    {
        // To detect redundant calls
        private bool _disposed = false;

        // Instantiate a SafeHandle instance.
        private SafeHandle _safeHandle = new SafeFileHandle(IntPtr.Zero, true);

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose() => Dispose(true);

        ~ObjectBase() => Dispose(false);

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                // TODO: dispose managed state (managed objects).
                _safeHandle?.Dispose();
            }
            
            OnDispose();

            _disposed = true;
        }
        /// <summary>
        /// Called when is disposed.
        /// TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
        /// TODO: set large fields to null.
        /// </summary>
        protected virtual void OnDispose()
        {

        }
    }
}
