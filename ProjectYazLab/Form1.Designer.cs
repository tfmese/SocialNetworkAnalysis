namespace ProjectYazLab
{
    partial class Form1
    {
        
        private System.ComponentModel.IContainer components = null;


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
            pnlMenu = new Panel();
            btn_Centrality = new Button();
            btn_AStar = new Button();
            btn_Save = new Button();
            label_Duration = new Label();
            pnlGroup = new Panel();
            txt_Activity = new TextBox();
            txt_Interaction = new TextBox();
            txt_Name = new TextBox();
            txt_ID = new TextBox();
            label_activity = new Label();
            label_interaction = new Label();
            label_name = new Label();
            label_ID = new Label();
            btn_Update = new Button();
            btn_Delete = new Button();
            labelGroup = new Label();
            label5 = new Label();
            label_Stats = new Label();
            btn_Components = new Button();
            btn_Coloring = new Button();
            btn_ResetColors = new Button();
            pnlMenu.SuspendLayout();
            pnlGroup.SuspendLayout();
            SuspendLayout();
            // 
            // pnlGraph
            // 
            pnlGraph.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlGraph.BackColor = SystemColors.ControlDark;
            pnlGraph.BorderStyle = BorderStyle.FixedSingle;
            pnlGraph.Location = new Point(29, 78);
            pnlGraph.Name = "pnlGraph";
            pnlGraph.Size = new Size(379, 442);
            pnlGraph.TabIndex = 0;
            pnlGraph.Paint += pnlGraph_Paint;
            pnlGraph.MouseClick += pnlGraph_MouseClick;
            pnlGraph.Resize += pnlGraph_Resize;
            // 
            // btnReset
            // 
            btnReset.BackColor = SystemColors.ButtonFace;
            btnReset.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnReset.Location = new Point(277, 295);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(86, 33);
            btnReset.TabIndex = 1;
            btnReset.Text = "Reset";
            btnReset.UseVisualStyleBackColor = false;
            btnReset.Click += btn_Reset_Click;
            // 
            // btnLoadCSV
            // 
            btnLoadCSV.BackColor = SystemColors.ButtonFace;
            btnLoadCSV.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnLoadCSV.Location = new Point(60, 295);
            btnLoadCSV.Name = "btnLoadCSV";
            btnLoadCSV.Size = new Size(88, 33);
            btnLoadCSV.TabIndex = 2;
            btnLoadCSV.Text = "Dosya Yükle";
            btnLoadCSV.UseVisualStyleBackColor = false;
            btnLoadCSV.Click += btnLoadCSV_Click;
            // 
            // btnArrange
            // 
            btnArrange.BackColor = SystemColors.ButtonFace;
            btnArrange.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnArrange.Location = new Point(170, 295);
            btnArrange.Name = "btnArrange";
            btnArrange.Size = new Size(88, 33);
            btnArrange.TabIndex = 3;
            btnArrange.Text = "Düzenle";
            btnArrange.UseVisualStyleBackColor = false;
            btnArrange.Click += btnArrange_Click;
            // 
            // btnRunBFS
            // 
            btnRunBFS.BackColor = SystemColors.ButtonFace;
            btnRunBFS.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnRunBFS.Location = new Point(60, 344);
            btnRunBFS.Name = "btnRunBFS";
            btnRunBFS.Size = new Size(88, 33);
            btnRunBFS.TabIndex = 4;
            btnRunBFS.Text = "BFS Başlat";
            btnRunBFS.UseVisualStyleBackColor = false;
            btnRunBFS.Click += btnRunBFS_Click;
            // 
            // btnRunDFS
            // 
            btnRunDFS.BackColor = SystemColors.ButtonFace;
            btnRunDFS.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btnRunDFS.Location = new Point(170, 344);
            btnRunDFS.Name = "btnRunDFS";
            btnRunDFS.Size = new Size(88, 33);
            btnRunDFS.TabIndex = 5;
            btnRunDFS.Text = "DFS Başlat";
            btnRunDFS.UseVisualStyleBackColor = false;
            btnRunDFS.Click += btnRunDFS_Click;
            // 
            // btn_Dijkstra
            // 
            btn_Dijkstra.BackColor = SystemColors.ButtonFace;
            btn_Dijkstra.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btn_Dijkstra.Location = new Point(277, 344);
            btn_Dijkstra.Name = "btn_Dijkstra";
            btn_Dijkstra.Size = new Size(88, 33);
            btn_Dijkstra.TabIndex = 6;
            btn_Dijkstra.Text = "En Kısa Yol";
            btn_Dijkstra.UseVisualStyleBackColor = false;
            btn_Dijkstra.Click += btn_Dijkstra_Click;
            // 
            // pnlMenu
            // 
            pnlMenu.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            pnlMenu.BackColor = Color.FromArgb(64, 64, 64);
            pnlMenu.Controls.Add(btn_ResetColors);
            pnlMenu.Controls.Add(btn_Coloring);
            pnlMenu.Controls.Add(btn_Components);
            pnlMenu.Controls.Add(btn_Centrality);
            pnlMenu.Controls.Add(btn_AStar);
            pnlMenu.Controls.Add(btn_Save);
            pnlMenu.Controls.Add(label_Duration);
            pnlMenu.Controls.Add(label_Stats);
            pnlMenu.Controls.Add(pnlGroup);
            pnlMenu.Controls.Add(labelGroup);
            pnlMenu.Controls.Add(btn_Dijkstra);
            pnlMenu.Controls.Add(btnRunDFS);
            pnlMenu.Controls.Add(btnRunBFS);
            pnlMenu.Controls.Add(btnLoadCSV);
            pnlMenu.Controls.Add(btnArrange);
            pnlMenu.Controls.Add(btnReset);
            pnlMenu.Location = new Point(444, 0);
            pnlMenu.Name = "pnlMenu";
            pnlMenu.Size = new Size(436, 588);
            pnlMenu.TabIndex = 11;
            // 
            // btn_Centrality
            // 
            btn_Centrality.BackColor = SystemColors.ButtonFace;
            btn_Centrality.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btn_Centrality.Location = new Point(170, 397);
            btn_Centrality.Name = "btn_Centrality";
            btn_Centrality.Size = new Size(88, 33);
            btn_Centrality.TabIndex = 17;
            btn_Centrality.Text = "En Popüler 5";
            btn_Centrality.UseVisualStyleBackColor = false;
            btn_Centrality.Click += btn_Centrality_Click;
            // 
            // btn_AStar
            // 
            btn_AStar.BackColor = SystemColors.ButtonFace;
            btn_AStar.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btn_AStar.Location = new Point(60, 397);
            btn_AStar.Name = "btn_AStar";
            btn_AStar.Size = new Size(88, 33);
            btn_AStar.TabIndex = 16;
            btn_AStar.Text = "A* Başlat";
            btn_AStar.UseVisualStyleBackColor = false;
            btn_AStar.Click += btn_AStar_Click;
            // 
            // btn_Save
            // 
            btn_Save.BackColor = SystemColors.ButtonFace;
            btn_Save.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btn_Save.Location = new Point(277, 397);
            btn_Save.Name = "btn_Save";
            btn_Save.Size = new Size(88, 33);
            btn_Save.TabIndex = 15;
            btn_Save.Text = "Dışa Aktar";
            btn_Save.UseVisualStyleBackColor = false;
            btn_Save.Click += btn_Save_Click;
            // 
            // label_Duration
            // 
            label_Duration.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label_Duration.AutoSize = true;
            label_Duration.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label_Duration.ForeColor = SystemColors.Control;
            label_Duration.Location = new Point(60, 541);
            label_Duration.Name = "label_Duration";
            label_Duration.Size = new Size(96, 25);
            label_Duration.TabIndex = 14;
            label_Duration.Text = "Süre: 0ms";
            // 
            // pnlGroup
            // 
            pnlGroup.Controls.Add(txt_Activity);
            pnlGroup.Controls.Add(txt_Interaction);
            pnlGroup.Controls.Add(txt_Name);
            pnlGroup.Controls.Add(txt_ID);
            pnlGroup.Controls.Add(label_activity);
            pnlGroup.Controls.Add(label_interaction);
            pnlGroup.Controls.Add(label_name);
            pnlGroup.Controls.Add(label_ID);
            pnlGroup.Controls.Add(btn_Update);
            pnlGroup.Controls.Add(btn_Delete);
            pnlGroup.Location = new Point(60, 78);
            pnlGroup.Name = "pnlGroup";
            pnlGroup.Size = new Size(305, 198);
            pnlGroup.TabIndex = 13;
            // 
            // txt_Activity
            // 
            txt_Activity.Location = new Point(110, 110);
            txt_Activity.Name = "txt_Activity";
            txt_Activity.Size = new Size(100, 23);
            txt_Activity.TabIndex = 18;
            // 
            // txt_Interaction
            // 
            txt_Interaction.Location = new Point(110, 80);
            txt_Interaction.Name = "txt_Interaction";
            txt_Interaction.Size = new Size(100, 23);
            txt_Interaction.TabIndex = 17;
            // 
            // txt_Name
            // 
            txt_Name.Location = new Point(110, 48);
            txt_Name.Name = "txt_Name";
            txt_Name.Size = new Size(100, 23);
            txt_Name.TabIndex = 16;
            // 
            // txt_ID
            // 
            txt_ID.Location = new Point(110, 17);
            txt_ID.Name = "txt_ID";
            txt_ID.Size = new Size(100, 23);
            txt_ID.TabIndex = 15;
            // 
            // label_activity
            // 
            label_activity.AutoSize = true;
            label_activity.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label_activity.ForeColor = SystemColors.Control;
            label_activity.Location = new Point(32, 110);
            label_activity.Name = "label_activity";
            label_activity.Size = new Size(65, 20);
            label_activity.TabIndex = 14;
            label_activity.Text = "Aktivite:";
            // 
            // label_interaction
            // 
            label_interaction.AutoSize = true;
            label_interaction.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label_interaction.ForeColor = SystemColors.Control;
            label_interaction.Location = new Point(32, 80);
            label_interaction.Name = "label_interaction";
            label_interaction.Size = new Size(73, 20);
            label_interaction.TabIndex = 13;
            label_interaction.Text = "Etkileşim:";
            // 
            // label_name
            // 
            label_name.AutoSize = true;
            label_name.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label_name.ForeColor = SystemColors.Control;
            label_name.Location = new Point(32, 48);
            label_name.Name = "label_name";
            label_name.Size = new Size(40, 20);
            label_name.TabIndex = 12;
            label_name.Text = "İsim:";
            // 
            // label_ID
            // 
            label_ID.AutoSize = true;
            label_ID.Font = new Font("Segoe UI Semibold", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label_ID.ForeColor = SystemColors.Control;
            label_ID.Location = new Point(32, 17);
            label_ID.Name = "label_ID";
            label_ID.Size = new Size(28, 20);
            label_ID.TabIndex = 11;
            label_ID.Text = "ID:";
            // 
            // btn_Update
            // 
            btn_Update.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            btn_Update.BackColor = Color.ForestGreen;
            btn_Update.FlatAppearance.BorderSize = 0;
            btn_Update.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btn_Update.ForeColor = SystemColors.Control;
            btn_Update.Location = new Point(0, 167);
            btn_Update.Name = "btn_Update";
            btn_Update.Size = new Size(85, 31);
            btn_Update.TabIndex = 10;
            btn_Update.Text = "Güncelle";
            btn_Update.UseVisualStyleBackColor = false;
            btn_Update.Click += btn_Update_Click;
            // 
            // btn_Delete
            // 
            btn_Delete.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            btn_Delete.BackColor = Color.Crimson;
            btn_Delete.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btn_Delete.ForeColor = SystemColors.Control;
            btn_Delete.Location = new Point(220, 167);
            btn_Delete.Name = "btn_Delete";
            btn_Delete.Size = new Size(85, 31);
            btn_Delete.TabIndex = 9;
            btn_Delete.Text = "Sil";
            btn_Delete.UseVisualStyleBackColor = false;
            btn_Delete.Click += btn_Delete_Click;
            // 
            // labelGroup
            // 
            labelGroup.AutoSize = true;
            labelGroup.Font = new Font("Segoe UI Semibold", 12.75F, FontStyle.Bold, GraphicsUnit.Point, 162);
            labelGroup.ForeColor = SystemColors.Control;
            labelGroup.Location = new Point(60, 21);
            labelGroup.Name = "labelGroup";
            labelGroup.Size = new Size(127, 23);
            labelGroup.TabIndex = 12;
            labelGroup.Text = "Düğüm Bilgileri";
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 162);
            label5.ForeColor = SystemColors.Control;
            label5.Location = new Point(156, 19);
            label5.Name = "label5";
            label5.Size = new Size(116, 25);
            label5.TabIndex = 12;
            label5.Text = "Çizim Paneli";
            // 
            // btn_Components
            // 
            btn_Components.BackColor = SystemColors.ButtonFace;
            btn_Components.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btn_Components.Location = new Point(60, 450);
            btn_Components.Name = "btn_Components";
            btn_Components.Size = new Size(88, 39);
            btn_Components.TabIndex = 18;
            btn_Components.Text = "Ayrık Topl. Bul";
            btn_Components.UseVisualStyleBackColor = false;
            btn_Components.Click += btn_Components_Click;
            // 
            // btn_Coloring
            // 
            btn_Coloring.BackColor = SystemColors.ButtonFace;
            btn_Coloring.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btn_Coloring.Location = new Point(170, 450);
            btn_Coloring.Name = "btn_Coloring";
            btn_Coloring.Size = new Size(88, 39);
            btn_Coloring.TabIndex = 19;
            btn_Coloring.Text = "Graf Renklendir";
            btn_Coloring.UseVisualStyleBackColor = false;
            btn_Coloring.Click += btn_Coloring_Click;
            // 
            // label_Stats
            // 
            label_Stats.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label_Stats.AutoSize = true;
            label_Stats.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 162);
            label_Stats.ForeColor = SystemColors.Control;
            label_Stats.Location = new Point(60, 510);
            label_Stats.Name = "label_Stats";
            label_Stats.Size = new Size(150, 19);
            label_Stats.TabIndex = 20;
            label_Stats.Text = "Düğüm: 0 | Kenar: 0";
            // 
            // btn_ResetColors
            // 
            btn_ResetColors.BackColor = SystemColors.ButtonFace;
            btn_ResetColors.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 162);
            btn_ResetColors.Location = new Point(277, 450);
            btn_ResetColors.Name = "btn_ResetColors";
            btn_ResetColors.Size = new Size(88, 39);
            btn_ResetColors.TabIndex = 21;
            btn_ResetColors.Text = "Renkleri Sıfırla";
            btn_ResetColors.UseVisualStyleBackColor = false;
            btn_ResetColors.Click += btn_ResetColors_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Black;
            ClientSize = new Size(880, 588);
            Controls.Add(label5);
            Controls.Add(pnlGraph);
            Controls.Add(pnlMenu);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load;
            pnlMenu.ResumeLayout(false);
            pnlMenu.PerformLayout();
            pnlGroup.ResumeLayout(false);
            pnlGroup.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel pnlGraph;
        private Button btnReset;
        private Button btnLoadCSV;
        private Button btnArrange;
        private Button btnRunBFS;
        private Button btnRunDFS;
        private Button btn_Dijkstra;
        private Panel pnlMenu;
        private Label label5;
        private Label labelGroup;
        private Panel pnlGroup;
        private Button btn_Update;
        private Button btn_Delete;
        private Label label_activity;
        private Label label_interaction;
        private Label label_name;
        private Label label_ID;
        private TextBox txt_Activity;
        private TextBox txt_Interaction;
        private TextBox txt_Name;
        private TextBox txt_ID;
        private Label label_Duration;
        private Button btn_Save;
        private Button btn_AStar;
        private Button btn_Centrality;
        private Button btn_Components;
        private Button btn_Coloring;
        private Label label_Stats;
        private Button btn_ResetColors;
    }
}
