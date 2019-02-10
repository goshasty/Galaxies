namespace Galaxies
{
    partial class Form1
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button_openfile = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.button_visualisation = new System.Windows.Forms.Button();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.vScrollBar1 = new System.Windows.Forms.VScrollBar();
            this.hScrollBar1 = new System.Windows.Forms.HScrollBar();
            this.button_return_to_file = new System.Windows.Forms.Button();
            this.button_next_algorithm = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.JoinToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.NextAlgorithmToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ReturnToFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MinCut = new System.Windows.Forms.Label();
            this.MaxCut = new System.Windows.Forms.Label();
            this.MinText = new System.Windows.Forms.TextBox();
            this.MaxText = new System.Windows.Forms.TextBox();
            this.Cut = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.SystemColors.ControlText;
            this.panel1.Controls.Add(this.button_openfile);
            this.panel1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.panel1.Location = new System.Drawing.Point(350, 76);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(148, 327);
            this.panel1.TabIndex = 0;
            // 
            // button_openfile
            // 
            this.button_openfile.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.button_openfile.BackColor = System.Drawing.SystemColors.ControlText;
            this.button_openfile.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_openfile.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.button_openfile.Location = new System.Drawing.Point(-67, 245);
            this.button_openfile.Name = "button_openfile";
            this.button_openfile.Size = new System.Drawing.Size(200, 100);
            this.button_openfile.TabIndex = 1;
            this.button_openfile.Text = "Открыть файл";
            this.button_openfile.UseVisualStyleBackColor = false;
            this.button_openfile.Click += new System.EventHandler(this.button_openfile_Click);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.SystemColors.ControlText;
            this.panel2.Controls.Add(this.button_visualisation);
            this.panel2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.panel2.Location = new System.Drawing.Point(12, 47);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(293, 398);
            this.panel2.TabIndex = 3;
            // 
            // button_visualisation
            // 
            this.button_visualisation.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.button_visualisation.BackColor = System.Drawing.SystemColors.ControlText;
            this.button_visualisation.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_visualisation.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.button_visualisation.Location = new System.Drawing.Point(10, 239);
            this.button_visualisation.Name = "button_visualisation";
            this.button_visualisation.Size = new System.Drawing.Size(200, 100);
            this.button_visualisation.TabIndex = 1;
            this.button_visualisation.Text = "Запустить";
            this.button_visualisation.UseVisualStyleBackColor = false;
            this.button_visualisation.UseWaitCursor = true;
            this.button_visualisation.Click += new System.EventHandler(this.button_visualisation_Click);
            // 
            // trackBar1
            // 
            this.trackBar1.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.trackBar1.BackColor = System.Drawing.SystemColors.ControlText;
            this.trackBar1.Location = new System.Drawing.Point(142, 497);
            this.trackBar1.Maximum = 100;
            this.trackBar1.Minimum = 1;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(365, 45);
            this.trackBar1.TabIndex = 7;
            this.trackBar1.Value = 1;
            this.trackBar1.ValueChanged += new System.EventHandler(this.trackBar1_ValueChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ControlText;
            this.pictureBox1.Enabled = false;
            this.pictureBox1.Location = new System.Drawing.Point(537, 47);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(172, 155);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox1_Paint);
            // 
            // vScrollBar1
            // 
            this.vScrollBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.vScrollBar1.Location = new System.Drawing.Point(725, 1);
            this.vScrollBar1.Maximum = 1000;
            this.vScrollBar1.Name = "vScrollBar1";
            this.vScrollBar1.Size = new System.Drawing.Size(17, 557);
            this.vScrollBar1.TabIndex = 3;
            this.vScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBar1_Scroll);
            // 
            // hScrollBar1
            // 
            this.hScrollBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.hScrollBar1.Location = new System.Drawing.Point(1, 541);
            this.hScrollBar1.Maximum = 1000;
            this.hScrollBar1.Name = "hScrollBar1";
            this.hScrollBar1.Size = new System.Drawing.Size(724, 17);
            this.hScrollBar1.TabIndex = 4;
            this.hScrollBar1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hScrollBar1_Scroll);
            // 
            // button_return_to_file
            // 
            this.button_return_to_file.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button_return_to_file.BackColor = System.Drawing.SystemColors.ControlText;
            this.button_return_to_file.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_return_to_file.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.button_return_to_file.Location = new System.Drawing.Point(544, 493);
            this.button_return_to_file.Name = "button_return_to_file";
            this.button_return_to_file.Size = new System.Drawing.Size(175, 49);
            this.button_return_to_file.TabIndex = 6;
            this.button_return_to_file.Text = "Вернуться к выбору файла";
            this.button_return_to_file.UseVisualStyleBackColor = false;
            this.button_return_to_file.Click += new System.EventHandler(this.button_return_to_file_Click);
            // 
            // button_next_algorithm
            // 
            this.button_next_algorithm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button_next_algorithm.BackColor = System.Drawing.SystemColors.ControlText;
            this.button_next_algorithm.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button_next_algorithm.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.button_next_algorithm.Location = new System.Drawing.Point(1, 493);
            this.button_next_algorithm.Name = "button_next_algorithm";
            this.button_next_algorithm.Size = new System.Drawing.Size(116, 49);
            this.button_next_algorithm.TabIndex = 6;
            this.button_next_algorithm.Text = "Следующий алгоритм";
            this.button_next_algorithm.UseVisualStyleBackColor = false;
            this.button_next_algorithm.Click += new System.EventHandler(this.button_next_algorithm_Click);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.ControlText;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label1.Location = new System.Drawing.Point(541, 237);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "label1";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.ControlText;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label2.Location = new System.Drawing.Point(615, 213);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "label2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.ControlText;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.label3.Location = new System.Drawing.Point(627, 239);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 15);
            this.label3.TabIndex = 8;
            this.label3.Text = "label3";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ControlText;
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.JoinToolStripMenuItem,
            this.NextAlgorithmToolStripMenuItem,
            this.ReturnToFileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(389, 24);
            this.menuStrip1.TabIndex = 9;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // JoinToolStripMenuItem
            // 
            this.JoinToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.JoinToolStripMenuItem.Name = "JoinToolStripMenuItem";
            this.JoinToolStripMenuItem.Size = new System.Drawing.Size(106, 20);
            this.JoinToolStripMenuItem.Text = "Собрать файлы";
            this.JoinToolStripMenuItem.Click += new System.EventHandler(this.JoinToolStripMenuItem_Click);
            // 
            // NextAlgorithmToolStripMenuItem
            // 
            this.NextAlgorithmToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.NextAlgorithmToolStripMenuItem.Name = "NextAlgorithmToolStripMenuItem";
            this.NextAlgorithmToolStripMenuItem.Size = new System.Drawing.Size(143, 20);
            this.NextAlgorithmToolStripMenuItem.Text = "Следующий алгоритм";
            this.NextAlgorithmToolStripMenuItem.Click += new System.EventHandler(this.NextAlgorithmToolStripMenuItem_Click);
            // 
            // ReturnToFileToolStripMenuItem
            // 
            this.ReturnToFileToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.ReturnToFileToolStripMenuItem.Name = "ReturnToFileToolStripMenuItem";
            this.ReturnToFileToolStripMenuItem.Size = new System.Drawing.Size(167, 20);
            this.ReturnToFileToolStripMenuItem.Text = "Вернуться к выбору файла";
            this.ReturnToFileToolStripMenuItem.Click += new System.EventHandler(this.ReturnToFileToolStripMenuItem_Click);
            // 
            // MinCut
            // 
            this.MinCut.AutoSize = true;
            this.MinCut.BackColor = System.Drawing.SystemColors.ControlText;
            this.MinCut.Enabled = false;
            this.MinCut.Location = new System.Drawing.Point(577, 333);
            this.MinCut.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.MinCut.Name = "MinCut";
            this.MinCut.Size = new System.Drawing.Size(24, 13);
            this.MinCut.TabIndex = 10;
            this.MinCut.Text = "Min";
            this.MinCut.Visible = false;
            this.MinCut.Click += new System.EventHandler(this.MinCut_Click);
            // 
            // MaxCut
            // 
            this.MaxCut.AutoSize = true;
            this.MaxCut.Enabled = false;
            this.MaxCut.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.MaxCut.Location = new System.Drawing.Point(604, 333);
            this.MaxCut.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.MaxCut.Name = "MaxCut";
            this.MaxCut.Size = new System.Drawing.Size(27, 13);
            this.MaxCut.TabIndex = 11;
            this.MaxCut.Text = "Max";
            this.MaxCut.Visible = false;
            // 
            // MinText
            // 
            this.MinText.BackColor = System.Drawing.SystemColors.Window;
            this.MinText.Enabled = false;
            this.MinText.Location = new System.Drawing.Point(579, 353);
            this.MinText.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MinText.Name = "MinText";
            this.MinText.Size = new System.Drawing.Size(76, 20);
            this.MinText.TabIndex = 12;
            this.MinText.Visible = false;
            this.MinText.TextChanged += new System.EventHandler(this.MinText_TextChanged);
            // 
            // MaxText
            // 
            this.MaxText.Enabled = false;
            this.MaxText.Location = new System.Drawing.Point(579, 385);
            this.MaxText.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaxText.Name = "MaxText";
            this.MaxText.Size = new System.Drawing.Size(76, 20);
            this.MaxText.TabIndex = 13;
            this.MaxText.Visible = false;
            this.MaxText.TextChanged += new System.EventHandler(this.MaxText_TextChanged);
            // 
            // Cut
            // 
            this.Cut.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Cut.Enabled = false;
            this.Cut.Location = new System.Drawing.Point(617, 422);
            this.Cut.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Cut.Name = "Cut";
            this.Cut.Size = new System.Drawing.Size(75, 32);
            this.Cut.TabIndex = 14;
            this.Cut.Text = "Обрезать";
            this.Cut.UseVisualStyleBackColor = false;
            this.Cut.Visible = false;
            this.Cut.Click += new System.EventHandler(this.Cut_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlText;
            this.ClientSize = new System.Drawing.Size(742, 557);
            this.Controls.Add(this.Cut);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.MaxText);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.MinText);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.MaxCut);
            this.Controls.Add(this.MinCut);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.vScrollBar1);
            this.Controls.Add(this.button_next_algorithm);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.button_return_to_file);
            this.Controls.Add(this.hScrollBar1);
            this.Controls.Add(this.menuStrip1);
            this.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "Galaxies";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.SizeChanged += new System.EventHandler(this.Form1_SizeChanged);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button_openfile;
        private System.Windows.Forms.Button button_visualisation;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.VScrollBar vScrollBar1;
        private System.Windows.Forms.HScrollBar hScrollBar1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button_return_to_file;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Button button_next_algorithm;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem JoinToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem NextAlgorithmToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ReturnToFileToolStripMenuItem;
        private System.Windows.Forms.Button Cut;
        private System.Windows.Forms.TextBox MaxText;
        private System.Windows.Forms.TextBox MinText;
        private System.Windows.Forms.Label MinCut;
        private System.Windows.Forms.Label MaxCut;
    }
}

