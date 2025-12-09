using System;
using System.Collections.Generic;
using System.Text;

namespace ProjectYazLab
{
    public class Edge
    {
        public Node Source { get; set; } 
        public Node Target { get; set; } 
        public double Weight { get; set; } // formülden gelecek

        public Edge(Node source, Node target)
        {
            Source = source;
            Target = target;
            Weight = CalculateWeight(); 
        }

        public double CalculateWeight()
        {
            // Formül:(aktiflikİ-aktiflikJ)^2 + (etkileşimİ-etkileşimJ)^2 + (bağlantıSayısıİ-bağlantıSayısıJ)^2
            double actDiff = Source.Activity - Target.Activity;
            double intDiff = Source.Interaction - Target.Interaction;
            double conDiff = Source.ConnectionCount - Target.ConnectionCount;

            return 1 + Math.Sqrt((actDiff * actDiff) + (intDiff * intDiff) + (conDiff * conDiff));
        }
    }
}
