using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using ProjectYazLab.Models;

namespace ProjectYazLab.AlgoModule
{
    // Degree centrality hesaplayan sınıf
    // AbstractGraphAnalyzer'dan türetiyoruz
    public class DegreeCentralityAnalyzer : AbstractGraphAnalyzer
    {
        public override void Analyze(Graph graph, object resultContainer, Label timeLabel = null)
        {
            if (!(resultContainer is DataGridView table))
            {
                throw new ArgumentException("resultContainer must be a DataGridView");
            }

            // UI işlemleri süre ölçümüne dahil değil
            table.Rows.Clear();
            table.Columns.Clear();

            // Tabloya sütunları ekleme
            table.Columns.Add("ID", "Kullanıcı ID");
            table.Columns.Add("Name", "İsim");
            table.Columns.Add("Degree", "Derece");
            table.Columns.Add("Score", "Skor");

            if (graph.Nodes.Count == 0) return;

            // Algoritma süre ölçümü buradan başlıyor
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // düğümlerin kertesini hesaplıyor
            var nodeDegrees = new List<Tuple<Node, int>>();
            foreach (var node in graph.Nodes)
            {
                int degree = graph.Edges.Count(e => e.Source == node || e.Target == node);
                nodeDegrees.Add(new Tuple<Node, int>(node, degree));
            }

            // kerteye göre azalan sırada düğümleri sıralıyor
            var sortedNodes = nodeDegrees.OrderByDescending(x => x.Item2).ToList();

            // İlk 5'i al
            int count = Math.Min(5, sortedNodes.Count);
            List<Tuple<Node, int, double>> results = new List<Tuple<Node, int, double>>();
            for (int i = 0; i < count; i++)
            {
                Node n = sortedNodes[i].Item1;
                int degree = sortedNodes[i].Item2;
                double score = (double)degree / (graph.Nodes.Count - 1); // Merkezilik formülü
                results.Add(new Tuple<Node, int, double>(n, degree, score));
            }

            stopwatch.Stop();

            // UI işlemleri süre ölçümüne dahil değil
            foreach (var result in results)
            {
                table.Rows.Add(result.Item1.Id, result.Item1.Name, result.Item2, result.Item3.ToString("0.00"));
            }

            if (timeLabel != null)
            {
                timeLabel.Text = $"Degree Centrality Süresi: {stopwatch.Elapsed.TotalMilliseconds:F3} ms ({stopwatch.ElapsedTicks} Ticks)";
            }
        }
    }
}

