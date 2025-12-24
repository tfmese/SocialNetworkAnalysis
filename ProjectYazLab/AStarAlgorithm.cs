using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectYazLab
{
    /// <summary>
    /// A* pathfinding algorithm implementation
    /// Uses heuristic function (Euclidean distance) to find optimal path
    /// Formula: f = g (actual cost) + h (heuristic estimate)
    /// </summary>
    public class AStarAlgorithm : AbstractPathfindingAlgorithm
    {
        // Dijkstra'dan farkı: Bir sonraki düğümü seçerken sadece gidilen yola (g) değil,
        // hedefe kalan tahmini mesafeye de (h) bakar.
        // Formül: f = g (gerçek maliyet) + h (kuş uçuşu mesafe)
        public override async Task ExecuteAsync(Graph graph, Node startNode, Node endNode, Panel drawingPanel, Label timeLabel)
        {
            ResetGraph(graph); // Renkleri sıfırla

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // gScore: Başlangıçtan şu anki düğüme kadar olan GERÇEK maliyet
            var gScore = new Dictionary<Node, double>();

            // fScore: gScore + Heuristic (Tahmini Toplam Maliyet)
            // Algoritma sıralamayı buna göre yapar.
            var fScore = new Dictionary<Node, double>();

            var previousNodes = new Dictionary<Node, Node>();

            // İşlenecek düğümler listesi (Open Set)
            var openSet = new List<Node>();

            // Başlangıç değerlerini ata
            foreach (var node in graph.Nodes)
            {
                gScore[node] = double.MaxValue;
                fScore[node] = double.MaxValue;
                previousNodes[node] = null;
            }

            gScore[startNode] = 0;
            fScore[startNode] = CalculateHeuristic(startNode, endNode);
            openSet.Add(startNode);

            while (openSet.Count > 0)
            {
                // fScore değeri en düşük olan düğümü seç (En mantıklı tercih)
                openSet.Sort((x, y) => fScore[x].CompareTo(fScore[y]));
                Node current = openSet[0];

                // Hedefe ulaştık mı?
                if (current == endNode)
                {
                    break;
                }

                openSet.Remove(current);

                // Komşuları gez
                List<Node> neighbors = GetNeighbors(graph, current); // Senin yazdığın fonksiyonu kullanıyoruz

                foreach (Node neighbor in neighbors)
                {
                    // Aradaki kenarın ağırlığını bul
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

            // --- YOLU ÇİZME KISMI (Dijkstra ile aynı) ---

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

            // Yolu boya
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

