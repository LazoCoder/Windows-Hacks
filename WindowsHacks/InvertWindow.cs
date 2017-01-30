using ImageProcessing;
using System;
using System.Drawing;
using System.Windows.Forms;
using WindowsAPI;

namespace WindowsHacks
{

    /// <summary>
    /// Invert the colors of a specified window.
    /// </summary>
    public static class InvertWindow
    {
        public static void Run()
        {
            string input = getWindowTitle();
            IntPtr hWnd = Window.Get(input);

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

            screenshot = Effect.Invert(screenshot);
            Mask layer = new Mask(hWnd, screenshot);
            Window.Close(hWnd);
            System.Threading.Thread.Sleep(100);
            screenshot.Dispose();
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
