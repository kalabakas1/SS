using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MPE.SS.Interfaces;
using MPE.SS.Models;
using MPE.SS.Models.Graphs;

namespace MPE.SS.Logic.Rendering.Graphs
{
    internal class GraphRenderService : IGraphRenderService
    {
        private IBuilder<Graph> _graphBuilder;
        private IBuilder<Node> _nodeBuilder;
        public GraphRenderService(
            IBuilder<Graph> graphBuilder,
            IBuilder<Node> nodeBuilder)
        {
            _graphBuilder = graphBuilder;
            _nodeBuilder = nodeBuilder;
        }

        public Graph GenerateGraph(Configuration configuration)
        {
            return _graphBuilder
                .Where(x => x.Nodes = GetNodes(configuration.Servers))
                .Where(x => x.Edges = GetEdges(configuration.Servers))
                .Build();
        }

        public List<Node> GetNodes(List<Server> servers)
        {
            var nodes = new List<Node>();
            foreach (var server in servers)
            {
                nodes.Add(Node.Convert(server));
            }

            var areas = servers.Where(x => !string.IsNullOrEmpty(x.Label)).Select(x => x.Label).Distinct().ToList();
            foreach (var area in areas)
            {
                nodes.Add(_nodeBuilder.Where(x => x.DisplayName = area).Build());
            }

            return nodes;
        }

        public List<Edge> GetEdges(List<Server> servers)
        {
            var edges = new List<Edge>();
            foreach (var server in servers)
            {
                if (server.RequiredConnections != null
                    && server.RequiredConnections.Any())
                {
                    foreach (var serverRequiredConnection in server.RequiredConnections)
                    {
                        var targetServer = servers.FirstOrDefault(x => x.Name == serverRequiredConnection.Host
                            || x.Label == serverRequiredConnection.Host);
                        if (targetServer != null)
                        {
                            string target;
                            string source;
                            if (server.Label == targetServer.Label)
                            {
                                source = server.DisplayName;
                                target = targetServer.DisplayName;
                            }
                            else
                            {
                                target = string.IsNullOrEmpty(targetServer.Label) ? targetServer.DisplayName : targetServer.Label;
                                source = string.IsNullOrEmpty(server.Label) ? server.DisplayName : server.Label;                                
                            }
                            if (!edges.Any(x => x.Source == source && x.Target == target))
                                edges.Add(new Edge
                                {
                                    Source = source,
                                    Target = target
                                });
                        }
                    }
                }
            }
            return edges;
        }
    }
}
