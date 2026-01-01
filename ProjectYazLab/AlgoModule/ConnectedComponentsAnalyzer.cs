using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using ProjectYazLab.Models;

namespace ProjectYazLab.AlgoModule
{
    // Bağlı bileşenleri bulan sınıf
    // AbstractGraphAnalyzer'dan türetiyoruzz
    public class ConnectedComponentsAnalyzer : AbstractGraphAnalyzer
    {
        public override void Analyze(Graph graph, object resultContainer, Label timeLabel = null)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            ResetGraph(graph);

            List<List<Node>> components = new List<List<Node>>();

            foreach (var node in graph.Nodes)
            {
                if (!node.Visited)
                {
                    
                    List<Node> newComponent = new List<Node>();
                    Queue<Node> queue = new Queue<Node>();

                    node.Visited = true;
                    queue.Enqueue(node);
                    newComponent.Add(node);

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

            // Sonuçları resultContainer'a yazıyoruz
            if (resultContainer is List<List<Node>> resultList)
            {
                resultList.Clear();
                resultList.AddRange(components);
            }

            stopwatch.Stop();
            if (timeLabel != null)
            {
                timeLabel.Text = $"Connected Components Süresi: {stopwatch.Elapsed.TotalMilliseconds:F3} ms ({stopwatch.ElapsedTicks} Ticks)";
            }
        }
    }
}

