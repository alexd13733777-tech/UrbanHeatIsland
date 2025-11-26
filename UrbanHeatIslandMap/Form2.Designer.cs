namespace UrbanHeatIsland
{
    partial class Form2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            monthCalendar1 = new MonthCalendar();
            groupBox1 = new GroupBox();
            label14 = new Label();
            label13 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            label10 = new Label();
            groupBox2 = new GroupBox();
            label16 = new Label();
            label15 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            progressBar1 = new ProgressBar();
            maskedTextBox1 = new MaskedTextBox();
            maskedTextBox2 = new MaskedTextBox();
            groupBox3 = new GroupBox();
            label11 = new Label();
            label9 = new Label();
            button1 = new Button();
            button2 = new Button();
            button3 = new Button();
            comboBox1 = new ComboBox();
            label12 = new Label();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            button4 = new Button();
            groupBox4 = new GroupBox();
            label17 = new Label();
            label18 = new Label();
            label19 = new Label();
            label20 = new Label();
            label21 = new Label();
            label22 = new Label();
            statusStrip1 = new StatusStrip();
            toolStripSplitButton1 = new ToolStripSplitButton();
            switchToOldVersionToolStripMenuItem = new ToolStripMenuItem();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox4.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // monthCalendar1
            // 
            monthCalendar1.Location = new Point(216, 456);
            monthCalendar1.Name = "monthCalendar1";
            monthCalendar1.TabIndex = 2;
            monthCalendar1.Visible = false;
            monthCalendar1.DateSelected += monthCalendar1_DateSelected;
            monthCalendar1.Leave += monthCalendar1_Leave;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label14);
            groupBox1.Controls.Add(label13);
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Location = new Point(148, 456);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(100, 187);
            groupBox1.TabIndex = 7;
            groupBox1.TabStop = false;
            groupBox1.Text = "Forecast";
            groupBox1.Visible = false;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.ForeColor = Color.Blue;
            label14.Location = new Point(6, 152);
            label14.Name = "label14";
            label14.Size = new Size(69, 25);
            label14.TabIndex = 21;
            label14.Text = "label14";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.ForeColor = Color.Red;
            label13.Location = new Point(6, 77);
            label13.Name = "label13";
            label13.Size = new Size(69, 25);
            label13.TabIndex = 21;
            label13.Text = "label13";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.ForeColor = Color.Blue;
            label4.Location = new Point(6, 127);
            label4.Name = "label4";
            label4.Size = new Size(59, 25);
            label4.TabIndex = 10;
            label4.Text = "label4";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.ForeColor = Color.Blue;
            label3.Location = new Point(6, 102);
            label3.Name = "label3";
            label3.Size = new Size(59, 25);
            label3.TabIndex = 9;
            label3.Text = "label3";
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
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(12, 493);
            label10.Name = "label10";
            label10.Size = new Size(88, 25);
            label10.TabIndex = 10;
            label10.Text = "Loading...";
            label10.Visible = false;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(label16);
            groupBox2.Controls.Add(label15);
            groupBox2.Controls.Add(label5);
            groupBox2.Controls.Add(label6);
            groupBox2.Controls.Add(label7);
            groupBox2.Controls.Add(label8);
            groupBox2.Location = new Point(254, 456);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(100, 187);
            groupBox2.TabIndex = 8;
            groupBox2.TabStop = false;
            groupBox2.Text = "Extreme";
            groupBox2.Visible = false;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.ForeColor = Color.Blue;
            label16.Location = new Point(6, 152);
            label16.Name = "label16";
            label16.Size = new Size(69, 25);
            label16.TabIndex = 21;
            label16.Text = "label16";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.ForeColor = Color.Red;
            label15.Location = new Point(6, 77);
            label15.Name = "label15";
            label15.Size = new Size(69, 25);
            label15.TabIndex = 21;
            label15.Text = "label15";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.ForeColor = Color.Blue;
            label5.Location = new Point(6, 127);
            label5.Name = "label5";
            label5.Size = new Size(59, 25);
            label5.TabIndex = 10;
            label5.Text = "label5";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.ForeColor = Color.Blue;
            label6.Location = new Point(6, 102);
            label6.Name = "label6";
            label6.Size = new Size(59, 25);
            label6.TabIndex = 9;
            label6.Text = "label6";
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
            // progressBar1
            // 
            progressBar1.Location = new Point(12, 456);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(320, 34);
            progressBar1.Style = ProgressBarStyle.Marquee;
            progressBar1.TabIndex = 21;
            progressBar1.Visible = false;
            // 
            // maskedTextBox1
            // 
            maskedTextBox1.BeepOnError = true;
            maskedTextBox1.Culture = new System.Globalization.CultureInfo("en-US");
            maskedTextBox1.Location = new Point(63, 30);
            maskedTextBox1.Mask = "00/00/0000";
            maskedTextBox1.Name = "maskedTextBox1";
            maskedTextBox1.RejectInputOnFirstFailure = true;
            maskedTextBox1.Size = new Size(100, 31);
            maskedTextBox1.TabIndex = 12;
            maskedTextBox1.ValidatingType = typeof(DateTime);
            maskedTextBox1.TextChanged += maskedTextBox1_TextChanged;
            maskedTextBox1.Enter += maskedTextBox1_Enter;
            // 
            // maskedTextBox2
            // 
            maskedTextBox2.Culture = new System.Globalization.CultureInfo("en-US");
            maskedTextBox2.Location = new Point(204, 30);
            maskedTextBox2.Mask = "00/00/0000";
            maskedTextBox2.Name = "maskedTextBox2";
            maskedTextBox2.Size = new Size(100, 31);
            maskedTextBox2.TabIndex = 13;
            maskedTextBox2.ValidatingType = typeof(DateTime);
            maskedTextBox2.DataContextChanged += maskedTextBox2_DataContextChanged;
            maskedTextBox2.Enter += maskedTextBox2_Enter;
            maskedTextBox2.Validated += maskedTextBox2_Validated;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(label11);
            groupBox3.Controls.Add(label9);
            groupBox3.Controls.Add(maskedTextBox1);
            groupBox3.Controls.Add(maskedTextBox2);
            groupBox3.Location = new Point(12, 383);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(320, 67);
            groupBox3.TabIndex = 14;
            groupBox3.TabStop = false;
            groupBox3.Text = "period";
            groupBox3.Leave += groupBox3_Leave;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(169, 36);
            label11.Name = "label11";
            label11.Size = new Size(29, 25);
            label11.TabIndex = 15;
            label11.Text = "to";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(6, 33);
            label9.Name = "label9";
            label9.Size = new Size(51, 25);
            label9.TabIndex = 14;
            label9.Text = "from";
            // 
            // button1
            // 
            button1.Location = new Point(315, 12);
            button1.Name = "button1";
            button1.Size = new Size(42, 42);
            button1.TabIndex = 15;
            button1.Text = "+";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(315, 60);
            button2.Name = "button2";
            button2.Size = new Size(42, 42);
            button2.TabIndex = 16;
            button2.Text = "-";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button3
            // 
            button3.Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point, 204);
            button3.Location = new Point(315, 108);
            button3.Name = "button3";
            button3.Size = new Size(42, 42);
            button3.TabIndex = 17;
            button3.Text = "•";
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // comboBox1
            // 
            comboBox1.AllowDrop = true;
            comboBox1.AutoCompleteCustomSource.AddRange(new string[] { "New York", "London", "Bejcin", "Tunis", "Addis Ababa", "Moskow", "Cairo", "New Deli", "Chelyabinsk", "San Francisco", "Sydney", "Dubai" });
            comboBox1.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(60, 344);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(249, 33);
            comboBox1.TabIndex = 18;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            comboBox1.SelectionChangeCommitted += comboBox1_SelectionChangeCommitted;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(12, 352);
            label12.Name = "label12";
            label12.Size = new Size(42, 25);
            label12.TabIndex = 19;
            label12.Text = "City";
            // 
            // backgroundWorker1
            // 
            backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            backgroundWorker1.ProgressChanged += backgroundWorker1_ProgressChanged;
            backgroundWorker1.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
            // 
            // button4
            // 
            button4.Location = new Point(315, 338);
            button4.Name = "button4";
            button4.Size = new Size(42, 42);
            button4.TabIndex = 22;
            button4.Text = ">";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click_1;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(label17);
            groupBox4.Controls.Add(label18);
            groupBox4.Controls.Add(label19);
            groupBox4.Controls.Add(label20);
            groupBox4.Controls.Add(label21);
            groupBox4.Controls.Add(label22);
            groupBox4.Location = new Point(12, 456);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(130, 187);
            groupBox4.TabIndex = 23;
            groupBox4.TabStop = false;
            groupBox4.Visible = false;
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.ForeColor = Color.Blue;
            label17.Location = new Point(6, 152);
            label17.Name = "label17";
            label17.Size = new Size(87, 25);
            label17.TabIndex = 21;
            label17.Text = "RH2M(%)";
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.ForeColor = Color.Red;
            label18.Location = new Point(6, 77);
            label18.Name = "label18";
            label18.Size = new Size(87, 25);
            label18.TabIndex = 21;
            label18.Text = "RH2M(%)";
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.ForeColor = Color.Blue;
            label19.Location = new Point(6, 127);
            label19.Name = "label19";
            label19.Size = new Size(113, 25);
            label19.TabIndex = 10;
            label19.Text = "PREC(mm/d)";
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.ForeColor = Color.Blue;
            label20.Location = new Point(6, 102);
            label20.Name = "label20";
            label20.Size = new Size(80, 25);
            label20.TabIndex = 9;
            label20.Text = "T2M (°С)";
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.ForeColor = Color.Red;
            label21.Location = new Point(6, 52);
            label21.Name = "label21";
            label21.Size = new Size(113, 25);
            label21.TabIndex = 8;
            label21.Text = "PREC(mm/d)";
            // 
            // label22
            // 
            label22.AutoSize = true;
            label22.ForeColor = Color.Red;
            label22.Location = new Point(6, 27);
            label22.Name = "label22";
            label22.Size = new Size(80, 25);
            label22.TabIndex = 7;
            label22.Text = "T2M (°С)";
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(24, 24);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripSplitButton1 });
            statusStrip1.Location = new Point(0, 811);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1189, 32);
            statusStrip1.TabIndex = 24;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripSplitButton1
            // 
            toolStripSplitButton1.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripSplitButton1.DropDownItems.AddRange(new ToolStripItem[] { switchToOldVersionToolStripMenuItem });
            toolStripSplitButton1.Image = (Image)resources.GetObject("toolStripSplitButton1.Image");
            toolStripSplitButton1.ImageTransparentColor = Color.Magenta;
            toolStripSplitButton1.Name = "toolStripSplitButton1";
            toolStripSplitButton1.Size = new Size(45, 29);
            toolStripSplitButton1.Text = "...";
            // 
            // switchToOldVersionToolStripMenuItem
            // 
            switchToOldVersionToolStripMenuItem.Name = "switchToOldVersionToolStripMenuItem";
            switchToOldVersionToolStripMenuItem.Size = new Size(280, 34);
            switchToOldVersionToolStripMenuItem.Text = "Switch to old version";
            switchToOldVersionToolStripMenuItem.Click += switchToOldVersionToolStripMenuItem_Click;
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1189, 843);
            Controls.Add(statusStrip1);
            Controls.Add(monthCalendar1);
            Controls.Add(groupBox4);
            Controls.Add(button4);
            Controls.Add(label10);
            Controls.Add(progressBar1);
            Controls.Add(label12);
            Controls.Add(comboBox1);
            Controls.Add(button3);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(groupBox3);
            Controls.Add(groupBox1);
            Controls.Add(groupBox2);
            Name = "Form2";
            Text = "Urban heat island forecast";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            Paint += Form1_Paint;
            Enter += Form2_Enter;
            MouseDown += Form2_MouseDown;
            MouseMove += Form2_MouseMove;
            MouseUp += Form2_MouseUp;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private MonthCalendar monthCalendar1;
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
        private MaskedTextBox maskedTextBox1;
        private MaskedTextBox maskedTextBox2;
        private GroupBox groupBox3;
        private Label label11;
        private Label label9;
        private Button button1;
        private Button button2;
        private Button button3;
        private ComboBox comboBox1;
        private Label label12;
        private Label label13;
        private Label label14;
        private Label label16;
        private Label label15;
        private ProgressBar progressBar1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Button button4;
        private GroupBox groupBox4;
        private Label label17;
        private Label label18;
        private Label label19;
        private Label label20;
        private Label label21;
        private Label label22;
        private StatusStrip statusStrip1;
        private ToolStripSplitButton toolStripSplitButton1;
        private ToolStripMenuItem switchToOldVersionToolStripMenuItem;
    }
}
