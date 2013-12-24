namespace _069subdivision
{
  partial class Form1
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose ( bool disposing )
    {
      if ( disposing && (components != null) )
      {
        components.Dispose();
      }
      base.Dispose( disposing );
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent ()
    {
      this.panel1 = new System.Windows.Forms.Panel();
      this.pictureBox1 = new System.Windows.Forms.PictureBox();
      this.label2 = new System.Windows.Forms.Label();
      this.buttonRedraw = new System.Windows.Forms.Button();
      this.buttonSave = new System.Windows.Forms.Button();
      this.textParam = new System.Windows.Forms.TextBox();
      this.labelElapsed = new System.Windows.Forms.Label();
      this.trackBar1 = new System.Windows.Forms.TrackBar();
      this.trackBar2 = new System.Windows.Forms.TrackBar();
      this.numericYres = new System.Windows.Forms.NumericUpDown();
      this.label3 = new System.Windows.Forms.Label();
      this.numericXres = new System.Windows.Forms.NumericUpDown();
      this.label1 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.checkBox1 = new System.Windows.Forms.CheckBox();
      this.label6 = new System.Windows.Forms.Label();
      this.trackBar3 = new System.Windows.Forms.TrackBar();
      this.button1 = new System.Windows.Forms.Button();
      this.button2 = new System.Windows.Forms.Button();
      this.comboBox1 = new System.Windows.Forms.ComboBox();
      this.checkBox2 = new System.Windows.Forms.CheckBox();
      this.panel1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericYres)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericXres)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.trackBar3)).BeginInit();
      this.SuspendLayout();
      // 
      // panel1
      // 
      this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.panel1.AutoScroll = true;
      this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.panel1.Controls.Add(this.pictureBox1);
      this.panel1.Location = new System.Drawing.Point(228, 13);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(815, 632);
      this.panel1.TabIndex = 0;
      // 
      // pictureBox1
      // 
      this.pictureBox1.Location = new System.Drawing.Point(3, 3);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new System.Drawing.Size(680, 380);
      this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
      this.pictureBox1.TabIndex = 2;
      this.pictureBox1.TabStop = false;
      this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
      this.pictureBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseMove);
      this.pictureBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseUp);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(19, 455);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(40, 13);
      this.label2.TabIndex = 6;
      this.label2.Text = "Param:";
      // 
      // buttonRedraw
      // 
      this.buttonRedraw.Location = new System.Drawing.Point(21, 350);
      this.buttonRedraw.Name = "buttonRedraw";
      this.buttonRedraw.Size = new System.Drawing.Size(92, 23);
      this.buttonRedraw.TabIndex = 13;
      this.buttonRedraw.Text = "Draw user input";
      this.buttonRedraw.UseVisualStyleBackColor = true;
      this.buttonRedraw.Click += new System.EventHandler(this.buttonRedraw_Click);
      // 
      // buttonSave
      // 
      this.buttonSave.Enabled = false;
      this.buttonSave.Location = new System.Drawing.Point(22, 479);
      this.buttonSave.Name = "buttonSave";
      this.buttonSave.Size = new System.Drawing.Size(108, 23);
      this.buttonSave.TabIndex = 14;
      this.buttonSave.Text = "Save image";
      this.buttonSave.UseVisualStyleBackColor = true;
      this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
      // 
      // textParam
      // 
      this.textParam.Location = new System.Drawing.Point(65, 453);
      this.textParam.Name = "textParam";
      this.textParam.Size = new System.Drawing.Size(135, 20);
      this.textParam.TabIndex = 15;
      // 
      // labelElapsed
      // 
      this.labelElapsed.AutoSize = true;
      this.labelElapsed.Location = new System.Drawing.Point(140, 355);
      this.labelElapsed.Name = "labelElapsed";
      this.labelElapsed.Size = new System.Drawing.Size(48, 13);
      this.labelElapsed.TabIndex = 16;
      this.labelElapsed.Text = "Elapsed:";
      // 
      // trackBar1
      // 
      this.trackBar1.LargeChange = 1;
      this.trackBar1.Location = new System.Drawing.Point(96, 159);
      this.trackBar1.Maximum = 60;
      this.trackBar1.Name = "trackBar1";
      this.trackBar1.Size = new System.Drawing.Size(104, 45);
      this.trackBar1.TabIndex = 17;
      this.trackBar1.TickFrequency = 15;
      this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
      // 
      // trackBar2
      // 
      this.trackBar2.LargeChange = 1;
      this.trackBar2.Location = new System.Drawing.Point(96, 200);
      this.trackBar2.Minimum = 1;
      this.trackBar2.Name = "trackBar2";
      this.trackBar2.Size = new System.Drawing.Size(104, 45);
      this.trackBar2.TabIndex = 18;
      this.trackBar2.Value = 5;
      this.trackBar2.Scroll += new System.EventHandler(this.trackBar2_Scroll);
      // 
      // numericYres
      // 
      this.numericYres.Increment = new decimal(new int[] {
            30,
            0,
            0,
            0});
      this.numericYres.Location = new System.Drawing.Point(96, 54);
      this.numericYres.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
      this.numericYres.Name = "numericYres";
      this.numericYres.Size = new System.Drawing.Size(71, 20);
      this.numericYres.TabIndex = 11;
      this.numericYres.Value = new decimal(new int[] {
            600,
            0,
            0,
            0});
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(41, 56);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(31, 13);
      this.label3.TabIndex = 10;
      this.label3.Text = "Yres:";
      // 
      // numericXres
      // 
      this.numericXres.Increment = new decimal(new int[] {
            40,
            0,
            0,
            0});
      this.numericXres.Location = new System.Drawing.Point(96, 28);
      this.numericXres.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
      this.numericXres.Name = "numericXres";
      this.numericXres.Size = new System.Drawing.Size(71, 20);
      this.numericXres.TabIndex = 3;
      this.numericXres.Value = new decimal(new int[] {
            800,
            0,
            0,
            0});
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(41, 30);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(31, 13);
      this.label1.TabIndex = 4;
      this.label1.Text = "Xres:";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(19, 200);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(53, 13);
      this.label4.TabIndex = 19;
      this.label4.Text = "Iterations:";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(12, 159);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(75, 13);
      this.label5.TabIndex = 20;
      this.label5.Text = "Chaikin coeff.:";
      // 
      // checkBox1
      // 
      this.checkBox1.AutoSize = true;
      this.checkBox1.Checked = true;
      this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkBox1.Location = new System.Drawing.Point(39, 292);
      this.checkBox1.Name = "checkBox1";
      this.checkBox1.Size = new System.Drawing.Size(128, 17);
      this.checkBox1.TabIndex = 21;
      this.checkBox1.Text = "Show control polygon";
      this.checkBox1.UseVisualStyleBackColor = true;
      this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(19, 241);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(58, 13);
      this.label6.TabIndex = 23;
      this.label6.Text = "Line width:";
      // 
      // trackBar3
      // 
      this.trackBar3.LargeChange = 1;
      this.trackBar3.Location = new System.Drawing.Point(96, 241);
      this.trackBar3.Maximum = 20;
      this.trackBar3.Minimum = 1;
      this.trackBar3.Name = "trackBar3";
      this.trackBar3.Size = new System.Drawing.Size(104, 45);
      this.trackBar3.TabIndex = 22;
      this.trackBar3.Value = 1;
      this.trackBar3.Scroll += new System.EventHandler(this.trackBar3_Scroll);
      // 
      // button1
      // 
      this.button1.Location = new System.Drawing.Point(21, 379);
      this.button1.Name = "button1";
      this.button1.Size = new System.Drawing.Size(89, 23);
      this.button1.TabIndex = 24;
      this.button1.Text = "Clear user input";
      this.button1.UseVisualStyleBackColor = true;
      this.button1.Click += new System.EventHandler(this.button1_Click);
      // 
      // button2
      // 
      this.button2.Location = new System.Drawing.Point(22, 408);
      this.button2.Name = "button2";
      this.button2.Size = new System.Drawing.Size(88, 23);
      this.button2.TabIndex = 25;
      this.button2.Text = "Close polygon";
      this.button2.UseVisualStyleBackColor = true;
      this.button2.Click += new System.EventHandler(this.button2_Click);
      // 
      // comboBox1
      // 
      this.comboBox1.FormattingEnabled = true;
      this.comboBox1.Items.AddRange(new object[] {
            "Chaikin",
            "Cubic B-spline",
            "Dyn-Levin-Gregory",
            "Custom"});
      this.comboBox1.Location = new System.Drawing.Point(44, 104);
      this.comboBox1.Name = "comboBox1";
      this.comboBox1.Size = new System.Drawing.Size(121, 21);
      this.comboBox1.TabIndex = 26;
      this.comboBox1.Text = "Chaikin";
      this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
      // 
      // checkBox2
      // 
      this.checkBox2.AutoSize = true;
      this.checkBox2.Enabled = false;
      this.checkBox2.Location = new System.Drawing.Point(39, 315);
      this.checkBox2.Name = "checkBox2";
      this.checkBox2.Size = new System.Drawing.Size(87, 17);
      this.checkBox2.TabIndex = 27;
      this.checkBox2.Text = "Free drawing";
      this.checkBox2.UseVisualStyleBackColor = true;
      this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
      // 
      // Form1
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(1055, 663);
      this.Controls.Add(this.checkBox2);
      this.Controls.Add(this.comboBox1);
      this.Controls.Add(this.button2);
      this.Controls.Add(this.button1);
      this.Controls.Add(this.label6);
      this.Controls.Add(this.trackBar3);
      this.Controls.Add(this.checkBox1);
      this.Controls.Add(this.label5);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.labelElapsed);
      this.Controls.Add(this.textParam);
      this.Controls.Add(this.trackBar2);
      this.Controls.Add(this.buttonSave);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.buttonRedraw);
      this.Controls.Add(this.trackBar1);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.numericXres);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.numericYres);
      this.Controls.Add(this.panel1);
      this.MinimumSize = new System.Drawing.Size(600, 240);
      this.Name = "Form1";
      this.Text = "069 subdivision";
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.trackBar2)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericYres)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericXres)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.trackBar3)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.PictureBox pictureBox1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Button buttonRedraw;
    private System.Windows.Forms.Button buttonSave;
    private System.Windows.Forms.TextBox textParam;
    private System.Windows.Forms.Label labelElapsed;
    private System.Windows.Forms.TrackBar trackBar1;
    private System.Windows.Forms.TrackBar trackBar2;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.NumericUpDown numericXres;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.NumericUpDown numericYres;
    private System.Windows.Forms.CheckBox checkBox1;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.TrackBar trackBar3;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.ComboBox comboBox1;
    private System.Windows.Forms.CheckBox checkBox2;
  }
}

