using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using WindowsAPI;
using ImageProcessing;

namespace WindowsHacks
{

    /// <summary>
    /// Detects motion in any Window that is currently open.
    /// </summary>
    public static class MotionDetector
    {

        // Keep the last few screenshots of the Window to produce the fade out effect.
        static List<Bitmap> listOfFrames = new List<Bitmap>();

        // The output window.
        static ViewerForm v;

        // For drawing the output to the screen.
        static Bitmap canvas = new Bitmap(1, 1);

        // The pointer to the window.
        static IntPtr hWnd;
        static string windowName;

        // The initial two frames and their difference.
        static Bitmap snapshot1;
        static Bitmap snapshot2;
        static Bitmap diff;

        // Width and height of the display window.
        static int width;
        static int height;
        static double proportion;

        /// <summary>
        /// High Quality motion detection on a big Window can slow things down.
        /// </summary>
        public enum Quality
        {
            Low = 0,
            Medium = 1,
            High = 2
        }

        /// <summary>
        /// Begin motion detection.
        /// </summary>
        /// <param name="frames">The number of recent frames to keep track of.</param>
        /// <param name="quality">The quality of the display.</param>
        public static void Run(int frames, Quality quality)
        {
            GetUserInput();
            SetQuality(quality);
            LoadForm();
            MotionDetectorLoop(frames);
        }

        /// <summary>
        /// Set the quality of the display for the motion detector.
        /// </summary>
        /// <param name="quality">The quality to set to.</param>
        private static void SetQuality(Quality quality)
        {
            if (quality == Quality.Low)
                proportion = 0.2;
            else if (quality == Quality.Medium)
                proportion = 0.5;
            else if (quality == Quality.High)
                proportion = 1;
        }

        /// <summary>
        /// Loads the display window.
        /// </summary>
        private static void LoadForm()
        {
            v = new ViewerForm();

            SetSizes();

            v.Show();
            v.TopMost = true;
            v.Location = new Point(100, 100);
        }

        /// <summary>
        /// Set the size of the display window.
        /// </summary>
        private static void SetSizes()
        {
            width = Window.GetSize(hWnd).Width;
            height = Window.GetSize(hWnd).Height;
            canvas = BlankBitmap((int)(width * proportion), (int)(height * proportion));
            v.Size = new Size(width + 20, height + 39);
        }

        /// <summary>
        /// Contains the main loop for the motion detector.
        /// </summary>
        /// <param name="frames">The number of recent screenshots of the window to keep track of.</param>
        private static void MotionDetectorLoop(int frames)
        {
            snapshot1 = Tools.Resize(Window.Screenshot(hWnd), (int)(width * proportion), (int)(height * proportion));
            snapshot2 = Tools.Resize(Window.Screenshot(hWnd), (int)(width * proportion), (int)(height * proportion));

            while (Window.DoesExist(windowName))
            {
                if (!TakeSnapshot()) return;

                // Detect motion, add to list, then draw list.
                diff = BasicVision.PrintDifference(snapshot1, snapshot2, false);
                listOfFrames.Add(new Bitmap(diff));
                DrawList(frames);

                if (!UpdatePictureBox()) return;

            }
        }


        private static bool TakeSnapshot()
        {
            if (!Window.DoesExist(windowName))
                return false;

            snapshot1.Dispose();
            snapshot1 = new Bitmap(snapshot2);
            snapshot2.Dispose();
            snapshot2 = Tools.Resize(Window.Screenshot(hWnd), (int)(width * proportion), (int)(height * proportion));

            return true;
        }

        /// <summary>
        /// Allow the user to specify which window to perform the motion detection on.
        /// </summary>
        private static void GetUserInput()
        {
            Console.Write("Insert Window Title: ");
            windowName = Console.ReadLine();

            if (!Window.DoesExist(windowName))
            {
                Console.WriteLine("Window not found.");
                GetUserInput();
            }
            else hWnd = Window.Get(windowName);

        }

        private static void Wait(int ms)
        {
            System.Threading.Thread.Sleep(ms);
        }

        /// <summary>
        /// Update the display.
        /// </summary>
        private static bool UpdatePictureBox()
        {
            if (!Window.DoesExist(windowName))
                return false;

            diff.Dispose();

            int widthNew = Window.GetSize(hWnd).Width;
            int heightNew = Window.GetSize(hWnd).Height;
            Size sizeNew = new Size(widthNew, heightNew);

            v.PictureBox.Image = Tools.Resize(canvas, widthNew, heightNew);
            if (v.Size != sizeNew) v.Size = new Size(widthNew + 20, heightNew + 39);
            v.PictureBox.Update();

            return true;
        }

        private static void DrawList(int maxListSize)
        {
            int i = 0;

            if (listOfFrames.Count > maxListSize)
            {
                listOfFrames[0].Dispose();
                listOfFrames.RemoveAt(0);
            }

            foreach (Bitmap bmp in listOfFrames)
            {
                DrawDifference(bmp, Color.FromArgb(i, i, i));
                i += 255 / (maxListSize - 1);
            }

        }

        /// <summary>
        /// Draws all the white pixels to the canvas.
        /// </summary>
        /// <param name="bmp">The bitmap to search for white pixels.</param>
        /// <param name="color">The color to draw the white pixels as on the canvas.</param>
        private static void DrawDifference(Bitmap bmp, Color color)
        {
            // Bitmaps should be of equal size. 
            if (bmp.Size != canvas.Size) throw new Exception("Images are not of equal sizes.");

            unsafe
            {
                // Lock the bitmaps into memory.
                BitmapData bitmapData1 = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);
                BitmapData bitmapData2 = canvas.LockBits(new Rectangle(0, 0, canvas.Width, canvas.Height), ImageLockMode.ReadWrite, canvas.PixelFormat);

                int bytesPerPixel = Bitmap.GetPixelFormatSize(bmp.PixelFormat) / 8;
                int bytesPerPixel2 = Bitmap.GetPixelFormatSize(canvas.PixelFormat) / 8;

                int heightInPixels = bitmapData1.Height;
                int widthInBytes = bitmapData1.Width * bytesPerPixel;

                byte* PtrFirstPixel1 = (byte*)bitmapData1.Scan0;
                byte* PtrFirstPixel2 = (byte*)bitmapData2.Scan0;

                // For each row in both bitmaps.
                Parallel.For(0, heightInPixels, y =>
                {
                    byte* currentLine1 = PtrFirstPixel1 + (y * bitmapData1.Stride);
                    byte* currentLine2 = PtrFirstPixel2 + (y * bitmapData2.Stride);

                    // For each pixel in that row.
                    for (int x = 0; x < widthInBytes; x = x + bytesPerPixel)
                    {

                        // RGB values of the pixel in the first bitmap.
                        int bmp1Blue = currentLine1[x];
                        int bmp1Green = currentLine1[x + 1];
                        int bmp1Red = currentLine1[x + 2];

                        // If both pixels match, draw that pixel in the third bitmap.
                        if (bmp1Blue == 255 && bmp1Green == 255 && bmp1Red == 255)
                        {
                            currentLine2[x] = (byte)color.B;
                            currentLine2[x + 1] = (byte)color.G;
                            currentLine2[x + 2] = (byte)color.R;
                        }

                    }
                });
                bmp.UnlockBits(bitmapData1);
                canvas.UnlockBits(bitmapData2);
            }
        }

        /// <summary>
        /// Helper method that creates a blank black bitmap.
        /// </summary>
        /// <param name="width">The width of the bitmap.</param>
        /// <param name="height">The height of the bitmap.</param>
        /// <returns></returns>
        private static Bitmap BlankBitmap(int width, int height)
        {
            Bitmap bitmap = new Bitmap(width, height, Window.Screenshot(hWnd).PixelFormat);
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                Rectangle imageSize = new Rectangle(0, 0, width, height);
                g.FillRectangle(Brushes.Black, imageSize);
            }
            return bitmap;
        }

        /// <summary>
        /// Helper method that makes a bitmap black.
        /// </summary>
        /// <param name="bmp">The bitmap to blank out.</param>
        private static void MakeBlank(Bitmap bmp)
        {
            using (Graphics g = Graphics.FromImage(bmp))
            {
                Rectangle imageSize = new Rectangle(0, 0, bmp.Width, bmp.Height);
                g.FillRectangle(Brushes.Black, imageSize);
            }
        }
    }
}
