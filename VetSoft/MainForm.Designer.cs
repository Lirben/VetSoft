namespace VetSoft
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.label1 = new System.Windows.Forms.Label();
            this.pnScrollRight = new System.Windows.Forms.Panel();
            this.lsConsole = new System.Windows.Forms.ListBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnMeasure = new System.Windows.Forms.Button();
            this.btnIdle = new System.Windows.Forms.Button();
            this.btnLoadFile = new System.Windows.Forms.Button();
            this.pbNoConnect = new System.Windows.Forms.PictureBox();
            this.chFR = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chFL = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.cbTopLeft = new System.Windows.Forms.CheckBox();
            this.cbTopRight = new System.Windows.Forms.CheckBox();
            this.cbRearLeft = new System.Windows.Forms.CheckBox();
            this.cbRearRight = new System.Windows.Forms.CheckBox();
            this.btnAnalyse = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.pnScrollLeft = new System.Windows.Forms.Panel();
            this.btnZoomIn = new System.Windows.Forms.Button();
            this.btnZoomOut = new System.Windows.Forms.Button();
            this.pnScrollRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbNoConnect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chFR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chFL)).BeginInit();
            this.pnScrollLeft.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 72F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(3, 325);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 146);
            this.label1.TabIndex = 1;
            this.label1.Text = ">";
            this.label1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pnScrollRight_MouseClick);
            // 
            // pnScrollRight
            // 
            this.pnScrollRight.Controls.Add(this.label1);
            this.pnScrollRight.Location = new System.Drawing.Point(1184, 3);
            this.pnScrollRight.Name = "pnScrollRight";
            this.pnScrollRight.Size = new System.Drawing.Size(124, 981);
            this.pnScrollRight.TabIndex = 3;
            this.pnScrollRight.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pnScrollRight_MouseClick);
            // 
            // lsConsole
            // 
            this.lsConsole.BackColor = System.Drawing.Color.Black;
            this.lsConsole.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lsConsole.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lsConsole.ForeColor = System.Drawing.Color.White;
            this.lsConsole.FormattingEnabled = true;
            this.lsConsole.ItemHeight = 24;
            this.lsConsole.Location = new System.Drawing.Point(1309, 0);
            this.lsConsole.Name = "lsConsole";
            this.lsConsole.Size = new System.Drawing.Size(619, 768);
            this.lsConsole.TabIndex = 4;
            this.lsConsole.SelectedValueChanged += new System.EventHandler(this.lsConsole_SelectedValueChanged);
            // 
            // btnConnect
            // 
            this.btnConnect.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnConnect.FlatAppearance.BorderSize = 5;
            this.btnConnect.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gainsboro;
            this.btnConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConnect.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConnect.ForeColor = System.Drawing.Color.White;
            this.btnConnect.Location = new System.Drawing.Point(1309, 783);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(279, 57);
            this.btnConnect.TabIndex = 6;
            this.btnConnect.Text = "Find hoofs";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnDisconnect.FlatAppearance.BorderSize = 5;
            this.btnDisconnect.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gainsboro;
            this.btnDisconnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDisconnect.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDisconnect.ForeColor = System.Drawing.Color.White;
            this.btnDisconnect.Location = new System.Drawing.Point(1309, 783);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new System.Drawing.Size(279, 57);
            this.btnDisconnect.TabIndex = 8;
            this.btnDisconnect.Text = "Disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Visible = false;
            this.btnDisconnect.Click += new System.EventHandler(this.btnDisconnect_Click);
            // 
            // btnMeasure
            // 
            this.btnMeasure.Enabled = false;
            this.btnMeasure.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnMeasure.FlatAppearance.BorderSize = 5;
            this.btnMeasure.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gainsboro;
            this.btnMeasure.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMeasure.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMeasure.ForeColor = System.Drawing.Color.White;
            this.btnMeasure.Location = new System.Drawing.Point(1309, 855);
            this.btnMeasure.Name = "btnMeasure";
            this.btnMeasure.Size = new System.Drawing.Size(279, 57);
            this.btnMeasure.TabIndex = 9;
            this.btnMeasure.Text = "Measure gait";
            this.btnMeasure.UseVisualStyleBackColor = true;
            this.btnMeasure.Click += new System.EventHandler(this.btnMeasure_Click);
            // 
            // btnIdle
            // 
            this.btnIdle.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnIdle.FlatAppearance.BorderSize = 5;
            this.btnIdle.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gainsboro;
            this.btnIdle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnIdle.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnIdle.ForeColor = System.Drawing.Color.White;
            this.btnIdle.Location = new System.Drawing.Point(1309, 855);
            this.btnIdle.Name = "btnIdle";
            this.btnIdle.Size = new System.Drawing.Size(279, 57);
            this.btnIdle.TabIndex = 10;
            this.btnIdle.Text = "Idle";
            this.btnIdle.UseVisualStyleBackColor = true;
            this.btnIdle.Visible = false;
            this.btnIdle.Click += new System.EventHandler(this.btnIdle_Click);
            // 
            // btnLoadFile
            // 
            this.btnLoadFile.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnLoadFile.FlatAppearance.BorderSize = 5;
            this.btnLoadFile.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gainsboro;
            this.btnLoadFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadFile.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoadFile.ForeColor = System.Drawing.Color.White;
            this.btnLoadFile.Location = new System.Drawing.Point(1618, 783);
            this.btnLoadFile.Name = "btnLoadFile";
            this.btnLoadFile.Size = new System.Drawing.Size(279, 57);
            this.btnLoadFile.TabIndex = 14;
            this.btnLoadFile.Text = "Load file";
            this.btnLoadFile.UseVisualStyleBackColor = true;
            this.btnLoadFile.Click += new System.EventHandler(this.btnLoadFile_Click);
            // 
            // pbNoConnect
            // 
            this.pbNoConnect.Image = global::VetSoft.Properties.Resources.warning;
            this.pbNoConnect.Location = new System.Drawing.Point(1260, 791);
            this.pbNoConnect.Name = "pbNoConnect";
            this.pbNoConnect.Size = new System.Drawing.Size(40, 40);
            this.pbNoConnect.TabIndex = 16;
            this.pbNoConnect.TabStop = false;
            this.pbNoConnect.Visible = false;
            // 
            // chFR
            // 
            this.chFR.BackColor = System.Drawing.Color.SteelBlue;
            chartArea1.BackColor = System.Drawing.Color.SteelBlue;
            chartArea1.Name = "ChartArea1";
            this.chFR.ChartAreas.Add(chartArea1);
            legend1.BackColor = System.Drawing.Color.SteelBlue;
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            legend1.Name = "Legend1";
            this.chFR.Legends.Add(legend1);
            this.chFR.Location = new System.Drawing.Point(121, 106);
            this.chFR.Name = "chFR";
            this.chFR.Size = new System.Drawing.Size(1086, 229);
            this.chFR.TabIndex = 18;
            this.chFR.Text = "chart3";
            // 
            // chFL
            // 
            this.chFL.BackColor = System.Drawing.Color.SteelBlue;
            chartArea2.BackColor = System.Drawing.Color.SteelBlue;
            chartArea2.Name = "ChartArea1";
            this.chFL.ChartAreas.Add(chartArea2);
            legend2.BackColor = System.Drawing.Color.SteelBlue;
            legend2.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            legend2.Name = "Legend1";
            this.chFL.Legends.Add(legend2);
            this.chFL.Location = new System.Drawing.Point(121, 490);
            this.chFL.Name = "chFL";
            this.chFL.Size = new System.Drawing.Size(1086, 229);
            this.chFL.TabIndex = 19;
            this.chFL.Text = "chart4";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Calibri", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(161, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(134, 46);
            this.label6.TabIndex = 23;
            this.label6.Text = "Front R";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Calibri", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(132, 428);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(129, 46);
            this.label4.TabIndex = 24;
            this.label4.Text = "Front L";
            // 
            // btnSave
            // 
            this.btnSave.Enabled = false;
            this.btnSave.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnSave.FlatAppearance.BorderSize = 5;
            this.btnSave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gainsboro;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(1309, 927);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(279, 57);
            this.btnSave.TabIndex = 25;
            this.btnSave.Text = "Save to file";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // cbTopLeft
            // 
            this.cbTopLeft.AutoSize = true;
            this.cbTopLeft.Checked = true;
            this.cbTopLeft.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbTopLeft.Font = new System.Drawing.Font("Calibri", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbTopLeft.ForeColor = System.Drawing.Color.White;
            this.cbTopLeft.Location = new System.Drawing.Point(140, 747);
            this.cbTopLeft.Name = "cbTopLeft";
            this.cbTopLeft.Size = new System.Drawing.Size(109, 33);
            this.cbTopLeft.TabIndex = 29;
            this.cbTopLeft.Text = "Top left";
            this.cbTopLeft.UseVisualStyleBackColor = true;
            this.cbTopLeft.CheckedChanged += new System.EventHandler(this.cbTopLeft_CheckedChanged);
            // 
            // cbTopRight
            // 
            this.cbTopRight.AutoSize = true;
            this.cbTopRight.Checked = true;
            this.cbTopRight.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbTopRight.Font = new System.Drawing.Font("Calibri", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbTopRight.ForeColor = System.Drawing.Color.White;
            this.cbTopRight.Location = new System.Drawing.Point(264, 747);
            this.cbTopRight.Name = "cbTopRight";
            this.cbTopRight.Size = new System.Drawing.Size(122, 33);
            this.cbTopRight.TabIndex = 30;
            this.cbTopRight.Text = "Top right";
            this.cbTopRight.UseVisualStyleBackColor = true;
            this.cbTopRight.CheckedChanged += new System.EventHandler(this.cbTopRight_CheckedChanged);
            // 
            // cbRearLeft
            // 
            this.cbRearLeft.AutoSize = true;
            this.cbRearLeft.Checked = true;
            this.cbRearLeft.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbRearLeft.Font = new System.Drawing.Font("Calibri", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbRearLeft.ForeColor = System.Drawing.Color.White;
            this.cbRearLeft.Location = new System.Drawing.Point(140, 791);
            this.cbRearLeft.Name = "cbRearLeft";
            this.cbRearLeft.Size = new System.Drawing.Size(118, 33);
            this.cbRearLeft.TabIndex = 31;
            this.cbRearLeft.Text = "Rear left";
            this.cbRearLeft.UseVisualStyleBackColor = true;
            this.cbRearLeft.CheckedChanged += new System.EventHandler(this.cbRearLeft_CheckedChanged);
            // 
            // cbRearRight
            // 
            this.cbRearRight.AutoSize = true;
            this.cbRearRight.Checked = true;
            this.cbRearRight.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbRearRight.Font = new System.Drawing.Font("Calibri", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbRearRight.ForeColor = System.Drawing.Color.White;
            this.cbRearRight.Location = new System.Drawing.Point(264, 791);
            this.cbRearRight.Name = "cbRearRight";
            this.cbRearRight.Size = new System.Drawing.Size(131, 33);
            this.cbRearRight.TabIndex = 32;
            this.cbRearRight.Text = "Rear right";
            this.cbRearRight.UseVisualStyleBackColor = true;
            this.cbRearRight.CheckedChanged += new System.EventHandler(this.cbRearRight_CheckedChanged);
            // 
            // btnAnalyse
            // 
            this.btnAnalyse.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnAnalyse.FlatAppearance.BorderSize = 5;
            this.btnAnalyse.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gainsboro;
            this.btnAnalyse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAnalyse.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAnalyse.ForeColor = System.Drawing.Color.White;
            this.btnAnalyse.Location = new System.Drawing.Point(1618, 855);
            this.btnAnalyse.Name = "btnAnalyse";
            this.btnAnalyse.Size = new System.Drawing.Size(279, 129);
            this.btnAnalyse.TabIndex = 33;
            this.btnAnalyse.Text = "Analyse";
            this.btnAnalyse.UseVisualStyleBackColor = true;
            this.btnAnalyse.Click += new System.EventHandler(this.btnAnalyse_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 72F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(-1, 324);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 146);
            this.label2.TabIndex = 1;
            this.label2.Text = "<";
            this.label2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pnScrollLeft_MouseClick);
            // 
            // pnScrollLeft
            // 
            this.pnScrollLeft.Controls.Add(this.label2);
            this.pnScrollLeft.Location = new System.Drawing.Point(2, 4);
            this.pnScrollLeft.Name = "pnScrollLeft";
            this.pnScrollLeft.Size = new System.Drawing.Size(124, 980);
            this.pnScrollLeft.TabIndex = 7;
            this.pnScrollLeft.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pnScrollLeft_MouseClick);
            // 
            // btnZoomIn
            // 
            this.btnZoomIn.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnZoomIn.FlatAppearance.BorderSize = 5;
            this.btnZoomIn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gainsboro;
            this.btnZoomIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnZoomIn.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnZoomIn.ForeColor = System.Drawing.Color.White;
            this.btnZoomIn.Location = new System.Drawing.Point(560, 735);
            this.btnZoomIn.Name = "btnZoomIn";
            this.btnZoomIn.Size = new System.Drawing.Size(67, 57);
            this.btnZoomIn.TabIndex = 34;
            this.btnZoomIn.Text = "+";
            this.btnZoomIn.UseVisualStyleBackColor = true;
            this.btnZoomIn.Click += new System.EventHandler(this.btnZoomIn_Click);
            // 
            // btnZoomOut
            // 
            this.btnZoomOut.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnZoomOut.FlatAppearance.BorderSize = 5;
            this.btnZoomOut.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gainsboro;
            this.btnZoomOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnZoomOut.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnZoomOut.ForeColor = System.Drawing.Color.White;
            this.btnZoomOut.Location = new System.Drawing.Point(633, 735);
            this.btnZoomOut.Name = "btnZoomOut";
            this.btnZoomOut.Size = new System.Drawing.Size(67, 57);
            this.btnZoomOut.TabIndex = 35;
            this.btnZoomOut.Text = "-";
            this.btnZoomOut.UseVisualStyleBackColor = true;
            this.btnZoomOut.Click += new System.EventHandler(this.btnZoomOut_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SteelBlue;
            this.ClientSize = new System.Drawing.Size(1924, 987);
            this.Controls.Add(this.btnZoomOut);
            this.Controls.Add(this.btnZoomIn);
            this.Controls.Add(this.btnAnalyse);
            this.Controls.Add(this.cbRearRight);
            this.Controls.Add(this.cbRearLeft);
            this.Controls.Add(this.cbTopRight);
            this.Controls.Add(this.cbTopLeft);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.chFL);
            this.Controls.Add(this.chFR);
            this.Controls.Add(this.pbNoConnect);
            this.Controls.Add(this.btnLoadFile);
            this.Controls.Add(this.btnIdle);
            this.Controls.Add(this.btnMeasure);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.pnScrollLeft);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.lsConsole);
            this.Controls.Add(this.pnScrollRight);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "VetSoft";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.pnScrollRight.ResumeLayout(false);
            this.pnScrollRight.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbNoConnect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chFR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chFL)).EndInit();
            this.pnScrollLeft.ResumeLayout(false);
            this.pnScrollLeft.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnScrollRight;
        private System.Windows.Forms.ListBox lsConsole;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnMeasure;
        private System.Windows.Forms.Button btnIdle;
        private System.Windows.Forms.Button btnLoadFile;
        private System.Windows.Forms.PictureBox pbNoConnect;
        private System.Windows.Forms.DataVisualization.Charting.Chart chFR;
        private System.Windows.Forms.DataVisualization.Charting.Chart chFL;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.CheckBox cbTopLeft;
        private System.Windows.Forms.CheckBox cbTopRight;
        private System.Windows.Forms.CheckBox cbRearLeft;
        private System.Windows.Forms.CheckBox cbRearRight;
        private System.Windows.Forms.Button btnAnalyse;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel pnScrollLeft;
        private System.Windows.Forms.Button btnZoomIn;
        private System.Windows.Forms.Button btnZoomOut;
    }
}

