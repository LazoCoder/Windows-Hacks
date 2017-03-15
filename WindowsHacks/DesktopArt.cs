using ImageProcessing;
using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using WindowsAPI;

namespace WindowsHacks
{

    /// <summary>
    /// Draws a thresholded image straight to the desktop, on top of all the windows.
    /// </summary>
    public static class DesktopArt
    {

        static Bitmap bmp;
        static Bitmap temp;
        static Mask mask;
        static int[] array;

        public enum Sample {
            Shotgun,
            FamilyGuy,
            Tree,
            Mountains,
            Desert
        }

        public static void Run()
        {
            GetInput();
            Initialize();
            DoGreyScale();
            Thread.Sleep(2000);
            mask.Dispose();
        }

        public static void Run(Sample s)
        {
            Thread.Sleep(2000);

            if (s == Sample.Desert) bmp = new Bitmap(Properties.Resources.desert);
            else if (s == Sample.FamilyGuy) bmp = new Bitmap(Properties.Resources.fam);
            else if (s == Sample.Mountains) bmp = new Bitmap(Properties.Resources.mountains);
            else if (s == Sample.Shotgun) bmp = new Bitmap(Properties.Resources.shotgun);
            else if (s == Sample.Tree) bmp = new Bitmap(Properties.Resources.tree);

            Initialize();
            DoGreyScale();
            Thread.Sleep(2000);
        }

        public static void RunMovie()
        {
            int[] array = new int[3];
            array[0] = 147;
            array[1] = 147;
            array[2] = 255;

            IntPtr hWnd = OtherFunctions.GetFocusedWindow();

            Bitmap bmp = Window.Screenshot(hWnd);

            Mask mask = new Mask(hWnd, bmp);
            Window.EnableMouseTransparency(mask.Handle);

            while (true)
            {
                mask.Picture.Image = Effect.Threshold(Window.Screenshot(hWnd), array);
                mask.Update();
                Thread.Sleep(1);
            }
        }
        
        private static void GetInput()
        {
            bool validPath = false;
            string path = null;
            while (!validPath)
            {
                Console.Write("Drag in a bitmap or type in the full path name: ");
                path = Console.ReadLine();
                validPath = File.Exists(path);
                if (!validPath) {
                    Console.Write("Invalid file path. Try again.\n");
                }
            }
            
            bmp = new Bitmap(@path);
        }

        private static void Initialize()
        {
            mask = new Mask();
            mask.TransparencyKey = Color.White;
            mask.TopMost = true;
            mask.Size = new Size(Desktop.GetWidth(), Desktop.GetHeight());
            mask.Picture.SizeMode = PictureBoxSizeMode.CenterImage;
            mask.Visible = true;
            mask.Location = new Point(0, 0);

            array = new int[3];
        }

        private static void BeginBlack()
        {
            for (int i = 0; i <= 51; i++)
            {
                array[0] = i;
                array[1] = i;
                array[2] = i;
                temp = (Bitmap)bmp.Clone();
                mask.Picture.Image = Effect.Threshold(temp, array);
                mask.Picture.Update();
                Thread.Sleep(1);
                temp.Dispose();
            }
        }

        private static void BeginDarkGrey()
        {
            for (int i = 51; i <= 102; i++)
            {
                array[0] = 51;
                array[1] = i;
                array[2] = i;
                temp = (Bitmap)bmp.Clone();
                mask.Picture.Image = Effect.Threshold(temp, array);
                mask.Picture.Update();
                Thread.Sleep(1);
                temp.Dispose();
            }
        }

        private static void BeginLightGrey()
        {
            for (int i = 102; i <= 255; i++)
            {
                array[0] = 51;
                array[1] = 102;
                array[2] = i;
                temp = (Bitmap)bmp.Clone();
                mask.Picture.Image = Effect.Threshold(temp, array);
                mask.Picture.Update();
                Thread.Sleep(1);
                temp.Dispose();
            }
        }

        private static void ContinueDarkGrey()
        {
            for (int i = 102; i <= 255; i++)
            {
                array[0] = 51;
                array[1] = i;
                array[2] = 255;
                temp = (Bitmap)bmp.Clone();
                mask.Picture.Image = Effect.Threshold(temp, array);
                mask.Picture.Update();
                Thread.Sleep(1);
                temp.Dispose();
            }
        }

        private static void ContinueBlack()
        {
            for (int i = 51; i <= 255; i++)
            {
                array[0] = i;
                array[1] = 255;
                array[2] = 255;
                temp = (Bitmap)bmp.Clone();

                mask.Picture.Image = Effect.Threshold(temp, array);
                mask.Picture.Update();
                Thread.Sleep(1);
                temp.Dispose();
            }
        }

        private static void DoGreyScale()
        {
            BeginBlack();
            BeginDarkGrey();
            BeginLightGrey();
            ContinueDarkGrey();
            ContinueBlack();
        }


    }
}
