using System.Collections.Generic;
using System.Linq;
using MPE.SS.Interfaces;

namespace MPE.SS.Logic.Workers
{
    internal class Worker : IMainWorker
    {
       
        private List<IWorker> _workers;

        public Worker(
            
            IEnumerable<IWorker> workers)
        {
            
            _workers = workers.ToList();
        }

        public void Start()
        {
            foreach (var worker in _workers)
            {
                worker.Start();
            }
        }

        public void Stop()
        {
            foreach (var worker in _workers)
            {
                try
                {
                    worker.Stop();
                }
                catch { }
            }
            PowerShellService.Dispose();
        }
    }
}
