using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProjectYazLab.Models;

namespace ProjectYazLab.AlgoModule
{
  
    public class DFSAlgorithm : AbstractPathfindingAlgorithm
    {
        //DFS algoritması derinlik öncelikli arama yapar. Labirentteki bir fareye benzetilebilir. Bir düğümden başlayarak gidebildiği kadar gider yani karşısına duvar çıkana kadar. Ardından backtrack yaparak diğer yollar için aynı şeyi yapar.
        //LIFO prensibine göre çalışır yani son eklenen ilk çıkar mantığıyla çalışır. Yani dfs taraması yapmak istediğimiz düğümün 3 komşusu olsun, bu komşulardan 3. olarak bağlanana ilk gider. Bu 3. komşunun da komşuları varsa onlar için de aynı olay devam eder
        //ta ki olmayana kadar. Bu yüzden DFS algoritmasında stack(yığın) kullanırız.
        public override async Task ExecuteAsync(Graph graph, Node startNode, Node endNode, Panel drawingPanel, Label timeLabel)
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
    }
}

