namespace UrbanHeatIsland
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            monthCalendar1 = new MonthCalendar();
            button1 = new Button();
            groupBox1 = new GroupBox();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            groupBox2 = new GroupBox();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            label10 = new Label();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // monthCalendar1
            // 
            monthCalendar1.Location = new Point(18, 55);
            monthCalendar1.Name = "monthCalendar1";
            monthCalendar1.TabIndex = 2;
            monthCalendar1.DateSelected += monthCalendar1_DateSelected;
            // 
            // button1
            // 
            button1.Location = new Point(18, 9);
            button1.Name = "button1";
            button1.Size = new Size(112, 34);
            button1.TabIndex = 4;
            button1.Text = "Open";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(18, 320);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(112, 142);
            groupBox1.TabIndex = 7;
            groupBox1.TabStop = false;
            groupBox1.Text = "Forecast";
            groupBox1.Visible = false;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.ForeColor = Color.Blue;
            label4.Location = new Point(6, 102);
            label4.Name = "label4";
            label4.Size = new Size(59, 25);
            label4.TabIndex = 10;
            label4.Text = "label4";
            label4.Visible = false;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ForeColor = Color.Blue;
            label3.Location = new Point(6, 77);
            label3.Name = "label3";
            label3.Size = new Size(59, 25);
            label3.TabIndex = 9;
            label3.Text = "label3";
            label3.Visible = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = Color.Red;
            label2.Location = new Point(6, 52);
            label2.Name = "label2";
            label2.Size = new Size(59, 25);
            label2.TabIndex = 8;
            label2.Text = "label2";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = Color.Red;
            label1.Location = new Point(6, 27);
            label1.Name = "label1";
            label1.Size = new Size(59, 25);
            label1.TabIndex = 7;
            label1.Text = "label1";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(label5);
            groupBox2.Controls.Add(label6);
            groupBox2.Controls.Add(label7);
            groupBox2.Controls.Add(label8);
            groupBox2.Location = new Point(136, 320);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(112, 142);
            groupBox2.TabIndex = 8;
            groupBox2.TabStop = false;
            groupBox2.Text = "Extreme";
            groupBox2.Visible = false;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.ForeColor = Color.Blue;
            label5.Location = new Point(6, 102);
            label5.Name = "label5";
            label5.Size = new Size(59, 25);
            label5.TabIndex = 10;
            label5.Text = "label5";
            label5.Visible = false;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.ForeColor = Color.Blue;
            label6.Location = new Point(6, 77);
            label6.Name = "label6";
            label6.Size = new Size(59, 25);
            label6.TabIndex = 9;
            label6.Text = "label6";
            label6.Visible = false;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.ForeColor = Color.Red;
            label7.Location = new Point(6, 52);
            label7.Name = "label7";
            label7.Size = new Size(59, 25);
            label7.TabIndex = 8;
            label7.Text = "label7";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.ForeColor = Color.Red;
            label8.Location = new Point(6, 27);
            label8.Name = "label8";
            label8.Size = new Size(59, 25);
            label8.TabIndex = 7;
            label8.Text = "label8";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(545, 271);
            label10.Name = "label10";
            label10.Size = new Size(145, 25);
            label10.TabIndex = 10;
            label10.Text = "No data to show";
            label10.Visible = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1168, 625);
            Controls.Add(label10);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(button1);
            Controls.Add(monthCalendar1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "Urban heat island forecast";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            Paint += Form1_Paint;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private MonthCalendar monthCalendar1;
        private Button button1;
        private GroupBox groupBox1;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private GroupBox groupBox2;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label10;
    }
}
