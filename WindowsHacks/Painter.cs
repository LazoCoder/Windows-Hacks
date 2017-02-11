using ImageProcessing;
using System;
using System.Drawing;
using WindowsAPI;

namespace WindowsHacks
{
    class Painter
    {

        private const int StartX = 15;
        private const int StartY = 150;
        private const int PixelSize = 1;
        private const int MaxWidth = 168;
        private const int MaxHeight = 67;
        private static IntPtr hWnd;

        /// <summary>
        /// Draw a bitmap manually into Microsoft Paint.
        /// </summary>
        public static void Run()
        {

            if (!Window.DoesExist("Untitled - Paint"))
            {
                Console.WriteLine("Microsoft Paint Window with the name \"Untitled - Paint\" not found.");
                Console.ReadLine();
                return;
            }

            hWnd = Window.Get("Untitled - Paint");
            Bitmap bitmap = Properties.Resources.mona;

            Bitmap bmp = Effect.Threshold(bitmap, 150);

            Color pixel;
            bool pencilDown = false;

            Window.SetFocused(hWnd);
            //Window.Move(hWnd, 0, 0);

            System.Threading.Thread.Sleep(100);

            // The actual drawing.
            for (int y = 0; y < bmp.Height; y++)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    pixel = bmp.GetPixel(x, y);

                    if (Window.IsFocused(hWnd) == false) return;

                    if (pixel.ToArgb() == Color.Black.ToArgb() && pencilDown == false)
                    {
                        System.Threading.Thread.Sleep(10);
                        MouseDown(x, y);
                        pencilDown = true;
                    }
                    else if (pixel.ToArgb() == Color.White.ToArgb() && pencilDown == true)
                    {
                        System.Threading.Thread.Sleep(10);
                        MouseUp(x - 1, y);
                        pencilDown = false;
                    }
                }

                if (pencilDown)
                {
                    MouseUp(bmp.Width, y);
                    pencilDown = false;
                }

            }
        }

        private static void SelectBlackPencil()
        {
            Mouse.LeftClick(Window.GetLocation(hWnd).X + 763, Window.GetLocation(hWnd).Y + 60);
        }

        private static void SelectWhitePencil()
        {
            Mouse.LeftClick(Window.GetLocation(hWnd).X + 763, Window.GetLocation(hWnd).Y + 80);
        }

        private static void MouseDown(int x, int y)
        {
            Mouse.LeftDown(Window.GetLocation(hWnd).X + StartX + (PixelSize * x), Window.GetLocation(hWnd).Y + StartY + (PixelSize * y));
        }

        private static void MouseUp(int x, int y)
        {
            Mouse.LeftUp(Window.GetLocation(hWnd).X + StartX + (PixelSize * x), Window.GetLocation(hWnd).Y + StartY + (PixelSize * y));
        }

        private static void MouseUp()
        {
            Mouse.LeftUp();
        }

    }
}
