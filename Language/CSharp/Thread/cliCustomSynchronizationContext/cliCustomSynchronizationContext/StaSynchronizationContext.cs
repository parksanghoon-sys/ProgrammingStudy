using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace cliCustomSynchronizationContext
{
    public class StaSynchronizationContext
    {
        private static readonly object _lockObject = new object();
        private static StaSynchronizationContext _instance;
        private SimpleStaSynchronizationContext SyncContext { get; set; }

        private StaSynchronizationContext()
        {
            SyncContext = new SimpleStaSynchronizationContext("StaSyncContext");
        }

        private static StaSynchronizationContext Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObject)
                    {
                        if (_instance == null)
                        {
                            _instance = new StaSynchronizationContext();
                        }
                    }
                }

                return _instance;
            }
        }

        public static bool IsInWorker
        {
            get { return Instance.SyncContext.IsInWorker; }
        }

        public static void Post(SendOrPostCallback d, object state)
        {
            Instance.SyncContext.Post(d, state);
        }

        public static void Post(SendOrPostCallback d)
        {
            Instance.SyncContext.Post(d, null);
        }

        public static void Send(SendOrPostCallback d, object state)
        {
            Instance.SyncContext.Send(d, state);
        }

        public static void Send(SendOrPostCallback d)
        {
            Instance.SyncContext.Send(d, null);
        }

        public static void Release()
        {
            Instance.SyncContext.Dispose();
        }
    }    
}
