using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPE.SS.Models;

namespace MPE.SS.Logic.Rendering.ViewModels
{
    public class View<T>
        where T : Report
    {
        public T Model { get; set; }
        public List<string> Scripts { get; set; }
        public List<string> Links { get; set; }

        public string GenerateNodesString()
        {
            var nodes = Model.Graph.Nodes
                .Select(x =>
                {
                    var summary = new List<string>();
                    summary.Add(x.Name);

                    if (x.Summaries != null)
                        foreach (var sum in x.Summaries)
                        {
                            summary.Add("");
                            summary.Add(string.Format("<b>{0}</b>", sum.Header));
                            summary.AddRange(sum.Items);
                        }

                    return string.Format("{{ data: {{ id: '{0}', parent: '{2}' }}, scratch: {{data: '{1}' }} }}"
                        , x.DisplayName
                        , string.Join("<br/>", summary)
                        , x.Section);
                }).ToList();
            return string.Join(",", nodes);
        }

        public string GenerateEdgesString()
        {
            var edges = Model.Graph.Edges.Select(x => string.Format("{{ data: {{ source: '{0}', target: '{1}' }} }}", x.Source, x.Target));
            return string.Join(",", edges);
        }
    }
}
