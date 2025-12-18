using System;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
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


        Node selectedNode = null;

        public Form1()
        {
            InitializeComponent();
            // çizimlerde titremeyi engellemek için DoubleBuffered 
            this.DoubleBuffered = true;
            oldWidth = pnlGraph.Width;
            oldHeight = pnlGraph.Height;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            oldWidth = pnlGraph.Width;
            oldHeight = pnlGraph.Height;
        }

        private void btn_Reset_Click(object sender, EventArgs e)
        {
            socialGraph = new Graph();
            nodeIdCounter = 1;
            selectedNode = null;
            pnlGraph.Invalidate();
            ShowNodeInfo();
        }

        private void pnlGraph_Paint(object sender, PaintEventArgs e)
        {
            Pen edgePen = new Pen(Color.Gray, 2);

            Brush selectedBrush = Brushes.OrangeRed;
            Pen nodeBorder = new Pen(Color.Black, 2);
            Font font = new Font("Arial", 10);
            int radius = 15;

            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            foreach (var edge in socialGraph.Edges)
            {
                g.DrawLine(edgePen, edge.Source.X, edge.Source.Y, edge.Target.X, edge.Target.Y);

                float midX = (edge.Source.X + edge.Target.X) / 2;
                float midY = (edge.Source.Y + edge.Target.Y) / 2;
                g.FillRectangle(Brushes.White, midX, midY, 25, 15);
                g.DrawString(edge.Weight.ToString("0.0"), font, Brushes.Red, midX, midY);
            }

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
                            var existingEdge = socialGraph.Edges.FirstOrDefault(edge =>
                                (edge.Source == selectedNode && edge.Target == clickedNode) ||
                                (edge.Source == clickedNode && edge.Target == selectedNode));

                            if (existingEdge == null)
                            {
                                Edge newEdge = new Edge(selectedNode, clickedNode);
                                socialGraph.Edges.Add(newEdge);
                            }
                            else
                            {
                                MessageBox.Show("Bu baðlantý zaten mevcut!");
                            }
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

                    if (endNode != null) endNode.CurrentColor = Color.Blue;
                    endNode = clickedNode;
                    endNode.CurrentColor = Color.DarkSeaGreen;

                    MessageBox.Show($"Hedef Belirlendi: {endNode.Name}");
                }
            }
            else
            {
                Edge clickedEdge = FindEdgeAtPoint(e.X, e.Y);

                if (clickedEdge != null)
                {
                    DialogResult result = MessageBox.Show(
                        $"Bu baðlantýyý silmek istiyor musunuz?\nAðýrlýk: {clickedEdge.Weight:F2}",
                        "Baðlantý Sil", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        socialGraph.Edges.Remove(clickedEdge);
                        pnlGraph.Invalidate();
                    }
                }
                else
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        
                        if (selectedNode != null || startNode != null || endNode != null)
                        {
                            startNode = null;  
                            endNode = null;    
                            selectedNode = null; 

                            foreach (var node in socialGraph.Nodes)
                            {
                                node.CurrentColor = Color.Blue;
                                node.Visited = false;
                            }
                            foreach (var edge in socialGraph.Edges)
                            {
                                edge.Color = Color.Black;
                                edge.Thickness = 2;
                            }
                        }
                        else
                        {
                            Node newNode = new Node(nodeIdCounter, "User" + nodeIdCounter, e.X, e.Y);
                            newNode.Activity = Math.Round(rnd.NextDouble(), 2);
                            newNode.Interaction = rnd.Next(1, 50);
                            newNode.ConnectionCount = rnd.Next(0, 10);
                            newNode.CurrentColor = Color.Blue;

                            socialGraph.AddNode(newNode);
                            nodeIdCounter++;
                        }
                    }
                }
            }

            pnlGraph.Invalidate();
            ShowNodeInfo();
        }
        private Edge FindEdgeAtPoint(int mouseX, int mouseY)
        {
            float tolerance = 10.0f;

            foreach (var edge in socialGraph.Edges)
            {
                float x1 = edge.Source.X;
                float y1 = edge.Source.Y;
                float x2 = edge.Target.X;
                float y2 = edge.Target.Y;

                // Pisagor mantýðý: A-B arasýndaki mesafe ile (A-Mouse + Mouse-B) mesafesini kýyaslar.
                double distSourceToMouse = Math.Sqrt(Math.Pow(x1 - mouseX, 2) + Math.Pow(y1 - mouseY, 2));
                double distTargetToMouse = Math.Sqrt(Math.Pow(x2 - mouseX, 2) + Math.Pow(y2 - mouseY, 2));
                double edgeLength = Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));

                // Eðer mouse tam çizgi üzerindeyse (veya çok yakýnsa) bu fark 0'a yakýn olur.
                if (Math.Abs((distSourceToMouse + distTargetToMouse) - edgeLength) < tolerance)
                {
                    return edge;
                }
            }
            return null;
        }


        private Node FindNodeAtPoint(float x, float y)
        {
            int radius = 15; // çizimdeki yarýçapla ayný olmalý

            foreach (var node in socialGraph.Nodes)
            {
                // pisagor teoremi ile nokta arassý mesafe
                // dairenin içinde miyiz
                double distance = Math.Sqrt(Math.Pow(node.X - x, 2) + Math.Pow(node.Y - y, 2));

                if (distance <= radius)
                {
                    return node; //bu düðüme týkanldý
                }
            }
            return null; 
        }


        private void btnLoadCSV_Click(object sender, EventArgs e)
        {
            FileManager fileManager = new FileManager();

            string path = "users.csv";

            // Oku ve gradiðe koy
            Graph loadedGraph = fileManager.LoadGraphFromCSV(path, pnlGraph.Width, pnlGraph.Height);

            if (loadedGraph != null)
            {
                socialGraph = loadedGraph;

                
                if (socialGraph.Nodes.Count > 0)
                {
                    // Listeyi ID ye göre sýrala, sonuncuyu al
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

                pnlGraph.Invalidate();
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

            await algo.RunBFS(socialGraph, selectedNode, pnlGraph, label_Duration);

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
            await algo.RunDFS(socialGraph, selectedNode, pnlGraph, label_Duration);
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
            await algo.RunDijkstra(socialGraph, startNode, endNode, pnlGraph, label_Duration);
            btn_Dijkstra.Enabled = true;




        }
        private void ClearPreviousPath()
        {
            foreach (var node in socialGraph.Nodes)
            {
                // eðer bu düðüm baþlangýç veya hedef deðilse, rengini sýfýrla
                if (node != startNode && node != endNode && node != selectedNode)
                {
                    node.CurrentColor = Color.Blue;
                }
            }
        }

        private void ShowNodeInfo()
        {
            if (selectedNode != null)
            {
                txt_ID.Text = selectedNode.Id.ToString();
                txt_Name.Text = selectedNode.Name;
                txt_Interaction.Text = selectedNode.Interaction.ToString();
                txt_Activity.Text = selectedNode.Activity.ToString();

                btn_Update.Enabled = true;
                btn_Delete.Enabled = true;
            }
            else
            {
                txt_ID.Clear();
                txt_Name.Clear();
                txt_Interaction.Clear();
                txt_Activity.Clear();

                btn_Update.Enabled = false;
                btn_Delete.Enabled = false;
            }
        }

        private void btn_Update_Click(object sender, EventArgs e)
        {
            if (selectedNode == null) return;

            try
            {
                selectedNode.Name = txt_Name.Text;
                selectedNode.Interaction = double.Parse(txt_Interaction.Text);
                selectedNode.Activity = double.Parse(txt_Activity.Text);


                foreach (var edge in socialGraph.Edges)
                {
                    if (edge.Source == selectedNode || edge.Target == selectedNode)
                    {
                        edge.Weight = edge.CalculateWeight();
                    }
                }



                pnlGraph.Invalidate();
                MessageBox.Show("Bilgiler güncellendi!");
            }
            catch
            {
                MessageBox.Show("Lütfen sayýsal deðerleri doðru giriniz!");
            }
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            if (selectedNode == null) return;

            socialGraph.Edges.RemoveAll(edge => edge.Source == selectedNode || edge.Target == selectedNode);

            // 2. Düðümü sil
            socialGraph.Nodes.Remove(selectedNode);

            selectedNode = null;
            ShowNodeInfo(); 
            pnlGraph.Invalidate(); 

        }
    }
}
