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

            Mask layer = new Mask(hWnd, screenshot);

            // The actual blurring.
            for (int i = 0; i < 10; i++)
            {
                screenshot = Filter.BlurFast(screenshot);
                System.Threading.Thread.Sleep(100);
                layer.Picture.Image = screenshot;
                layer.Picture.Update();
            }

            Window.Close(hWnd);
            System.Threading.Thread.Sleep(100);
            Application.Run();
        }

    }
}
