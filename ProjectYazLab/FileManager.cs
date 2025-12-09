using System;
using System.IO;
using System.Windows.Forms;

namespace ProjectYazLab
{
    public class FileManager
    {
        public Graph LoadGraphFromCSV(string filePath, int maxWidth, int maxHeight)
        {
            Graph graph = new Graph();

            if (!File.Exists(filePath))
            {
                MessageBox.Show("HATA: 'users.csv' dosyası bulunamadı!\n" + filePath);
                return null;
            }

            string[] lines = File.ReadAllLines(filePath);
            Random rnd = new Random();

            // 2. TUR: Önce sadece Düğümleri (İnsanları) oluştur
            // i=1 diyoruz çünkü ilk satır başlık (Header)
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] parts = line.Split(',');

                // CSV Formatı Id,Name,Activity,Interaction,ConnectionCount,Neighbors
                int id = int.Parse(parts[0]);
                string name = parts[1];

                // Ondalık sayıları okurken nokta/virgül hatası olmasın diye Replace 
                double activity = double.Parse(parts[2].Replace('.', ','));
                double interaction = double.Parse(parts[3]);
                double connCount = double.Parse(parts[4]);

                float x = rnd.Next(50, maxWidth-50);
                float y = rnd.Next(50, maxHeight-50);

                Node newNode = new Node(id, name, x, y);
                newNode.Activity = activity;
                newNode.Interaction = interaction;
                newNode.ConnectionCount = connCount;

                graph.AddNode(newNode);
            }

            for (int i = 1; i < lines.Length; i++)
            {
                string[] parts = lines[i].Split(',');
                int currentId = int.Parse(parts[0]);
                string neighborsString = parts[5]; 

                // Şu anki düğümü bul
                Node source = graph.Nodes.Find(n => n.Id == currentId);

                if (!string.IsNullOrEmpty(neighborsString) && source != null)
                {
                    string[] neighborIds = neighborsString.Split('-');

                    foreach (string nId in neighborIds)
                    {
                        int targetId = int.Parse(nId);
                        Node target = graph.Nodes.Find(n => n.Id == targetId);

                        if (target != null)
                        {
                            graph.AddEdge(source, target);
                        }
                    }
                }
            }

            return graph;
        }
    }
}