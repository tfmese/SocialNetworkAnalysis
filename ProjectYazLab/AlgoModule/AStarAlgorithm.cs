using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProjectYazLab.Models;

namespace ProjectYazLab.AlgoModule
{
    
    public class AStarAlgorithm : AbstractPathfindingAlgorithm
    {
        // Dijkstra'dan farkı: Bir sonraki düğümü seçerken sadece gidilen yola (g) değil,
        // hedefe kalan tahmini mesafeye de (h) bakar.
        // Formül: f = g (gerçek maliyet) + h (kuş uçuşu mesafe)
        public override async Task ExecuteAsync(Graph graph, Node startNode, Node endNode, Panel drawingPanel, Label timeLabel)
        {
            ResetGraph(graph); 

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // gScore: Başlangıçtan şu anki düğüme kadar olan gerçek maliyet
            var gScore = new Dictionary<Node, double>();

            // fScore: gScore + Heuristic tahmini toplam maliyet)
            // Algoritma sıralamayı buna göre yapar.
            var fScore = new Dictionary<Node, double>();

            var previousNodes = new Dictionary<Node, Node>();

            // İşlenecek düğümler listesi 
            var openSet = new HashSet<Node>();
            // Ziyaret edilen düğümler (closed set)
            var closedSet = new HashSet<Node>();

            foreach (var node in graph.Nodes)
            {
                gScore[node] = double.MaxValue;
                fScore[node] = double.MaxValue;
                previousNodes[node] = null;
            }

            gScore[startNode] = 0;
            fScore[startNode] = CalculateHeuristic(startNode, endNode);
            openSet.Add(startNode);
            int visitedCount = 0;

            while (openSet.Count > 0)
            {
                // fScore değeri en düşük olan düğümü seç 
                Node current = openSet.OrderBy(n => fScore[n]).First();

                if (current == endNode)
                {
                    break;
                }

                openSet.Remove(current);
                closedSet.Add(current);
                visitedCount++;

                List<Node> neighbors = GetNeighbors(graph, current); 

                foreach (Node neighbor in neighbors)
                {
                    // Eğer komşu zaten ziyaret edildiyse atla
                    if (closedSet.Contains(neighbor))
                        continue;

                    Edge edge = FindEdge(graph, current, neighbor);

                    if (edge != null)
                    {
                        double tentative_gScore = gScore[current] + edge.Weight;

                        // Eğer bu komşuya daha kısa bir yoldan geldiysek güncelle
                        if (tentative_gScore < gScore[neighbor])
                        {
                            previousNodes[neighbor] = current;
                            gScore[neighbor] = tentative_gScore;

                            // f = g + h
                            fScore[neighbor] = gScore[neighbor] + CalculateHeuristic(neighbor, endNode);

                            // Listede yoksa ekle ki bir sonraki turda değerlendirilsin
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

            // yol çizme, dijkstra ile aynıq

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
            int pathLength = path.Count;
            MessageBox.Show($"A* ile En Kısa Yol Bulundu!\nToplam Maliyet: {totalCost:0.00}\nZiyaret Edilen Düğüm Sayısı: {visitedCount}\nYol Uzunluğu (Düğüm): {pathLength}");
        }
    }
}

