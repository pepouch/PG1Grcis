﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _034ascii
{
  public partial class Form1 : Form
  {
    /// <summary>
    /// The bitmap to be used as source data
    /// </summary>
    protected Bitmap inputImage = null;

    /// <summary>
    /// Default output size in characters.
    /// </summary>
    protected const int BiggerSize = 100;

    public Form1 ()
    {
      InitializeComponent();
      String[] tok = "$Rev: 243 $".Split( ' ' );
      Text += " (rev: " + tok[ 1 ] + ')';
    }

    private void btnOpen_Click ( object sender, EventArgs e )
    {
      OpenFileDialog dlg = new OpenFileDialog();

      dlg.Title = "Open Image File";
      dlg.Filter = "Bitmap Files|*.bmp" +
          "|Gif Files|*.gif" +
          "|JPEG Files|*.jpg" +
          "|PNG Files|*.png" +
          "|TIFF Files|*.tif" +
          "|All image types|*.bmp;*.gif;*.jpg;*.png;*.tif";

      dlg.FilterIndex = 6;

      if ( dlg.ShowDialog() == DialogResult.OK )
      {
        try
        {
          inputImage = new Bitmap( Bitmap.FromFile( dlg.FileName ) );
        }
        catch ( Exception exc )
        {
          MessageBox.Show( exc.Message );
        }

        pictureBox1.Image = inputImage;
      }

      int w = inputImage.Width;
      int h = inputImage.Height;
      double factor = 1.0;

      if ( w > h )
        factor = (double)BiggerSize / (double)w;
      else
        factor = (double)BiggerSize / (double)h;

      txtHeight.Text = Math.Round((double)h * factor * 0.65d).ToString();
      txtWidth.Text  = Math.Round( (double)w * factor ).ToString();

    }

    private void btnConvert_Click ( object sender, EventArgs e )
    {
      int w = 100, h = 100;
      int.TryParse( txtWidth.Text, out w );
      int.TryParse( txtHeight.Text, out h );

      string text = AsciiArt.Process( inputImage, w, h, textParam.Text );
      Font fnt = AsciiArt.GetFont();

      Output dlgOut = new Output();
      dlgOut.WndText = text;
      dlgOut.Fnt = fnt;
      dlgOut.ShowDialog();
    }
  }
}
