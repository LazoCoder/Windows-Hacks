using System;
using WindowsAPI;
using System.Drawing;
using ImageProcessing;

namespace WindowsHacks
{

    /// <summary>
    /// Performs an effect that makes the desktop appear that it is melting.
    /// </summary>
    public static class MeltScreen
    {
        public static void Run()
        {
            Bitmap screenshot = Desktop.Screenshot();
            Mask mask = new Mask(screenshot);
            Desktop.HideTaskBar();

            for (int i = 0; i < 80; i++)
            {
                Mouse.Move(0, 0);
                screenshot = Filter.BlurFast(screenshot);
                mask.Picture.Image = screenshot;
                mask.Picture.Update();
            }

            for (int i = 0; i < 500; i++)
            {
                Mouse.Move(0, 0);
                System.Threading.Thread.Sleep(10);
            }
            mask.Close();
            Desktop.ShowTaskBar();

            Mouse.Move(15, Desktop.GetWidth() - 15);
        }
    }
}
