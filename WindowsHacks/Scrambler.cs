using ImageProcessing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsAPI;

namespace WindowsHacks
{
    class Scrambler
    {
        static Mask mask;

        public static void Run()
        {
            string windowTitle = getWindowTitle();
            IntPtr hWnd = Window.Get(windowTitle);

            Window.Normalize(hWnd);
            Window.SetFocused(hWnd);
            System.Threading.Thread.Sleep(1000);

            Bitmap screenshot = Desktop.Screenshot();
            screenshot = Tools.Crop(screenshot, new Rectangle(
                Window.GetLocation(hWnd).X,
                Window.GetLocation(hWnd).Y,
                Window.GetSize(hWnd).Width,
                Window.GetSize(hWnd).Height
                ));

            mask = new Mask(hWnd, screenshot);
            Window.Close(hWnd);
            LoadWidth(0, mask.Width-1);
            LoadHeight(0, mask.Height-1);
            Application.Run();
            //Display();
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
        /// Render all the scrambling ahead of time.
        /// </summary>
        private static void Load()
        {
            int numOfPieces = 2;
            Bitmap[] pieces = new Bitmap[numOfPieces];
            int pieceWidth = mask.Width / numOfPieces;
            int pieceHeight = mask.Height;

            // Collect pieces.
            for (int i = 0; i < numOfPieces; i++)
            {
                Bitmap temp = ImageProcessing.Tools.BlankBitmap(pieceWidth, pieceHeight);
                ImageProcessing.Tools.Copy(temp, (Bitmap)mask.Picture.Image, -i * pieceWidth, 0);
                pieces[i] = temp;
            }

            Bitmap tempy = (Bitmap)mask.Picture.Image;
            ImageProcessing.Tools.Copy(tempy, pieces[1], 0, 0);
            ImageProcessing.Tools.Copy(tempy, pieces[0], pieceWidth, 0);

            mask.Picture.Image = tempy;
            mask.Picture.Update();
        }

        private static void LoadWidth(int left, int right)
        {
            if (right - left < 100)
                return;

            System.Threading.Thread.Sleep(50);

            int numOfPieces = 2;
            Bitmap[] pieces = new Bitmap[numOfPieces];
            int pieceWidth = (right - left) / numOfPieces;
            int pieceHeight = mask.Height;

            // Collect pieces.
            for (int i = 0; i < numOfPieces; i++)
            {
                Bitmap temp = ImageProcessing.Tools.BlankBitmap(pieceWidth, pieceHeight);
                ImageProcessing.Tools.Copy(temp, (Bitmap)mask.Picture.Image, -i * pieceWidth - left, 0);
                pieces[i] = temp;
            }

            Bitmap tempy = (Bitmap)mask.Picture.Image;
            ImageProcessing.Tools.Copy(tempy, pieces[1], left, 0);
            ImageProcessing.Tools.Copy(tempy, pieces[0], left + pieceWidth, 0);

            mask.Picture.Image = tempy;
            mask.Picture.Update();

            int mid = (left + right) / 2;
            LoadWidth(left, mid - 1);
            LoadWidth(mid, right);
        }

        private static void LoadHeight(int top, int bottom)
        {
            if (bottom - top < 100)
                return;

            System.Threading.Thread.Sleep(50);

            int numOfPieces = 2;
            Bitmap[] pieces = new Bitmap[numOfPieces];
            int pieceWidth = mask.Width;
            int pieceHeight = (bottom - top) / numOfPieces;

            // Collect pieces.
            for (int i = 0; i < numOfPieces; i++)
            {
                Bitmap temp = ImageProcessing.Tools.BlankBitmap(pieceWidth, pieceHeight);
                ImageProcessing.Tools.Copy(temp, (Bitmap)mask.Picture.Image, 0, -i * pieceHeight - top);
                pieces[i] = temp;
            }

            Bitmap tempy = (Bitmap)mask.Picture.Image;
            ImageProcessing.Tools.Copy(tempy, pieces[1], 0, top);
            ImageProcessing.Tools.Copy(tempy, pieces[0], 0, top + pieceHeight);

            mask.Picture.Image = tempy;
            mask.Picture.Update();

            int mid = (top + bottom) / 2;
            LoadHeight(top, mid - 1);
            LoadHeight(mid, bottom);
        }

    }
}