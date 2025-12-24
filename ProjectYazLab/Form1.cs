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
            // Çizimlerde titremeyi engellemek için DoubleBuffered 
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
                                MessageBox.Show("Bu bağlantı zaten mevcut!");
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
                        $"Bu bağlantıyı silmek istiyor musunuz?\nAğırlık: {clickedEdge.Weight:F2}",
                        "Bağlantı Sil", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

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

                // Pisagor mantığı: A-B arasındaki mesafe ile (A-Mouse + Mouse-B) mesafesini kıyaslar.
                double distSourceToMouse = Math.Sqrt(Math.Pow(x1 - mouseX, 2) + Math.Pow(y1 - mouseY, 2));
                double distTargetToMouse = Math.Sqrt(Math.Pow(x2 - mouseX, 2) + Math.Pow(y2 - mouseY, 2));
                double edgeLength = Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));

                // Eğer mouse tam çizgi üzerindeyse (veya çok yakınsa) bu fark 0'a yakın olur.
                if (Math.Abs((distSourceToMouse + distTargetToMouse) - edgeLength) < tolerance)
                {
                    return edge;
                }
            }
            return null;
        }


        private Node FindNodeAtPoint(float x, float y)
        {
            int radius = 15; // Çizimdeki yarıçapla aynı olmalı

            foreach (var node in socialGraph.Nodes)
            {
                // pisagor teoremi ile nokta arası mesafe
                // dairenin içinde miyiz
                double distance = Math.Sqrt(Math.Pow(node.X - x, 2) + Math.Pow(node.Y - y, 2));

                if (distance <= radius)
                {
                    return node; //bu düğüme tıklanıldı
                }
            }
            return null;
        }


        private void btnLoadCSV_Click(object sender, EventArgs e)
        {
            // dosya seçme penceresi açıq
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV Dosyaları|*.csv|Tüm Dosyalar|*.*";
            openFileDialog.Title = "Yüklenecek Graph Dosyasını Seçin";


            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedPath = openFileDialog.FileName; 

                IFileHandler fileHandler = new FileManager();

                // gelen yolu kullanarak grafı yüklüyoruz
                Graph loadedGraph = fileHandler.LoadGraphFromCSV(selectedPath, pnlGraph.Width, pnlGraph.Height);

                if (loadedGraph != null)
                {
                    // yüklenen graph=ana graph
                    socialGraph = loadedGraph;

                    // ID sayacını güncelle 
                    if (socialGraph.Nodes.Count > 0)
                    {
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
                    MessageBox.Show("Dosya başarıyla yüklendi: " + System.IO.Path.GetFileName(selectedPath));
                }
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

            //yeni boyut/eski boyut ile oran alarak ne oranda değiştiğini buluyoruz
            float ratioX = pnlGraph.Width / oldWidth;
            float ratioY = pnlGraph.Height / oldHeight;


            foreach (var node in socialGraph.Nodes)
            {
                node.X *= ratioX;
                node.Y *= ratioY;
            }

            // yeni boyutları eski olarak kaydediyoruz ki sonraki değişimde baştan başlasın
            oldWidth = pnlGraph.Width;
            oldHeight = pnlGraph.Height;


            pnlGraph.Invalidate();
        }

        private void btnArrange_Click(object sender, EventArgs e)
        {
            if (socialGraph.Nodes.Count == 0) return;

            // Panelin tam ortasını buluyoruz ve çember yarıçapını ayarlıyoruz
            int centerX = pnlGraph.Width / 2;
            int centerY = pnlGraph.Height / 2;
            int radius = Math.Min(centerX, centerY) - 50; // Yarıçap, Kenarlardan 50px boşluk bırakmıştık nodeların dağılımını düzenlerken 

            double angleStep = 360.0 / socialGraph.Nodes.Count; // düğüm sayısına bölerek her düğüme düşen açı yani iki düğüm arasındaki mesafeyi buluyoruz

            for (int i = 0; i < socialGraph.Nodes.Count; i++)
            {
                // elimizde açı ve yarıçap var,ekrana çizmek için yatay ve dikey koordinatlar lazım yani x ve y.  x ve y'yi hesaplamak için cos(x) bir noktanın merkeze olan yatay uzaklığını, sin(x) ise dikey uzaklığını verir bu şekilde düğümleri çember etrafına yerleştiriyoruz
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
                MessageBox.Show("Lütfen BFS'nin başlayacağı düğümü seçin, ardından BFS'i başlatın.");
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
                MessageBox.Show("Lütfen DFS'nin başlayacağı düğümü seçin, ardından DFS'i başlatın.");
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
                MessageBox.Show("Lütfen önce sol tık ile kaynak, sağ tık ile hedef düğümü seçin.");
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
                // eğer bu düğüm başlangıç veya hedef değilse, rengini sıfırla
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
                MessageBox.Show("Lütfen sayısal değerleri doğru giriniz!");
            }
        }

        private void btn_Delete_Click(object sender, EventArgs e)
        {
            if (selectedNode == null) return;

            socialGraph.Edges.RemoveAll(edge => edge.Source == selectedNode || edge.Target == selectedNode);

            // 2. Düğümü sil
            socialGraph.Nodes.Remove(selectedNode);

            selectedNode = null;
            ShowNodeInfo();
            pnlGraph.Invalidate();

        }
        private async void btn_AStar_Click(object sender, EventArgs e)
        {
            // Başlangıç ve Bitiş seçili mi kontrol et
            if (startNode == null || endNode == null)
            {
                MessageBox.Show("Lütfen sol tık ile kaynak, sağ tık ile hedef düğümü seçin.");
                return;
            }

            Algorithms algo = new Algorithms();

            // Butonu pasif yap (çift tıklamayı önlemek için)
            btn_AStar.Enabled = false;

            // socialGraph: Senin ana graf değişkenin
            // startNode, endNode: Seçili düğümler
            await algo.RunAStar(socialGraph, startNode, endNode, pnlGraph, label_Duration);

            // İşlem bitince butonu aç
            btn_AStar.Enabled = true;
        }


        private void btn_Save_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV Dosyası|*.csv|Metin Dosyası (Matris)|*.txt";
            saveFileDialog.Title = "Grafı Kaydet";
            saveFileDialog.FileName = "users_export.csv"; // varsayılan isim

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                IFileHandler fileHandler = new FileManager();

                if (saveFileDialog.FileName.EndsWith(".csv"))
                {
                    bool basari = fileHandler.SaveGraphToCSV(socialGraph, saveFileDialog.FileName);
                    if (basari) MessageBox.Show("CSV dosyası başarıyla kaydedildi!");
                }
                else
                {
                    // İster madde 31: Komşuluk matrisi formatında kaydet
                    fileHandler.SaveAdjacencyMatrix(socialGraph, saveFileDialog.FileName);
                    MessageBox.Show("Matris başarıyla kaydedildi!");
                }
            }
        }

        private void btn_Centrality_Click(object sender, EventArgs e)
        {
            if (socialGraph == null || socialGraph.Nodes.Count == 0)
            {
                MessageBox.Show("Graf boş! Önce veri yükleyin.");
                return;
            }

            // 1. Yeni Bir Pencere (Pop-up Form) Oluşturuyoruz
            Form popupForm = new Form();
            popupForm.Text = "En Etkili 5 Kullanıcı (Degree Centrality)";
            popupForm.Size = new Size(500, 300);
            popupForm.StartPosition = FormStartPosition.CenterParent; // Ana ekranın ortasında aç

            // 2. İçine Bir Tablo (DataGridView) Oluşturuyoruz
            DataGridView dgv = new DataGridView();
            dgv.Dock = DockStyle.Fill; // Pencerenin tamamını kaplasın
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; // Sütunları yay
            dgv.ReadOnly = true;       // Kullanıcı değiştiremesin
            dgv.AllowUserToAddRows = false; // Boş satır eklemesin
            dgv.RowHeadersVisible = false;  // Soldaki gri başlığı gizle

            // 3. Tabloyu Pencereye Ekle
            popupForm.Controls.Add(dgv);

            // 4. Algoritmayı çalıştır ve Tabloyu Doldur
            Algorithms algo = new Algorithms();
            algo.CalculateDegreeCentrality(socialGraph, dgv);

            // 5. Pencereyi Göster (ShowDialog: Kapatmadan arkaya geçilemez)
            popupForm.ShowDialog();
        }
        private void btn_Components_Click(object sender, EventArgs e)
        {
            if (socialGraph == null || socialGraph.Nodes.Count == 0)
            {
                MessageBox.Show("Graf boş! Önce veri yükleyin.");
                return;
            }

            Algorithms algo = new Algorithms();

            // Algoritmayı çalıştır ve listeyi al
            List<List<Node>> components = algo.GetConnectedComponents(socialGraph);

            // --- SONUCU GÖSTERME (POP-UP) ---
            Form resultForm = new Form();
            resultForm.Text = $"Analiz Sonucu: {components.Count} Adet Ayrık Topluluk Bulundu";
            resultForm.Size = new Size(400, 400);
            resultForm.StartPosition = FormStartPosition.CenterParent;

            // Sonuçları yazacağımız büyük metin kutusu
            RichTextBox rtb = new RichTextBox();
            rtb.Dock = DockStyle.Fill;
            rtb.Font = new Font("Consolas", 10); // Düzgün hizalama için
            rtb.ReadOnly = true;

            resultForm.Controls.Add(rtb);

            // Yazıları oluştur
            rtb.AppendText($"TOPLAM AYRIK TOPLULUK SAYISI: {components.Count}\n");
            rtb.AppendText("--------------------------------------------------\n\n");

            for (int i = 0; i < components.Count; i++)
            {
                rtb.AppendText($"TOPLULUK #{i + 1} (Kişi Sayısı: {components[i].Count})\n");
                rtb.AppendText("- Üyeler: ");

                // O topluluktaki kişilerin isimlerini yazdır
                List<string> names = new List<string>();
                foreach (var node in components[i])
                {
                    names.Add($"{node.Name} (ID:{node.Id})");
                }

                rtb.AppendText(string.Join(", ", names));
                rtb.AppendText("\n\n");
            }

            resultForm.ShowDialog();
        }
    }
}
