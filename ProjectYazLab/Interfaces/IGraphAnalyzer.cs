using ProjectYazLab.Models;

namespace ProjectYazLab.Interfaces
{
    // Analiz algoritmaları için interface
    public interface IGraphAnalyzer
    {
        // Graf analizi yapan metod
        void Analyze(Graph graph, object resultContainer);
    }
}

