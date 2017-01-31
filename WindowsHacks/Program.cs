using System;
using System.Windows.Forms;
using WindowsAPI;

namespace WindowsHacks
{
    class Program
    {
        static void Main(string[] args)
        {

            while (true)
            {
                DisplayMenu();
                Console.Write("Input: ");
                int input = 0;
                bool inputIsInt = int.TryParse(Console.ReadLine(), out input);

                // I used if statements instead of switch deliberately.
                // Typing in "break" after each switch doubles the lines of code and looks ugly.
                if (!inputIsInt) Console.WriteLine("Invalid input.");
                else if (input == 1) WriteToDesktop.Run();
                else if (input == 2) RotateDesktop.Run();
                else if (input == 3) MeltScreen.Run();
                else if (input == 4) OtherFunctions.ShakeMouse();
                else if (input == 5) MouseSpam.Run();
                else if (input == 6) MouseSpam.RunExtreme();
                else if (input == 7) ShrinkWindow.Run();
                else if (input == 8) HueShiftWindow.Run();
                else if (input == 9) MeltWindow.Run();
                else if (input == 10) InvertWindow.Run();
                else if (input == 11) Wavy.Run();
                else if (input == 12) Scrambler.Run();
                else if (input == 13) TraceWindow.Run(1);
                else if (input == 14) OtherFunctions.WindowShaker();
                else if (input == 15) OtherFunctions.WindowShakerExtreme();
                else if (input == 16) OtherFunctions.SetTitle();
                else if (input == 17) OtherFunctions.ResizeBorders();
                else if (input == 18) OtherFunctions.MouseTransparency();
                else if (input == 19) OtherFunctions.Hide();
                else if (input == 20) OtherFunctions.Show();
                else if (input == 21) OtherFunctions.RemoveMenu();
                else if (input == 22) OtherFunctions.DisableClose();
                else if (input == 23) OtherFunctions.DisableMaximize();
                else if (input == 24) OtherFunctions.DisableMinimize();
                else if (input == 25) RippleEffect.Run();
                else if (input == 26) DesktopArt.Run();
                else if (input == 0) break;
            }
        }

        static void DisplayMenu()
        {
            Console.WriteLine("-----------------------------DESKTOP & MOUSE-----------------------------");
            Console.WriteLine("1.  Write to Desktop                         4. Shake Mouse");
            Console.WriteLine("2.  Rotate Desktop                           5. Mouse Spam");
            Console.WriteLine("3.  Melt Desktop                             6. Mouse Spam Extreme");
            Console.WriteLine("----------------------------WINDOW APPEARANCE----------------------------");
            Console.WriteLine("7.  Shrink                                   12. Scramble");
            Console.WriteLine("8.  Hue Shift                                13. Trace");
            Console.WriteLine("9.  Melt                                     14. Shake");
            Console.WriteLine("10. Invert                                   15. Shake Extreme");
            Console.WriteLine("11. Wavy");
            Console.WriteLine("----------------------------WINDOW PROPERTIES----------------------------");
            Console.WriteLine("16. Set Title                                21. Remove Menu");
            Console.WriteLine("17. Resize Borders                           22. Disable Close Button");
            Console.WriteLine("18. Mouse Transparency                       23. Disable Maximize Button");
            Console.WriteLine("19. Hide                                     24. Disable Minimize Button");
            Console.WriteLine("20. Show");
            Console.WriteLine("------------------------------MISCELLANEOUS------------------------------");
            Console.WriteLine("25. Ripple Effect                            0.  Quit");
            Console.WriteLine("26. Desktop Art");
            Console.WriteLine("-------------------------------------------------------------------------");
        }
    }
}
