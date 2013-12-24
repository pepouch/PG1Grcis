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

    static double ChaikinCoef = 0.25;
    static int nIterations = 5;
    static float lineWidth = 1f;
    static bool showControlPolygon = true;
    static List<List<Vector2d>> userPoints = new List<List<Vector2d>>();
    static List<Vector2d> demoPoints = new List<Vector2d>();
    static bool drawUserPoints = false;
    public enum mask { CHAIKIN, B3SPLINE, DLG, CUSTOM };
    public static mask avgMask;
    static Random rnd;

    public static void CloseUserPoints()
    {
      if (userPoints.Count > 0 && userPoints[userPoints.Count - 1].Count > 0)
        userPoints[userPoints.Count - 1].Add(userPoints[userPoints.Count - 1][0]);
    }

    public static void ClearUserPoints()
    {
      userPoints.Clear();
      AddUserPath();
    }

    public static void ResetRndSeed()
    {
      rnd = new Random(DateTime.UtcNow.Millisecond);
    }

    public static void omitUserPoints(int n)
    {
      int nPaths = userPoints.Count;
      int nPoints = userPoints[nPaths - 1].Count;
      Vector2d sum = new Vector2d(0, 0);
      int cnt = 0;
      for (int i = 1; i < Math.Min(nPoints, n); i++)
      {
        sum += userPoints[nPaths - 1][nPoints - 1 - i];
        cnt++;
        userPoints[nPaths - 1].RemoveAt(nPoints - 1 - i);
      }
      // userPoints[nPaths - 1].Insert(userPoints[nPaths-1].Count - 2, sum * (1f/(float)cnt));
    }

    public static void AddUserPoint(int x, int y)
    {
      userPoints[userPoints.Count - 1].Add(new Vector2d(x, y));
    }

    public static void AddUserPath()
    {
      userPoints.Add(new List<Vector2d>());
    }

    public static void setDrawUserPoints(bool draw)
    {
      drawUserPoints = draw;
    }

    public static bool getDrawUserPoints()
    {
      return drawUserPoints;
    }

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

    private static List<Vector2d> Subdivide(List<Vector2d> P)
    {
      List<Vector2d> Q = new List<Vector2d>();
      Q.Add(P[0]);
      for (int i = 0; i < P.Count - 1; i++)
      {
        Q.Add(0.5f * P[i] + 0.5f * P[i + 1]);
        Q.Add(P[i + 1]);
      }
      return Q;
    }

    private static List<Vector2d> RefineDLG(List<Vector2d> P)
    {
      List<Vector2d> Q = new List<Vector2d>();
      P = Subdivide(P);
      int Pn = P.Count;
      for (int i = 0; i < Pn - 2; i++)
      {
        if (i % 2 == 0 || i < 2)
          Q.Add(P[i]);
        else
        {
          Q.Add((1f / 18f) * (-2f * P[i - 2] + 6f * P[i - 1] + 10f * P[i] + 6f * P[i + 1] - 2f * P[i + 2]));
        }
      }

      Vector2d P3 = new Vector2d(0, 0);
      if (Pn > 3)
        P3 = P[3];

      if (P[0] == P[Pn - 1])
      {
        Q.Add((1f / 18f) * (-2f * P[Pn - 4] + 6f * P[Pn - 3] + 10f * P[Pn - 2] + 6f * P[Pn - 1] - 2f * P[1]));
        Q.Add(P[Pn - 1]);
        Q[1] = (1f / 18f) * (-2f * P[Pn - 2] + 6f * P[0] + 10f * P[1] + 6f * P[2] - 2f * P3);
      }
      else
      {
        Q.Add(P[Pn - 2]);
        Q.Add(P[Pn - 1]);
      }
      return Q;
    }

    private static List<Vector2d> RefineBSpline3(List<Vector2d> P)
    {
      List<Vector2d> Q = new List<Vector2d>();
      double c = ChaikinCoef;
      Q.Add(P[0]);
      for (int i = 0; i < P.Count - 2; i++)
      {
        Q.Add(0.5f * P[i] + 0.5f * P[i + 1]);
        Q.Add(0.125f * P[i] + 0.75f * P[i + 1] + 0.125f * P[i + 2]);
      }

      if (P[0] == P[P.Count - 1])
      {
        Q.Add(0.5f * P[P.Count - 2] + 0.5f * P[P.Count - 1]);
        Q.Add(0.125f * P[P.Count - 2] + 0.75f * P[0] + 0.125f * P[1]);
        Q[0] = Q[Q.Count - 1];
      }
      else
      {
        Q.Add(P[P.Count - 1]);
      }
      return Q;
    }

    private static List<Vector2d> RefineChaikin(List<Vector2d> P)
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
      if (P.Count < 1)
        return;

      if (levelsCount > 0 && P.Count >= 2)
      {
        if (avgMask == mask.CHAIKIN)
          DrawCurve(output, RefineChaikin(P), col, levelsCount - 1);
        else if (avgMask == mask.B3SPLINE)
          DrawCurve(output, RefineBSpline3(P), col, levelsCount - 1);
        else if (avgMask == mask.DLG)
          DrawCurve(output, RefineDLG(P), col, levelsCount - 1);
        return;
      }

      Graphics gfx = Graphics.FromImage(output);
      gfx.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
      Pen pen = new Pen(col, lineWidth);
      pen.SetLineCap(System.Drawing.Drawing2D.LineCap.Round, System.Drawing.Drawing2D.LineCap.Round, System.Drawing.Drawing2D.DashCap.Round);

      if (P.Count == 1)
      {
        gfx.DrawEllipse(pen, (float)P[0].X - 1f, (float)P[0].Y - 1f, 2f, 2f);
      }
      else
      {
        for (int i = 0; i < P.Count - 1; i++)
          //Draw.Line( output, (int)Math.Round( P[ i ].X ), (int)Math.Round( P[ i ].Y ), (int)Math.Round( P[ i + 1 ].X ), (int)Math.Round( P[ i + 1 ].Y ), col );
          gfx.DrawLine(pen, (float)P[i].X, (float)P[i].Y, (float)P[i + 1].X, (float)P[i + 1].Y);
        gfx.Dispose();
      }
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

      int width = output.Width;
      int height = output.Height;

      if (drawUserPoints)
      {
        for (int i = 0; i < userPoints.Count; i++)
        {
          if (showControlPolygon)
            DrawCurve(output, userPoints[i], Color.Gray);
          DrawCurve(output, userPoints[i], Color.White, nIterations);
        }
      }
      else
      {
        if (showControlPolygon)
          DrawCurve(output, demoPoints, Color.Gray);
        DrawCurve(output, demoPoints, Color.White, nIterations);

        // !!!}}
      }
    }

    public static void GenerateDemoPoints(int width, int height)
    {
      demoPoints.Clear();
      Vector2d point = new Vector2d(width / 2, height / 2);
      demoPoints.Add(point);

      for (int i = 0; i < 100; i++)
      {
        point.X += rnd.Next(Math.Max(-width/10, (int)-point.X), Math.Min(width/10, width - (int)point.X));
        point.Y += rnd.Next(Math.Max(-height/10, (int)-point.Y), Math.Min(height/10, (height - (int)point.Y)));
        demoPoints.Add(point);
      }
    }
  }
}