using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks; 
using System.Windows.Forms;

namespace ProjectYazLab
{
    public class Algorithms
    {
        // BFS algoritması ile başlangıç düğümünden itibaren en yakın komşuları ziyaret ederek tarama yapar. FIFO prensibine göre çalışır. Amaç yakınları önce halletmek olduğundan önce tüm komşular sıraya atanır,
        //ardından 1. bağlanan komşudan başlayarak 2 ve 3 diye devam eder. DFS algoritmasının aksine halka halka devam eder yani 1. dereceden bağlılar dolaşılmadan 2. dereceye geçilmez.
        public async Task RunBFS(Graph graph, Node startNode, Panel drawingPanel)
        {

            ResetGraph(graph);
            // kuyrruk oluştur
            Queue<Node> queue = new Queue<Node>();


            startNode.Visited = true;
            queue.Enqueue(startNode);



            while (queue.Count > 0)
            {
                Node current = queue.Dequeue();

                // işlendiğini görebilmemiz için rengi değiştir
                current.CurrentColor = Color.LightGreen;

                // boynadığı görülsün diye paneli yenile
                drawingPanel.Invalidate();


                await Task.Delay(1500);


                List<Node> neighbors = GetNeighbors(graph, current);

                foreach (Node neighbor in neighbors)
                {
                    if (!neighbor.Visited)
                    {
                        neighbor.Visited = true;
                        neighbor.CurrentColor = Color.Yellow; // Sırada bekleyenleri Sarı yapalım
                        queue.Enqueue(neighbor);
                        drawingPanel.Invalidate();
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

        public async Task RunDFS(Graph graph, Node startNode, Panel drawingPanel)
        {
            ResetGraph(graph);

            // stack oluştur
            Stack<Node> stack = new Stack<Node>();


            stack.Push(startNode);

            while (stack.Count > 0)
            {
                // en üsttekini  al
                Node current = stack.Pop();

                // eğer daha önce ziyaret etmediysek
                if (!current.Visited)
                {
                    current.Visited = true;
                    current.CurrentColor = Color.Orange;
                    drawingPanel.Invalidate();
                    await Task.Delay(1500);
                }

                List<Node> neighbors = GetNeighbors(graph, current);

                foreach (Node neighbor in neighbors)
                {
                    if (!neighbor.Visited)
                    {
                        stack.Push(neighbor);
                    }
                }
            }
            MessageBox.Show("DFS Taraması Tamamlandı!");
        }
        
        //Dijkstra algoritamasıyla düğümler arası en kısa yol bulunur. Başlangıç düğümünden hedef düğüme giden yolda tüm düğümleri dolaşır,
        //maliyetlerini hesaplar ve bunlardan en düşük maliyetli yolu seçer.

        public async Task RunDijkstra(Graph graph, Node startNode, Node endNode, Panel drawingPanel)
        {
            ResetGraph(graph); 

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
