using System;
using System.Collections.Generic;
using System.Linq;
using MPE.SS.Enums;
using MPE.SS.Interfaces;
using MPE.SS.Logic.Configurations;
using MPE.SS.Models;
using MPE.SS.Models.Graphs;
using NLog;

namespace MPE.SS.Logic.Charts
{
    internal abstract class ChartAbstractBase<T> : IChartDataGenerator
        where T : Data<Server>
    {
        protected IBuilder<Chart> _chartBuilder;
        protected IBuilder<DataSet> _dataSetBuilder;
        protected IBuilder<Point> _pointBuilder;
        protected IRepository<T> _repository;
        protected IBaselineService _baselineService;

        private static readonly object _lock = new object();

        protected ChartAbstractBase(
            IBuilder<Chart> chartBuilder,
            IBuilder<DataSet> dataSetBuilder,
            IBuilder<Point> pointBuilder,
            IRepository<T> repository,
            IBaselineService baselineService)
        {
            _chartBuilder = chartBuilder;
            _dataSetBuilder = dataSetBuilder;
            _pointBuilder = pointBuilder;
            _repository = repository;
            _baselineService = baselineService;
        }

        protected abstract Chart DefineChart(List<T> data, string serverName);
        public abstract bool CanHandle(DataCollector type);


        public Chart GenerateChart(string serverName)
        {
            lock (_lock)
            {
                var data = _repository.GetAll().Where(x => x.Reference == serverName).ToList();
                var newest = data.Any() ? data.Max(x => x.CreatedOn) : DateTime.Now;
                data = data.Where(x => x.CreatedOn > newest.AddMinutes(-1 * AppConfiguration.Configuration.DataCollectionIntervalMin)).ToList();

                return DefineChart(data, serverName);
            }
        }

        public void UpdateBaseline(string name, string reference, DataSet set)
        {
            lock (_lock)
            {
                if (set.Points != null)
                {
                    foreach (var point in set.Points)
                    {
                        try
                        {
                            _baselineService.UpdateOrCreateBaseline(reference, name,
                                DateTime.Parse(point.Label.ToString()),
                                long.Parse(point.Value.ToString()));
                        }
                        catch(Exception e)
                        {
                            AppConfiguration.Logger.Log(LogLevel.Fatal, e);
                        }
                    }
                }
            }
        }

        protected DataSet PeakChart(DataSet set)
        {
            lock (_lock)
            {
                long value = 0;
                foreach (var point in set.Points)
                {
                    var prior =
                        set.Points.Where(x => DateTime.Parse(x.Label) < DateTime.Parse(point.Label))
                            .OrderByDescending(x => x.Label)
                            .FirstOrDefault();
                    var pointValue = point.Value;
                    if (prior != null)
                    {
                        point.Value = Convert.ToInt64(point.Value) - Convert.ToInt64(value);
                    }
                    else
                    {
                        point.Value = 0;
                    }
                    value = Convert.ToInt64(pointValue);
                }
                return set;
            }
        }

        protected DataSet CreateDataSet<T>(List<T> data, string name, Color color, Func<T, int> fieldSelection)
            where T : Data<Server>
        {
            lock (_lock)
            {
                _dataSetBuilder.Build();

                var result = data.Select(x => x.CreatedOn.ToString("yyyy-MM-dd HH:mm"))
                        .Distinct()
                        .ToDictionary(x => x,
                            x =>
                                data.Where(z => z.CreatedOn.ToString("yyyy-MM-dd HH:mm") == x)
                                    .Max(fieldSelection.Invoke))
                        .OrderBy(x => x.Key);

                _dataSetBuilder.Where(x => x.Points = result.Select(q =>
                        _pointBuilder.Where(z => z.Label = q.Key)
                            .Where(z => z.Value = q.Value).Build()).ToList())
                    .Where(x => x.Label = name)
                    .Where(x => x.Color = color);

                return _dataSetBuilder.Build();
            }
        }

        protected DataSet CreateBaseline(DataSet set, string name, string reference, string baselineName)
        {
            lock (_lock)
            {
                var baseline = _baselineService.GetBaseline(reference, name);
                if (baseline != null)
                {
                    _dataSetBuilder.Build();
                    _pointBuilder.Build();
                    _dataSetBuilder.Where(x => x.Label = "Base")
                        .Where(x => x.DontFill = true)
                        .Where(x => x.Color = set.Color.Negative())
                        .Where(
                            x =>
                                x.Points =
                                    set.Points.Select(
                                        z =>
                                            _pointBuilder.Where(p => p.Label = z.Label)
                                                .Where(p => p.Value = baseline.Average)
                                                .Build()).ToList());
                    var dataset = _dataSetBuilder.Build();

                    return dataset;
                }
                return null;
            }
        }

        public abstract string ChartName { get; }
    }
}
