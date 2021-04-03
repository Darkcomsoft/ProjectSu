﻿using Projectsln.darkcomsoft.src.engine;
using System;
using System.Collections.Generic;
using System.Text;

namespace Projectsln.darkcomsoft.src
{
    public abstract class ClassBase : IDisposable
    {
        public ClassBase()
        {
            //GC.KeepAlive(this);
        }

        // To detect redundant calls
        private bool _disposed = false;

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
        }

        // Protected implementation of Dispose pattern.
        private void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
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
            Debug.Log("DISPOSED: " + this.GetType() + " (HASH: " + this.GetHashCode() + ")", "DISPOSESYSTEM");
        }
    }
}
