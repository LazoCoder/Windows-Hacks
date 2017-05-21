using System;
using System.Collections.Generic;
using System.Drawing;
using WindowsAPI;
using ImageProcessing;

namespace WindowsHacks
{
    public static class RotateDesktop
    {

        static Bitmap screenshot = Desktop.Screenshot();
        static Bitmap adjusted = new Bitmap(screenshot);
        static Bitmap zoomed;
        static Mask mask;
        static List<Bitmap> listZoom = new List<Bitmap>();
        static List<Bitmap> listRotate = new List<Bitmap>();

        public static void Run()
        {
            Mouse.Move(0, 0);

            screenshot = Desktop.Screenshot();
            mask = new Mask(screenshot);
            System.Threading.Thread.Sleep(100);
            Desktop.HideTaskBar();
            //mask.Hide();

            try
            {
                Render();
            }
            catch (Exception ex)
            {
                CleanUp();
                Console.WriteLine("Unable to perform this function.\n" +
                    "You might not have enough RAM available.\n" +
                    "Either upgrade your RAM or decrease your screen resolution.\n" +
                    "Press any key to continue...");
                Console.ReadKey();
                return;
            }

            Play();
            CleanUp();

            Mouse.Move(15, Desktop.GetWidth() - 15);
        }

        private static void Render()
        {
            LoadZoomIn();
            zoomed = new Bitmap(adjusted);
            LoadRotate();
        }

        private static void Play()
        {
            DisplayList(listZoom);

            for (int i = 0; i < 7; i++)
            {
                DisplayList(listRotate);
            }

            LoadZoomOut();
            DisplayList(listZoom);
        }

        private static void CleanUp()
        {
            Desktop.ShowTaskBar();
            DisposeList(listZoom);
            DisposeList(listRotate);
            mask.Dispose();
        }

        /// <summary>
        /// method to rotate an image either clockwise or counter-clockwise
        /// </summary>
        /// <param name="img">the image to be rotated</param>
        /// <param name="rotationAngle">the angle (in degrees).
        /// NOTE: 
        /// Positive values will rotate clockwise
        /// negative values will rotate counter-clockwise
        /// </param>
        /// <returns></returns>
        private static Image RotateImage(Image img, float rotationAngle)
        {
            //create an empty Bitmap image
            Bitmap bmp = new Bitmap(img.Width, img.Height);

            //turn the Bitmap into a Graphics object
            Graphics gfx = Graphics.FromImage(bmp);

            //now we set the rotation point to the center of our image
            gfx.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);

            //now rotate the image
            gfx.RotateTransform(rotationAngle);

            gfx.TranslateTransform(-(float)bmp.Width / 2, -(float)bmp.Height / 2);

            //set the InterpolationMode to HighQualityBicubic so to ensure a high
            //quality image once it is transformed to the specified size
            //gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;

            //now draw our new image onto the graphics object
            gfx.DrawImage(img, new Point(0, 0));

            //dispose of our Graphics object
            gfx.Dispose();

            //return the image
            return bmp;
        }

        private static void LoadZoomIn()
        {
            for (double i = 1; i < 2; i += 0.1)
            {
                adjusted.Dispose();
                adjusted = Tools.Resize(
                    screenshot,
                    (int)(screenshot.Width * i),
                    (int)(screenshot.Height * i)
                );
                listZoom.Add(new Bitmap(adjusted));
            }
        }

        private static void LoadRotate()
        {
            for (int i = 0; i < 360; i += 20)
            {
                adjusted.Dispose();
                adjusted = (Bitmap)RotateImage(zoomed, i);
                listRotate.Add(new Bitmap(adjusted));
            }
        }

        private static void LoadZoomOut()
        {
            listZoom.Reverse();
        }

        private static void DisplayList(List<Bitmap> list)
        {
            foreach (Bitmap bmp in list)
            {
                mask.Picture.Image = bmp;
                mask.Picture.Update();
                System.Threading.Thread.Sleep(50);
            }
        }

        private static void DisposeList(List<Bitmap> list)
        {
            foreach (Bitmap bmp in list) bmp.Dispose();
            list.Clear();
        }

    }
}
