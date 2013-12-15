﻿using System.Windows.Forms;
using System.Drawing;

namespace _066histogram
{
  public partial class HistogramForm : Form
  {
    /// <summary>
    /// Main window pointer.
    /// </summary>
    protected Form1 parent;

    /// <summary>
    /// Back-buffer. Resized to current client-size of the form.
    /// </summary>
    protected Bitmap backBuffer;

    public HistogramForm ( Form1 par )
    {
      parent = par;
      InitializeComponent();
    }

    private void HistogramForm_FormClosed ( object sender, FormClosedEventArgs e )
    {
      parent.histogramForm = null;
    }

    private void HistogramForm_Paint ( object sender, PaintEventArgs e )
    {
      if ( backBuffer == null || parent.dirtyRedraw || parent.dirtyRecompute )
      {
        if ( backBuffer == null )
          backBuffer = new Bitmap( ClientSize.Width, ClientSize.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb );

        parent.ComputeHistogram( (Bitmap)parent.inputImage, backBuffer, parent.param );
        parent.dirtyRedraw = false;
      }

      e.Graphics.DrawImageUnscaled( backBuffer, 0, 0 );
    }

    private void HistogramForm_Resize ( object sender, System.EventArgs e )
    {
      if ( backBuffer != null &&
           (backBuffer.Width != ClientSize.Width || backBuffer.Height != ClientSize.Height) )
      {
        backBuffer = null;
        Invalidate();
      }
    }
  }
}
