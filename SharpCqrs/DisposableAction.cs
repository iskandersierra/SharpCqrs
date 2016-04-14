using System;

namespace SharpCqrs
{
    public class DisposableAction : IDisposable
    {
        private byte state = 0; // 0, 1, 2
        private readonly Action disposeAction;

        public DisposableAction(Action disposeAction) 
            : this(() => { }, disposeAction)
        {
        }

        public DisposableAction(Action initializeAction, Action disposeAction)
        {
            if (initializeAction == null) throw new ArgumentNullException(nameof(initializeAction));
            if (disposeAction == null) throw new ArgumentNullException(nameof(disposeAction));

            this.disposeAction = disposeAction;

            state = 0;

            initializeAction();

            state = 1;
        }

        public void Dispose()
        {
            if (IsActive)
            {
                try
                {
                    state = 2;

                    disposeAction();

                    GC.SuppressFinalize(this);
                }
                catch (Exception ex)
                {
                    DisposingException = ex;

                    GC.SuppressFinalize(this);

                    throw;
                }
            }
        }

        public bool IsInitialized => state >= 1;
        public bool IsActive => state == 1;
        public bool IsDisposed => state == 2;
        public Exception DisposingException { get; private set; }
    }
}
