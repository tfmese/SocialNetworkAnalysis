using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectYazLab
{
  
    public interface IGraphAlgorithm
    {
        // Tüm algoritmalar bu metodu implement etmek zorunda
        Task ExecuteAsync(Graph graph, Node startNode, Node endNode, Panel drawingPanel, Label timeLabel);
    }
}

