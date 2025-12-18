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
        // BFS algoritması ile başlangıç düğümünden itibaren en yakın komşuları ziyaret ederek tarama yapar. FIFO prensibine göre çalışır. Amaç yakınları önce halletmek olduğundan önce tüm komşular sıraya atanır,
        //ardından 1. bağlanan komşudan başlayarak 2 ve 3 diye devam eder. DFS algoritmasının aksine halka halka devam eder yani 1. dereceden bağlılar dolaşılmadan 2. dereceye geçilmez.
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


            //duration ölçümü bittikten sonra görselleştirme için tekrar sıfırlıyoruz görsel için.
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

        // düğümün komşularını bulma
        private List<Node> GetNeighbors(Graph graph, Node node)
        {
            List<Node> neighbors = new List<Node>();

            foreach (var edge in graph.Edges)
            {
                // bağlantı yönsüz olduğu için iki tarafa da bakıyoruz
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

        // Algoritma başlamadan önce herkesi sıfırlar
        private void ResetGraph(Graph graph)
        {
            foreach (var node in graph.Nodes)
            {
                node.Visited = false;
                node.CurrentColor = Color.Blue; // Varsayılan maviye dön
            }
        }

        //DFS algoritması derinlik öncelikli arama yapar. Labirentteki bir fareye benzetilebilir. Bir düğümden başlayarak gidebildiği kadar gider yani karşısına duvar çıkana kadar. Ardından backtrack yaparak diğer yollar için aynı şeyi yapar.
        //LIFO prensibine göre çalışır yani son eklenen ilk çıkar mantığıyla çalışır. Yani dfs taraması yapmak istediğimiz düğümün 3 komşusu olsun, bu komşulardan 3. olarak bağlanana ilk gider. Bu 3. komşunun da komşuları varsa onlar için de aynı olay devam eder
        //ta ki olmayana kadar. Bu yüzden DFS algoritmasında stack(yığın) kullanırız.

        public async Task RunDFS(Graph graph, Node startNode, Panel drawingPanel, Label timeLabel)
        {
            ResetGraph(graph);
           List<Node> visitOrder = new List<Node>();

            // stack oluştur
            Stack<Node> stack = new Stack<Node>();


            stack.Push(startNode);
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            while (stack.Count > 0)
            {
                // en üsttekini  al
                Node current = stack.Pop();

                // eğer daha önce ziyaret etmediysek
                if (!current.Visited)
                {
                    
                    current.Visited = true;
                    visitOrder.Add(current);
                    // bu bloğu içeri ekleyince bu düğüme ilk defa geliyorsam komşularımı sıraya ekle, daha önce geldiysem zaten komşularla işim bitmiştir tekrar bakma anlamına geliyor.
                    // Dışarıdayken yaklaşık %40-50 civarı daha yavaş çalışıyor. Çünkü her seferinde komşuları alıp stack e ekliyor ziyaret edilmiş olsa bile.
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
        
        //Dijkstra algoritamasıyla düğümler arası en kısa yol bulunur. Başlangıç düğümünden hedef düğüme giden yolda tüm düğümleri dolaşır,
        //maliyetlerini hesaplar ve bunlardan en düşük maliyetli yolu seçer.

        public async Task RunDijkstra(Graph graph, Node startNode, Node endNode, Panel drawingPanel, Label timeLabel)
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

            while (unvisited.Count > 0)
            {
                //ziyaret edilmemişleri weight/maliyetlerine göre sıralıyor hemen ardından 0. yani en düşük maliyetli düğümü alıyor ve ziyaret edilmemiş listesinden çıkarıyoruz.
                unvisited.Sort((x,y) => distances[x].CompareTo(distances[y]));
                var currentNode = unvisited[0];
                unvisited.Remove(currentNode);

               
                if (currentNode == endNode || distances[currentNode] == double.MaxValue)
                {
                    break;
                }

                
                foreach (var edge in graph.Edges)
                {
                    Node neighbor = null;

                    // kenar currentNode a bağlı mı 
                    if (edge.Source == currentNode) { 
                        neighbor = edge.Target;
                    }
                    else if (edge.Target == currentNode)
                        neighbor = edge.Source;

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
            MessageBox.Show($"En Kısa Yol Bulundu!\nToplam Maliyet: {totalCost:0.00}");
        }
    }
}
