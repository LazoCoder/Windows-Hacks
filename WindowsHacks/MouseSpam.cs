using System;
using System.Drawing;
using WindowsAPI;

namespace WindowsHacks
{
    class MouseSpam
    {

        static int mouses = 1000;
        static Point[] locations;
        static Point[] velocities;
        static Bitmap blankBitmap;
        static Bitmap mouse;
        static Mask mask;
        static bool extreme = false;

        public static void Run()
        {
            blankBitmap = ImageProcessing.Tools.BlankBitmap(Desktop.GetWidth(), Desktop.GetHeight());
            mask = new Mask(blankBitmap);
            mask.AllowTransparency = true;
            mask.TransparencyKey = Color.White;
            mask.Picture.BackColor = Color.Transparent;
            Window.EnableMouseTransparency(mask.Handle);
            mouse = (Bitmap)Properties.Resources.mouse.Clone();
            mouse.MakeTransparent(Color.Transparent);

            SpawnMouses();
            InitializeVelocities();
            for (int i = 0; i < 10000; i++)
            {
                mask.TopMost = true;
                MoveMouses();
                DrawMouses();
                System.Threading.Thread.Sleep(10);
            }

            mask.Dispose();
        }

        public static void RunExtreme()
        {
            extreme = true;
            mouses = 500;
            Run();
        }

        private static void SpawnMouses()
        {
            locations = new Point[mouses];
            Random r = new Random();
            for (int i = 0; i < mouses; i++)
            {
                locations[i] = new Point(r.Next(Desktop.GetWidth()), r.Next(Desktop.GetHeight()));
            }
        }

        private static void InitializeVelocities()
        {
            velocities = new Point[mouses];
            Random r = new Random();
            for (int i = 0; i < mouses; i++)
            {
                int x = r.Next(5, 10);
                int y = r.Next(5, 10);
                if (r.Next(2) == 0)
                    x *= -1;
                if (r.Next(2) == 0)
                    y *= -1;
                velocities[i] = new Point(x, y);
            }
        }

        private static void MoveMouses()
        {
            Random r = new Random();
            for (int i = 0; i < locations.Length; i++)
            {
                locations[i].X += velocities[i].X;
                locations[i].Y += velocities[i].Y;
                if (locations[i].X > Desktop.GetWidth() || locations[i].X < 0)
                    locations[i] = new Point(r.Next(Desktop.GetWidth()), r.Next(Desktop.GetHeight()));
                if (locations[i].Y > Desktop.GetHeight() || locations[i].Y < 0)
                    locations[i] = new Point(r.Next(Desktop.GetWidth()), r.Next(Desktop.GetHeight()));

            }
        }

        private static void DrawMouses()
        {
            Bitmap temp;
            if (extreme)
                temp = blankBitmap;
            else
                temp = (Bitmap)blankBitmap.Clone();
            foreach (Point p in locations)
            {
                ImageProcessing.Tools.Copy(temp, mouse, p.X, p.Y);
            }
            mask.Picture.Image = temp;
            mask.Picture.Update();
        }
    }
}
