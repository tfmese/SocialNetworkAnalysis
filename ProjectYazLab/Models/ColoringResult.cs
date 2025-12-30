using System.Collections.Generic;

namespace ProjectYazLab.Models
{
    // Renklendirme sonuçlarını tutan sınıf
    public class ColoringResult
    {
        public List<Node> Component { get; set; } = new List<Node>();
        public Dictionary<Node, int> NodeColors { get; set; } = new Dictionary<Node, int>();
        public int ColorCount { get; set; }
    }
}

