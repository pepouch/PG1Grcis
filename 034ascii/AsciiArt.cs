using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Raster;
using System;

namespace _034ascii
{
    class Preprocessing
    {
        public static Bitmap Preprocess(IntPtr ip, int w, int h)
        {
            Preprocessing.ip = ip;
            Preprocessing.w = w;
            Preprocessing.h = h;
            Preprocessing.output = new Bitmap(w, h);

            Rectangle rectangle = new Rectangle(0, 0, w, h);
            System.Drawing.Imaging.BitmapData bitmapDataOut = output.LockBits(rectangle
                                                 , System.Drawing.Imaging.ImageLockMode.WriteOnly
                                                 , output.PixelFormat);
            Preprocessing.op = bitmapDataOut.Scan0;

            float[,] matrix1 = new float[3, 3] { { 1, 1, 1 }, { 0, 0f, 0 }, { -1, -1, -1 } };
            float[,] matrix2 = new float[3, 3] { { 1, 0, -1 }, { 1, 0f, -1 }, { 1, 0, -1 } };

            for (int x = 0; x < w - 2; x++)
                for (int y = 0; y < h; y++)
                {
                    int value1 = (int)ComputeConv(ref matrix1, w, h, x, y);
                    int value2 = (int)ComputeConv(ref matrix2, w, h, x, y);
                    float value = Math.Min((float)(Math.Pow(value1, 2) + Math.Pow(value2, 2)), 5000f) / 5000f;

                    int grayLevel = 255 - (int)(value * (float)(255 - getGray(w, x, y)));

                    setGray(w, x, y, grayLevel);
                }

            output.UnlockBits(bitmapDataOut);

            return output;
        }
        private void ComputeHistEqArray()
        {
            histArrayY = new int[256];
            histEq = new int[256];

            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                {
                    int Y = getGray(w, x, y);
                    histArrayY[Y]++;
                }
            int total = w * h;
            int runningSum = 0;
            for (int i = 0; i < 256; i++)
            {
                runningSum += histArrayY[i];
            }
        }

        private static float ComputeConv(ref float[,] matrix, int width, int height, int x, int y)
        {
            float result = 0f;
            for (int i = -1; i < 2; i++)
                for (int j = -1; j < 2; j++)
                {
                    int gray;
                    int gi = i, gj = j;

                    if (x + i < 0)
                        gi = -x;
                    if (y + j < 0)
                        gj = -y;
                    if (x + i >= width)
                        gi = width - x - 1;
                    if (y + j >= height)
                        gj = height - y - 1;

                    gray = getGray(width, x + gi, y + gj);
                    result += matrix[i + 1, j + 1] * gray;
                }
            return Math.Min(Math.Abs(result), 255f);

        }
        private static int getGray(int width, int x, int y)
        {
            unsafe
            {
                byte* ptr = (byte*)ip;
                return Draw.RgbToGray(ptr[4 * width * y + 4 * x + 0], ptr[4 * width * y + 4 * x + 1], ptr[4 * width * y + 4 * x + 2]);
            }
        }
        private static void setGray(int width, int x, int y, int gray)
        {
            unsafe
            {
                byte* ptr = (byte*)op;
                ptr[4 * width * y + 4 * x + 0]
                    = ptr[4 * width * y + 4 * x + 1]
                    = ptr[4 * width * y + 4 * x + 2]
                    = (byte)gray;
            }
        }
        private static Bitmap output;
        private static IntPtr ip, op;
        private static int w, h;
        private static int[] histArrayY;
        private static int[] histEq;
    }
   class Glyph
    {
        public Glyph(char c, int lvl) { character = c; level = lvl; }
        public char character;
        public int level;
    }
  public class AsciiArt
  {
      private static char[] characterSet;
      private static readonly char[] defaultCharacterSet = "#*:. ěščřžýáíé".ToCharArray();
      private static int[] grayLevels;
      private static List<Glyph> glyphs;
      static bool levelsComputed = false;
      
      private static char getCharAtLevel(int level)
      {
          //return characterSet[grayLevels[(int)((float)(AsciiArt.characterSet.Length - 1) * (float)level / 256f)]];
          int i = 0;
          for (; i < characterSet.Length; i++)
          {
              if (level < glyphs[i].level || i == characterSet.Length - 1)
              {
                  if (i > 0 && level < (glyphs[i].level + glyphs[i - 1].level) / 2)
                      i--;
                  break;
              }
          }
          return glyphs[i].character;
      }

      private static void computeLevels()
      {
          grayLevels = new int[AsciiArt.characterSet.Length];
          Font font = AsciiArt.GetFont();
          glyphs = new List<Glyph>();

          for (int lvl = 0; lvl < AsciiArt.characterSet.Length; lvl++)
          {
              string s = AsciiArt.characterSet[lvl].ToString();
              Bitmap bmp = new Bitmap(100, 100);
              Graphics g = Graphics.FromImage(bmp);
              g.Clear(Color.White);
              g.DrawString(s, font, Brushes.Black, 0, 0);
              int width = (int)g.MeasureString("#", font).Width - 2;
              int height = (int)g.MeasureString("#", font).Height;

              int totalGray = 0;
              for (int x = 0; x < width; x++)
              {
                  for (int y = 0; y < height; y++)
                  {
                      Color col = bmp.GetPixel(x, y);
                      int value = Draw.RgbToGray(col.R, col.G, col.B);
                      totalGray += value;
                  }
              }
              int grayLevel = (int)(255f * (float)totalGray / (float)(width * height * 255));
              glyphs.Add(new Glyph(characterSet[lvl], grayLevel));
          }
          glyphs.Sort(delegate(Glyph g1, Glyph g2) { return g1.level.CompareTo(g2.level); });
          levelsComputed = true;
      }

      public static Font GetFont()
    {
      // !!!{{ TODO: if you need a font different from the default one, change this..

      return new Font( "Lucida Console", 6.0f );

      // !!!}}
    }

    /// <summary>
    /// Converts the given input bitmap into an ASCCI art string.
    /// </summary>
    /// <param name="src">Source image.</param>
    /// <param name="width">Required output width in characters.</param>
    /// <param name="height">Required output height in characters.</param>
    /// <param name="param">Textual parameter.</param>
    /// <returns>String (height x width ASCII table)</returns>
    public static string Process ( Bitmap src, int width, int height, string param )
    {
      // !!!{{ TODO: replace this with your own bitmap -> ASCII conversion code

      if ( src == null || width <= 0 || height <= 0 )
        return "";

      float widthBmp  = src.Width;
      float heightBmp = src.Height;

      if (param.Length > 1)
          characterSet = param.ToCharArray();
      else
          characterSet = defaultCharacterSet;
      //if (! levelsComputed)
        AsciiArt.computeLevels();

    // preprocess
      Rectangle rectangle = new Rectangle(0, 0, src.Width, src.Height);
      System.Drawing.Imaging.BitmapData bitmapDataIn = src.LockBits(rectangle
                                                       , System.Drawing.Imaging.ImageLockMode.ReadOnly
                                                       , src.PixelFormat);
      IntPtr ip = bitmapDataIn.Scan0;
      Bitmap img = Preprocessing.Preprocess(ip, (int)widthBmp, (int)heightBmp);
      src.UnlockBits(bitmapDataIn);

      StringBuilder sb = new StringBuilder();

      for ( int y = 0; y < height; y++ )
      {
        float fYBmp = y * heightBmp / height;

        for ( int x = 0; x < width; x++ )
        {
          float fXBmp = x * widthBmp / width;

          Color c = img.GetPixel( (int)fXBmp, (int)fYBmp );

          int luma = c.R; // all channels are the same, img is in grayscale // Draw.RgbToGray(c.R, c.G, c.B);

          // Alternative (luma): Y = 0.2126 * R + 0.7152 * G + 0.0722 * B
          //int luma = (54 * (int)c.R + 183 * (int)c.G + 19 * (int)c.B) >> 8;

          //sb.Append( (luma < 140) ? ((luma < 90)?MAX_LEVEL:MID_LEVEL) : MIN_LEVEL );
          sb.Append( AsciiArt.getCharAtLevel(luma) );
        }

        sb.Append( "\r\n" );
      }

      // !!!}}

      return sb.ToString();
    }
  }
}
