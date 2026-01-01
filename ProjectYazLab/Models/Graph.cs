using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectYazLab.Models
{
    public class Graph
    {
        public List<Node> Nodes { get; set; } = new List<Node>();
        public List<Edge> Edges { get; set; } = new List<Edge>();

        public void AddNode(Node node)
        {
            Nodes.Add(node);
        }

        public void AddEdge(Node source, Node target)
        {
            //self-loop engelle
            if (source == target) return;

            // Duplicate edge kontrolü - aynı kenar zaten varsa ekleme
            bool edgeExists = Edges.Exists(e =>
                (e.Source == source && e.Target == target) ||
                (e.Source == target && e.Target == source));

            if (!edgeExists)
            {
                Edges.Add(new Edge(source, target));
            }
        }
    }
}

