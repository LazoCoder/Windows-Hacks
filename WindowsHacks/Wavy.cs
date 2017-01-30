using System;
using System.Collections.Generic;
using System.Drawing;
using WindowsAPI;

namespace WindowsHacks
{
    class Wavy
    {

        static Bitmap[] columns;
        static Bitmap[] stretchedCols;
        static KeyValuePair<int, int>[] stretch;
        static Bitmap blankBitmap;
        static Mask mask;
        static Point point;
        static IntPtr hWnd;
        static Size size;

        public static void Run()
        {
            string windowTitle = GetWindowTitle();
            hWnd = Window.Get(windowTitle);
            System.Threading.Thread.Sleep(100);
            blankBitmap = ImageProcessing.Tools.BlankBitmap(Window.GetSize(hWnd).Width, Window.GetSize(hWnd).Height * 2);

            System.Threading.Thread.Sleep(100);

            for (int x = 0; x < blankBitmap.Width; x++)
                for (int y = 0; y < blankBitmap.Height; y++)
                    blankBitmap.SetPixel(x, y, Color.FromArgb(85, 255, 0));
            System.Threading.Thread.Sleep(100);


            mask = new Mask(hWnd, blankBitmap);
            mask.Height *= 2;

            Console.WriteLine(mask.Size);
            Console.WriteLine(mask.Picture.Size);
            Console.WriteLine(mask.Picture.Image.Size);

            mask.AllowTransparency = true;
            mask.TransparencyKey = Color.FromArgb(85, 255, 0);
            mask.Picture.BackColor = Color.FromArgb(85, 255, 0);
            mask.BringToFront();
            System.Threading.Thread.Sleep(1000);
            point = Window.GetLocation(hWnd);
            size = Window.GetSize(hWnd);
            LoadColumns(hWnd);
            Window.Close(hWnd);
            System.Threading.Thread.Sleep(100);


            stretch = new KeyValuePair<int, int>[columns.Length];
            for (int i = 0; i < stretch.Length; i++)
                stretch[i] = new KeyValuePair<int, int>(0, 0);

            for (int i = 0; i < size.Height; i++)
            {
                Bitmap temp = (Bitmap)blankBitmap.Clone();
                for (int j = 0; j < columns.Length; j++)
                {
                    Bitmap slice = new Bitmap(1, size.Height + stretch[j].Value);
                    slice = ImageProcessing.Tools.Resize(columns[j], 1,size.Height + stretch[j].Value);
                    ImageProcessing.Tools.Copy(temp, slice, j, 0);
                }

                mask.Picture.Image = temp;
                mask.Picture.Update();
                for (int k = 0; k < 20; k++)
                    UpdateStrech();
                System.Threading.Thread.Sleep(10);
            }
        }

        static void UpdateStrech()
        {
            for (int i = 0; i < stretch.Length; i++)
            {
                if (stretch[i].Key != 0)
                {
                    int formula = (int)(Math.Sin(stretch[i].Key/50.0) * size.Height / 2);
                    stretch[i] = new KeyValuePair<int, int>(stretch[i].Key + 1, formula);
                }
                else
                {
                    stretch[i] = new KeyValuePair<int, int>(stretch[i].Key + 1, 0);
                    break;
                }
              
            }
        }

        static void LoadColumns(IntPtr hWnd)
        {
            Bitmap screenshot = Desktop.Screenshot();
            int x = Window.GetLocation(hWnd).X;
            int y = Window.GetLocation(hWnd).Y;
            int w = Window.GetSize(hWnd).Width;
            int h = Window.GetSize(hWnd).Height;
            screenshot = ImageProcessing.Tools.Crop(screenshot, new Rectangle(x, y, w, h));
            columns = new Bitmap[screenshot.Width];
            for (int i = 0; i < screenshot.Width; i++)
            {
                columns[i] = ImageProcessing.Tools.BlankBitmap(1, screenshot.Height);
                ImageProcessing.Tools.Copy(columns[i], screenshot, -i, 0);
            }
        }

        static string GetWindowTitle()
        {
            Console.Write("Insert Window Title: ");
            string windowTitle = Console.ReadLine();

            if (!Window.DoesExist(windowTitle))
            {
                Console.WriteLine("Window not found.");
                return GetWindowTitle();
            }

            return windowTitle;
        }
    }

}
