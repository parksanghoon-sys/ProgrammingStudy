using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace cliCustomSynchronizationContext
{
    internal class SimpleStaSynchronizationContext : SynchronizationContext, IDisposable
    {
        private BlockingCollection<Action> _workingCollection;
        private Thread _workerThread;
        private bool _isDisposed;

        private static uint _copyCount;
        public SimpleStaSynchronizationContext()
            : this("Noname")
        {

        }
        public SimpleStaSynchronizationContext(string contextName)
        {
            _workingCollection = new();
            _workerThread = new Thread(DoWork)
            {
                Name = contextName
            };
            _workerThread.Start();
        }
        public bool IsInWorker
        {
            get { return Thread.CurrentThread.ManagedThreadId == _workerThread.ManagedThreadId; }
        }
        private void DoWork()
        {
            SynchronizationContext.SetSynchronizationContext(this);
            while (_isDisposed == false)
            {
                try
                {
                    Action item;
                    if (_workingCollection.TryTake(out item))
                    {
                        //SynchronizationContext.SetSynchronizationContext(this);
                        item();
                    }
                }
                catch (Exception ex)
                {

                    Debug.WriteLine("Worker exception :" + ex);
                }
            }
            Console.WriteLine("Worker Stop");
        }
        public override void Post(SendOrPostCallback d, object? state)
        {
            try
            {
                _workingCollection.TryAdd(() => d(state));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Post Exection : {ex}");
            }
        }
        public override void Send(SendOrPostCallback d, object? state)
        {
            try
            {
                var future = new TaskCompletionSource<bool>();
                _workingCollection.TryAdd(() => WaitForWorkDon(d, state, future));
                future.Task.Wait();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Send Exection : {ex}");
            }
        }
        public override SynchronizationContext CreateCopy()
        {
            _copyCount++;
            return new SimpleStaSynchronizationContext("Copy context_" + _copyCount);
        }


        private void WaitForWorkDon(SendOrPostCallback d, object? state, TaskCompletionSource<bool> future)
        {
            try
            {
                d(state);
                future.SetResult(true);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"DoWork Exection by Send : {ex}");
                future.SetException(ex);
            }
        }
        ~SimpleStaSynchronizationContext()
        {
            Console.WriteLine("SyncContext Finalizer");
            DoDispose(false);
        }
        public void Dispose()
        {
            Console.WriteLine("Sync Dispose");
            DoDispose(true);
            GC.SuppressFinalize(this);
        }
        private void DoDispose(bool disposing)
        {
            try
            {
                if (_isDisposed) { return; }
                if (disposing == false)
                    return;
                this.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("SyncContext dispose ex" + ex);
                throw;
            }
            finally
            {
                Console.WriteLine("Disposed");
            }
        }

        private void Close()
        {
            _workerThread.Abort();
            _workerThread = null;

            _workingCollection.Dispose();
            _workingCollection = null;
            _isDisposed = true;
        }


    }
}
