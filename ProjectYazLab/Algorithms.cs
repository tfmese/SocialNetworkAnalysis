using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace ProjectYazLab
{
    // Bu sınıf algoritmaları çağırmak için kullanılıyor
    // Interface ve abstract class kullanarak yaptım
    public class Algorithms
    {
        // Her algoritma için bir değişken tanımladım
        private IGraphAlgorithm bfsAlgo;
        private IGraphAlgorithm dfsAlgo;
        private IGraphAlgorithm dijkstraAlgo;
        private IGraphAlgorithm aStarAlgo;
        private IGraphAnalyzer centralityAnalyzer;
        private IGraphAnalyzer componentsAnalyzer;
        private IGraphAnalyzer coloringAnalyzer;

        public Algorithms()
        {
            // Constructor'da algoritmaları oluşturuyorum
            bfsAlgo = new BFSAlgorithm();
            dfsAlgo = new DFSAlgorithm();
            dijkstraAlgo = new DijkstraAlgorithm();
            aStarAlgo = new AStarAlgorithm();
            centralityAnalyzer = new DegreeCentralityAnalyzer();
            componentsAnalyzer = new ConnectedComponentsAnalyzer();
            coloringAnalyzer = new WelshPowellColoringAnalyzer();
        }

        // BFS algoritmasını çalıştır
        public async System.Threading.Tasks.Task RunBFS(Graph graph, Node startNode, Panel drawingPanel, Label timeLabel)
        {
            await bfsAlgo.ExecuteAsync(graph, startNode, null, drawingPanel, timeLabel);
        }

        // DFS algoritmasını çalıştır
        public async System.Threading.Tasks.Task RunDFS(Graph graph, Node startNode, Panel drawingPanel, Label timeLabel)
        {
            await dfsAlgo.ExecuteAsync(graph, startNode, null, drawingPanel, timeLabel);
        }

        // Dijkstra algoritmasını çalıştır
        public async System.Threading.Tasks.Task RunDijkstra(Graph graph, Node startNode, Node endNode, Panel drawingPanel, Label timeLabel)
        {
            await dijkstraAlgo.ExecuteAsync(graph, startNode, endNode, drawingPanel, timeLabel);
        }

        // A* algoritmasını çalıştır
        public async System.Threading.Tasks.Task RunAStar(Graph graph, Node startNode, Node endNode, Panel drawingPanel, Label timeLabel)
        {
            await aStarAlgo.ExecuteAsync(graph, startNode, endNode, drawingPanel, timeLabel);
        }

        // Degree centrality hesapla
        public void CalculateDegreeCentrality(Graph graph, DataGridView table)
        {
            centralityAnalyzer.Analyze(graph, table);
        }

        // Bağlı bileşenleri bul
        public List<List<Node>> GetConnectedComponents(Graph graph)
        {
            List<List<Node>> result = new List<List<Node>>();
            componentsAnalyzer.Analyze(graph, result);
            return result;
        }

        // Welsh-Powell renklendirme algoritmasını çalıştır
        public List<ColoringResult> RunWelshPowellColoring(Graph graph, Label timeLabel)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            List<ColoringResult> results = new List<ColoringResult>();
            coloringAnalyzer.Analyze(graph, results);

            stopwatch.Stop();
            if (timeLabel != null)
            {
                timeLabel.Text = $"Welsh-Powell Süresi: {stopwatch.Elapsed.TotalMilliseconds:F3} ms ({stopwatch.ElapsedTicks} Ticks)";
            }

            return results;
        }
    }
}
