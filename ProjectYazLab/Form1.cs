using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProjectYazLab
{
    public partial class Form1 : Form
    {
        Graph socialGraph = new Graph();
        Random rnd = new Random();
        int nodeIdCounter = 1;
        float oldWidth;
        float oldHeight;


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
            Brush nodeBrush = Brushes.DeepSkyBlue;
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

                // Eðer bu düðüm "Seçili" ise Rengini Kýrmýzý yap, deðilse Mavi
                if (node == selectedNode)
                {
                    g.FillEllipse(selectedBrush, drawX, drawY, diameter, diameter);
                }
                else
                {
                    g.FillEllipse(nodeBrush, drawX, drawY, diameter, diameter);
                }

                g.DrawEllipse(nodeBorder, drawX, drawY, diameter, diameter);

                // ID'yi ortalamak için basit bir ayar
                g.DrawString(node.Id.ToString(), font, Brushes.Black, drawX + 8, drawY + 8);
            }
        }

        private void pnlGraph_MouseClick(object sender, MouseEventArgs e)
        {
            // Týkladýðým yerde bir düðüm var mý?
            Node clickedNode = FindNodeAtPoint(e.X, e.Y);

            if (clickedNode != null)
            {
                // -- BÝR DÜÐÜME TIKLANDI --

                if (selectedNode == null)
                {
                    // Hiçbir þey seçili deðilse, bunu seç
                    selectedNode = clickedNode;
                }
                else
                {
                    // Zaten biri seçiliymiþ, þimdi ikinciye týkladýk -> BAÐLA
                    if (selectedNode != clickedNode) // Kendine baðlamayý engelle
                    {
                        socialGraph.AddEdge(selectedNode, clickedNode);
                        selectedNode = null; // Seçimi sýfýrla
                    }
                    else
                    {
                        // Ayný düðüme tekrar týklarsan seçimi iptal et
                        selectedNode = null;
                    }
                }
            }
            else
            {


                if (selectedNode != null)
                {
                    selectedNode = null;
                }
                else
                {
                    Node newNode = new Node(nodeIdCounter, "User" + nodeIdCounter, e.X, e.Y);

                    // RASTGELE DEÐERLER 

                    newNode.Activity = Math.Round(rnd.NextDouble(), 2);

                    // 1 ile 50 arasýnda rastgele etkileþim puaný
                    newNode.Interaction = rnd.Next(1, 50);


                    newNode.ConnectionCount = rnd.Next(0, 10);

                    socialGraph.AddNode(newNode);
                    nodeIdCounter++;
                }
            }

            // Ekraný yenile (Paint çalýþsýn)
            pnlGraph.Invalidate();
        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {

        }
        // Týklanan koordinatta bir düðüm var mý diye bakan yardýmcý metot
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

            // 1. Oraný Hesapla: Yeni Boyut / Eski Boyut
            // Örnek: Ekran 2 katýna çýktýysa oran 2.0 olur.
            float ratioX = pnlGraph.Width / oldWidth;
            float ratioY = pnlGraph.Height / oldHeight;

            // 2. Tüm düðümleri bu oranla çarp (Geniþlet veya Daralt)
            foreach (var node in socialGraph.Nodes)
            {
                node.X *= ratioX;
                node.Y *= ratioY;
            }

            // 3. Yeni boyutlarý "Eski" olarak kaydet (Bir sonraki deðiþim için)
            oldWidth = pnlGraph.Width;
            oldHeight = pnlGraph.Height;

            // 4. Yeniden çiz
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
    }
}
