#nullable disable // <--- BU SATIR TÜM NULL UYARILARINI KAPATIR VE KODU RAHATLATIR

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace ProjectYazLab
{
    public class Algorithms
    {
        // --- BFS ALGORİTMASI ---
        public async Task RunBFS(Graph graph, Node startNode, Panel drawingPanel, Label timeLabel)
        {
            ResetGraph(graph);
            Queue<Node> queueMeasure = new Queue<Node>();

            Stopwatch sw = new Stopwatch();
            sw.Start();

            startNode.Visited = true;
            queueMeasure.Enqueue(startNode);

            while (queueMeasure.Count > 0)
            {
                Node current = queueMeasure.Dequeue();
                List<Node> neighbors = GetNeighbors(graph, current);

                foreach (Node neighbor in neighbors)
                {
                    if (!neighbor.Visited)
                    {
                        neighbor.Visited = true;
                        queueMeasure.Enqueue(neighbor);
                    }
                }
            }

            sw.Stop();
            timeLabel.Text = ($"BFS Süresi: {sw.Elapsed.TotalMilliseconds} ms ({sw.ElapsedTicks} Ticks)");

            // Görselleştirme kısmı
            ResetGraph(graph);
            Queue<Node> queueVisual = new Queue<Node>();

            startNode.Visited = true;
            startNode.CurrentColor = Color.Orange;
            queueVisual.Enqueue(startNode);
            drawingPanel.Invalidate();

            while (queueVisual.Count > 0)
            {
                Node current = queueVisual.Dequeue();
                current.CurrentColor = Color.LightGreen;
                drawingPanel.Invalidate();

                await Task.Delay(500);

                List<Node> neighbors = GetNeighbors(graph, current);

                foreach (Node neighbor in neighbors)
                {
                    if (!neighbor.Visited)
                    {
                        neighbor.Visited = true;
                        neighbor.CurrentColor = Color.Orange;
                        queueVisual.Enqueue(neighbor);

                        drawingPanel.Invalidate();
                        await Task.Delay(200);
                    }
                }
            }

            MessageBox.Show("BFS Taraması Tamamlandı!");
        }

        // --- YARDIMCI METOTLAR ---
        private List<Node> GetNeighbors(Graph graph, Node node)
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

        private void ResetGraph(Graph graph)
        {
            foreach (var node in graph.Nodes)
            {
                node.Visited = false;
                node.CurrentColor = Color.Blue;
            }
        }

        // --- DFS ALGORİTMASI ---
        public async Task RunDFS(Graph graph, Node startNode, Panel drawingPanel, Label timeLabel)
        {
            ResetGraph(graph);
            List<Node> visitOrder = new List<Node>();
            Stack<Node> stack = new Stack<Node>();

            stack.Push(startNode);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            while (stack.Count > 0)
            {
                Node current = stack.Pop();

                if (!current.Visited)
                {
                    current.Visited = true;
                    visitOrder.Add(current);

                    List<Node> neighbors = GetNeighbors(graph, current);

                    foreach (Node neighbor in neighbors)
                    {
                        if (!neighbor.Visited)
                        {
                            stack.Push(neighbor);
                        }
                    }
                }
            }

            stopwatch.Stop();
            timeLabel.Text = ($"DFS Süresi: {stopwatch.Elapsed.TotalMilliseconds} ms ({stopwatch.ElapsedTicks} Ticks)");

            foreach (var node in visitOrder)
            {
                node.CurrentColor = Color.Orange;
                drawingPanel.Invalidate();
                await Task.Delay(500);
            }

            MessageBox.Show("DFS Taraması Tamamlandı!");
        }

        // --- DIJKSTRA ALGORİTMASI ---
        public async Task RunDijkstra(Graph graph, Node startNode, Node endNode, Panel drawingPanel, Label timeLabel)
        {
            ResetGraph(graph);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var distances = new Dictionary<Node, double>();
            var previousNodes = new Dictionary<Node, Node>(); // ? işaretlerine gerek kalmadı
            var unvisited = new List<Node>();

            foreach (var node in graph.Nodes)
            {
                distances[node] = double.MaxValue;
                previousNodes[node] = null;
                unvisited.Add(node);
            }

            distances[startNode] = 0;

            while (unvisited.Count > 0)
            {
                unvisited.Sort((x, y) => distances[x].CompareTo(distances[y]));
                var currentNode = unvisited[0];
                unvisited.Remove(currentNode);

                if (currentNode == endNode || distances[currentNode] == double.MaxValue)
                {
                    break;
                }

                foreach (var edge in graph.Edges)
                {
                    Node neighbor = null;

                    if (edge.Source == currentNode)
                    {
                        neighbor = edge.Target;
                    }
                    else if (edge.Target == currentNode)
                        neighbor = edge.Source;

                    if (neighbor != null && unvisited.Contains(neighbor))
                    {
                        double newDist = distances[currentNode] + edge.Weight;
                        if (newDist < distances[neighbor])
                        {
                            distances[neighbor] = newDist;
                            previousNodes[neighbor] = currentNode;
                        }
                    }
                }
            }
            stopwatch.Stop();
            timeLabel.Text = ($"Dijkstra Süresi: {stopwatch.Elapsed.TotalMilliseconds} ms ({stopwatch.ElapsedTicks} Ticks)");

            if (!previousNodes.ContainsKey(endNode))
            {
                MessageBox.Show("HATA: Hedef düğüme ulaşılamadı veya düğüm grafiğin parçası değil.");
                return;
            }

            if (previousNodes[endNode] == null && startNode != endNode)
            {
                MessageBox.Show("Bu iki düğüm arasında bağlantı yok!");
                return;
            }

            var path = new List<Node>();
            var tempNode = endNode;

            while (tempNode != null)
            {
                path.Insert(0, tempNode);
                if (previousNodes.ContainsKey(tempNode))
                {
                    tempNode = previousNodes[tempNode];
                }
                else
                {
                    tempNode = null;
                }
            }

            foreach (var node in path)
            {
                node.CurrentColor = Color.Purple;
                drawingPanel.Invalidate();
                await Task.Delay(500);
            }

            double totalCost = distances[endNode];
            MessageBox.Show($"En Kısa Yol Bulundu!\nToplam Maliyet: {totalCost:0.00}");
        }

        // ============================================================
        // A* (A STAR) KISMI
        // ============================================================

        private double Heuristic(Node a, Node b)
        {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }

        public async Task RunAStar(Graph graph, Node startNode, Node endNode, Panel drawingPanel, Label timeLabel)
        {
            ResetGraph(graph);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var gScore = new Dictionary<Node, double>();
            var fScore = new Dictionary<Node, double>();
            var previousNodes = new Dictionary<Node, Node>(); // Disable sayesinde ? gerek yok

            var openSet = new List<Node>();

            foreach (var node in graph.Nodes)
            {
                gScore[node] = double.MaxValue;
                fScore[node] = double.MaxValue;
                previousNodes[node] = null;
            }

            gScore[startNode] = 0;
            fScore[startNode] = Heuristic(startNode, endNode);
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                openSet.Sort((x, y) => fScore[x].CompareTo(fScore[y]));
                Node current = openSet[0];

                if (current == endNode)
                {
                    break;
                }

                openSet.Remove(current);
                List<Node> neighbors = GetNeighbors(graph, current);

                foreach (Node neighbor in neighbors)
                {
                    var edge = graph.Edges.FirstOrDefault(e =>
                        (e.Source == current && e.Target == neighbor) ||
                        (e.Source == neighbor && e.Target == current));

                    if (edge != null)
                    {
                        double tentative_gScore = gScore[current] + edge.Weight;

                        if (tentative_gScore < gScore[neighbor])
                        {
                            previousNodes[neighbor] = current;
                            gScore[neighbor] = tentative_gScore;
                            fScore[neighbor] = gScore[neighbor] + Heuristic(neighbor, endNode);

                            if (!openSet.Contains(neighbor))
                            {
                                openSet.Add(neighbor);
                            }
                        }
                    }
                }
            }

            stopwatch.Stop();
            timeLabel.Text = ($"A* Süresi: {stopwatch.Elapsed.TotalMilliseconds} ms ({stopwatch.ElapsedTicks} Ticks)");

            // --- YOLU ÇİZME ---

            if (!previousNodes.ContainsKey(endNode) || (previousNodes[endNode] == null && startNode != endNode))
            {
                MessageBox.Show("Hedefe ulaşılamadı!");
                return;
            }

            var path = new List<Node>();
            var tempNode = endNode;

            while (tempNode != null)
            {
                path.Insert(0, tempNode);
                tempNode = previousNodes[tempNode];
            }

            foreach (var node in path)
            {
                node.CurrentColor = Color.Purple;
                drawingPanel.Invalidate();
                await Task.Delay(500);
            }

            double totalCost = gScore[endNode];
            MessageBox.Show($"A* ile En Kısa Yol Bulundu!\nToplam Maliyet: {totalCost:0.00}");
        }
    }
}