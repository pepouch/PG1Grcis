﻿namespace _066histogram
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
      this.buttonOpen = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.buttonHistogram = new System.Windows.Forms.Button();
      this.textParam = new System.Windows.Forms.TextBox();
      this.panel1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
      this.SuspendLayout();
      // 
      // panel1
      // 
      this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
      | System.Windows.Forms.AnchorStyles.Left)
      | System.Windows.Forms.AnchorStyles.Right)));
      this.panel1.AutoScroll = true;
      this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.panel1.Controls.Add( this.pictureBox1 );
      this.panel1.Location = new System.Drawing.Point( 13, 13 );
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size( 680, 380 );
      this.panel1.TabIndex = 0;
      // 
      // pictureBox1
      // 
      this.pictureBox1.Location = new System.Drawing.Point( 0, 0 );
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new System.Drawing.Size( 680, 380 );
      this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
      this.pictureBox1.TabIndex = 2;
      this.pictureBox1.TabStop = false;
      // 
      // buttonOpen
      // 
      this.buttonOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonOpen.Location = new System.Drawing.Point( 13, 408 );
      this.buttonOpen.Name = "buttonOpen";
      this.buttonOpen.Size = new System.Drawing.Size( 101, 23 );
      this.buttonOpen.TabIndex = 1;
      this.buttonOpen.Text = "Load image";
      this.buttonOpen.UseVisualStyleBackColor = true;
      this.buttonOpen.Click += new System.EventHandler( this.buttonOpen_Click );
      // 
      // label1
      // 
      this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point( 129, 413 );
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size( 58, 13 );
      this.label1.TabIndex = 5;
      this.label1.Text = "Parameter:";
      // 
      // buttonHistogram
      // 
      this.buttonHistogram.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.buttonHistogram.Location = new System.Drawing.Point( 441, 408 );
      this.buttonHistogram.Name = "buttonHistogram";
      this.buttonHistogram.Size = new System.Drawing.Size( 84, 23 );
      this.buttonHistogram.TabIndex = 6;
      this.buttonHistogram.Text = "Histogram";
      this.buttonHistogram.UseVisualStyleBackColor = true;
      this.buttonHistogram.Click += new System.EventHandler( this.buttonHistogram_Click );
      // 
      // textParam
      // 
      this.textParam.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.textParam.Location = new System.Drawing.Point( 195, 410 );
      this.textParam.Name = "textParam";
      this.textParam.Size = new System.Drawing.Size( 229, 20 );
      this.textParam.TabIndex = 7;
      this.textParam.KeyPress += new System.Windows.Forms.KeyPressEventHandler( this.textParam_KeyPress );
      // 
      // Form1
      // 
      this.AllowDrop = true;
      this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size( 710, 443 );
      this.Controls.Add( this.textParam );
      this.Controls.Add( this.buttonHistogram );
      this.Controls.Add( this.label1 );
      this.Controls.Add( this.buttonOpen );
      this.Controls.Add( this.panel1 );
      this.MinimumSize = new System.Drawing.Size( 710, 200 );
      this.Name = "Form1";
      this.Text = "066 image histogram";
      this.DragDrop += new System.Windows.Forms.DragEventHandler( this.Form1_DragDrop );
      this.DragEnter += new System.Windows.Forms.DragEventHandler( this.Form1_DragEnter );
      this.panel1.ResumeLayout( false );
      this.panel1.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
      this.ResumeLayout( false );
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.PictureBox pictureBox1;
    private System.Windows.Forms.Button buttonOpen;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Button buttonHistogram;
    private System.Windows.Forms.TextBox textParam;
  }
}

