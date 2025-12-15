namespace ProjectYazLab
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pnlGraph = new Panel();
            btnReset = new Button();
            btnLoadCSV = new Button();
            btnArrange = new Button();
            btnRunBFS = new Button();
            btnRunDFS = new Button();
            btn_Dijkstra = new Button();
            SuspendLayout();
            // 
            // pnlGraph
            // 
            pnlGraph.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlGraph.BorderStyle = BorderStyle.FixedSingle;
            pnlGraph.Location = new Point(215, 60);
            pnlGraph.Name = "pnlGraph";
            pnlGraph.Size = new Size(395, 331);
            pnlGraph.TabIndex = 0;
            pnlGraph.Paint += pnlGraph_Paint;
            pnlGraph.MouseClick += pnlGraph_MouseClick;
            pnlGraph.Resize += pnlGraph_Resize;
            // 
            // btnReset
            // 
            btnReset.Anchor = AnchorStyles.Left;
            btnReset.BackColor = SystemColors.ButtonFace;
            btnReset.Location = new Point(111, 188);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(86, 33);
            btnReset.TabIndex = 1;
            btnReset.Text = "Reset";
            btnReset.UseVisualStyleBackColor = false;
            btnReset.Click += button1_Click;
            // 
            // btnLoadCSV
            // 
            btnLoadCSV.Anchor = AnchorStyles.Right;
            btnLoadCSV.BackColor = SystemColors.ButtonFace;
            btnLoadCSV.Location = new Point(629, 278);
            btnLoadCSV.Name = "btnLoadCSV";
            btnLoadCSV.Size = new Size(88, 33);
            btnLoadCSV.TabIndex = 2;
            btnLoadCSV.Text = "Dosya Yükle";
            btnLoadCSV.UseVisualStyleBackColor = false;
            btnLoadCSV.Click += btnLoadCSV_Click;
            // 
            // btnArrange
            // 
            btnArrange.Anchor = AnchorStyles.Right;
            btnArrange.BackColor = SystemColors.ButtonFace;
            btnArrange.Location = new Point(629, 239);
            btnArrange.Name = "btnArrange";
            btnArrange.Size = new Size(88, 33);
            btnArrange.TabIndex = 3;
            btnArrange.Text = "Düzenle";
            btnArrange.UseVisualStyleBackColor = false;
            btnArrange.Click += btnArrange_Click;
            // 
            // btnRunBFS
            // 
            btnRunBFS.Anchor = AnchorStyles.Right;
            btnRunBFS.BackColor = SystemColors.ButtonFace;
            btnRunBFS.Location = new Point(629, 200);
            btnRunBFS.Name = "btnRunBFS";
            btnRunBFS.Size = new Size(88, 33);
            btnRunBFS.TabIndex = 4;
            btnRunBFS.Text = "BFS Başlat";
            btnRunBFS.UseVisualStyleBackColor = false;
            btnRunBFS.Click += btnRunBFS_Click;
            // 
            // btnRunDFS
            // 
            btnRunDFS.Anchor = AnchorStyles.Right;
            btnRunDFS.BackColor = SystemColors.ButtonFace;
            btnRunDFS.Location = new Point(629, 161);
            btnRunDFS.Name = "btnRunDFS";
            btnRunDFS.Size = new Size(88, 33);
            btnRunDFS.TabIndex = 5;
            btnRunDFS.Text = "DFS Başlat";
            btnRunDFS.UseVisualStyleBackColor = false;
            btnRunDFS.Click += btnRunDFS_Click;
            // 
            // btn_Dijkstra
            // 
            btn_Dijkstra.Anchor = AnchorStyles.Right;
            btn_Dijkstra.BackColor = SystemColors.ButtonFace;
            btn_Dijkstra.Location = new Point(629, 122);
            btn_Dijkstra.Name = "btn_Dijkstra";
            btn_Dijkstra.Size = new Size(88, 33);
            btn_Dijkstra.TabIndex = 6;
            btn_Dijkstra.Text = "En Kısa Yol";
            btn_Dijkstra.UseVisualStyleBackColor = false;
            btn_Dijkstra.Click += btn_Dijkstra_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(828, 450);
            Controls.Add(btn_Dijkstra);
            Controls.Add(btnRunDFS);
            Controls.Add(btnRunBFS);
            Controls.Add(btnArrange);
            Controls.Add(btnLoadCSV);
            Controls.Add(btnReset);
            Controls.Add(pnlGraph);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            MouseClick += Form1_MouseClick;
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlGraph;
        private Button btnReset;
        private Button btnLoadCSV;
        private Button btnArrange;
        private Button btnRunBFS;
        private Button btnRunDFS;
        private Button btn_Dijkstra;
    }
}
