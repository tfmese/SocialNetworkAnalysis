using ProjectYazLab.Models;

namespace ProjectYazLab.Interfaces
{
    // Dosya işlemleri için interface
    public interface IFileHandler
    {
        // CSV'den graf yükle
        Graph LoadGraphFromCSV(string filePath, int maxWidth, int maxHeight);

        // Grafi CSV'ye kaydet
        bool SaveGraphToCSV(Graph graph, string filePath);

        // Komşuluk matrisi olarak kaydet
        void SaveAdjacencyMatrix(Graph graph, string filePath);
    }
}

