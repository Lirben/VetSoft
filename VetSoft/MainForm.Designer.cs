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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea3 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend3 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.label1 = new System.Windows.Forms.Label();
            this.pnCloseConsole = new System.Windows.Forms.Panel();
            this.lsConsole = new System.Windows.Forms.ListBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.pnOpenConsole = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.btnDisconnect = new System.Windows.Forms.Button();
            this.btnMeasure = new System.Windows.Forms.Button();
            this.btnIdle = new System.Windows.Forms.Button();
            this.chSteps = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.btnLoadFile = new System.Windows.Forms.Button();
            this.pbNoConnect = new System.Windows.Forms.PictureBox();
            this.chFR = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chFL = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label3 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtScale = new System.Windows.Forms.TextBox();
            this.lblScrollRight = new System.Windows.Forms.Label();
            this.lblScrollLeft = new System.Windows.Forms.Label();
            this.pnCloseConsole.SuspendLayout();
            this.pnOpenConsole.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chSteps)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbNoConnect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chFR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chFL)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 72F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(2, 325);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(122, 146);
            this.label1.TabIndex = 1;
            this.label1.Text = ">";
            this.label1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pnCloseConsole_MouseClick);
            // 
            // pnCloseConsole
            // 
            this.pnCloseConsole.Controls.Add(this.label1);
            this.pnCloseConsole.Location = new System.Drawing.Point(1184, 3);
            this.pnCloseConsole.Name = "pnCloseConsole";
            this.pnCloseConsole.Size = new System.Drawing.Size(124, 813);
            this.pnCloseConsole.TabIndex = 3;
            this.pnCloseConsole.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pnCloseConsole_MouseClick);
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
            this.lsConsole.Size = new System.Drawing.Size(619, 816);
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
            this.btnConnect.Location = new System.Drawing.Point(1309, 831);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(279, 57);
            this.btnConnect.TabIndex = 6;
            this.btnConnect.Text = "Find hoofs";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // pnOpenConsole
            // 
            this.pnOpenConsole.Controls.Add(this.label2);
            this.pnOpenConsole.Location = new System.Drawing.Point(1804, 0);
            this.pnOpenConsole.Name = "pnOpenConsole";
            this.pnOpenConsole.Size = new System.Drawing.Size(124, 816);
            this.pnOpenConsole.TabIndex = 7;
            this.pnOpenConsole.Visible = false;
            this.pnOpenConsole.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pnOpenConsole_MouseClick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 72F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(2, 320);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 146);
            this.label2.TabIndex = 1;
            this.label2.Text = "<";
            this.label2.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pnOpenConsole_MouseClick);
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnDisconnect.FlatAppearance.BorderSize = 5;
            this.btnDisconnect.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gainsboro;
            this.btnDisconnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDisconnect.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDisconnect.ForeColor = System.Drawing.Color.White;
            this.btnDisconnect.Location = new System.Drawing.Point(1309, 831);
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
            this.btnMeasure.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnMeasure.FlatAppearance.BorderSize = 5;
            this.btnMeasure.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gainsboro;
            this.btnMeasure.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMeasure.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMeasure.ForeColor = System.Drawing.Color.White;
            this.btnMeasure.Location = new System.Drawing.Point(1609, 831);
            this.btnMeasure.Name = "btnMeasure";
            this.btnMeasure.Size = new System.Drawing.Size(279, 57);
            this.btnMeasure.TabIndex = 9;
            this.btnMeasure.Text = "Measure gait";
            this.btnMeasure.UseVisualStyleBackColor = true;
            this.btnMeasure.Visible = false;
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
            this.btnIdle.Location = new System.Drawing.Point(1609, 831);
            this.btnIdle.Name = "btnIdle";
            this.btnIdle.Size = new System.Drawing.Size(279, 57);
            this.btnIdle.TabIndex = 10;
            this.btnIdle.Text = "Idle";
            this.btnIdle.UseVisualStyleBackColor = true;
            this.btnIdle.Visible = false;
            this.btnIdle.Click += new System.EventHandler(this.btnIdle_Click);
            // 
            // chSteps
            // 
            this.chSteps.BackColor = System.Drawing.Color.SteelBlue;
            chartArea1.BackColor = System.Drawing.Color.SteelBlue;
            chartArea1.Name = "ChartArea1";
            this.chSteps.ChartAreas.Add(chartArea1);
            legend1.BackColor = System.Drawing.Color.SteelBlue;
            legend1.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            legend1.Name = "Legend1";
            this.chSteps.Legends.Add(legend1);
            this.chSteps.Location = new System.Drawing.Point(92, 1);
            this.chSteps.Name = "chSteps";
            this.chSteps.Size = new System.Drawing.Size(1086, 229);
            this.chSteps.TabIndex = 11;
            this.chSteps.Text = "Front Left";
            // 
            // btnLoadFile
            // 
            this.btnLoadFile.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnLoadFile.FlatAppearance.BorderSize = 5;
            this.btnLoadFile.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Gainsboro;
            this.btnLoadFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadFile.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLoadFile.ForeColor = System.Drawing.Color.White;
            this.btnLoadFile.Location = new System.Drawing.Point(1309, 900);
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
            this.pbNoConnect.Location = new System.Drawing.Point(1260, 839);
            this.pbNoConnect.Name = "pbNoConnect";
            this.pbNoConnect.Size = new System.Drawing.Size(40, 40);
            this.pbNoConnect.TabIndex = 16;
            this.pbNoConnect.TabStop = false;
            this.pbNoConnect.Visible = false;
            // 
            // chFR
            // 
            this.chFR.BackColor = System.Drawing.Color.SteelBlue;
            chartArea2.BackColor = System.Drawing.Color.SteelBlue;
            chartArea2.Name = "ChartArea1";
            this.chFR.ChartAreas.Add(chartArea2);
            legend2.BackColor = System.Drawing.Color.SteelBlue;
            legend2.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            legend2.Name = "Legend1";
            this.chFR.Legends.Add(legend2);
            this.chFR.Location = new System.Drawing.Point(92, 370);
            this.chFR.Name = "chFR";
            this.chFR.Size = new System.Drawing.Size(1086, 229);
            this.chFR.TabIndex = 18;
            this.chFR.Text = "chart3";
            // 
            // chFL
            // 
            this.chFL.BackColor = System.Drawing.Color.SteelBlue;
            chartArea3.BackColor = System.Drawing.Color.SteelBlue;
            chartArea3.Name = "ChartArea1";
            this.chFL.ChartAreas.Add(chartArea3);
            legend3.BackColor = System.Drawing.Color.SteelBlue;
            legend3.Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Bottom;
            legend3.Name = "Legend1";
            this.chFL.Legends.Add(legend3);
            this.chFL.Location = new System.Drawing.Point(92, 654);
            this.chFL.Name = "chFL";
            this.chFL.Size = new System.Drawing.Size(1086, 229);
            this.chFL.TabIndex = 19;
            this.chFL.Text = "chart4";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(1, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 46);
            this.label3.TabIndex = 20;
            this.label3.Text = "Steps";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Calibri", 22.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(1, 434);
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
            this.label4.Location = new System.Drawing.Point(1, 726);
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
            this.btnSave.Location = new System.Drawing.Point(1609, 900);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(279, 57);
            this.btnSave.TabIndex = 25;
            this.btnSave.Text = "Save to file";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtScale
            // 
            this.txtScale.Location = new System.Drawing.Point(565, 920);
            this.txtScale.Name = "txtScale";
            this.txtScale.Size = new System.Drawing.Size(100, 22);
            this.txtScale.TabIndex = 26;
            this.txtScale.TextChanged += new System.EventHandler(this.txtScale_TextChanged);
            // 
            // lblScrollRight
            // 
            this.lblScrollRight.AutoSize = true;
            this.lblScrollRight.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScrollRight.ForeColor = System.Drawing.Color.White;
            this.lblScrollRight.Location = new System.Drawing.Point(671, 912);
            this.lblScrollRight.Name = "lblScrollRight";
            this.lblScrollRight.Size = new System.Drawing.Size(32, 37);
            this.lblScrollRight.TabIndex = 27;
            this.lblScrollRight.Text = ">";
            this.lblScrollRight.Click += new System.EventHandler(this.lblRight_Click);
            // 
            // lblScrollLeft
            // 
            this.lblScrollLeft.AutoSize = true;
            this.lblScrollLeft.Font = new System.Drawing.Font("Calibri", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScrollLeft.ForeColor = System.Drawing.Color.White;
            this.lblScrollLeft.Location = new System.Drawing.Point(527, 912);
            this.lblScrollLeft.Name = "lblScrollLeft";
            this.lblScrollLeft.Size = new System.Drawing.Size(32, 37);
            this.lblScrollLeft.TabIndex = 28;
            this.lblScrollLeft.Text = "<";
            this.lblScrollLeft.Click += new System.EventHandler(this.lblLeft_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SteelBlue;
            this.ClientSize = new System.Drawing.Size(1924, 969);
            this.Controls.Add(this.lblScrollLeft);
            this.Controls.Add(this.lblScrollRight);
            this.Controls.Add(this.txtScale);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.chFL);
            this.Controls.Add(this.chFR);
            this.Controls.Add(this.pbNoConnect);
            this.Controls.Add(this.btnLoadFile);
            this.Controls.Add(this.chSteps);
            this.Controls.Add(this.btnIdle);
            this.Controls.Add(this.btnMeasure);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.pnOpenConsole);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.lsConsole);
            this.Controls.Add(this.pnCloseConsole);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "VetSoft";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.pnCloseConsole.ResumeLayout(false);
            this.pnCloseConsole.PerformLayout();
            this.pnOpenConsole.ResumeLayout(false);
            this.pnOpenConsole.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chSteps)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbNoConnect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chFR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chFL)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnCloseConsole;
        private System.Windows.Forms.ListBox lsConsole;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Panel pnOpenConsole;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnDisconnect;
        private System.Windows.Forms.Button btnMeasure;
        private System.Windows.Forms.Button btnIdle;
        private System.Windows.Forms.DataVisualization.Charting.Chart chSteps;
        private System.Windows.Forms.Button btnLoadFile;
        private System.Windows.Forms.PictureBox pbNoConnect;
        private System.Windows.Forms.DataVisualization.Charting.Chart chFR;
        private System.Windows.Forms.DataVisualization.Charting.Chart chFL;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtScale;
        private System.Windows.Forms.Label lblScrollRight;
        private System.Windows.Forms.Label lblScrollLeft;
    }
}

