using ProjectYazLab.AlgoModule;
using ProjectYazLab.Interfaces;
using ProjectYazLab.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace ProjectYazLab.Services
{
    // Bu sınıf algoritmaları çağırmak için kullanılıyor
    // Interface ve abstract class kullanarak yaptık
    public class Algorithms
    {
        // Her algoritma için bir değişken tanımladık
        private IGraphAlgorithm bfsAlgo;
        private IGraphAlgorithm dfsAlgo;
        private IGraphAlgorithm dijkstraAlgo;
        private IGraphAlgorithm aStarAlgo;
        private IGraphAnalyzer centralityAnalyzer;
        private IGraphAnalyzer componentsAnalyzer;
        private IGraphAnalyzer coloringAnalyzer;

        public Algorithms()
        {
            // Constructor da algoritmaları oluşturuyoruz
            bfsAlgo = new BFSAlgorithm();
            dfsAlgo = new DFSAlgorithm();
            dijkstraAlgo = new DijkstraAlgorithm();
            aStarAlgo = new AStarAlgorithm();
            centralityAnalyzer = new DegreeCentralityAnalyzer();
            componentsAnalyzer = new ConnectedComponentsAnalyzer();
            coloringAnalyzer = new WelshPowellColoringAnalyzer();
        }

        // BFS  çalıştır
        public async System.Threading.Tasks.Task RunBFS(Graph graph, Node startNode, Panel drawingPanel, Label timeLabel)
        {
            await bfsAlgo.ExecuteAsync(graph, startNode, null, drawingPanel, timeLabel);
        }

        // DFS  çalıştır
        public async System.Threading.Tasks.Task RunDFS(Graph graph, Node startNode, Panel drawingPanel, Label timeLabel)
        {
            await dfsAlgo.ExecuteAsync(graph, startNode, null, drawingPanel, timeLabel);
        }

        // Dijkstra  çalıştır
        public async System.Threading.Tasks.Task RunDijkstra(Graph graph, Node startNode, Node endNode, Panel drawingPanel, Label timeLabel)
        {
            await dijkstraAlgo.ExecuteAsync(graph, startNode, endNode, drawingPanel, timeLabel);
        }

        // A*  çalıştır
        public async System.Threading.Tasks.Task RunAStar(Graph graph, Node startNode, Node endNode, Panel drawingPanel, Label timeLabel)
        {
            await aStarAlgo.ExecuteAsync(graph, startNode, endNode, drawingPanel, timeLabel);
        }

        // Degree centrality hesapla
        public void CalculateDegreeCentrality(Graph graph, DataGridView table, Label timeLabel = null)
        {
            centralityAnalyzer.Analyze(graph, table, timeLabel);
        }

        // Bağlı bileşenleri bul
        public List<List<Node>> GetConnectedComponents(Graph graph, Label timeLabel = null)
        {
            List<List<Node>> result = new List<List<Node>>();
            componentsAnalyzer.Analyze(graph, result, timeLabel);
            return result;
        }

        // Welsh-Powell renklendirme algoritmasını çalıştır
        public List<ColoringResult> RunWelshPowellColoring(Graph graph, Label timeLabel)
        {
            List<ColoringResult> results = new List<ColoringResult>();
            coloringAnalyzer.Analyze(graph, results, timeLabel);
            return results;
        }
    }
}

