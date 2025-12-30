using System.Threading.Tasks;
using System.Windows.Forms;
using ProjectYazLab.Models;

namespace ProjectYazLab.Interfaces
{
    public interface IGraphAlgorithm
    {
        // Tüm algoritmalar bu metodu implement etmek zorunda
        Task ExecuteAsync(Graph graph, Node startNode, Node endNode, Panel drawingPanel, Label timeLabel);
    }
}

