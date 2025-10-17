namespace prog2
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelCanvas;
        private System.Windows.Forms.Button btnDirected;
        private System.Windows.Forms.Button btnUndirected;
        private System.Windows.Forms.Button btnDFS;
        private System.Windows.Forms.Button btnBFS;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Label lblLog;
        // NEW:
        private System.Windows.Forms.Button btnDijkstra;
        private System.Windows.Forms.Button btnFloyd;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelCanvas = new System.Windows.Forms.Panel();
            this.btnDirected = new System.Windows.Forms.Button();
            this.btnUndirected = new System.Windows.Forms.Button();
            this.btnDFS = new System.Windows.Forms.Button();
            this.btnBFS = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.lblLog = new System.Windows.Forms.Label();
            this.btnDijkstra = new System.Windows.Forms.Button();
            this.btnFloyd = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // panelCanvas
            // 
            this.panelCanvas.BackColor = System.Drawing.Color.White;
            this.panelCanvas.Location = new System.Drawing.Point(12, 12);
            this.panelCanvas.Name = "panelCanvas";
            this.panelCanvas.Size = new System.Drawing.Size(760, 450);
            this.panelCanvas.TabIndex = 0;
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(790, 35);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(300, 427);
            this.txtLog.TabIndex = 5;
            // 
            // lblLog
            // 
            this.lblLog.AutoSize = true;
            this.lblLog.Location = new System.Drawing.Point(787, 12);
            this.lblLog.Name = "lblLog";
            this.lblLog.Size = new System.Drawing.Size(77, 13);
            this.lblLog.TabIndex = 6;
            this.lblLog.Text = "Лог / Результат:";
            // 
            // btnDirected
            // 
            this.btnDirected.Location = new System.Drawing.Point(12, 480);
            this.btnDirected.Name = "btnDirected";
            this.btnDirected.Size = new System.Drawing.Size(170, 30);
            this.btnDirected.TabIndex = 1;
            this.btnDirected.Text = "Створити орієнтований";
            this.btnDirected.UseVisualStyleBackColor = true;
            this.btnDirected.Click += new System.EventHandler(this.BtnDirected_Click);
            // 
            // btnUndirected
            // 
            this.btnUndirected.Location = new System.Drawing.Point(188, 480);
            this.btnUndirected.Name = "btnUndirected";
            this.btnUndirected.Size = new System.Drawing.Size(180, 30);
            this.btnUndirected.TabIndex = 2;
            this.btnUndirected.Text = "Створити неорієнтований";
            this.btnUndirected.UseVisualStyleBackColor = true;
            this.btnUndirected.Click += new System.EventHandler(this.BtnUndirected_Click);
            // 
            // btnDFS
            // 
            this.btnDFS.Location = new System.Drawing.Point(374, 480);
            this.btnDFS.Name = "btnDFS";
            this.btnDFS.Size = new System.Drawing.Size(110, 30);
            this.btnDFS.TabIndex = 3;
            this.btnDFS.Text = "DFS";
            this.btnDFS.UseVisualStyleBackColor = true;
            this.btnDFS.Click += new System.EventHandler(this.BtnDFS_Click);
            // 
            // btnBFS
            // 
            this.btnBFS.Location = new System.Drawing.Point(490, 480);
            this.btnBFS.Name = "btnBFS";
            this.btnBFS.Size = new System.Drawing.Size(110, 30);
            this.btnBFS.TabIndex = 4;
            this.btnBFS.Text = "BFS";
            this.btnBFS.UseVisualStyleBackColor = true;
            this.btnBFS.Click += new System.EventHandler(this.BtnBFS_Click);
            // 
            // btnDijkstra
            // 
            this.btnDijkstra.Location = new System.Drawing.Point(606, 480);
            this.btnDijkstra.Name = "btnDijkstra";
            this.btnDijkstra.Size = new System.Drawing.Size(120, 30);
            this.btnDijkstra.TabIndex = 7;
            this.btnDijkstra.Text = "Dijkstra (from a)";
            this.btnDijkstra.UseVisualStyleBackColor = true;
            this.btnDijkstra.Click += new System.EventHandler(this.BtnDijkstra_Click);
            // 
            // btnFloyd
            // 
            this.btnFloyd.Location = new System.Drawing.Point(732, 480);
            this.btnFloyd.Name = "btnFloyd";
            this.btnFloyd.Size = new System.Drawing.Size(120, 30);
            this.btnFloyd.TabIndex = 8;
            this.btnFloyd.Text = "Floyd–Warshall";
            this.btnFloyd.UseVisualStyleBackColor = true;
            this.btnFloyd.Click += new System.EventHandler(this.BtnFloyd_Click);
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(1105, 521);
            this.Controls.Add(this.btnFloyd);
            this.Controls.Add(this.btnDijkstra);
            this.Controls.Add(this.lblLog);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.btnBFS);
            this.Controls.Add(this.btnDFS);
            this.Controls.Add(this.btnUndirected);
            this.Controls.Add(this.btnDirected);
            this.Controls.Add(this.panelCanvas);
            this.Name = "MainForm";
            this.Text = "Обхід графів + Shortest Paths";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
