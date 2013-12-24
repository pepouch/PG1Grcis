using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace _069subdivision
{
  public partial class Form1 : Form
  {
    protected Image outputImage = null;
    bool freeDraw = false;
    bool currentPath = false;
    int skippedPoints = 0;
    const int skippedPointsBigLimit = 20;
    const int skippedPointsSmallLimit = 5;
    Bitmap output;

    public Form1()
    {
      InitializeComponent();
      String[] tok = "$Rev: 251 $".Split(' ');
      Text += " (rev: " + tok[1] + ')';
      Subdivision.AddUserPath();
      Subdivision.ResetRndSeed();
      int width = (int)numericXres.Value;
      int height = (int)numericYres.Value;
      Subdivision.GenerateDemoPoints(width, height);
      output  = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
      doRedraw();
    }

    private void buttonRedraw_Click(object sender, EventArgs e)
    {
      if (Subdivision.getDrawUserPoints())
      {
        buttonRedraw.Text = "Draw user input";
        Subdivision.setDrawUserPoints(false);
        checkBox2.Enabled = false;
        button3.Enabled = true;
      }
      else
      {
        buttonRedraw.Text = "Draw demo";
        Subdivision.setDrawUserPoints(true);
        checkBox2.Enabled = true;
        button3.Enabled = false;
      }
      doRedraw();
    }

    private void doRedraw()
    {
      buttonSave.Enabled = false;      

      Stopwatch sw = new Stopwatch();
      sw.Start();

        Graphics gfx = Graphics.FromImage(output);
        gfx.Clear(Color.Black);
        gfx.Dispose();
        Subdivision.TestImage(output, textParam.Text);

      sw.Stop();
      float elapsed = 1.0e-3f * sw.ElapsedMilliseconds;

      labelElapsed.Text = string.Format(CultureInfo.InvariantCulture, "Elapsed: {0:f3}s", elapsed);

      pictureBox1.Image = output;
      buttonSave.Enabled = true;
    }

    private void buttonSave_Click(object sender, EventArgs e)
    {
      if (outputImage == null) return;

      SaveFileDialog sfd = new SaveFileDialog();
      sfd.Title = "Save PNG file";
      sfd.Filter = "PNG Files|*.png";
      sfd.AddExtension = true;
      sfd.FileName = "";
      if (sfd.ShowDialog() != DialogResult.OK)
        return;

      outputImage.Save(sfd.FileName, System.Drawing.Imaging.ImageFormat.Png);
    }

    private void trackBar1_Scroll(object sender, EventArgs e)
    {
      Subdivision.SetParam("coef", (double)trackBar1.Value / (double)trackBar1.Maximum);
      doRedraw();
    }

    private void trackBar2_Scroll(object sender, EventArgs e)
    {
      Subdivision.SetParam("niter", trackBar2.Value);
      doRedraw();
    }

    private void trackBar3_Scroll(object sender, EventArgs e)
    {
      Subdivision.SetParam("width", (double)trackBar3.Value);
      doRedraw();
    }

    private void checkBox1_CheckedChanged(object sender, EventArgs e)
    {
      Subdivision.SetParam("polygon", checkBox1.Checked);
      doRedraw();
    }

    private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
    {
      Subdivision.AddUserPoint(e.X, e.Y);
      doRedraw();
    }

    private void button1_Click(object sender, EventArgs e)
    {
      Subdivision.ClearUserPoints();
      doRedraw();
    }

    private void button2_Click(object sender, EventArgs e)
    {
      Subdivision.CloseUserPoints();
      doRedraw();
    }

    private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
      Subdivision.avgMask = (Subdivision.mask)comboBox1.SelectedIndex;
      if (Subdivision.avgMask == Subdivision.mask.CHAIKIN)
        trackBar1.Enabled = true;
      else
        trackBar1.Enabled = false;
      doRedraw();
    }

    private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
    {
      if (!freeDraw)
        pictureBox1_MouseClick(sender, e);
      else
      {
        Subdivision.AddUserPath();
        skippedPoints = 0;
        currentPath = true;
      }
    }

    private void pictureBox1_MouseUp(object sender, EventArgs e)
    {
      if (!freeDraw)
        return;
      currentPath = false;
      skippedPoints = 0;
    }

    private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
    {
      if (!freeDraw || !currentPath)
        return;
      skippedPoints++;
      if (skippedPoints % skippedPointsSmallLimit == 0)
        Subdivision.AddUserPoint(e.X, e.Y);
      doRedraw();
      if (skippedPoints > skippedPointsBigLimit)
      {
        Subdivision.omitUserPoints((skippedPointsBigLimit/skippedPointsSmallLimit)-1);
        skippedPoints = 0;
      }
    }

    private void checkBox2_CheckedChanged(object sender, EventArgs e)
    {
      freeDraw = checkBox2.Checked;
      if (!freeDraw)
        Subdivision.AddUserPath();
    }

    private void button3_Click(object sender, EventArgs e)
    {
      int width = (int)numericXres.Value;
      int height = (int)numericYres.Value;
      Subdivision.GenerateDemoPoints(width, height);
      doRedraw();
    }
  }
}
