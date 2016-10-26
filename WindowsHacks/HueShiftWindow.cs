using ImageProcessing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using WindowsAPI;

namespace WindowsHacks
{

    /// <summary>
    /// Shifts the hue of a specified window continously for a period of time.
    /// </summary>
    public static class HueShiftWindow
    {
        static Bitmap bmp;
        static Bitmap temp;
        static Mask mask;

        // Renders the hue shifting before displaying it to the screen and stores it here.
        static List<Bitmap> list;

        public static void Run()
        {
            list = new List<Bitmap>();

            string windowTitle = getWindowTitle();
            IntPtr hWnd = Window.Get(windowTitle);

            Window.SetFocused(hWnd);
            System.Threading.Thread.Sleep(200);

            bmp = Desktop.Screenshot();
            bmp = Tools.Crop(bmp, new Rectangle(
                Window.GetLocation(hWnd).X,
                Window.GetLocation(hWnd).Y,
                Window.GetSize(hWnd).Width,
                Window.GetSize(hWnd).Height
                ));

            temp = new Bitmap(bmp);
            mask = new Mask(hWnd, bmp);

            Load();
            Display();
        }

        /// <summary>
        /// Allow the user to select a window.
        /// </summary>
        /// <returns>True if a window with the specified title is found.</returns>
        private static string getWindowTitle()
        {
            Console.Write("Insert Window Title: ");
            string windowTitle = Console.ReadLine();

            if (!Window.DoesExist(windowTitle))
            {
                Console.WriteLine("Window not found.");
                return getWindowTitle();
            }

            return windowTitle;
        }

        /// <summary>
        /// Shift the hue of the pixels in an image.
        /// </summary>
        /// <param name="bitmap">The bitmap to perform hue shifting on.</param>
        /// <param name="degrees">The amount to hue shift.</param>
        /// <returns>The hue shifted image.</returns>
        private static Bitmap Change(Bitmap bitmap, int degrees)
        {
            Color colorRGB;
            HSV colorHSV;
            Bitmap bmp = new Bitmap(bitmap);

            unsafe
            {
                BitmapData bitmapData = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);

                int bytesPerPixel = System.Drawing.Bitmap.GetPixelFormatSize(bmp.PixelFormat) / 8;
                int heightInPixels = bitmapData.Height;
                int widthInBytes = bitmapData.Width * bytesPerPixel;
                byte* PtrFirstPixel = (byte*)bitmapData.Scan0;

                for (int y = 0; y < heightInPixels; y++)
                {
                    byte* currentLine = PtrFirstPixel + (y * bitmapData.Stride);
                    for (int x = 0; x < widthInBytes; x = x + bytesPerPixel)
                    {
                        int oldBlue = currentLine[x];
                        int oldGreen = currentLine[x + 1];
                        int oldRed = currentLine[x + 2];

                        colorRGB = Color.FromArgb(oldBlue, oldGreen, oldRed);
                        colorHSV = colorRGB.ToHSV();
                        colorHSV.SetHue(colorHSV.GetHue() + degrees);
                        colorRGB = colorHSV.ToRGB();


                        currentLine[x] = (byte)colorRGB.B;
                        currentLine[x + 1] = (byte)colorRGB.G;
                        currentLine[x + 2] = (byte)colorRGB.R;
                    }
                }
                bmp.UnlockBits(bitmapData);
            }
            return bmp;
        }

        /// <summary>
        /// Render all the hue shifting ahead of time.
        /// </summary>
        private static void Load()
        {
            double percentComplete = 0.0;

            for (int i = 0; i <= 360; i += 10)
            {
                percentComplete = i / 360.0 * 100.0;
                string percent = Math.Round(percentComplete).ToString() + "%";
                Console.Title = percent;
                list.Add(Change(bmp, i));
            }
        }

        /// <summary>
        /// Display the pre-rendered images with the shifted hues.
        /// </summary>
        private static void Display()
        {
            for (int i = 0; i < 5; i++)
            {
                foreach (Bitmap bitmap in list)
                {
                    mask.Picture.Image = bitmap;
                    mask.Picture.Update();
                    System.Threading.Thread.Sleep(100);
                }
            }
        }
    }
}
