
namespace YapayZekaDroneOdevi
{
    partial class Anamenu
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
            this.components = new System.ComponentModel.Container();
            this.panelCizim = new System.Windows.Forms.Panel();
            this.timerRefresh = new System.Windows.Forms.Timer(this.components);
            this.labelDroneCount = new System.Windows.Forms.Label();
            this.cmbBoxDroneCount = new System.Windows.Forms.ComboBox();
            this.labelDroneLocation = new System.Windows.Forms.Label();
            this.buttonToggleTimer = new System.Windows.Forms.Button();
            this.buttonReset = new System.Windows.Forms.Button();
            this.labelGenCount = new System.Windows.Forms.Label();
            this.labelLastGenInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // panelCizim
            // 
            this.panelCizim.Location = new System.Drawing.Point(12, 12);
            this.panelCizim.Name = "panelCizim";
            this.panelCizim.Size = new System.Drawing.Size(900, 900);
            this.panelCizim.TabIndex = 0;
            // 
            // timerRefresh
            // 
            this.timerRefresh.Interval = 10;
            // 
            // labelDroneCount
            // 
            this.labelDroneCount.AutoSize = true;
            this.labelDroneCount.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelDroneCount.ForeColor = System.Drawing.Color.Orange;
            this.labelDroneCount.Location = new System.Drawing.Point(941, 22);
            this.labelDroneCount.Name = "labelDroneCount";
            this.labelDroneCount.Size = new System.Drawing.Size(193, 30);
            this.labelDroneCount.TabIndex = 1;
            this.labelDroneCount.Text = "Select Drone Count";
            // 
            // cmbBoxDroneCount
            // 
            this.cmbBoxDroneCount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(19)))));
            this.cmbBoxDroneCount.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbBoxDroneCount.ForeColor = System.Drawing.Color.Gold;
            this.cmbBoxDroneCount.FormattingEnabled = true;
            this.cmbBoxDroneCount.Items.AddRange(new object[] {
            "One (1) drone.",
            "Two (2) drones.",
            "Three (3) drones.",
            "Four (4) drones.",
            "Five (5) drones.",
            "Six (6) drones.",
            "Seven (7) drones.",
            "Eight (8) drones."});
            this.cmbBoxDroneCount.Location = new System.Drawing.Point(941, 55);
            this.cmbBoxDroneCount.Name = "cmbBoxDroneCount";
            this.cmbBoxDroneCount.Size = new System.Drawing.Size(225, 23);
            this.cmbBoxDroneCount.TabIndex = 2;
            // 
            // labelDroneLocation
            // 
            this.labelDroneLocation.AutoSize = true;
            this.labelDroneLocation.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelDroneLocation.ForeColor = System.Drawing.Color.Orange;
            this.labelDroneLocation.Location = new System.Drawing.Point(941, 96);
            this.labelDroneLocation.Name = "labelDroneLocation";
            this.labelDroneLocation.Size = new System.Drawing.Size(216, 30);
            this.labelDroneLocation.TabIndex = 3;
            this.labelDroneLocation.Text = "Select Drone Location";
            // 
            // buttonToggleTimer
            // 
            this.buttonToggleTimer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.buttonToggleTimer.FlatAppearance.BorderSize = 0;
            this.buttonToggleTimer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonToggleTimer.ForeColor = System.Drawing.Color.Gold;
            this.buttonToggleTimer.Location = new System.Drawing.Point(941, 129);
            this.buttonToggleTimer.Name = "buttonToggleTimer";
            this.buttonToggleTimer.Size = new System.Drawing.Size(216, 62);
            this.buttonToggleTimer.TabIndex = 4;
            this.buttonToggleTimer.Text = "Start";
            this.buttonToggleTimer.UseVisualStyleBackColor = false;
            // 
            // buttonReset
            // 
            this.buttonReset.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(33)))), ((int)(((byte)(33)))));
            this.buttonReset.FlatAppearance.BorderSize = 0;
            this.buttonReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonReset.ForeColor = System.Drawing.Color.Gold;
            this.buttonReset.Location = new System.Drawing.Point(941, 850);
            this.buttonReset.Name = "buttonReset";
            this.buttonReset.Size = new System.Drawing.Size(216, 62);
            this.buttonReset.TabIndex = 5;
            this.buttonReset.Text = "Reset";
            this.buttonReset.UseVisualStyleBackColor = false;
            // 
            // labelGenCount
            // 
            this.labelGenCount.AutoSize = true;
            this.labelGenCount.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelGenCount.ForeColor = System.Drawing.Color.Orange;
            this.labelGenCount.Location = new System.Drawing.Point(941, 208);
            this.labelGenCount.Name = "labelGenCount";
            this.labelGenCount.Size = new System.Drawing.Size(40, 30);
            this.labelGenCount.TabIndex = 6;
            this.labelGenCount.Text = "XD";
            this.labelGenCount.Visible = false;
            // 
            // labelLastGenInfo
            // 
            this.labelLastGenInfo.AutoSize = true;
            this.labelLastGenInfo.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelLastGenInfo.ForeColor = System.Drawing.Color.Orange;
            this.labelLastGenInfo.Location = new System.Drawing.Point(941, 238);
            this.labelLastGenInfo.Name = "labelLastGenInfo";
            this.labelLastGenInfo.Size = new System.Drawing.Size(0, 20);
            this.labelLastGenInfo.TabIndex = 7;
            // 
            // Anamenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.ClientSize = new System.Drawing.Size(1175, 926);
            this.Controls.Add(this.labelLastGenInfo);
            this.Controls.Add(this.labelGenCount);
            this.Controls.Add(this.buttonReset);
            this.Controls.Add(this.buttonToggleTimer);
            this.Controls.Add(this.labelDroneLocation);
            this.Controls.Add(this.cmbBoxDroneCount);
            this.Controls.Add(this.labelDroneCount);
            this.Controls.Add(this.panelCizim);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Anamenu";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Drone Simülasyonu";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Panel panelCizim;
        public System.Windows.Forms.Timer timerRefresh;
        private System.Windows.Forms.Label labelDroneCount;
        public System.Windows.Forms.Label labelDroneLocation;
        public System.Windows.Forms.ComboBox cmbBoxDroneCount;
        public System.Windows.Forms.Button buttonToggleTimer;
        public System.Windows.Forms.Button buttonReset;
        public System.Windows.Forms.Label labelGenCount;
        public System.Windows.Forms.Label labelLastGenInfo;
    }
}

