using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSuelen.src.Engine
{
    public class ClassBase : IDisposable
    {
        // To detect redundant calls
        private bool _disposed = false;

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
        }

        ~ClassBase() => Dispose(false);

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
                GC.SuppressFinalize(this);
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
