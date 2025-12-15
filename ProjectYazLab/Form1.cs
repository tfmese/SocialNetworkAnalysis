using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Diagnostics.Eventing.Reader;
namespace ProjectYazLab
{
    public partial class Form1 : Form
    {
        Graph socialGraph = new Graph();
        Random rnd = new Random();

        int nodeIdCounter = 1;
        float oldWidth;
        float oldHeight;

        Node startNode = null;
        Node endNode = null;


        // Baðlantý kurmak için "Seçili olan düðümü" hafýzada tutmamýz lazým
        Node selectedNode = null;

        public Form1()
        {
            InitializeComponent();
            // Çizimlerde titremeyi engellemek için DoubleBuffered açalým
            this.DoubleBuffered = true;
            oldWidth = pnlGraph.Width;
            oldHeight = pnlGraph.Height;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Temizle butonu (Eðer butona btnReset ismini vermediysen button1 kalabilir)
            socialGraph = new Graph();
            nodeIdCounter = 1;
            selectedNode = null;
            pnlGraph.Invalidate();
        }

        private void pnlGraph_Paint(object sender, PaintEventArgs e)
        {
            Pen edgePen = new Pen(Color.Gray, 2);

            Brush selectedBrush = Brushes.OrangeRed; // Seçili olan farklý renk olsun
            Pen nodeBorder = new Pen(Color.Black, 2);
            Font font = new Font("Arial", 10);
            int radius = 15;

            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // 1. Kenarlarý (Edges) Çiz
            foreach (var edge in socialGraph.Edges)
            {
                g.DrawLine(edgePen, edge.Source.X, edge.Source.Y, edge.Target.X, edge.Target.Y);

                // Aðýrlýðý yazdýr
                float midX = (edge.Source.X + edge.Target.X) / 2;
                float midY = (edge.Source.Y + edge.Target.Y) / 2;
                // Arkaplaný beyaz yapalým ki çizgi üstünde okunabilsin
                g.FillRectangle(Brushes.White, midX, midY, 25, 15);
                g.DrawString(edge.Weight.ToString("0.0"), font, Brushes.Red, midX, midY);
            }

            // 2. Düðümleri (Nodes) Çiz
            foreach (var node in socialGraph.Nodes)
            {
                float drawX = node.X - radius;
                float drawY = node.Y - radius;
                float diameter = radius * 2;
                Brush currentBrush;
                if (node == selectedNode)
                {
                    currentBrush = selectedBrush;
                }
                else
                {
                    currentBrush = new SolidBrush(node.CurrentColor);
                }
                g.FillEllipse(currentBrush, drawX, drawY, diameter, diameter);
                g.DrawEllipse(nodeBorder, drawX, drawY, diameter, diameter);

                // ID'yi ortalamak için basit bir ayar
                g.DrawString(node.Id.ToString(), font, Brushes.Black, drawX + 8, drawY + 8);
            }
        }

        private void pnlGraph_MouseClick(object sender, MouseEventArgs e)
        {
            Node clickedNode = FindNodeAtPoint(e.X, e.Y);

            if (clickedNode != null)
            {
                

               
                if (e.Button == MouseButtons.Left)
                {
                    ClearPreviousPath();

                    startNode = clickedNode;

                   
                    if (selectedNode == null)
                    {
                        selectedNode = clickedNode;
                        selectedNode.CurrentColor = Color.OrangeRed;
                    }
                    else
                    {
                        if (selectedNode != clickedNode)
                        {
                            socialGraph.AddEdge(selectedNode, clickedNode);

                            selectedNode.CurrentColor = Color.Blue; 
                            selectedNode = null;
                        }
                        else
                        {
                            selectedNode.CurrentColor = Color.Blue;
                            selectedNode = null;
                        }
                    }
                }

                else if (e.Button == MouseButtons.Right)
                {
                    ClearPreviousPath();

                    if (endNode != null)
                    {
                        endNode.CurrentColor = Color.Blue;
                    }
                    endNode = clickedNode;
                    endNode.CurrentColor = Color.DarkSeaGreen;

                    MessageBox.Show($"Hedef Belirlendi: {endNode.Name}");
                }
            }
            else
            {
                if (e.Button == MouseButtons.Left)
                {

                    if (selectedNode != null)
                    {
                        selectedNode.CurrentColor = Color.Blue;
                        selectedNode = null;
                    }
                    else
                    {
                        Node newNode = new Node(nodeIdCounter, "User" + nodeIdCounter, e.X, e.Y);
                        newNode.Activity = Math.Round(rnd.NextDouble(), 2);
                        newNode.Interaction = rnd.Next(1, 50);
                        newNode.ConnectionCount = rnd.Next(0, 10);

                        socialGraph.AddNode(newNode);
                        nodeIdCounter++;
                    }
                }
            }

            pnlGraph.Invalidate();
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {

        }
        private Node FindNodeAtPoint(float x, float y)
        {
            int radius = 15; // Çizimdeki yarýçapla ayný olmalý

            foreach (var node in socialGraph.Nodes)
            {
                // Pisagor teoremi: Ýki nokta arasýndaki mesafe
                // Dairenin içinde miyiz?
                double distance = Math.Sqrt(Math.Pow(node.X - x, 2) + Math.Pow(node.Y - y, 2));

                if (distance <= radius)
                {
                    return node; // Bu düðüme týkladýk!
                }
            }
            return null; // Boþluða týkladýk
        }


        private void btnLoadCSV_Click(object sender, EventArgs e)
        {
            // Dosya yöneticimizi çaðýrýyoruz
            FileManager fileManager = new FileManager();

            // Dosya yolunu belirtiyoruz (bin/Debug klasöründe olmalý)
            string path = "users.csv";

            // Oku ve grafiðe yükle
            Graph loadedGraph = fileManager.LoadGraphFromCSV(path, pnlGraph.Width, pnlGraph.Height);

            if (loadedGraph != null)
            {
                // Mevcut grafiði yenisiyle deðiþtir
                socialGraph = loadedGraph;

                // ID sayacýný güncelle (en son eklenen ID'den devam etsin diye)
                // Linq kullanarak en yüksek ID'yi buluyoruz, yoksa 1 yapýyoruz
                if (socialGraph.Nodes.Count > 0)
                {
                    // Listeyi ID'ye göre sýrala, sonuncuyu al
                    int maxId = 0;
                    foreach (var node in socialGraph.Nodes)
                    {
                        if (node.Id > maxId) maxId = node.Id;
                    }
                    nodeIdCounter = maxId + 1;
                }
                else
                {
                    nodeIdCounter = 1;
                }

                // Ekraný yenile
                pnlGraph.Invalidate();
                MessageBox.Show("Veriler baþarýyla yüklendi!");
            }
        }

        private void pnlGraph_Resize(object sender, EventArgs e)
        {
            if (socialGraph.Nodes.Count == 0 || oldWidth == 0 || oldHeight == 0)
            {
                oldWidth = pnlGraph.Width;
                oldHeight = pnlGraph.Height;
                return;
            }

            //ynei boyut/eski boyut ile oran alarak ne oranda deðiþtiðini buluyoruz
            float ratioX = pnlGraph.Width / oldWidth;
            float ratioY = pnlGraph.Height / oldHeight;

            
            foreach (var node in socialGraph.Nodes)
            {
                node.X *= ratioX;
                node.Y *= ratioY;
            }

            // yeni boyutlarý eski olarak kaydediyoruz ki sonraki deðiþimde baþtan baþlasýn
            oldWidth = pnlGraph.Width;
            oldHeight = pnlGraph.Height;

            
            pnlGraph.Invalidate();
        }

        private void btnArrange_Click(object sender, EventArgs e)
        {
            if (socialGraph.Nodes.Count == 0) return;

            // Panelin tam ortasýný buluyoruz ve çember yarýçapýný ayarlýyoruz
            int centerX = pnlGraph.Width / 2;
            int centerY = pnlGraph.Height / 2;
            int radius = Math.Min(centerX, centerY) - 50; // Yarýçap, Kenarlardan 50px boþluk býrakmýþtýk nodelarýn daðýlýmýný düzenlerken 

            double angleStep = 360.0 / socialGraph.Nodes.Count; // düðüm sayýsýna bölerek her düðüme düþen açý yani iki düðüm arasýndaki mesafeyi buluyoruz

            for (int i = 0; i < socialGraph.Nodes.Count; i++)
            {
                // elimizde açý ve yarýçap var,ekrana çizmek için yatay ve dikey koordinatlar lazým yani x ve y.  x ve y'yi hesaplamak için cos(x) bir noktanýn merkeze olan yatay uzaklýðýný, sin(x) ise dikey uzaklýðýný verir bu þekilde düðümleri çember etrafýna yerleþtiriyoruz
                double angle = (i * angleStep) * (Math.PI / 180); // Dereceyi Radyana çevir

                socialGraph.Nodes[i].X = centerX + (float)(radius * Math.Cos(angle));
                socialGraph.Nodes[i].Y = centerY + (float)(radius * Math.Sin(angle));
            }

            pnlGraph.Invalidate();
        }

        private async void btnRunBFS_Click(object sender, EventArgs e)
        {
            if (selectedNode == null)
            {
                MessageBox.Show("Lütfen BFS'nin baþlayacaðý düðümü seçin, ardýndan BFS'i baþlatýn.");
                return;
            }


            Algorithms algo = new Algorithms();

            btnRunBFS.Enabled = false;

            await algo.RunBFS(socialGraph, selectedNode, pnlGraph);

            btnRunBFS.Enabled = true;
        }

        private async void btnRunDFS_Click(object sender, EventArgs e)
        {
            if (selectedNode == null)
            {
                MessageBox.Show("Lütfen DFS'nin baþlayacaðý düðümü seçin, ardýndan DFS'i baþlatýn.");
                return;
            }

            Algorithms algo = new Algorithms();
            btnRunDFS.Enabled = false;
            await algo.RunDFS(socialGraph, selectedNode, pnlGraph);
            btnRunDFS.Enabled = true;

        }

        private async void btn_Dijkstra_Click(object sender, EventArgs e)
        {
            if (startNode == null || endNode == null)
            {
                MessageBox.Show("Lütfen önce sol týk ile kaynak, sað týk ile hedef düðümü seçin.");
                return;
            }

            Algorithms algo = new Algorithms();
            btn_Dijkstra.Enabled = false;
            await algo.RunDijkstra(socialGraph, startNode, endNode, pnlGraph);
            btn_Dijkstra.Enabled = true;


            

        }
        private void ClearPreviousPath()
        {
            foreach (var node in socialGraph.Nodes)
            {
                // Eðer bu düðüm Baþlangýç veya Hedef DEÐÝLSE, rengini sýfýrla
                if (node != startNode && node != endNode && node != selectedNode)
                {
                    // Eski morluklarý, yeþillikleri sil, normale dön
                    node.CurrentColor = Color.Blue;
                }
            }
        }

    }
}
