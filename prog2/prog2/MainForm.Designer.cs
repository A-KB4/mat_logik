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
            // btnDirected
            // 
            this.btnDirected.Location = new System.Drawing.Point(12, 480);
            this.btnDirected.Name = "btnDirected";
            this.btnDirected.Size = new System.Drawing.Size(170, 30);
            this.btnDirected.TabIndex = 1;
            this.btnDirected.Text = "Створити орієнтований граф";
            this.btnDirected.UseVisualStyleBackColor = true;
            this.btnDirected.Click += new System.EventHandler(this.BtnDirected_Click);
            // 
            // btnUndirected
            // 
            this.btnUndirected.Location = new System.Drawing.Point(200, 480);
            this.btnUndirected.Name = "btnUndirected";
            this.btnUndirected.Size = new System.Drawing.Size(180, 30);
            this.btnUndirected.TabIndex = 2;
            this.btnUndirected.Text = "Створити неорієнтований граф";
            this.btnUndirected.UseVisualStyleBackColor = true;
            this.btnUndirected.Click += new System.EventHandler(this.BtnUndirected_Click);
            // 
            // btnDFS
            // 
            this.btnDFS.Location = new System.Drawing.Point(400, 480);
            this.btnDFS.Name = "btnDFS";
            this.btnDFS.Size = new System.Drawing.Size(120, 30);
            this.btnDFS.TabIndex = 3;
            this.btnDFS.Text = "Обхід у глибину (DFS)";
            this.btnDFS.UseVisualStyleBackColor = true;
            this.btnDFS.Click += new System.EventHandler(this.BtnDFS_Click);
            // 
            // btnBFS
            // 
            this.btnBFS.Location = new System.Drawing.Point(540, 480);
            this.btnBFS.Name = "btnBFS";
            this.btnBFS.Size = new System.Drawing.Size(120, 30);
            this.btnBFS.TabIndex = 4;
            this.btnBFS.Text = "Обхід у ширину (BFS)";
            this.btnBFS.UseVisualStyleBackColor = true;
            this.btnBFS.Click += new System.EventHandler(this.BtnBFS_Click);
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(784, 521);
            this.Controls.Add(this.btnBFS);
            this.Controls.Add(this.btnDFS);
            this.Controls.Add(this.btnUndirected);
            this.Controls.Add(this.btnDirected);
            this.Controls.Add(this.panelCanvas);
            this.Name = "MainForm";
            this.Text = "Обхід графів";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }
    }
}
