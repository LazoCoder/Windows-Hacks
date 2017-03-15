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
            IntPtr hWnd = OtherFunctions.GetFocusedWindow();

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
    }
}
