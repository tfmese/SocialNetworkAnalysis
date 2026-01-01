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
   
    public class DijkstraAlgorithm : AbstractPathfindingAlgorithm
    {
        //Dijkstra algoritamasıyla düğümler arası en kısa yol bulunur. Başlangıç düğümünden hedef düğüme giden yolda tüm düğümleri dolaşır,
        //maliyetlerini hesaplar ve bunlardan en düşük maliyetli yolu seçer.
        public override async Task ExecuteAsync(Graph graph, Node startNode, Node endNode, Panel drawingPanel, Label timeLabel)
        {
            ResetGraph(graph);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            var distances = new Dictionary<Node, double>();
            var previousNodes = new Dictionary<Node, Node>();
            var unvisited = new List<Node>();

            foreach (var node in graph.Nodes)
            {
                distances[node] = double.MaxValue;
                previousNodes[node] = null;
                unvisited.Add(node);
            }

            distances[startNode] = 0;
            int visitedCount = 0;

            while (unvisited.Count > 0)
            {
                //ziyaret edilmemişleri weight/maliyetlerine göre sıralıyor hemen ardından 0. yani en düşük maliyetli düğümü alıyor ve ziyaret edilmemiş listesinden çıkarıyoruz.
                unvisited.Sort((x, y) => distances[x].CompareTo(distances[y]));
                var currentNode = unvisited[0];
                unvisited.Remove(currentNode);
                visitedCount++;

                if (currentNode == endNode || distances[currentNode] == double.MaxValue)
                {
                    break;
                }

                foreach (var edge in graph.Edges)
                {
                    Node neighbor = null;

                    // kenar currentNode a bağlı mı 
                    if (edge.Source == currentNode)
                    {
                        neighbor = edge.Target;
                    }
                    else if (edge.Target == currentNode)
                    {
                        neighbor = edge.Source;
                    }

                    //  komşu varsa ve ziyaret edilmemişler listesindeyse
                    if (neighbor != null && unvisited.Contains(neighbor))
                    {
                        //başlangıçtan buraya olan mesafe + current köprün ağırlığı/maliyeti
                        double newDist = distances[currentNode] + edge.Weight;
                        // eğer bu yeni bulduğum yol daha kısaysa bu yeni yolum olur.
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

            // diğer taramaları yaptıktan sonra dijsktra başlatınca nullexception hatası alınyor çünkü previousNodes içinde endNode yoktu. Bunun için kontrol kodu .
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
            //elde şuan sadece endNode var. Ordan başlyarak previousNodes dan geriye doğru gidip yolu oluşturuyoruz.
            var path = new List<Node>();
            var tempNode = endNode;

            while (tempNode != null)
            {
                path.Insert(0, tempNode);
                //previousNodes da tempNode varsa tempNode u güncelle vedevam
                if (previousNodes.ContainsKey(tempNode))
                {
                    tempNode = previousNodes[tempNode];
                }
                else
                {
                    tempNode = null;
                }
            }

            // yolu boya
            foreach (var node in path)
            {
                node.CurrentColor = Color.Purple;
                drawingPanel.Invalidate();
                await Task.Delay(500);
            }

            // maliyet
            double totalCost = distances[endNode];
            int pathLength = path.Count;
            MessageBox.Show($"En Kısa Yol Bulundu!\nToplam Maliyet: {totalCost:0.00}\nZiyaret Edilen Düğüm Sayısı: {visitedCount}\nYol Uzunluğu (Düğüm): {pathLength}");
        }
    }
}

