using System.Collections.Generic;
using System.Linq;

namespace MPE.SS.Models.Graphs
{
    public class Node
    {
        public string Section { get; set; }
        public string DisplayName { get; set; }
        public string Name { get; set; }

        public List<NodeSummarySection> Summaries { get; set; }

        public static Node Convert(Server server)
        {
            var node = new Node
            {
                Section = server.Label,
                Name = server.Name,
                DisplayName = server.DisplayName,
                Summaries = new List<NodeSummarySection>()
            };

            if (server.Services != null && server.Services.Any())
            {
                node.Summaries.Add(new NodeSummarySection
                {
                    Header = "Services",
                    Items = server.Services.ToList()
                });
            }

            if (server.AppPools != null && server.AppPools.Any())
            {
                node.Summaries.Add(new NodeSummarySection
                {
                    Header = "AppPools",
                    Items = server.AppPools.ToList()
                });
            }

            if (server.ScheduledTasks != null && server.ScheduledTasks.Any())
            {
                node.Summaries.Add(new NodeSummarySection
                {
                    Header = "Scheduled tasks",
                    Items = server.ScheduledTasks.ToList()
                });
            }

            if (server.RequiredConnections != null && server.RequiredConnections.Any())
            {
                node.Summaries.Add(new NodeSummarySection
                {
                    Header = "Connections",
                    Items = server.RequiredConnections.Select(x => string.Format("{0}:{1}", x.Host, x.Port)).ToList()
                });
            }

            return node;
        }
    }
}
