using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ProjectYazLab.Models;

namespace ProjectYazLab.AlgoModule
{
    // Welsh-Powell graf renklendirme algoritması
    // Her ayrık topluluk için ayrı ayrı renklendirme yapar
    public class WelshPowellColoringAnalyzer : AbstractGraphAnalyzer
    {
        // Renk paleti - farklı renkler için
        private static readonly Color[] ColorPalette = new Color[]
        {
            Color.Red,
            Color.Green,
            Color.Blue,
            Color.Yellow,
            Color.Orange,
            Color.Purple,
            Color.Cyan,
            Color.Magenta,
            Color.Lime,
            Color.Pink,
            Color.Brown,
            Color.Gold,
            Color.Silver,
            Color.Teal,
            Color.Indigo,
            Color.Coral,
            Color.Turquoise,
            Color.Violet,
            Color.Khaki,
            Color.Salmon
        };

        public override void Analyze(Graph graph, object resultContainer, Label timeLabel = null)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            ResetGraph(graph);

            //  ayrık toplulukları bul
            ConnectedComponentsAnalyzer componentsAnalyzer = new ConnectedComponentsAnalyzer();
            List<List<Node>> components = new List<List<Node>>();
            componentsAnalyzer.Analyze(graph, components, null); // Welsh-Powell içinde süre ölçümü yapılmaz, sadece toplulukları bulur

            // her topluluk için renklendirme yap
            Dictionary<Node, int> nodeToColor = new Dictionary<Node, int>();
            List<ColoringResult> coloringResults = new List<ColoringResult>();

            foreach (var component in components)
            {
                ColoringResult result = ColorComponent(graph, component);
                coloringResults.Add(result);

                // düğümleri renklendir
                foreach (var node in component)
                {
                    if (result.NodeColors.ContainsKey(node))
                    {
                        int colorIndex = result.NodeColors[node];
                        node.CurrentColor = ColorPalette[colorIndex % ColorPalette.Length];
                    }
                }
            }

            // sonuçları resultContainer'a yaz
            if (resultContainer is List<ColoringResult> resultList)
            {
                resultList.Clear();
                resultList.AddRange(coloringResults);
            }

            stopwatch.Stop();
            if (timeLabel != null)
            {
                timeLabel.Text = $"Welsh-Powell Süresi: {stopwatch.Elapsed.TotalMilliseconds:F3} ms ({stopwatch.ElapsedTicks} Ticks)";
            }
        }

        // Bir topluluk için Welsh-Powell renklendirme algoritması
        private ColoringResult ColorComponent(Graph graph, List<Node> component)
        {
            ColoringResult result = new ColoringResult
            {
                Component = component,
                NodeColors = new Dictionary<Node, int>(),
                ColorCount = 0
            };

            if (component.Count == 0)
                return result;

            //  Düğümleri derecelerine göre azalan sırada sırala
            List<Node> sortedNodes = component.OrderByDescending(node => GetDegree(graph, node)).ToList();

            //  Her düğüm için renklendirme 
            foreach (var node in sortedNodes)
            {
                // Komşularında kullanılan renkleri bulur
                HashSet<int> usedColors = new HashSet<int>();
                List<Node> neighbors = GetNeighbors(graph, node);

                foreach (var neighbor in neighbors)
                {
                    if (result.NodeColors.ContainsKey(neighbor))
                    {
                        usedColors.Add(result.NodeColors[neighbor]);
                    }
                }

                // kullanılmayan en küçük rengi bul
                int colorIndex = 0;
                while (usedColors.Contains(colorIndex))
                {
                    colorIndex++;
                }

                // düğüme rengi ata
                result.NodeColors[node] = colorIndex;

                // kullanılan renk sayısını güncelle
                if (colorIndex >= result.ColorCount)
                {
                    result.ColorCount = colorIndex + 1;
                }
            }

            return result;
        }

        //düğümün derecesini hesapla 
        private int GetDegree(Graph graph, Node node)
        {
            return GetNeighbors(graph, node).Count;
        }
    }
}

