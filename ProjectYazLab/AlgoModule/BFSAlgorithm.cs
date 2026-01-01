using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProjectYazLab.Models;

namespace ProjectYazLab.AlgoModule
{
    
    public class BFSAlgorithm : AbstractPathfindingAlgorithm
    {
        // BFS algoritması ile başlangıç düğümünden itibaren en yakın komşuları ziyaret ederek tarama yapar. FIFO prensibine göre çalışır. Amaç yakınları önce halletmek olduğundan önce tüm komşular sıraya atanır,
        //ardından 1. bağlanan komşudan başlayarak 2 ve 3 diye devam eder. DFS algoritmasının aksine halka halka devam eder yani 1. dereceden bağlılar dolaşılmadan 2. dereceye geçilmez.
        public override async Task ExecuteAsync(Graph graph, Node startNode, Node endNode, Panel drawingPanel, Label timeLabel)
        {
            ResetGraph(graph);
            Queue<Node> queueMeasure = new Queue<Node>();

            Stopwatch sw = new Stopwatch();
            sw.Start();

            int visitedCount = 0;
            startNode.Visited = true;
            visitedCount++;
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
                        visitedCount++;
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

            MessageBox.Show($"BFS Taraması Tamamlandı!\nZiyaret Edilen Düğüm Sayısı: {visitedCount}\nYol Uzunluğu: N/A (Tarama algoritması)");
        }
    }
}

