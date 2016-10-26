using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WindowsAPI;

namespace WindowsHacks
{

    /// <summary>
    /// Creates ever expanding squares at the location of the mouse.
    /// </summary>
    public static class RippleEffect
    {
        public static void Run()
        {
            Bitmap bmp = ImageProcessing.Tools.BlankBitmap(Desktop.GetWidth(), Desktop.GetHeight());
            Mask mask = new Mask(bmp);

            mask.TransparencyKey = Color.White;
            mask.AllowTransparency = true;
            mask.Show();
            Window.EnableMouseTransparency(mask.Handle);

            Ripples r = new Ripples(mask.Picture);

            Console.WriteLine("Press enter");
            Console.ReadLine();

            while (true)
            {
                System.Threading.Thread.Sleep(10);
                Point pt = Cursor.Position;
                r.AddRipple(pt.X, pt.Y);
                mask.Picture.Update();
            }

        }

    }

    internal class Ripples
    {
        private List<Ripple> list;
        private PictureBox picBox = new PictureBox();
        private Bitmap bmp;

        public Ripples(PictureBox picBox)
        {
            list = new List<Ripple>();
            this.picBox = picBox;
            bmp = BlankBitmap(picBox.Width, picBox.Height);
        }

        public void AddRipple(int x, int y)
        {
            list.Add(new Ripple(x, y));
            IncrementSize();
        }

        private void IncrementSize()
        {
            Bitmap doubleBuffer = BlankBitmap(bmp.Width, bmp.Height);
            Pen pen = new Pen(Color.DarkRed);

            list.RemoveAll(x => x.GetDiameter() > 1000 - 10);

            using (Graphics g = Graphics.FromImage(doubleBuffer))
            {
                foreach (Ripple r in list)
                {
                    r.IncrementSize();
                    pen.Color = r.GetColor();
                    g.DrawRectangle(pen, r.GetRectangle());
                }
            }

            bmp = doubleBuffer;
            picBox.Image = bmp;
            picBox.Update();

        }

        private Bitmap BlankBitmap(int x, int y)
        {
            Bitmap bmp = new Bitmap(x, y);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                Rectangle r = new Rectangle(0, 0, x, y);
                g.FillRectangle(Brushes.White, r);
            }

            return bmp;
        }

        private class Ripple
        {
            private Point pt;
            private double diameter;

            public Ripple(int x, int y)
            {
                pt = new Point(x, y);
                diameter = 0.0;
            }

            public void IncrementSize()
            {
                diameter += 10.0;
            }

            public Rectangle GetRectangle()
            {
                int x = pt.X - (int)(diameter / 2);
                int y = pt.Y - (int)(diameter / 2);

                return new Rectangle(x, y, (int)diameter, (int)diameter);
            }

            public Color GetColor()
            {
                double ratio = diameter / 1000;
                int color = (int)Math.Floor(255 * ratio);
                return Color.FromArgb(0, 255-color, 255-color);
            }

            public double GetDiameter()
            {
                return diameter;
            }

        }

    }
}
