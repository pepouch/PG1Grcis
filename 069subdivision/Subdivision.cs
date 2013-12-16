using System;
using System.Drawing;
using OpenTK;
using Raster;
using System.Collections.Generic;

namespace _069subdivision
{
  public class Subdivision
  {
    /// <summary>
    /// Separator for string parameter.
    /// </summary>
    static readonly char COMMA = ',';

    static double ChaikinCoef = 0.25;
    static int nIterations = 5;
    static float lineWidth = 1f;
    static bool showControlPolygon = true;

    public static void SetParam(string name, int value)
    {
      if (name.Equals("niter", StringComparison.InvariantCultureIgnoreCase))
        nIterations = value;
    }

    public static void SetParam(string name, double value)
    {
      if (name.Equals("coef", StringComparison.InvariantCultureIgnoreCase))
        ChaikinCoef = value;
      if (name.Equals("width", StringComparison.InvariantCultureIgnoreCase))
        lineWidth = (float)value;
    }

    public static void SetParam(string name, bool value)
    {
      if (name.Equals("polygon", StringComparison.InvariantCultureIgnoreCase))
        showControlPolygon = value;
    }

    private static List<Vector2d> Refine(List<Vector2d> P)
    {
      List<Vector2d> Q = new List<Vector2d>();
      for (int i = 0; i < P.Count - 1; i++)
      {
        Q.Add((1 - ChaikinCoef) * P[i] + ChaikinCoef * P[i + 1]);
        Q.Add(ChaikinCoef * P[i] + (1 - ChaikinCoef) * P[i + 1]);
      }
      if (P[0] == P[P.Count - 1])
      {
        Q.Add((1 - ChaikinCoef) * P[0] + ChaikinCoef * P[1]);
      }
      return Q;
    }

    /// <summary>
    /// Draw one subdivision curve.
    /// </summary>
    /// <param name="output">Target bitmap.</param>
    /// <param name="P">Array of control points.</param>
    /// <param name="col">Drawing color.</param>
    public static void DrawCurve(Bitmap output, List<Vector2d> P, Color col, int levelsCount = 0)
    {
      // !!!{{ TODO: write your own subdivision curve rasterization code here
      if (levelsCount > 0)
      {
        DrawCurve(output, Refine(P), col, levelsCount - 1);
        return;
      }

      Graphics gfx = Graphics.FromImage(output);
      gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
      Pen pen = new Pen(col, lineWidth);

      for (int i = 0; i < P.Count - 1; i++)
        //Draw.Line( output, (int)Math.Round( P[ i ].X ), (int)Math.Round( P[ i ].Y ), (int)Math.Round( P[ i + 1 ].X ), (int)Math.Round( P[ i + 1 ].Y ), col );
        gfx.DrawLine(pen, (float)P[i].X, (float)P[i].Y, (float)P[i + 1].X, (float)P[i + 1].Y);
      gfx.Dispose();
      // !!!}}
    }

    /// <summary>
    /// Draw the test image into pre-allocated bitmap.
    /// </summary>
    /// <param name="output">Bitmap to fill.</param>
    /// <param name="param">Optional parameter string.</param>
    public static void TestImage(Bitmap output, string param)
    {
      // !!!{{ TODO: write your own test-image drawing here
      try
      {
        SetParam("coef", Convert.ToDouble(param));
      }
      catch
      {
        ;
      }

      int width = output.Width;
      int height = output.Height;

      List<Vector2d> P = new List<Vector2d>();
      P.Add(new Vector2d(width * 0.05, height * 0.06));
      P.Add(new Vector2d(width * 0.45, height * 0.16));
      P.Add(new Vector2d(width * 0.37, height * 0.86));
      //P.Add( new Vector2d( width * 0.07, height * 0.86 ) );
      P.Add(new Vector2d(width * 0.05, height * 0.06));

      if (showControlPolygon)
        DrawCurve(output, P, Color.Gray);
      DrawCurve(output, P, Color.White, nIterations);

      P.Clear();
      P.Add(new Vector2d(width * 0.55, height * 0.76));
      P.Add(new Vector2d(width * 0.55, height * 0.76));
      P.Add(new Vector2d(width * 0.55, height * 0.08));
      P.Add(new Vector2d(width * 0.75, height * 0.42));
      P.Add(new Vector2d(width * 0.95, height * 0.08));
      P.Add(new Vector2d(width * 0.95, height * 0.76));
      P.Add(new Vector2d(width * 0.95, height * 0.76));

      if (showControlPolygon)
        DrawCurve(output, P, Color.DarkOrange);
      DrawCurve(output, P, Color.Yellow, nIterations);

      // !!!}}
    }
  }
}
