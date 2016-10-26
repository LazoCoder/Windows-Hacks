using ImageProcessing;
using System;
using System.Drawing;
using System.Windows.Forms;
using WindowsAPI;

namespace WindowsHacks
{

    /// <summary>
    /// Blurs the specified window.
    /// </summary>
    public static class MeltWindow
    {
        public static void Run()
        {
            string windowTitle = getWindowTitle();
            IntPtr hWnd = Window.Get(windowTitle);

            Window.SetFocused(hWnd);
            System.Threading.Thread.Sleep(200);

            Bitmap screenshot = Desktop.Screenshot();
            screenshot = Tools.Crop(screenshot, new Rectangle(
                Window.GetLocation(hWnd).X,
                Window.GetLocation(hWnd).Y,
                Window.GetSize(hWnd).Width,
                Window.GetSize(hWnd).Height
                ));

            Mask layer = new Mask(hWnd, screenshot);

            // The actual blurring.
            for (int i = 0; i < 10; i++)
            {
                screenshot = Effect.Blur(screenshot);
                System.Threading.Thread.Sleep(100);
                layer.Picture.Image = screenshot;
                layer.Picture.Update();
            }

            Window.Close(hWnd);
            System.Threading.Thread.Sleep(100);
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
