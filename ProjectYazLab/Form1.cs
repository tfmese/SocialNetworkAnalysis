using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using ProjectYazLab.Models;
using ProjectYazLab.Services;
using ProjectYazLab.Interfaces;

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
            if (socialGraph.Nodes.Count > 0)
            {
                DialogResult result = MessageBox.Show(
                    "Tüm grafi silmek istediğinize emin misiniz?",
                    "Grafi Sıfırla",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result != DialogResult.Yes)
                    return;
            }

            socialGraph = new Graph();
            nodeIdCounter = 1;
            selectedNode = null;
            startNode = null;
            endNode = null;
            label_Duration.Text = "Süre: 0ms";
            UpdateGraphStats();
            pnlGraph.Invalidate();
            ShowNodeInfo();
        }

        private void UpdateGraphStats()
        {
            int nodeCount = socialGraph.Nodes.Count;
            int edgeCount = socialGraph.Edges.Count;
            label_Stats.Text = $"Düğüm: {nodeCount} | Kenar: {edgeCount}";
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

            UpdateGraphStats();
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

                IFileHandler fileHandler = new ProjectYazLab.Services.FileManager();

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

                    UpdateGraphStats();
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

            label_Duration.Text = "Hesaplanıyor...";
            ProjectYazLab.Services.Algorithms algo = new ProjectYazLab.Services.Algorithms();

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

            label_Duration.Text = "Hesaplanıyor...";
            ProjectYazLab.Services.Algorithms algo = new ProjectYazLab.Services.Algorithms();
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

            label_Duration.Text = "Hesaplanıyor...";
            ProjectYazLab.Services.Algorithms algo = new ProjectYazLab.Services.Algorithms();
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
            UpdateGraphStats();
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

            label_Duration.Text = "Hesaplanıyor...";
            ProjectYazLab.Services.Algorithms algo = new ProjectYazLab.Services.Algorithms();

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
                IFileHandler fileHandler = new ProjectYazLab.Services.FileManager();

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
            ProjectYazLab.Services.Algorithms algo = new ProjectYazLab.Services.Algorithms();
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

            ProjectYazLab.Services.Algorithms algo = new ProjectYazLab.Services.Algorithms();

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

        private void btn_Coloring_Click(object sender, EventArgs e)
        {
            if (socialGraph == null || socialGraph.Nodes.Count == 0)
            {
                MessageBox.Show("Graf boş! Önce veri yükleyin.");
                return;
            }

            label_Duration.Text = "Hesaplanıyor...";
            ProjectYazLab.Services.Algorithms algo = new ProjectYazLab.Services.Algorithms();

            // Welsh-Powell renklendirme algoritmasını çalıştır
            List<ColoringResult> coloringResults = algo.RunWelshPowellColoring(socialGraph, label_Duration);

            // Grafi yeniden çiz
            pnlGraph.Invalidate();

            // --- SONUCU GÖSTERME (POP-UP) ---
            Form resultForm = new Form();
            resultForm.Text = $"Welsh-Powell Renklendirme Sonuçları - {coloringResults.Count} Topluluk";
            resultForm.Size = new Size(700, 500);
            resultForm.StartPosition = FormStartPosition.CenterParent;

            // DataGridView ile tablo oluştur
            DataGridView dgv = new DataGridView();
            dgv.Dock = DockStyle.Fill;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.ReadOnly = true;
            dgv.AllowUserToAddRows = false;
            dgv.RowHeadersVisible = false;

            // Sütunları oluştur
            dgv.Columns.Add("ToplulukNo", "Topluluk No");
            dgv.Columns.Add("DugumID", "Düğüm ID");
            dgv.Columns.Add("DugumAdi", "Düğüm Adı");
            dgv.Columns.Add("RenkIndex", "Renk İndeksi");
            dgv.Columns.Add("RenkAdi", "Renk Adı");
            dgv.Columns.Add("KullanilanRenkSayisi", "Toplam Renk Sayısı");

            // Renk adları için palet
            string[] colorNames = new string[]
            {
                "Kırmızı", "Yeşil", "Mavi", "Sarı", "Turuncu", "Mor", "Cyan", "Magenta",
                "Lime", "Pembe", "Kahverengi", "Altın", "Gümüş", "Teal", "İndigo", "Coral",
                "Turkuaz", "Menekşe", "Haki", "Somon"
            };

            // Özet bilgi için string oluştur
            string summary = $"TOPLAM TOPLULUK SAYISI: {coloringResults.Count}\n\n";
            
            // Her topluluk için verileri ekle
            for (int i = 0; i < coloringResults.Count; i++)
            {
                var result = coloringResults[i];
                bool isFirstRow = true;
                
                // Bu toplulukta kullanılan renkleri topla
                HashSet<int> usedColorIndices = new HashSet<int>();
                foreach (var node in result.Component)
                {
                    if (result.NodeColors.ContainsKey(node))
                    {
                        usedColorIndices.Add(result.NodeColors[node]);
                    }
                }
                
                summary += $"TOPLULUK #{i + 1}: {result.Component.Count} düğüm, {result.ColorCount} renk kullanıldı\n";
                summary += $"Kullanılan Renkler: ";
                List<string> usedColorNames = new List<string>();
                foreach (int idx in usedColorIndices.OrderBy(x => x))
                {
                    string colorName = idx < colorNames.Length ? colorNames[idx] : $"Renk {idx + 1}";
                    usedColorNames.Add($"{colorName} (İndeks: {idx})");
                }
                summary += string.Join(", ", usedColorNames) + "\n\n";

                foreach (var node in result.Component)
                {
                    int colorIndex = result.NodeColors[node];
                    string colorName = colorIndex < colorNames.Length ? colorNames[colorIndex] : $"Renk {colorIndex + 1}";

                    dgv.Rows.Add(
                        isFirstRow ? (i + 1).ToString() : "",
                        node.Id.ToString(),
                        node.Name,
                        colorIndex.ToString(),
                        colorName,
                        isFirstRow ? result.ColorCount.ToString() : ""
                    );

                    isFirstRow = false;
                }

                // Topluluklar arası boş satır ekle
                if (i < coloringResults.Count - 1)
                {
                    dgv.Rows.Add("", "", "", "", "", "");
                }
            }

            // Özet bilgiyi göster
            MessageBox.Show(summary, "Renklendirme Özeti", MessageBoxButtons.OK, MessageBoxIcon.Information);

            resultForm.Controls.Add(dgv);
            resultForm.ShowDialog();
        }

        private void btn_ResetColors_Click(object sender, EventArgs e)
        {
            foreach (var node in socialGraph.Nodes)
            {
                // Eğer düğüm seçili değilse, rengini varsayılan maviye döndür
                if (node != selectedNode && node != startNode && node != endNode)
                {
                    node.CurrentColor = Color.Blue;
                }
            }
            pnlGraph.Invalidate();
        }
    }
}
