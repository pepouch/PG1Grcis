using System.Drawing;
using System.Windows.Forms;
using MathSupport;
using Raster;
using System;

namespace _066histogram
{
  public partial class Form1 : Form
  {
    // The histogram should be redrawn
    public bool dirtyRedraw = false;
    // The histogram should be recomputed
    public bool dirtyRecompute = false;

    /// <summary>
    /// Cached histogram data.
    /// </summary>
    protected int[] histArrayR = null;      // red
    protected int[] histArrayG = null;      // green
    protected int[] histArrayB = null;      // blue
    protected int[] histArrayY = null;      // gray
    protected int[] histArrayHue = null;    // hue
    protected int[] histArraySat = null;    // saturation
    protected int[] histArrayVal = null;    // value

    // Computes histogram data for given input Bitmap
    private void ComputeHistArrays(Bitmap input)
    {
        histArrayR = new int[256];
        histArrayG = new int[256];
        histArrayB = new int[256];
        histArrayY = new int[256];

        // We will scale the H, S, V to integer values between 0 and 255
        histArrayHue = new int[256];
        histArraySat = new int[256];
        histArrayVal = new int[256];

        for (int x = 0; x < input.Width; x++) 
            for (int y = 0; y < input.Height; y++)
            {
                Color col = input.GetPixel(x, y);

                int Y = Draw.RgbToGray( col.R, col.G, col.B );

                double H, S, V;
                Arith.ColorToHSV( col, out H, out S, out V );

                histArrayR[col.R]++;
                histArrayG[col.G]++;
                histArrayB[col.B]++;
                histArrayY[Y]++;

                // If saturation or value is 0, the hue is undefined
                if (S > 1e-5f && V > 1e-5f)
                    histArrayHue[(int)(H * 255f / 360f)]++;
                histArraySat[(int)(S * 255f)]++;
                histArrayVal[(int)(V * 255f)]++;
            }
    }

    // Draws a linear graph of "data" to Bitmap "graph", scaled by "maxValue", using color "col"
    // alt - don't paint the area below the graph curve
    // modeLog - draw in logscale
    // pctBottomLeft - space left below x axis in percents (used for custom axis when plotting saturation and value)
    private void DrawLinearGraph(Bitmap graph, int[] data, int maxValue, Color col, bool alt = false, bool modeLog = false, float pctBottomLeft = 0.05f)
    {
        // This should not actually happen, but...
        if (maxValue == 0)
            return;

        Graphics gfx = Graphics.FromImage(graph);
        // Antialiasing of the graph curve,
        // not the painted area below, because the individual filled rectangles would become visible
        if (alt)
            gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

        // Graph scaling:
        float maxValueF = (float)maxValue;
        // Logaritmic scale:
        if (modeLog)
            maxValueF = (float)Math.Log(maxValueF);
        // Graph dimensions:
        float x0 = graph.Width * 0.05f;
        float y0 = graph.Height * (1f - pctBottomLeft);
        float kx = graph.Width * 0.9f / data.Length;
        float ky = -graph.Height * (1f - 0.05f -pctBottomLeft) / maxValueF;

        // Pens:
        Brush graphBrush = new SolidBrush(col);
        Pen graphPen = new Pen(col, 2f);
        Pen axisPen = new Pen(Color.Black);

        // Last and previous value to connect in the plot
        float data_last, data_prev = 0f;
        // Plot data:
        for (int x = 0; x < data.Length; x++)
        {
            data_last = (float)(modeLog ? (data[x] <= 1 ? 0 : Math.Log(data[x])) : data[x]);
            if (x != 0)
                data_prev = (float)(modeLog ? (data[x-1] <= 1 ? 0 : Math.Log(data[x-1])) : data[x-1]);
            float yHeight = -data_last * ky;
            if (alt && x != 0)
                gfx.DrawLine(graphPen, x0 + (x-0.5f) * kx, y0 + data_prev * ky, x0 + (x+0.5f) * kx, y0 + data_last * ky);
            else
                gfx.FillRectangle(graphBrush, x0 + x * kx, y0 + data_last * ky, kx, yHeight);
        }

        gfx.Dispose();
    }

    // Draws x and y axis for linear graph
    private void drawLinearAxes(Bitmap graph)
    {
        Graphics gfx = Graphics.FromImage(graph);
        Pen axisPen = new Pen(Color.Black);
        gfx.DrawLine(axisPen, graph.Width * 0.05f, graph.Height * 0.95f, graph.Width * 0.95f, graph.Height * 0.95f);
        gfx.DrawLine(axisPen, graph.Width * 0.05f, graph.Height * 0.95f, graph.Width * 0.05f, graph.Height * 0.05f);
        gfx.Dispose();
    }

    // Draws a circullar graph into the Bitmap "graph" using "data", scaling by "maxValue"; used for plotting hue
    // Draws also axes and the hue circle for reference
    // modeLog - draw in logscale
    private void DrawCircullarGraph(Bitmap graph, int[] data, int maxValue, bool modeLog = false)
    {
        Graphics gfx = Graphics.FromImage(graph);
        gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        

        // Graph scaling:
        float maxValueF = (float)maxValue;
        // Logaritmic scale
        if (modeLog)
            maxValueF = (maxValue == 0) ? 0: (float)Math.Log(maxValueF);
        maxValueF = Math.Max(maxValueF, 1f);

        // Pens:
        Pen graphPen = new Pen(Color.Black);
        Pen axisPen = new Pen(Color.Black);

        // Axes:
        Point p1 = new Point();
        Point p2 = new Point();
        Point p3 = new Point();
        Point p4 = new Point();
        
        // Data:
        float centerX = graph.Width * 0.5f;
        float centerY = graph.Height * 0.5f;
        float perimeter = Math.Min(graph.Width, graph.Height) * 0.85f * 0.5f;
        Point center = new Point((int)centerX, (int)centerY);
        Point border1 = new Point();
        Point border2 = new Point();
        int len = data.Length;

        for (int x = 0; x < data.Length; x++)
        {
            // Color, brush and pen for this circle fragment:
            Color graphColor = Arith.HSVToColor((float)(x) *360f/(float)len, 1f, 1f);
            graphPen.Color = graphColor;
            Brush graphBrush = new SolidBrush(graphColor);

            // Data conversion to angle-value
            float data_last = (float)(modeLog ? (data[x] <= 1 ? 0 : Math.Log(data[x])) : data[x]);
            float value = perimeter * data_last / (float)maxValueF;
            float angle = (float)((float)(x) * 2f * Math.PI / (float)(len));

            // Plot data:
            border1.X = (int)(centerX + value * (float)Math.Cos(angle));
            border1.Y = (int)(centerY + value * (float)Math.Sin(angle));
            border2.X = (int)(centerX + value * (float)Math.Cos(angle - 1.2f * 2f * Math.PI / (float)len));
            border2.Y = (int)(centerY + value * (float)Math.Sin(angle - 1.2f * 2f * Math.PI / (float)len));
            Point[] ray = { center, border1, border2 };
            gfx.DrawLine(graphPen, center, border1);
            gfx.FillPolygon(graphBrush, ray);

            // Plot referential hue circle:
            p1.X = (int)(centerX + perimeter * (float)Math.Cos(angle));
            p1.Y = (int)(centerY + perimeter * (float)Math.Sin(angle));
            p2.X = (int)(centerX + perimeter * 1.05 * (float)Math.Cos(angle));
            p2.Y = (int)(centerY + perimeter * 1.05 * (float)Math.Sin(angle));
            p3.X = (int)(centerX + perimeter * (float)Math.Cos(angle + 1.2f * 2f * Math.PI / (float)len));
            p3.Y = (int)(centerY + perimeter * (float)Math.Sin(angle + 1.2f * 2f * Math.PI / (float)len));
            p4.X = (int)(centerX + perimeter * 1.05 * (float)Math.Cos(angle + 1.2f * 2f * Math.PI / (float)len));
            p4.Y = (int)(centerY + perimeter * 1.05 * (float)Math.Sin(angle + 1.2f * 2f * Math.PI / (float)len));
            Point[] rainbow = { p1, p2, p4, p3 };
            gfx.FillPolygon(graphBrush, rainbow);
        }

        // Axes:
        gfx.DrawEllipse(axisPen, centerX - perimeter, centerY - perimeter, 2 * perimeter, 2 * perimeter);
        gfx.DrawEllipse(axisPen, centerX - 1.05f * perimeter, centerY - 1.05f * perimeter, 2.1f * perimeter, 2.1f * perimeter);
        gfx.DrawLine(axisPen, centerX, centerY, centerX + perimeter, centerY);
        gfx.DrawLine(axisPen, centerX, centerY, centerX + perimeter * (float)Math.Cos(2f * Math.PI / 3f), centerY + perimeter * (float)Math.Sin(2f * Math.PI / 3f));
        gfx.DrawLine(axisPen, centerX, centerY, centerX + perimeter * (float)Math.Cos(4f * Math.PI / 3f), centerY + perimeter * (float)Math.Sin(4f * Math.PI / 3f));

        gfx.Dispose();
    }

    private void DrawSatAxes(Bitmap graph, float pctBottomLeft)
    {
        float len = 255f;
        float x0 = graph.Width * 0.05f;
        float y0 = graph.Height * (1f - pctBottomLeft);
        float kx = graph.Width * 0.9f / len;
        float x1 = graph.Width * 0.95f;
        float axisWidth = graph.Height * (pctBottomLeft - 0.05f) * 0.333f;

        Graphics gfx = Graphics.FromImage(graph);

        // Horizontal axis
        for (int x = 0; x < len; x++)
        {
            Color axisColor = Arith.HSVToColor(0f, (float)x / 255f, 1f);
            Brush axisBrush = new SolidBrush(axisColor);
            gfx.FillRectangle(axisBrush, x0 + (float)x * kx, y0, kx, axisWidth);
            axisColor = Arith.HSVToColor(120f, (float)x / 255f, 1f);
            axisBrush = new SolidBrush(axisColor);
            gfx.FillRectangle(axisBrush, x0 + (float)x * kx, y0 + axisWidth, kx, axisWidth);
            axisColor = Arith.HSVToColor(240f, (float)x / 255f, 1f);
            axisBrush = new SolidBrush(axisColor);
            gfx.FillRectangle(axisBrush, x0 + (float)x * kx, y0 + 2f * axisWidth, kx, axisWidth);
        }

        // Vertical axis
        Pen axisPen = new Pen(Color.Black);
        gfx.DrawLine(axisPen, graph.Width * 0.05f, graph.Height * 0.95f, graph.Width * 0.05f, graph.Height * 0.05f);

        gfx.Dispose();
    }

    private void DrawValAxes(Bitmap graph, float pctBottomLeft)
    {
        float len = 255f;
        float x0 = graph.Width * 0.05f;
        float y0 = graph.Height * (1f - pctBottomLeft);
        float kx = graph.Width * 0.9f / len;
        float x1 = graph.Width * 0.95f;
        float axisWidth = graph.Height * (pctBottomLeft - 0.05f) * 0.333f;

        Graphics gfx = Graphics.FromImage(graph);

        // Horizontal axis
        for (int x = 0; x < len; x++)
        {
            Color axisColor = Arith.HSVToColor(0f, 1f, (float)x / 255f);
            Brush axisBrush = new SolidBrush(axisColor);
            gfx.FillRectangle(axisBrush, x0 + (float)x * kx, y0, kx, axisWidth);
            axisColor = Arith.HSVToColor(120f, 1f, (float)x / 255f);
            axisBrush = new SolidBrush(axisColor);
            gfx.FillRectangle(axisBrush, x0 + (float)x * kx, y0 + axisWidth, kx, axisWidth);
            axisColor = Arith.HSVToColor(240f, 1f, (float)x / 255f);
            axisBrush = new SolidBrush(axisColor);
            gfx.FillRectangle(axisBrush, x0 + (float)x * kx, y0 + 2f * axisWidth, kx, axisWidth);
        }

        // Vertical axis
        Pen axisPen = new Pen(Color.Black);
        gfx.DrawLine(axisPen, graph.Width * 0.05f, graph.Height * 0.95f, graph.Width * 0.05f, graph.Height * 0.05f);

        gfx.Dispose();
    }

    /// <summary>
    /// Recomputes image histogram (if needed) and draws the result in the given raster image.
    /// </summary>
    /// <param name="input">Input image.</param>
    /// <param name="graph">Result image (for the graph).</param>
    /// <param name="param">Textual parameter.</param>
    public void ComputeHistogram(Bitmap input, Bitmap graph, string param)
    {
      // Text parameters:
      param = param.ToLower().Trim();
      bool modeRed = param.IndexOf("red") >= 0;
      bool modeGreen = param.IndexOf("green") >= 0;
      bool modeBlue = param.IndexOf("blue") >= 0;
      bool modeGray = param.IndexOf("gray") >= 0;
      bool modeHue = param.IndexOf("hue") >= 0;
      bool modeSat = param.IndexOf("sat") >= 0;
      bool modeVal = param.IndexOf("val") >= 0;
      bool modeLog = param.IndexOf("log") >= 0;

      // Default parameters (if not specified): red green blue gray
      if (!modeRed && !modeGreen && !modeBlue && !modeGray && !modeHue && !modeSat && !modeVal)
          modeRed = modeGreen = modeBlue = modeGray = true;

      // Histogram recomputation:
      if ( this.dirtyRecompute)
      {
        this.dirtyRecompute = false;
        this.ComputeHistArrays(input);
      }

      // We need to compute the maximum value, which is then passed to plotting functions
      int max = 0;  // maximum of R, G, B, gray
      foreach ( var f in histArrayR ) if ( f > max ) max = f;
      foreach ( var f in histArrayG ) if ( f > max ) max = f;
      foreach ( var f in histArrayB ) if ( f > max ) max = f;
      foreach ( var f in histArrayY ) if ( f > max ) max = f;
      int hueMax = 0;   // maximum of hue
      foreach (var f in histArrayHue) if (f > hueMax) hueMax = f;
      int satMax = 0;   // maximum of saturation
      foreach (var f in histArraySat) if (f > satMax) satMax = f;
      int valMax = 0;   // maximum of value
      foreach (var f in histArrayVal) if (f > valMax) valMax = f;

      // Graph background:
      Graphics gfx = Graphics.FromImage(graph);
      gfx.Clear(Color.White);
      gfx.Dispose();

      // Draw hue graph
      if (modeHue)
      {
          this.DrawCircullarGraph(graph, histArrayHue, hueMax, modeLog);
          return;
      }
      // Draw saturation graph
      if (modeSat)
      {
          this.DrawLinearGraph(graph, histArraySat, satMax, Color.FromArgb(64, Color.Black), false, modeLog, 0.15f);
          this.DrawLinearGraph(graph, histArraySat, satMax, Color.Black, true, modeLog, 0.15f);
          this.DrawSatAxes(graph, 0.15f);
          return;
      }
      // Draw value graph
      if (modeVal)
      {
          this.DrawLinearGraph(graph, histArrayVal, valMax, Color.FromArgb(64, Color.Black), false, modeLog, 0.15f);
          this.DrawLinearGraph(graph, histArrayVal, valMax, Color.Black, true, modeLog, 0.15f);
          this.DrawValAxes(graph, 0.15f);
          return;
      }

      // Draw linear graph of selected data (red, green, blue, gray)
      if (modeBlue)
          this.DrawLinearGraph(graph, histArrayB, max, Color.FromArgb(128, Color.Blue), false, modeLog);
      if (modeRed)
          this.DrawLinearGraph(graph, histArrayR, max, Color.FromArgb(128, Color.Red), false, modeLog);
      if (modeGreen)
          this.DrawLinearGraph(graph, histArrayG, max, Color.FromArgb(128, Color.Green), false, modeLog);
      if (modeBlue)
          this.DrawLinearGraph(graph, histArrayB, max, Color.Blue, true, modeLog);
      if (modeRed)
          this.DrawLinearGraph(graph, histArrayR, max, Color.Red, true, modeLog);
      if (modeGreen)
          this.DrawLinearGraph(graph, histArrayG, max, Color.Green, true, modeLog);
      if (modeGray)
          this.DrawLinearGraph(graph, histArrayY, max, Color.Black, true, modeLog);

      this.drawLinearAxes(graph);
    }
  }
}
