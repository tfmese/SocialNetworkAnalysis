using System.Collections.Generic;
using System.Drawing;

namespace ProjectYazLab
{
    // Analiz algoritmaları için abstract class
    // Ortak metodları buraya koydum
    public abstract class AbstractGraphAnalyzer : IGraphAnalyzer
    {
        // Her analiz algoritması bunu kendi şekilde yazacak
        public abstract void Analyze(Graph graph, object resultContainer);

        // Komşu düğümleri bulma - analiz algoritmaları da kullanıyor
        protected List<Node> GetNeighbors(Graph graph, Node node)
        {
            List<Node> neighbors = new List<Node>();

            foreach (var edge in graph.Edges)
            {
                if (edge.Source == node)
                {
                    neighbors.Add(edge.Target);
                }
                else if (edge.Target == node)
                {
                    neighbors.Add(edge.Source);
                }
            }
            return neighbors;
        }

        // Grafı sıfırlama
        protected void ResetGraph(Graph graph)
        {
            foreach (var node in graph.Nodes)
            {
                node.Visited = false;
                node.CurrentColor = Color.Blue;
            }
        }
    }
}

