using System.Collections.Generic;
using System.Linq;
using ProjectYazLab.Models;

namespace ProjectYazLab.AlgoModule
{
    // Bağlı bileşenleri bulan sınıf
    // AbstractGraphAnalyzer'dan türetiyorum
    public class ConnectedComponentsAnalyzer : AbstractGraphAnalyzer
    {
        public override void Analyze(Graph graph, object resultContainer)
        {
            ResetGraph(graph);

            List<List<Node>> components = new List<List<Node>>();

            foreach (var node in graph.Nodes)
            {
                if (!node.Visited)
                {
                    // Yeni bir bileşen buldum
                    List<Node> newComponent = new List<Node>();
                    Queue<Node> queue = new Queue<Node>();

                    node.Visited = true;
                    queue.Enqueue(node);
                    newComponent.Add(node);

                    // BFS ile bu bileşendeki tüm düğümleri buluyorum
                    while (queue.Count > 0)
                    {
                        Node current = queue.Dequeue();
                        List<Node> neighbors = GetNeighbors(graph, current);

                        foreach (var neighbor in neighbors)
                        {
                            if (!neighbor.Visited)
                            {
                                neighbor.Visited = true;
                                newComponent.Add(neighbor);
                                queue.Enqueue(neighbor);
                            }
                        }
                    }

                    components.Add(newComponent);
                }
            }

            // Sonuçları resultContainer'a yazıyorum
            if (resultContainer is List<List<Node>> resultList)
            {
                resultList.Clear();
                resultList.AddRange(components);
            }
        }
    }
}

