using System;
using System.IO;
using System.Windows.Forms;
using ProjectYazLab.Interfaces;
using ProjectYazLab.Models;

namespace ProjectYazLab.Services
{
    public class FileManager : IFileHandler
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

            
            for (int i = 1; i < lines.Length; i++)
            {
                string line = lines[i];
                if (string.IsNullOrWhiteSpace(line)) continue;

                string[] parts = line.Split(';');

                int id = int.Parse(parts[0]);
                string name = parts[1];

                double activity = double.Parse(parts[2]);
                double interaction = double.Parse(parts[3]);
                double connCount = double.Parse(parts[4]);

                float x, y;

               
                bool xBasarili = false;
                bool yBasarili = false;
                float parsedX = 0;
                float parsedY = 0;

                if (parts.Length >= 8)
                {
                    
                    xBasarili = float.TryParse(parts[6], out parsedX);
                    yBasarili = float.TryParse(parts[7], out parsedY);
                }

                if (xBasarili && yBasarili)
                {
                    x = parsedX;
                    y = parsedY;
                }
                else
                {
                    x = rnd.Next(50, maxWidth - 50);
                    y = rnd.Next(50, maxHeight - 50);
                }

                Node newNode = new Node(id, name, x, y);
                newNode.Activity = activity;
                newNode.Interaction = interaction;
                newNode.ConnectionCount = connCount;

                graph.AddNode(newNode);
            }

            for (int i = 1; i < lines.Length; i++)
            {
                string[] parts = lines[i].Split(';');
                int currentId = int.Parse(parts[0]);

                if (parts.Length < 6) continue;

                string neighborsString = parts[5];

                Node source = graph.Nodes.Find(n => n.Id == currentId);

                if (!string.IsNullOrEmpty(neighborsString) && source != null)
                {
                    string[] neighborIds = neighborsString.Split('-');

                    foreach (string nId in neighborIds)
                    {
                        if (string.IsNullOrWhiteSpace(nId)) continue;

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

        
        public bool SaveGraphToCSV(Graph graph, string filePath)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(filePath))
                {
                    sw.WriteLine("Id;Name;Activity;Interaction;ConnectionCount;Neighbors;X;Y");

                    foreach (Node node in graph.Nodes)
                    {
                        List<string> nList = new List<string>();
                        foreach (Edge e in graph.Edges)
                        {
                            if (e.Source.Id == node.Id) nList.Add(e.Target.Id.ToString());
                            else if (e.Target.Id == node.Id) nList.Add(e.Source.Id.ToString());
                        }
                        string neighbors = string.Join("-", nList);

                        string line = $"{node.Id};{node.Name};{node.Activity};{node.Interaction};{node.ConnectionCount};{neighbors};{node.X};{node.Y}";

                        sw.WriteLine(line);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kaydetme hatası: " + ex.Message);
                return false;
            }
        }

       
        public void SaveAdjacencyMatrix(Graph graph, string filePath)
        {
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                int count = graph.Nodes.Count;

                sw.Write("Nodes;"); 
                foreach (Node n in graph.Nodes)
                {
                    sw.Write(n.Id + ";");
                }
                sw.WriteLine();
                for (int i = 0; i < count; i++)
                {
                    Node rowNode = graph.Nodes[i];
                    sw.Write(rowNode.Id + ";"); 

                    for (int j = 0; j < count; j++)
                    {
                        Node colNode = graph.Nodes[j];

                        bool isConnected = graph.Edges.Exists(e =>
                            (e.Source == rowNode && e.Target == colNode) ||
                            (e.Source == colNode && e.Target == rowNode));

                        sw.Write((isConnected ? "1" : "0") + ";");
                    }
                    sw.WriteLine();
                }
            }
        }
    }
}

