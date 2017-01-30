using System;
using System.Drawing;
using WindowsAPI;
using ImageProcessing;
using System.Windows.Forms;

namespace WindowsHacks
{

    /// <summary>
    /// Shrinks the specified window.
    /// </summary>
    public static class ShrinkWindow
    {
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

            Mask layer = new Mask(hWnd, screenshot);

            Window.Close(hWnd);

            System.Threading.Thread.Sleep(100);

            Bitmap resized = new Bitmap(screenshot);
            for (double i = 1; i > 0.1; i -= 0.02)
            {
                resized.Dispose();
                resized = Tools.Resize(
                    screenshot,
                    (int)(screenshot.Width * i),
                    (int)(screenshot.Height * i)
                    );
                layer.Picture.Image = resized;
                layer.Size = resized.Size;
                layer.Picture.Update();
                layer.Location = new Point(
                    layer.Location.X + (int)(screenshot.Width * 0.02 / 2),
                    layer.Location.Y + (int)(screenshot.Height * 0.02 / 2)
                    );
                System.Threading.Thread.Sleep(10);
            }

            Application.Run();
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
    }
}