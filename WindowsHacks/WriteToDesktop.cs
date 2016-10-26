using System;
using System.Drawing;
using WindowsAPI;

namespace WindowsHacks
{

    /// <summary>
    /// Writes directly to the Desktop.
    /// </summary>
    public static class WriteToDesktop
    {
        public static void Run()
        {
            int count = 0;
            while (true)
            {
                Console.Write("Input: ");
                string input = Console.ReadLine();
                Draw.String(input, 200, 200 + count, Color.White, 20);
                count += 22;
            }
        }
    }
}
