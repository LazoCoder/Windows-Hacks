using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsAPI;

namespace WindowsHacks
{
    class OtherFunctions
    {

        public static void ShakeMouse()
        {
            Random r = new Random();
            int offset = 20;

            while (true)
            {
                int currentX = Cursor.Position.X;
                int currentY = Cursor.Position.Y;
                int x = r.Next(currentX - offset, currentX + offset + 1);
                int y = r.Next(currentY - offset, currentY + offset + 1);
                Mouse.Move(x, y);
                System.Threading.Thread.Sleep(10);
            }

        }

        public static void WindowShaker()
        {
            string windowTitle = GetWindowTitle();
            IntPtr hWnd = Window.Get(windowTitle);
            Random r = new Random();
            for (int i = 0; i < 1000; i++)
            {
                int offset = 2;
                int currentX = Window.GetLocation(hWnd).X;
                int currentY = Window.GetLocation(hWnd).Y;
                int x = r.Next(currentX - offset, currentX + offset + 1);
                int y = r.Next(currentY - offset, currentY + offset + 1);
                Window.Move(hWnd, x, y);
                System.Threading.Thread.Sleep(10);
            }
        }

        public static void WindowShakerExtreme()
        {
            string windowTitle = GetWindowTitle();
            IntPtr hWnd = Window.Get(windowTitle);
            Random r = new Random();
            for (int i = 0; i < 1000; i++)
            {
                int x = r.Next(0, Desktop.GetWidth());
                int y = r.Next(0, Desktop.GetHeight());
                Window.Move(hWnd, x, y);
                System.Threading.Thread.Sleep(10);
            }
        }

        public static void SetTitle()
        {
            string windowTitle = GetWindowTitle();
            IntPtr hWnd = Window.Get(windowTitle);
            Console.Write("New Title: ");
            Window.SetTitle(hWnd, Console.ReadLine());
        }

        public static void ResizeBorders()
        {
            string windowTitle = GetWindowTitle();
            IntPtr hWnd = Window.Get(windowTitle);

            Console.Write("New Width: ");
            int width = 0;
            bool inputIsInt = int.TryParse(Console.ReadLine(), out width);
            if (!inputIsInt || width < 0)
            {
                Console.WriteLine("Invalid input.");
                return;
            }

            Console.Write("New Height: ");
            int height = 0;
            inputIsInt = int.TryParse(Console.ReadLine(), out height);
            if (!inputIsInt || height < 0)
            {
                Console.WriteLine("Invalid input.");
                return;
            }

            Window.Resize(hWnd, width, height);
        }

        public static void MouseTransparency()
        {
            string windowTitle = GetWindowTitle();
            IntPtr hWnd = Window.Get(windowTitle);
            Window.EnableMouseTransparency(hWnd);
        }

        public static void Hide()
        {
            string windowTitle = GetWindowTitle();
            IntPtr hWnd = Window.Get(windowTitle);
            Window.Hide(hWnd);
        }

        public static void Show()
        {
            string windowTitle = GetWindowTitle();
            IntPtr hWnd = Window.Get(windowTitle);
            Window.Show(hWnd);
        }

        public static void RemoveMenu()
        {
            string windowTitle = GetWindowTitle();
            IntPtr hWnd = Window.Get(windowTitle);
            Window.RemoveMenu(hWnd);
        }

        public static void DisableClose()
        {
            string windowTitle = GetWindowTitle();
            IntPtr hWnd = Window.Get(windowTitle);
            Window.DisableCloseButton(hWnd);
        }

        public static void DisableMaximize()
        {
            string windowTitle = GetWindowTitle();
            IntPtr hWnd = Window.Get(windowTitle);
            Window.DisableMaximizeButton(hWnd);
        }

        public static void DisableMinimize()
        {
            string windowTitle = GetWindowTitle();
            IntPtr hWnd = Window.Get(windowTitle);
            Window.DisableMinimizeButton(hWnd);
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
