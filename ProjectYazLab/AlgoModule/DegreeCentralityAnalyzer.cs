using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ProjectYazLab.Models;

namespace ProjectYazLab.AlgoModule
{
    // Degree centrality hesaplayan sınıf
    // AbstractGraphAnalyzer'dan türetiyorum
    public class DegreeCentralityAnalyzer : AbstractGraphAnalyzer
    {
        public override void Analyze(Graph graph, object resultContainer)
        {
            if (!(resultContainer is DataGridView table))
            {
                throw new ArgumentException("resultContainer must be a DataGridView");
            }

            table.Rows.Clear();
            table.Columns.Clear();

            // Tabloya sütunları ekliyorum
            table.Columns.Add("ID", "Kullanıcı ID");
            table.Columns.Add("Name", "İsim");
            table.Columns.Add("Degree", "Derece");
            table.Columns.Add("Score", "Skor");

            if (graph.Nodes.Count == 0) return;

            // Her düğümün derecesini hesaplıyorum
            var nodeDegrees = new List<Tuple<Node, int>>();
            foreach (var node in graph.Nodes)
            {
                int degree = graph.Edges.Count(e => e.Source == node || e.Target == node);
                nodeDegrees.Add(new Tuple<Node, int>(node, degree));
            }

            // Dereceye göre sıralıyorum
            var sortedNodes = nodeDegrees.OrderByDescending(x => x.Item2).ToList();

            // İlk 5'i alıyorum
            int count = Math.Min(5, sortedNodes.Count);
            for (int i = 0; i < count; i++)
            {
                Node n = sortedNodes[i].Item1;
                int degree = sortedNodes[i].Item2;
                double score = (double)degree / (graph.Nodes.Count - 1); // Merkezilik formülü

                table.Rows.Add(n.Id, n.Name, degree, score.ToString("0.00"));
            }
        }
    }
}

