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
            IntPtr hWnd = GetFocusedWindow();
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
            IntPtr hWnd = GetFocusedWindow();
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
            IntPtr hWnd = GetFocusedWindow();
            Console.Write("New Title: ");
            Window.SetTitle(hWnd, Console.ReadLine());
        }

        public static void ResizeBorders()
        {
            IntPtr hWnd = GetFocusedWindow();

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
            IntPtr hWnd = GetFocusedWindow();
            Window.EnableMouseTransparency(hWnd);
        }

        public static void Hide()
        {
            IntPtr hWnd = GetFocusedWindow();
            Window.Hide(hWnd);
            HidenWindows.Add((Window.GetTitle(hWnd), hWnd));
            Console.WriteLine(@"You can reshow it by select 'Show'(20.Show).");
        }

        //Need to install Nuget Package "System.ValueTuple"
        static List<(string, IntPtr)> HidenWindows = new List<(string, IntPtr)>();

        public static void Show()
        {
            try
            {
                //There is an error in the past version.That method need to show the hiden window,
                //but a window cannot be selected when its hiden.
                //IntPtr hWnd = GetFocusedWindow();
                (string, IntPtr) hide = GetHidenWindow();
                //Using Window.Show(IntPtr) will let thw window turn to the smallest size.
                //I don't know why it happens or how to deal with it.
                Window.Show(hide.Item2);
                HidenWindows.Remove(hide);
            }
            catch (ArgumentException a) 
            {
                if (a.Message == "InvalidInput")
                {
                    return;
                }
                else
                    throw;
            }
        }

        private static (string, IntPtr) GetHidenWindow()
        {
            if (HidenWindows.Count > 0)
            {
                Console.WriteLine("-----------------------------SELECT WINDOW-----------------------------");
                for (int i = 0; i < HidenWindows.Count; i++)
                {
                    Console.WriteLine("{0}.{1}", i + 1, HidenWindows[i].Item1);
                }
                Console.Write("Input:");
                bool canindex = int.TryParse(Console.ReadLine(), out int index);
                if (canindex)
                {
                    return HidenWindows[index - 1];
                }
                else
                {
                    Console.WriteLine("Invalid Input.");
                    throw new ArgumentException("InvalidInput");
                }
            }
            else
            {
                Console.WriteLine("No hiden windows.");
                throw new ArgumentException("InvalidInput");
            }
        }

        public static void FlipLeft()
        {
            IntPtr hWnd = GetFocusedWindow();
            Window.FlipLeft(hWnd);
        }

        public static void FlipRight()
        {
            IntPtr hWnd = GetFocusedWindow();
            Window.FlipRight(hWnd);
        }

        public static void RemoveMenu()
        {
            IntPtr hWnd = GetFocusedWindow();
            Window.RemoveMenu(hWnd);
        }

        public static void DisableClose()
        {
            IntPtr hWnd = GetFocusedWindow();
            Window.DisableCloseButton(hWnd);
        }

        public static void DisableMaximize()
        {
            IntPtr hWnd = GetFocusedWindow();
            Window.DisableMaximizeButton(hWnd);
        }

        public static void DisableMinimize()
        {
            IntPtr hWnd = GetFocusedWindow();
            Window.DisableMinimizeButton(hWnd);
        }

        /// <summary>
        /// get the currently focused window.
        /// </summary>
        internal static IntPtr GetFocusedWindow()
        {
            Console.WriteLine("Select a window within 2 seconds:");
            System.Threading.Thread.Sleep(2000);
            var ptr = Window.GetFocused();
            var windowName = Window.GetTitle(ptr);
            Console.WriteLine($"You've selected '{windowName}'");
            Console.WriteLine($"Type 'Y' to proceed or 'N' to retry, default is 'Y':");

            var response = Console.ReadLine();
            if (response.ToLower().Contains("n"))
                ptr = GetFocusedWindow();

            return ptr;
        }
    }
}
