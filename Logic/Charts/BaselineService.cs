using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPE.SS.Interfaces;
using MPE.SS.Logic.Configurations;
using MPE.SS.Models;

namespace MPE.SS.Logic.Charts
{
    internal class BaselineService : IBaselineService
    {
        private IRepository<Baseline> _baselineRepository;
        private IBuilder<Baseline> _baselineBuilder;
        public BaselineService(
            IRepository<Baseline> baselineRepository,
            IBuilder<Baseline> baselineBuilder)
        {
            _baselineRepository = baselineRepository;
            _baselineBuilder = baselineBuilder;
        }

        public void UpdateOrCreateBaseline(string server, string chartAlias, DateTime dataCollectionTimeStamp, long value)
        {
            var existing = GetBaseline(server, chartAlias);
            if (existing == null)
            {
                existing = _baselineBuilder
                    .Where(x => x.Server = server)
                    .Where(x => x.ChartAlias = chartAlias)
                    .Where(x => x.Collections = new Dictionary<DateTime, long>()).Build();
            }

            if (!existing.Collections.ContainsKey(dataCollectionTimeStamp))
            {
                existing.Collections.Add(dataCollectionTimeStamp, value);
                var data =
                    existing.Collections.Where(
                            x =>
                                x.Key >=
                                DateTime.Now.AddMinutes(-1 * AppConfiguration.Configuration.BaselineRenderingIntervalInMin))
                                .OrderByDescending(x => x.Value).ToList();

                int count = (data.Count * 75 / 100);
                data = data.Take(count).ToList();
                if (data.Count > 0)
                    existing.Average = data.Sum(x => x.Value) / data.Count();

                _baselineRepository.Save(existing);
            }
        }

        public Baseline GetBaseline(string server, string chartAlias)
        {
            return _baselineRepository.GetAll()
                    .FirstOrDefault(
                        x => x.Server == server
                            && x.ChartAlias == chartAlias);
        }
    }
}
