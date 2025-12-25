using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ProjectYazLab
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

        public override void Analyze(Graph graph, object resultContainer)
        {
            ResetGraph(graph);

            // Önce ayrık toplulukları bul
            ConnectedComponentsAnalyzer componentsAnalyzer = new ConnectedComponentsAnalyzer();
            List<List<Node>> components = new List<List<Node>>();
            componentsAnalyzer.Analyze(graph, components);

            // Her topluluk için renklendirme yap
            Dictionary<Node, int> nodeToColor = new Dictionary<Node, int>();
            List<ColoringResult> coloringResults = new List<ColoringResult>();

            foreach (var component in components)
            {
                ColoringResult result = ColorComponent(graph, component);
                coloringResults.Add(result);

                // Düğümleri renklendir
                foreach (var node in component)
                {
                    if (result.NodeColors.ContainsKey(node))
                    {
                        int colorIndex = result.NodeColors[node];
                        node.CurrentColor = ColorPalette[colorIndex % ColorPalette.Length];
                    }
                }
            }

            // Sonuçları resultContainer'a yaz
            if (resultContainer is List<ColoringResult> resultList)
            {
                resultList.Clear();
                resultList.AddRange(coloringResults);
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

            // 1. Düğümleri derecelerine göre azalan sırada sırala
            List<Node> sortedNodes = component.OrderByDescending(node => GetDegree(graph, node)).ToList();

            // 2. Her düğüm için renklendirme yap
            foreach (var node in sortedNodes)
            {
                // Komşularında kullanılan renkleri bul
                HashSet<int> usedColors = new HashSet<int>();
                List<Node> neighbors = GetNeighbors(graph, node);

                foreach (var neighbor in neighbors)
                {
                    if (result.NodeColors.ContainsKey(neighbor))
                    {
                        usedColors.Add(result.NodeColors[neighbor]);
                    }
                }

                // Kullanılmayan en küçük rengi bul
                int colorIndex = 0;
                while (usedColors.Contains(colorIndex))
                {
                    colorIndex++;
                }

                // Düğüme rengi ata
                result.NodeColors[node] = colorIndex;

                // Kullanılan renk sayısını güncelle
                if (colorIndex >= result.ColorCount)
                {
                    result.ColorCount = colorIndex + 1;
                }
            }

            return result;
        }

        // Bir düğümün derecesini hesapla (komşu sayısı)
        private int GetDegree(Graph graph, Node node)
        {
            return GetNeighbors(graph, node).Count;
        }
    }

    // Renklendirme sonuçlarını tutan sınıf
    public class ColoringResult
    {
        public List<Node> Component { get; set; }
        public Dictionary<Node, int> NodeColors { get; set; }
        public int ColorCount { get; set; }
    }
}

