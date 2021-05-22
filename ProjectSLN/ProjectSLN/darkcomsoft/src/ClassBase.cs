using ProjectSLN.darkcomsoft.src.debug;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectSLN.darkcomsoft.src
{
    public abstract class ClassBase : IDisposable
    {
        private bool m_showdebugDispose = true;

        public ClassBase()
        {
            //GC.KeepAlive(this);
        }

        ~ClassBase()
        {
            Dispose(false);
        }

        private bool m_isdisposedByTheGameEngine = false;

        // To detect redundant calls
        private bool _disposed = false;

        // Public implementation of Dispose pattern callable by consumers.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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
                //Dispose managed resources
                OnDispose();
                m_isdisposedByTheGameEngine = true;
            }

            /// free unmanaged resources (unmanaged objects) and override a finalizer below.
            /// set large fields to null.
            /// 
            /// If the Dispose() isn't called by the game engine, ts gona be called by the Finalizer
            if (!m_isdisposedByTheGameEngine)
            {
                Debug.Log("DISPOSED BY THE FINALIZER!!!", "DISPOSESYSTEM");
                OnDispose();
            }

            _disposed = true;
        }

        /// <summary>
        /// Called when is disposed.
        /// </summary>
        protected virtual void OnDispose()
        {
            if (m_showdebugDispose)
            {
                Debug.Log("DISPOSED: " + this.GetType() + " (HASH: " + this.GetHashCode() + ")", "DISPOSESYSTEM");
            }
        }

        /// <summary>
        /// if is true when this object is disposed is Write a debug text on console, Default=True
        /// </summary>
        protected bool ShowDisposeDebugMsg { get { return m_showdebugDispose; } set { m_showdebugDispose = value; } }
    }
}
