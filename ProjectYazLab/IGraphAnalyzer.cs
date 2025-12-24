using System.Collections.Generic;
using System.Windows.Forms;

namespace ProjectYazLab
{
    // Analiz algoritmaları için interface
    public interface IGraphAnalyzer
    {
        // Graf analizi yapan metod
        void Analyze(Graph graph, object resultContainer);
    }
}

