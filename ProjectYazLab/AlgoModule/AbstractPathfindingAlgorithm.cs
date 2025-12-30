using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProjectYazLab.Interfaces;
using ProjectYazLab.Models;

namespace ProjectYazLab.AlgoModule
{
    // Abstract class yaptım çünkü ortak metodları buraya koydum
    // Böylece her algoritmada aynı kodu tekrar yazmaya gerek kalmadı
    public abstract class AbstractPathfindingAlgorithm : IGraphAlgorithm
    {
        // Bu metodu her algoritma kendi şekilde yazacak
        public abstract Task ExecuteAsync(Graph graph, Node startNode, Node endNode, Panel drawingPanel, Label timeLabel);

        // Komşu düğümleri bulma metodu - tüm algoritmalar kullanıyor
        protected List<Node> GetNeighbors(Graph graph, Node node)
        {
            List<Node> neighbors = new List<Node>();

            foreach (var edge in graph.Edges)
            {
                // Graf yönsüz olduğu için iki yöne de bakıyoruz
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

        // Grafı sıfırlama metodu - her algoritma başlamadan önce kullanıyor
        protected void ResetGraph(Graph graph)
        {
            foreach (var node in graph.Nodes)
            {
                node.Visited = false;
                node.CurrentColor = Color.Blue; // Varsayılan mavi renk
            }
        }

        // İki düğüm arasındaki kenarı bulma
        protected Edge FindEdge(Graph graph, Node source, Node target)
        {
            return graph.Edges.FirstOrDefault(e =>
                (e.Source == source && e.Target == target) ||
                (e.Source == target && e.Target == source));
        }

        // Heuristic hesaplama (A* algoritması için kullanılıyor)
        // İki nokta arası mesafe hesaplıyor
        protected double CalculateHeuristic(Node a, Node b)
        {
            return Math.Sqrt(Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2));
        }
    }
}

